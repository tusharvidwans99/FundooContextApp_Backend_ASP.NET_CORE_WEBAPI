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
    public class CollaboratorController : ControllerBase
    {

        private readonly ICollaboratorBL icollaboratorBL;

        public CollaboratorController(ICollaboratorBL icollaboratorBL)
        {
            this.icollaboratorBL = icollaboratorBL;
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult AddCollaborate(long notesId, CollaboratedModel model)
        {
            long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

            if (jwtUserId == 0 && notesId == 0)
            {
                return BadRequest(new { Success = false, message = "Email Missing For Collaboration" });
            }

            CollabResponseModel collaborate = icollaboratorBL.AddCollaborate(notesId, jwtUserId, model);
            return Ok(new { Success = true, message = "Collaboration Successfull ", collaborate });
        }


        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteCollaborate(long collabId)
        {

            long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

            CollaboratorEntity collabid = icollaboratorBL.GetCollabWithId(collabId);
            if (collabid == null)
            {
                return BadRequest(new { Success = false, message = "No Collaboration Found" });
            }
            icollaboratorBL.DeleteCollab(collabid);
            return Ok(new { Success = true, message = "Collaborated Email Removed" });
        }




        [HttpGet]
        [Route("Get")]
        public IActionResult GetCollab()
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = icollaboratorBL.GetCollab(userID);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Got all collaborator notes.", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cannot get collaborator." });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }


    }
}
