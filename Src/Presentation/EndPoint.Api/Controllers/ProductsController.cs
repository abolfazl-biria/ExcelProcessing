using Application.Interfaces.Products.Commands;
using EndPoint.Api.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProcessingProductService processingProductService, ILogger<ProductsController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadExcel([FromForm] UploadExcelRequest request)
    {
        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        if (!Directory.Exists(wwwrootPath))
            Directory.CreateDirectory(wwwrootPath);

        var tempFileName = $"{Guid.NewGuid()}.xlsx";
        var filePath = Path.Combine(wwwrootPath, tempFileName);

        logger.LogInformation("Temporary file path: {FilePath}", filePath);

        try
        {
            await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                await request.File.CopyToAsync(fileStream);

            var jobId = BackgroundJob.Enqueue(() => processingProductService.ProcessExcelFileAsync(filePath, CancellationToken.None));

            logger.LogInformation("Background job created with ID: {JobId}", jobId);

            return Accepted(new { JobId = jobId, Message = "File is being processed." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the file.");
            throw;
        }
    }
}