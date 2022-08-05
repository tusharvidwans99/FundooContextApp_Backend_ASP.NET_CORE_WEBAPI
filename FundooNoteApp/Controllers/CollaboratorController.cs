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

        private readonly FundooContext fundooContext;

        public CollaboratorController(ICollaboratorBL icollaboratorBL, IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext)
        {
            this.icollaboratorBL = icollaboratorBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundooContext = fundooContext;
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
