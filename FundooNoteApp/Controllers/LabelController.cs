using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System;
using System.Linq;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {

        ILableBL ilableBL;

        public LabelController(ILableBL ilableBL)
        {
            this.ilableBL = ilableBL;
        }


        [HttpPost]
        [Route("Create")]
        public IActionResult CreateLable(long notesId, LableModel model)
        {

            try
            {


                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

                if (jwtUserId == 0 && notesId == 0)
                {
                    return BadRequest(new { Success = false, message = "Name Missing For Lable" });
                }
                else
                {

                    LableResponseModel lable = ilableBL.CreateLable(notesId, jwtUserId, model);

                    return Ok(new { Success = true, message = "Lable Created", lable });

                }


            }
            catch (Exception)
            {

                throw;
            }

            
        }


        [HttpGet]
        [Route("ReadAll")]
        public IActionResult GetAllLable()
        {

            try
            {

                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

                var result = ilableBL.GetAllLable(jwtUserId);
                
                if (result != null)
                {
                    return Ok(new { Success = true, message = "Retrived All lables ", result });

                }
                else
                {
                    return BadRequest(new { Success = false, message = "No lable in database " });

                }



            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpGet]
        [Route("GetLableById")]
        public IActionResult GetLableWithId(long lableId)
        {

            try
            {

                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = ilableBL.GetLableWithId(lableId, jwtUserId);

                if (result != null)
                {
                
                    return Ok(new { Success = true, message = "Retrived Lable ", result });

                }
                else
                {
                    return NotFound(new { Success = false, message = "No Lable With Particular LableId " });

                }


            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateLable(long lableId, UpdateLableModel model)
        {
            try
            {

                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                LableEntity updateLable = ilableBL.GetLablesWithId(lableId, jwtUserId);


                if (updateLable != null)
                {
                    LableResponseModel lable = ilableBL.UpdateLable(updateLable, model, jwtUserId);

                    return Ok(new { Success = true, message = "Lable Updated Sucessfully", lable });

                }
                else
                {
                    return BadRequest(new { Success = false, message = "No Notes Found With NotesId" });

                }



            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteLable(long lableId)
        {

            try
            {

                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                LableEntity lable = ilableBL.GetLablesWithId(lableId, jwtUserId);
                if (lable != null)
                {

                    ilableBL.DeleteLable(lable, jwtUserId);
                    return Ok(new { Success = true, message = "Lable Removed" });

                }
                else
                {
                    return BadRequest(new { Success = false, message = "No Lable Found" });

                }



            }
            catch (Exception)
            {

                throw;
            }

        }



    }
}
