namespace HjortApi.Models
{
    using System.Text.Json.Serialization;
    public class AdminUserReqModel
    {
        public string Username { get; set; }

        [JsonPropertyName("passwordHash")]
        public string Password { get; set; }
    }
}
