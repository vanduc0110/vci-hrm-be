namespace TTDesign.API.Domain.Models
{
  public partial class GoogleTimesheetKey
    {
        public long Id { get; set; }
        public string KeyName { get; set; } = null!;
        public string SpreadsheetId { get; set; } = null!;
        public string ClientEmail { get; set; } = null!;
        public string PrivateKey { get; set; } = null!;
        public string SheetId { get; set; } = null!;
    }
}
