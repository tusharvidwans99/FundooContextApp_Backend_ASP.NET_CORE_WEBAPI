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
    public class LabelController : ControllerBase
    {

        ILableBL ilableBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        private readonly FundooContext fundooContext;

        public LabelController(ILableBL ilableBL, IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext)
        {
            this.ilableBL = ilableBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundooContext = fundooContext;
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
