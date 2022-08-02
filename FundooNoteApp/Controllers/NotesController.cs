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
    public class NotesController : ControllerBase
    {

        private readonly INotesBL iNotesBL;

        public NotesController(INotesBL iNotesBL)
        {
            this.iNotesBL = iNotesBL;
        }



        
        [HttpPost]
        [Route("Create")]
        public IActionResult CreateNote(NoteCreateModel notesCreateModel)
        {

            try
            {

                long UserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

                var result = iNotesBL.AddNotes(notesCreateModel, UserId);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Notes Created successfully", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Notes Creation fail" });
                }

            }
            catch (System.Exception)
            {

                throw;
            }

        }

        

    }
}
