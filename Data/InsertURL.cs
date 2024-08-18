using AcortadorURL.Modelo.InsertURL;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AcortadorURL.Data
{
    public class InsertURL
    {
        private readonly string _connectionString;
        public InsertURL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyDatabaseConnectionString");
        }

        public ResponseInsertURL InsertarURL(RequestInsertURL url)
        {
            ResponseInsertURL response = new ResponseInsertURL();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("InsertURL", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@OriginalUrl", url.URL));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        response= new ResponseInsertURL
                        {
                            ShortURL = reader["ShortUrl"].ToString()
                        };
                        
                    }

                }
                
            }
            return response;
        }
    }
}
