using AcortadorURL.Modelo.OriginalURL;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AcortadorURL.Data
{
    public class OriginalURL
    {
        private readonly string _connectionString;
        public OriginalURL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyDatabaseConnectionString");
        }
        public OriginalUrlResponse GetUrlOriginal(OriginalUrlRequest request)
        {
            OriginalUrlResponse response = new OriginalUrlResponse();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetOriginalUrl", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ShortUrl", request.code));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        response.OriginalURL = reader["OriginalUrl"].ToString();
                    }
                }
            }
            return response;
        }
    }
}
