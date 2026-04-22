using Microsoft.AspNetCore.Mvc;

namespace TTDesign.API.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  public class DownloadController : ControllerBase
  {
    private readonly IWebHostEnvironment _env;
    public DownloadController( IWebHostEnvironment env )
    {
      _env = env;
    }
    [HttpGet( "download" )]
    public IActionResult DownloadLogFile( [FromQuery] string fileName )
    {
      if ( string.IsNullOrEmpty( fileName ) )
        return BadRequest( "File name is required." );

      var logsPath = Path.Combine( _env.ContentRootPath, "logs" );
      var filePath = Path.Combine( logsPath, fileName );

      if ( !System.IO.File.Exists( filePath ) )
        return NotFound( "File not found." );

      var contentType = "application/octet-stream";

      var fileBytes = System.IO.File.ReadAllBytes( filePath );

      return File( fileBytes, contentType, fileName );
    }
    [HttpPost( "upload" )]
    [Consumes( "multipart/form-data" )]
    public async Task<IActionResult> UploadFile( IFormFile file )
    {
      var fileName = file.FileName;
      var path = Path.Combine( _env.WebRootPath, "Excel" );
      // create folder storage
      if ( !Directory.Exists( path ) ) {
        Directory.CreateDirectory( path );
      }
      // save new file
      if ( System.IO.File.Exists( Path.Combine( path, fileName ) ) ) {
        System.IO.File.Delete( Path.Combine( path, fileName ) );
      }
      using ( var fileStream = new FileStream( Path.Combine( path, fileName ), FileMode.Create ) ) {
        await file.CopyToAsync( fileStream );
      }
      return Ok();
    }
  }
}
