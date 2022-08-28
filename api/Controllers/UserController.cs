using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using signup_api.Models;

namespace signup_api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {
        }

        // POST: api/user
        [HttpPost("")]
        public ActionResult<Response> AddUser(UserConfig newUser)
        {
            if (newUser)
            {

            } else
            {
                return new Response
                {
                    StatusCode = 400,
                    Response = "Malformed user details"
                };
            }
        } 

    }
}
