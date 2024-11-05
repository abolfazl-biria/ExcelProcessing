using Application.Interfaces.Contexts;
using Microsoft.Data.SqlClient;

namespace Persistence.Contexts;

public class DapperContext(string connectionString) : IDapperContext
{
    public SqlConnection CreateConnection() => new(connectionString);
}