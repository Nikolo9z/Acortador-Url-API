using AcortadorURL.Data;
using AcortadorURL.Modelo.InsertURL;
using AcortadorURL.Modelo.OriginalURL;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AcortadorURL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcortadorURLController : ControllerBase
    {
        private readonly InsertURL _insertURL;
        private readonly OriginalURL _originalURL;

        public AcortadorURLController(InsertURL insertURL, OriginalURL originalURL)
        {
            _insertURL = insertURL;
            _originalURL = originalURL;
        }

        [HttpPost]
        public IActionResult Post(RequestInsertURL url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url.URL) || !Uri.TryCreate(url.URL, UriKind.Absolute, out Uri uriResult))
                {
                    return BadRequest(new OriginalUrlResponse
                    {
                        error = "BadRequest",
                        statusCode = 400,
                        description = "The URL is invalid."
                    });
                }

                var response = _insertURL.InsertarURL(url);
                return Ok(new OriginalUrlResponse
                {
                    statusCode = 200,
                    description = "Ok",
                    OriginalURL = response.ShortURL
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OriginalUrlResponse
                {
                    error = "InternalServerError",
                    statusCode = 500,
                    description = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet]
        public IActionResult GetUrlOriginal(string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return BadRequest(new OriginalUrlResponse
                    {
                        error = "BadRequest",
                        statusCode = 400,
                        description = "Code is required."
                    });
                }

                var request = new OriginalUrlRequest { code = code };
                var response = _originalURL.GetUrlOriginal(request);

                if (response.OriginalURL == null)
                {
                    return Ok(new OriginalUrlResponse
                    {
                        error = "NotFound",
                        statusCode = 404,
                        description = "No se encontro el la URL."
                    });
                }

                return Ok(new OriginalUrlResponse
                {
                    statusCode = 200,
                    description = "Ok",
                    OriginalURL = response.OriginalURL
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new OriginalUrlResponse
                {
                    error = "InternalServerError",
                    statusCode = 500,
                    description = "An error occurred while processing your request."
                });
            }
        }
    }
}
