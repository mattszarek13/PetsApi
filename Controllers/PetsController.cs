using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionApi.Models;
using Newtonsoft.Json;

namespace PetAdoptionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PetsController : ControllerBase
    {
        private readonly PetsContext _petList;
        public PetsController(PetsContext context)
        { 
            _petList = context;
        }

        [HttpGet]
        public ActionResult<List<Pet>> GetAllPets()
        {
            return _petList.Pets.ToList<Pet>();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Pet> GetPetById(long id)
        {
            var pet = _petList.Pets.FirstOrDefault(p => p.Id == id);
            if(pet == null)
            {
                return NotFound();
            }

            return pet;
        }
        //Using alpha because of an issue where the path was too similar to GetByById
        //Since the path of GetPetsByType will never contain an integer, alpha seems to be the best fix
        [HttpGet("{type:alpha}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Pet>> GetPetsByType(string type)
        {
            var pets = _petList.Pets.Where(p => p.Pet_Type == type);

            if(pets == null)
            {
                return NotFound();
            }

            return pets.ToList<Pet>();
        }

        [HttpGet]
        [Route("maxAge")]
        public long GetMaxAge()
        {
            var oldestPet = _petList.Pets.Max(p => p.Age);

            return oldestPet;
        }

        [HttpGet]
        [Route("minAge")]
        public long GetMinAge()
        {
            var youngestPet = _petList.Pets.Min(p => p.Age);

            return youngestPet;
        }

        [HttpGet]
        [Route("petBreed/{type:alpha}")]
        public ActionResult<List<string>> GetPetBreeds(string type)
        {
            var petBreeds = _petList.Pets.Where(p => p.Pet_Type == type).Select(b => b.Breed).Distinct();
            return petBreeds.ToList();
        }
        [HttpGet]
        [Route("featured")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Pet>> GetFeaturedPets(string type)
        {
            var pets = _petList.Pets.Where(p => p.Featured == true);

            if(pets == null)
            {
                return NotFound();
            }

            return pets.ToList<Pet>();
        }

        [HttpGet]
        [Route("filterPets/{filters}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Pet>> FilterPets(string filters){
            FilterData userFilters = JsonConvert.DeserializeObject<FilterData>(filters);
            List<Pet> toReturn = new List<Pet>();

            var cats = _petList.Pets.Where(t => t.Pet_Type == "cat");
            var dogs = _petList.Pets.Where(t => t.Pet_Type == "dog");;
            var birds = _petList.Pets.Where(t => t.Pet_Type == "bird");;
            var reptiles = _petList.Pets.Where(t => t.Pet_Type == "reptile");;
            var other = _petList.Pets.Where(t => t.Pet_Type == "other");

            if(!userFilters.Cats && !userFilters.Dogs && !userFilters.Birds && !userFilters.Reptiles && !userFilters.Other)
            {
                return _petList.Pets.ToList();
            }

            if(userFilters.Cats) {
                foreach(var cat in cats.ToList())
                    toReturn.Add(cat);
            }
            if(userFilters.Dogs) {
                foreach(var dog in dogs.ToList())
                    toReturn.Add(dog);
            }
            if(userFilters.Birds) {
                foreach(var bird in birds.ToList())
                    toReturn.Add(bird);
            }
            if(userFilters.Reptiles) {
                foreach(var rep in reptiles.ToList())
                    toReturn.Add(rep);
            }
            if(userFilters.Other) {
                foreach(var o in other.ToList())
                    toReturn.Add(o);
            }
            //discrete math finally pays off
            var petsAge = _petList.Pets.Where(p => p.Age < userFilters.Max_Age);
            IEnumerable<Pet> petsThatFit = petsAge.ToList().AsEnumerable();
            petsThatFit = petsThatFit.Intersect(toReturn);
            if(userFilters.Max_Age != 0)
            {
                toReturn = petsThatFit.ToList();
            }

            if( userFilters.Max_Price != 0)
            {
                var petsPrice = _petList.Pets.Where(p => p.Price < userFilters.Max_Price);
                petsThatFit = petsPrice.ToList().AsEnumerable();
                petsThatFit = petsThatFit.Intersect(toReturn);
                toReturn = petsThatFit.ToList();
            }
            
            if(userFilters.City != "" && userFilters.State != "")
            {
                var petsLocation = _petList.Pets.Where(p => p.City == userFilters.City && p.State == userFilters.State);
                petsThatFit = petsLocation.ToList().AsEnumerable();
                petsThatFit = petsThatFit.Intersect(toReturn);
                toReturn = petsThatFit.ToList();
            }
                
            if(userFilters.Gender != "")
            {
                if(userFilters.Gender == "male")
                {
                    var petsGender = _petList.Pets.Where(p => p.Gender == "male");
                    petsThatFit = petsGender.ToList().AsEnumerable();
                    petsThatFit = petsThatFit.Intersect(toReturn);
                    toReturn = petsThatFit.ToList();
                }
                else if(userFilters.Gender == "female")
                {
                    var petsGender = _petList.Pets.Where(p => p.Gender == "female");
                    petsThatFit = petsGender.ToList().AsEnumerable();
                    petsThatFit = petsThatFit.Intersect(toReturn);
                    toReturn = petsThatFit.ToList();
                }
            }
            
            return toReturn;
        }

        
        
    }
}