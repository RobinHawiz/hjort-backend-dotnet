namespace HjortApi.Models
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    public class AdminUserReqModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "Password is required.")]
        [JsonPropertyName("passwordHash")]
        public string Password { get; set; } = default!;
    }
}
