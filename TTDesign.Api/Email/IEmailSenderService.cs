using TTDesign.API.Resources;
using static TTDesign.API.Constants.Policies;

namespace TTDesign.API.Email
{
  public interface IEmailSenderService
  {
    Task SendEmailAsync( EmailMessage message, EmailTemplates template );
  }
}
