using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
namespace PetAdoptionApi.Models
{
    [Table("users")]
    public class User
    {
        public int Id { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}