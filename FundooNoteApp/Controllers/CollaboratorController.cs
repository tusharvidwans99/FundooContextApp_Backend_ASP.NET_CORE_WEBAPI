using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    }
}
