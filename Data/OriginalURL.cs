using AcortadorURL.Modelo.OriginalURL;
using Npgsql; // Cambiar a Npgsql
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

            // Usar NpgsqlConnection para conectarse a PostgreSQL
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Llamar a la función de PostgreSQL con SELECT
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT public.get_original_url(@ShortUrl)", connection))
                {
                    // Agregar el parámetro
                    command.Parameters.AddWithValue("@ShortUrl", request.code);

                    // Ejecutar la consulta y obtener el resultado
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        response.OriginalURL = result.ToString();
                    }
                    else
                    {
                        throw new Exception($"La URL corta '{request.code}' no fue encontrada.");
                    }
                }
            }

            return response;
        }
    }
}
