using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserBL iuserBL;

        public UserController(IUserBL iuserBL)
        {
            this.iuserBL = iuserBL;
        }



        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUser(UserRegistrationModel userRegistration)
        {

            try
            {

                var result = iuserBL.Registration(userRegistration);

                if(result != null)
                {
                    return Ok(new {success = true, message = "Registration successful", data = result});
                }
                else
                {
                    return BadRequest(new { success = false, message = "Registration unsuccessful"});
                }

            }
            catch (System.Exception)
            {

                throw;
            }

        }



        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin(UserLogin userLogin)
        {

            try
            {

                var result = iuserBL.Login(userLogin);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Login successful", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Login unsuccessful" });
                }

            }
            catch (System.Exception)
            {

                throw;
            }

        }


    }
}
