using System.Collections.Generic;
using Newtonsoft.Json;
namespace PetAdoptionApi.Models
{
    public class FilterData
    {
        [JsonProperty("cats")]
        public bool Cats { get; set; }
        [JsonProperty("dogs")]
        public bool Dogs { get; set; }
        [JsonProperty("birds")]
        public bool Birds { get; set; }
        [JsonProperty("reptiles")]
        public bool Reptiles { get; set; }
        [JsonProperty("other")]
        public bool Other { get; set; }


        [JsonProperty("maximumAge")]
        public int Max_Age { get; set; }
        [JsonProperty("maximumPrice")]
        public float Max_Price { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }

    }
}