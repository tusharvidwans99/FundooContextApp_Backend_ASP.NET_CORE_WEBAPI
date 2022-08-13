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
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {

        ILableBL ilableBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<LabelController> logger;

        private readonly FundooContext fundooContext;

        public LabelController(ILableBL ilableBL, IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext, ILogger<LabelController> logger)
        {
            this.ilableBL = ilableBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundooContext = fundooContext;
            this.logger = logger;
        }


        /// <summary>
        /// This method is used to create lable based on noteID.
        /// </summary>
        /// <param name="notesId"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("Create")]    
        public IActionResult CreateLable(long notesId, LableModel model)
        {

            try
            {


                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

                if (jwtUserId == 0 && notesId == 0)
                {
                    logger.LogWarning($"Lable not created for {notesId} noteID");
                    return BadRequest(new { Success = false, message = "Lable not created" });
                }
                else
                {

                    LableResponseModel lable = ilableBL.CreateLable(notesId, jwtUserId, model);

                    logger.LogInformation($"Lable creatred for {notesId} NotesID");
                    return Ok(new { Success = true, message = "Lable Created", lable });

                }


            }
            catch (Exception)
            {
                logger.LogError("Exception occured in CreateLable API");
                throw;
            }

            
        }


        /// <summary>
        /// This method will show all the lables which is available/created by the authorize user.
        /// </summary>
        /// <returns></returns>
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
                    logger.LogInformation("Read all Lables");

                    return Ok(new { Success = true, message = "Retrived All lables ", result });

                }
                else
                {
                    logger.LogWarning("No Lable Found");
                    return BadRequest(new { Success = false, message = "No lable in database " });

                }

            }
            catch (Exception)
            {
                logger.LogError("Exception occured in ReadAllLable API");
                throw;
            }

        }


        /// <summary>
        /// This method will show the lable name base on the lableId.
        /// </summary>
        /// <param name="lableId"></param>
        /// <returns></returns>
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
                    logger.LogInformation($"Get Lable for {lableId} LableID");
                    return Ok(new { Success = true, message = "Retrived Lable ", result });

                }
                else
                {
                    logger.LogWarning($"Lable not found for {lableId} LableID");
                    return NotFound(new { Success = false, message = "No Lable found with particular LableId " });

                }

            }
            catch (Exception)
            {
                logger.LogError("Exception occured in GetLableByID API");
                throw;
            }

        }


        /// <summary>
        /// This method will change the name of the lable.
        /// </summary>
        /// <param name="lableId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
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

                    logger.LogInformation($"Lable updated successfully for {lableId} LableID");
                    return Ok(new { Success = true, message = "Lable Updated Sucessfully", lable });

                }
                else
                {
                    logger.LogWarning($"No Lable found for {lableId} LableID");
                    return BadRequest(new { Success = false, message = "No Lable found with LableId" });

                }



            }
            catch (Exception)
            {
                logger.LogError("Exception occured in UpdateLable API");
                throw;
            }

        }


        /// <summary>
        /// This method will delete the lable based on the lableID
        /// </summary>
        /// <param name="lableId"></param>
        /// <returns></returns>
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
                    logger.LogInformation($"Lable removed for {lableId} lableID");
                    return Ok(new { Success = true, message = "Lable Removed" });

                }
                else
                {
                    logger.LogWarning("Lable not found");
                    return BadRequest(new { Success = false, message = "No Lable Found" });

                }

            }
            catch (Exception)
            {
                logger.LogError("Exception occured in DeleteLable API");
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

            var cacheKey = "LableList";
            string serializedLableList;
            var LableList = new List<LableEntity>();
            var redisLableList = await distributedCache.GetAsync(cacheKey);
            if (redisLableList != null)
            {
                serializedLableList = Encoding.UTF8.GetString(redisLableList);
                LableList = JsonConvert.DeserializeObject<List<LableEntity>>(serializedLableList);
            }
            else
            {


                LableList = fundooContext.lableTable.ToList();
                serializedLableList = JsonConvert.SerializeObject(LableList);
                redisLableList = Encoding.UTF8.GetBytes(serializedLableList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisLableList, options);
            }
            return Ok(LableList);
        }

    }
}
