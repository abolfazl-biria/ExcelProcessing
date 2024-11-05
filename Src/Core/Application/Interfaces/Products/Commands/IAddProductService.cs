using Application.Models.Commands.Products;

namespace Application.Interfaces.Products.Commands;

public interface IAddProductService
{
    Task AddAsync(AddProductCommand command, CancellationToken cancellationToken);
}