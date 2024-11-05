using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Contexts;

public interface IDataBaseContext
{
    DbSet<Product> Products { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}