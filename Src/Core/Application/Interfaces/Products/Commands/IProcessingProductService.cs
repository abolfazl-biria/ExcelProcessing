namespace Application.Interfaces.Products.Commands;

public interface IProcessingProductService
{
    Task ProcessExcelFileAsync(string filePath, CancellationToken cancellationToken = new());
}