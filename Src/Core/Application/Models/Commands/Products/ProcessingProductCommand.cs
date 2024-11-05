namespace Application.Models.Commands.Products;

public class ProcessingProductCommand(Stream fileStream)
{
    public Stream FileStream { get; set; } = fileStream;
}