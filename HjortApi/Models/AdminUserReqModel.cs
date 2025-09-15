namespace HjortApi.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    public class AdminUserReqModel
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [JsonPropertyName("passwordHash")]
        public string Password { get; set; }
    }
}
