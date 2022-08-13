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
    public class CollaboratorController : ControllerBase
    {

        private readonly ICollaboratorBL icollaboratorBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        private readonly ILogger<CollaboratorController> logger;

        private readonly FundooContext fundooContext;

        public CollaboratorController(ICollaboratorBL icollaboratorBL, IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext, ILogger<CollaboratorController> logger)
        {
            this.icollaboratorBL = icollaboratorBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundooContext = fundooContext;
            this.logger = logger;
        }



        /// <summary>
        /// This method is used to add the other authorize people emailID for sharing notes.
        /// </summary>
        /// <param name="notesId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public IActionResult AddCollaborate(long notesId, CollaboratedModel model)
        {

            try
            {

                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

                if (jwtUserId == 0 && notesId == 0)
                {
                    logger.LogWarning("Email Missing for Collaboration");
                    return BadRequest(new { Success = false, message = "Email Missing For Collaboration" });
                }
                else
                {
                    CollabResponseModel collaborate = icollaboratorBL.AddCollaborate(notesId, jwtUserId, model);
                    logger.LogInformation($"Collaboration Succesfull {notesId}");
                    return Ok(new { Success = true, message = "Collaboration Successfull ", collaborate });

                }

            }
            catch (Exception)
            {
                logger.LogError("Exception occured in AddCollaborator API");
                throw;
            }


        }



        /// <summary>
        /// This method is used to delete the added people's email id in the collaborator.
        /// </summary>
        /// <param name="collabId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteCollaborate(long collabId)
        {
            try
            {


                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

                CollaboratorEntity collabid = icollaboratorBL.GetCollabWithId(collabId);

                if (collabid == null)
                {
                    logger.LogWarning($"No Email found for {collabId} collabID");

                    return BadRequest(new { Success = false, message = "No Collaboration Found" });
                }
                else
                {

                    icollaboratorBL.DeleteCollab(collabid);
                    logger.LogInformation($"Collaborated email removed for {collabId} collabID");
                    return Ok(new { Success = true, message = "Collaborated Email Removed" });

                }



            }
            catch (Exception)
            {
                logger.LogError("Exception occured in DeleteCollaborate API");
                throw;
            }
}



        /// <summary>
        /// This method is used to get all selected emailID.
        /// </summary>
        /// <returns></returns>
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
                    logger.LogInformation("All collaborator notes");
                    return Ok(new { success = true, message = "All collaborator notes.", data = result });
                }
                else
                {
                    logger.LogWarning("Cannot get all collaborator notes");
                    return BadRequest(new { success = false, message = "Cannot get collaborator notes." });
                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in GETcollab API");
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

            var cacheKey = "CollabList";
            string serializedCollabList;
            var CollabList = new List<CollaboratorEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedCollabList = Encoding.UTF8.GetString(redisNotesList);
                CollabList = JsonConvert.DeserializeObject<List<CollaboratorEntity>>(serializedCollabList);
            }
            else
            {


                CollabList = fundooContext.CollaboratorTable.ToList();
                serializedCollabList = JsonConvert.SerializeObject(CollabList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedCollabList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(CollabList);
        }

    }
}
