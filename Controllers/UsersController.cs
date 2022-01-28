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
    public class UsersController : ControllerBase
    {
        private readonly UsersContext _userList;
        public UsersController(UsersContext context)
        { 
            _userList = context;
        }

        [HttpGet]
        [Route("addUser/{userInfo}")]
        public bool AddUser(string userInfo)
        {
            User user = JsonConvert.DeserializeObject<User>(userInfo);
            if(_userList.Users.Where(u => u.UserName == user.UserName).ToList().Count > 0)
                return false;

            _userList.Users.Add(user);
            return true;
        }

        [HttpGet]
        [Route("verify/{userInfo}")]
        public bool VerifyUser(string userInfo)
        {
            User user = JsonConvert.DeserializeObject<User>(userInfo);
            if(_userList.Users.Where(u => u.UserName == user.UserName && u.Password == user.Password).ToList().Count > 0)
                return true;

            return false;
        }
    }
}