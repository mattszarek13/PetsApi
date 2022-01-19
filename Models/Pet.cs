namespace PetAdoptionApi.Models
{
    public class Pet
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Age { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public string Pet_Type { get; set; }
        public string Picture { get; set; }
        public string Breed { get; set; }
        public bool Featured { get; set; }
        public float Price { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}