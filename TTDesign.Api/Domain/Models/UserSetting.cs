namespace TTDesign.API.Domain.Models
{
  public partial class UserSetting
    {
        public long Id { get; set; }
        public bool EmailNotification { get; set; }
        public string Timezone { get; set; } = null!;
        public string Language { get; set; } = null!;
    }
}
