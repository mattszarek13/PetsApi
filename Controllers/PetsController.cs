using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionApi.Models;
using System.Threading.Tasks;

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
        
    }
}