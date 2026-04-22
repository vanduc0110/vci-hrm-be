namespace TTDesign.API.Resources
{
  public class EmailMessage
  {
    public IList<EmailMessageReceiver> Receivers { get; set; } = new List<EmailMessageReceiver>();

    public string Subject { get; set; } = string.Empty;

    public object Model { get; set; } = new object();
  }

  public class EmailMessageReceiver
  {
    public string To { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
  }
}
