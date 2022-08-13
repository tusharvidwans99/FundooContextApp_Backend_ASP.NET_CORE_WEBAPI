using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserBL iuserBL;

        private readonly ILogger<UserController> logger;

        public UserController(IUserBL iuserBL, ILogger<UserController> logger)
        {
            this.iuserBL = iuserBL;
            this.logger = logger;
        }




        /// <summary>
        /// This method is used to register a new user in the database.
        /// </summary>
        /// <param name="userRegistration"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUser(UserRegistrationModel userRegistration)
        {

            try
            {

                var result = iuserBL.Registration(userRegistration);

                if(result != null)
                {
                    logger.LogInformation("Registration Successful");
                    return Ok(new {success = true, message = "Registration successful", data = result});
                }
                else
                {
                    logger.LogWarning("Registration Unsuccessful");
                    return BadRequest(new { success = false, message = "Registration unsuccessful"});
                }

            }
            catch (System.Exception)
            {
                logger.LogError("Exception ocuured in Registration API");
                throw;
            }

        }


        /// <summary>
        /// This method will give the user his/her access to his data in the application.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin(UserLogin userLogin)
        {

            try
            {

                var result = iuserBL.Login(userLogin);

                if (result != null)
                {
                    logger.LogInformation("Login Successful");
                    return Ok(new { success = true, message = "Login successful", data = result });
                }
                else
                {
                    logger.LogWarning("Login Unsuccessful");
                    return BadRequest(new { success = false, message = "Login unsuccessful" });
                }

            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured  in User Login API");
                throw;
            }

        }



        /// <summary>
        /// This method is used to help the user for getting token through email.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ForgetPassword")]
        public IActionResult ForgetPassword(string Email)
        {

            try
            {

                var result = iuserBL.ForgetPassword(Email);

                if (result != null)
                {
                    logger.LogInformation($"Email sent successfull to {Email}");
                    return Ok(new { success = true, message = "Email sent successful"});
                }
                else
                {
                    logger.LogWarning("Email not found in our database");
                    return BadRequest(new { success = false, message = "Your email not found in our database" });
                }

            }
            catch (System.Exception)
            {
                logger.LogError("Exception Occured in Forget Password API");
                throw;
            }

        }



        /// <summary>
        /// This method will help the user to change his/her password using Token.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("ResetLink")]
        public IActionResult ResetLink(string password, string confirmPassword)
        {

            try
            {

                var Email = User.FindFirst(ClaimTypes.Email).Value.ToString();

                var result = iuserBL.ResetLink(Email, password, confirmPassword);

                if (result != null)
                {
                    logger.LogInformation("Reset Password Succesfull");
                    return Ok(new { success = true, message = "Reset Password successfull" });
                }
                else
                {
                    logger.LogWarning("Email not found in our database");
                    return BadRequest(new { success = false, message = "Email not found in our database" });
                }

            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in Reset Link API");
                throw;
            }

        }

    }
}
