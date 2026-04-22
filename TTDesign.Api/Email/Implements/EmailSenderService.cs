using Infrastructure.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.Text;
using TTDesign.API.Email;
using TTDesign.API.Resources;
using static TTDesign.API.Constants.Policies;

namespace HRM.Infrastructure.Email.Implements;

public class EmailSenderService : IEmailSenderService
{
  private readonly EmailSenderOptions _options;
  private readonly ILogger<EmailSenderService> _logger;
  public EmailSenderService( IOptionsMonitor<EmailSenderOptions> senderOptions, ILogger<EmailSenderService> logger )
  {
    _options = senderOptions.CurrentValue;
    _logger = logger;
  }


  public async Task SendEmailAsync( EmailMessage message, EmailTemplates template )
  {
    var htmlFile = Path.Combine( AppContext.BaseDirectory, "MailTemplate" );

    var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject( htmlFile )
            .UseMemoryCachingProvider()
            .Build();

    var body = await engine.CompileRenderAsync( $"{template}.cshtml", message.Model );
    var builder = new BodyBuilder
    {
      HtmlBody = body
    };

    var mail = new MimeMessage();
    mail.From.Add( new MailboxAddress( Encoding.UTF8, _options.DisplayName, _options.FromEmail ) );

    foreach ( var emailMessageReceiver in message.Receivers ) {
      mail.To.Add( new MailboxAddress( Encoding.UTF8, emailMessageReceiver.FullName, emailMessageReceiver.To ) );
    }

    mail.Subject = message.Subject;
    mail.Body = builder.ToMessageBody();

    using var client = new SmtpClient();
    try {
      await client.ConnectAsync( _options.Host, _options.Port );
      await client.AuthenticateAsync( _options.Username, _options.Password );
      var response = await client.SendAsync( mail );
      _logger.LogInformation( "Email sent! Message ID: " + response );
    }
    catch ( Exception ex ) {
      _logger.LogError( "Email sent! Message ID: " + ex.Message );
    }
  }
}
