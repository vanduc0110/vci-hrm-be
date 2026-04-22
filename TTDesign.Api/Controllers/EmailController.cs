using Microsoft.AspNetCore.Mvc;
using TTDesign.API.Email;
using TTDesign.API.Resources;
using static TTDesign.API.Constants.Policies;

namespace TTDesign.API.Controllers
{
  [Route( "api/[controller]" )]
  [ApiController]
  [Consumes( "application/json" )]
  [Produces( "application/json" )]
  public class EmailController : ControllerBase
  {
    private readonly IEmailSenderService _emailSenderService;
    public EmailController( IEmailSenderService emailSenderService )
    {
      _emailSenderService = emailSenderService;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail( [FromBody] SendEmailResource resource )
    {
      var data = new
      {
        resource.FullName,
        resource.PhoneNumber,
        resource.Email,
        resource.Content
      };
      var mailMessage = new EmailMessage
      {
        Receivers = new List<EmailMessageReceiver>
                            {
                                new EmailMessageReceiver
                                {
                                    To = "anhlt@vcijsc.com",
                                    FullName = "VCI"
                                }
                            },
        Subject = $"Thông tin về dự án",
        Model = data
      };
      var sendMailTask = new Task( async () =>
         {
           await _emailSenderService.SendEmailAsync( mailMessage, EmailTemplates.AboutProject );
         } );

      sendMailTask.Start();
      return Ok( "Đã gửi email" );
    }
  }
}
