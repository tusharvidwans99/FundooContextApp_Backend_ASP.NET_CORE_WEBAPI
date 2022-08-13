using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {

        private readonly INotesBL iNotesBL;

        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        private readonly ILogger<NotesController> logger;

        private readonly FundooContext fundooContext;

        public NotesController(INotesBL iNotesBL, IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext, ILogger<NotesController> logger)
        {
            this.iNotesBL = iNotesBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundooContext = fundooContext;
            this.logger = logger;
        }



        /// <summary>
        /// This method will create Note after successfull authorization of the user.
        /// </summary>
        /// <param name="notesCreateModel"></param>
        /// <returns></returns>
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
                    logger.LogInformation($"Note Created succesfully for {UserId} userId");
                    return Ok(new { success = true, message = "Notes Created successfully", data = result });
                }
                else
                {
                    logger.LogWarning("Note Creation Failed");
                    return BadRequest(new { success = false, message = "Notes Creation fail" });
                }

            }
            catch (System.Exception)
            {
                logger.LogError("Exception Occured in Create Note API");
                throw;
            }

        }


        /// <summary>
        /// This method will read all the notes which is available for the authorize user.
        /// </summary>
        /// <returns></returns>
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
                    logger.LogInformation("getting all the notes of authorize user");
                    return Ok(new { success = true, message = "Get the notes from database.", data = result });
                }
                else
                {
                    logger.LogWarning("didn't get the notes of the user");
                    return BadRequest(new { success = false, message = "Cannot get notes." });
                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in ReadNotes API");
                throw;
            }
        }


        /// <summary>
        /// This method will update the content of the existing note for the authorize user by boteid.
        /// </summary>
        /// <param name="noteModel"></param>
        /// <param name="NoteID"></param>
        /// <returns></returns>
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
                    logger.LogInformation($"Note updated succesfully for {NoteID} NoteID");
                    return Ok(new { success = true, message = "Note Updated Successfully.", data = result });
                }
                else
                {
                    logger.LogWarning($"Cannot update note for {NoteID} NoteID");
                    return BadRequest(new { success = false, message = "Cannot update note." });
                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in Update note API");
                throw;
            }
        }

        
        /// <summary>
        /// This method is used to delete the note of the authorize user by NoteID.
        /// </summary>
        /// <param name="NoteID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteNote(long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = iNotesBL.DeleteNotes(userID, NoteID);

                if (result != false)
                {
                    logger.LogInformation($"Note Deleted for {NoteID} NoteID");
                    return Ok(new { success = true, message = "Note Deleted." });
                }
                else
                {
                    logger.LogWarning($"Note not found for {NoteID} NoteID");
                    return BadRequest(new { success = false, message = "Note not found." });
                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in DeleteNotes API");
                throw;
            }
        }


        /// <summary>
        /// This method is used to change the state of the Pin feature.
        /// </summary>
        /// <param name="NoteID"></param>
        /// <returns></returns>
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
                    logger.LogInformation($"Note pinned successfull for {NoteID} NoteID");
                    return Ok(new { success = true, message = "Note Pinned Successfully" });
                }
                else if (result == false)
                {
                    logger.LogInformation($"Note unpinned successfull for {NoteID} NoteID");
                    return Ok(new { success = true, message = "Note Unpinned successfully." });
                }
                else
                {
                    logger.LogWarning($"Note not found for Pin/Unpin operation for {NoteID} NoteID");
                    return BadRequest(new { success = false, message = "Operation Unsuccessful." });

                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in NotePin API");
                throw;
            }
        }


        /// <summary>
        /// This method is used to change the state of the Archive feature.
        /// </summary>
        /// <param name="NoteID"></param>
        /// <returns></returns>
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
                    logger.LogInformation($"Note Archive Successfully for {NoteID} NoteID");
                    return Ok(new { success = true, message = "Note Archived successfully" });
                }
                else if (result == false)
                {
                    logger.LogInformation($"Note Unarchive Successfully for {NoteID} NoteID");
                    return Ok(new { success = true, message = "Note UnArchived successfully." });
                }
                else
                {
                    logger.LogWarning($"Notes not found  for Archive/Unarchive Operation for {NoteID} NoteID");
                    return BadRequest(new { success = false, message = "Operation Fail." });

                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in NotesArchive API");
                throw;
            }

        }


        /// <summary>
        /// This method is used to change the state of the Trash feature.
        /// </summary>
        /// <param name="NoteID"></param>
        /// <returns></returns>
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
                    logger.LogInformation($"Note trashed Successfully for {NoteID} NoteID");
                    return Ok(new { success = true, message = "Note Trashed successfully" });
                }
                else if (result == false)
                {
                    logger.LogInformation($"Note Untrashed Successfully for {NoteID} NoteID");
                    return Ok(new { success = true, message = "Note Untrashed successfully." });
                }
                else
                {
                    logger.LogWarning($"Note not found for Trashed/Untrashed operation for {NoteID} NoteID");
                    return BadRequest(new { success = false, message = "Operation Fail." });

                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception Occured in NoteTrash API");
                throw;
            }

        }


        /// <summary>
        /// This method is used to change the color of the note.
        /// </summary>
        /// <param name="NoteID"></param>
        /// <param name="color"></param>
        /// <returns></returns>
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
                    logger.LogInformation($"Note Color changed to {color} successfully for {NoteID} NoteID");
                    return Ok(new { success = true, message = "Color changed successfully" });
                }
                else
                {
                    logger.LogWarning($"Note not found to change the color for {NoteID} NoteID");
                    return BadRequest(new { success = false, message = "Color not changed." });
                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in Note Color API");
                throw;
            }

        }


        /// <summary>
        /// This method is used to upload the image to the cloud.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="NoteID"></param>
        /// <returns></returns>
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
                    logger.LogInformation($"Image Upload succesffuly to Cloudinary for {NoteID} NoteID");
                    return Ok(new { success = true, message = result });
                }
                else
                {
                    logger.LogWarning($"Note not found  to upload image to Cloudinary for {NoteID} NoteID");
                    return BadRequest(new { success = false, message = "Cannot upload image." });
                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in NoteImage API");
                throw;
            }
        }



        /// <summary>
        /// This method is used to store the data in cache, so that user can get data much faster.
        /// </summary>
        /// <returns></returns>
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);

            var cacheKey = "NotesList";
            string serializedNotesList;
            var NotesList = new List<NotesEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNotesList);
            }
            else
            {


                NotesList = fundooContext.NotesTable.ToList();
                serializedNotesList = JsonConvert.SerializeObject(NotesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(NotesList);
        }


    }
}
