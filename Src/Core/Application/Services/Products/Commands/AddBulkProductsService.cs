using Application.Interfaces.Contexts;
using Application.Interfaces.Products.Commands;
using Application.Models.Commands.Products;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Services.Products.Commands;

public class AddBulkProductsService(IDapperContext dapperContext, ILogger<AddBulkProductsService> logger) : IAddProductService
{
    public async Task AddAsync(AddProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting bulk product addition process.");

        await using var connection = dapperContext.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var transaction = connection.BeginTransaction();

        try
        {
            using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
            bulkCopy.DestinationTableName = "Products";

            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("InsertTime", typeof(DateTime));

            foreach (var name in command.Names)
            {
                dataTable.Rows.Add(0, name, DateTime.Now);
            }

            logger.LogInformation("Starting to write data to the table.");
            await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            logger.LogInformation("Transaction committed successfully and data saved.");

            await connection.CloseAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding records to the database.");
            await transaction.RollbackAsync(cancellationToken);

            throw new Exception("An error occurred while adding records to the database.", ex);
        }
    }
}