using TTDesign.API.Extensions;

namespace TTDesign.API.Services
{
  public class MailService
  {
    private MailSettings _mailSetting;
    private string _mailTo;
    private Dictionary<string, string>? _dataBody;

    // using code
    //var mailToEditor = new Mail( _mailSettings.Value, editor.Email, modelBodyMail );
    //Thread t1 = new Thread( new ThreadStart( mailToEditor.SendMailRegisterCode ) );
    //t1.Start();
    public MailService( MailSettings mailSetting, string mailTo, Dictionary<string, string>? dataBody )
    {
      _mailSetting = mailSetting;
      _mailTo = mailTo;
      _dataBody = dataBody;
    }

    public void SendMailRegisterCode()
    {
      var from = new System.Net.Mail.MailAddress( _mailSetting.Mail );
      var to = new System.Net.Mail.MailAddress( _mailTo );
      var message = new System.Net.Mail.MailMessage( from, to );
      message.Subject = "[Subject] Title";
      message.IsBodyHtml = true;
      //message.Body = this.PopulateBody( _dataBody, "EmailTempWhenCreateNewSheet.html" );

      var client = new System.Net.Mail.SmtpClient()
      {
        Host = "smtp.gmail.com",
        Port = 587,
        Credentials = new System.Net.NetworkCredential( _mailSetting.Mail, _mailSetting.Password ),
        EnableSsl = true
      };
      try {
        client.Send( message );
      }
      catch ( Exception ex ) {
        Console.WriteLine( ex.ToString() );
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataBody">keys: </param>
    /// <param name="mailTemp">file name</param>
    /// <returns></returns>
    private string PopulateBody( Dictionary<string, string> dataBody, string mailTemp )
    {
      string body = string.Empty;
      var pathFileTemp = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Mail", mailTemp );
      using ( StreamReader reader = new StreamReader( pathFileTemp ) ) {
        body = reader.ReadToEnd();
      }
      foreach ( var key in dataBody.Keys ) {
        body = body.Replace( "{" + key + "}", dataBody [ key ] );
      }
      return body;
    }
  }
}
