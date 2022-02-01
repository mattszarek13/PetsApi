using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionApi.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
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

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        [HttpGet]
        [Route("addUser/{userInfo}")]
        public bool AddUser(string userInfo)
        {
            User user = JsonConvert.DeserializeObject<User>(userInfo);
            //hash password
            string hashedPw = GetHashString(user.Password);
            user.Password = hashedPw;
            if(_userList.Users.Where(u => u.UserName == user.UserName).ToList().Count > 0)
                return false;
            user.Id = _userList.Users.Count() + 1;
            _userList.Users.Add(user);
            _userList.SaveChanges();
            return true;
        }

        [HttpGet]
        [Route("verify/{userInfo}")]
        public bool VerifyUser(string userInfo)
        {
            User user = JsonConvert.DeserializeObject<User>(userInfo);
            //hash password
            string hashedPw = GetHashString(user.Password);
            user.Password = hashedPw;
            if(_userList.Users.Where(u => u.UserName == user.UserName && u.Password == user.Password).ToList().Count > 0)
                return true;

            return false;
        }
    }
}