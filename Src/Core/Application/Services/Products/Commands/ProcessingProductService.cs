using Application.Interfaces.Products.Commands;
using Application.Models.Commands.Products;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Application.Services.Products.Commands;

public class ProcessingProductService(IAddProductService addProductService, ILogger<ProcessingProductService> logger) : IProcessingProductService
{
    private const int BatchSize = 1000;

    public async Task ProcessExcelFileAsync(string filePath, CancellationToken cancellationToken = new())
    {
        try
        {
            logger.LogInformation("Starting to process the Excel file.");

            var names = new List<string>();

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            logger.LogInformation("Worksheet loaded successfully. Total rows: {RowCount}", rowCount);

            for (var row = 2; row <= rowCount; row++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    logger.LogWarning("Processing was cancelled.");
                    return;
                }

                var name = worksheet.Cells[row, 1].Value?.ToString();

                if (string.IsNullOrWhiteSpace(name))
                {
                    logger.LogWarning("Empty or whitespace-only name found at row {RowNumber}. Skipping.", row); continue;
                }

                names.Add(name);

                if (names.Count == BatchSize)
                {
                    logger.LogInformation("Processing a batch of {BatchSize} items.", BatchSize);
                    await addProductService.AddAsync(new AddProductCommand { Names = names }, cancellationToken);
                    names.Clear();
                }
            }

            if (names.Count > 0)
            {
                logger.LogInformation("Processing the final batch of {RemainingItems} items.", names.Count);
                await addProductService.AddAsync(new AddProductCommand { Names = names }, cancellationToken);
            }

            logger.LogInformation("Excel file processing completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the Excel file.");
        }
        finally
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}