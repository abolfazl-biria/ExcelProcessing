using Microsoft.Data.SqlClient;

namespace Application.Interfaces.Contexts;

public interface IDapperContext
{
    SqlConnection CreateConnection();
}