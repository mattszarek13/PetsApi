using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionApi.Models;
using System.Threading.Tasks;
using System.Web.Helpers;

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

        [HttpPost]
        [Route("filterPets/{type:string}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Pet>> FilterPets(string filters){
            dynamic filterData = Json.Decode(filters);
            
            return null;
        }
        
    }
}