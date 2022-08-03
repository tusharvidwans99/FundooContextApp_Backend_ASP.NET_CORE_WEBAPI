﻿using BusinessLayer.Interface;
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
        public IActionResult CreateNote(NoteModel notesCreateModel)
        {

            try
            {

                long UserId = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);

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


        [HttpGet]
        [Route("Read")]
        public IActionResult ReadNotes()
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = iNotesBL.ReadNotes(userID);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Got Notes from databse.", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cannot get notes." });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }



        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateNote(NoteModel noteModel, long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = iNotesBL.UpdateNote(noteModel, NoteID, userID);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Note Updated Successfully.", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cannot update note." });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteNotes(long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = iNotesBL.DeleteNotes(userID, NoteID);
                if (result != false)
                {
                    return Ok(new { success = true, message = "Note Deleted." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cannot delete note." });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }



        [HttpPut]
        [Route("Pin")]
        public IActionResult pinToTop(long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = iNotesBL.PinToTop(NoteID, userID);
                if (result == true)
                {
                    return Ok(new { success = true, message = "Note Pinned Successfully" });
                }
                else if (result == false)
                {
                    return Ok(new { success = true, message = "Note Unpinned successfully." });
                }
                return BadRequest(new { success = false, message = "Operation Unsuccessful." });
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        [HttpPut]
        [Route("Archive")]
        public IActionResult Archive(long NoteID)
        {

            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = iNotesBL.Archive(NoteID, userID);

                if (result == true)
                {
                    return Ok(new { success = true, message = "Note Archived successfully" });
                }
                else if (result == false)
                {
                    return Ok(new { success = true, message = "Note UnArchived successfully." });
                }
                return BadRequest(new { success = false, message = "Operation Fail." });
            }
            catch (System.Exception)
            {
                throw;
            }

        }



        [HttpPut]
        [Route("Trash")]
        public IActionResult Trash(long NoteID)
        {

            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = iNotesBL.Trash(NoteID, userID);

                if (result == true)
                {
                    return Ok(new { success = true, message = "Note Trashed successfully" });
                }
                else if (result == false)
                {
                    return Ok(new { success = true, message = "Note Untrashed successfully." });
                }
                return BadRequest(new { success = false, message = "Operation Fail." });
            }
            catch (System.Exception)
            {
                throw;
            }

        }



        [HttpPut]
        [Route("Color")]
        public IActionResult Color(long NoteID, string color)
        {

            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = iNotesBL.Color(NoteID, color);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Color changed successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Color not changed." });
                }
            }
            catch (System.Exception)
            {
                throw;
            }

        }



        [HttpPut]
        [Route("Image")]
        public IActionResult Image(IFormFile image, long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = iNotesBL.Image(image, NoteID, userID);
                if (result != null)
                {
                    return Ok(new { success = true, message = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cannot upload image." });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }


    }
}
