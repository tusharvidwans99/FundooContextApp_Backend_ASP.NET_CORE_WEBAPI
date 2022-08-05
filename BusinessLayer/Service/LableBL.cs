using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class LableBL : ILableBL
    {

        ILableRL ilableRL;

        public LableBL(ILableRL ilableRL)
        {
            this.ilableRL = ilableRL;
        }

       

        public LableResponseModel CreateLable(long notesId, long jwtUserId, LableModel model)
        {

            try
            {

                return ilableRL.CreateLable(notesId, jwtUserId, model);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DeleteLable(LableEntity lable, long jwtUserId)
        {

            try
            {
                ilableRL.DeleteLable(lable, jwtUserId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<LableEntity> GetAllLable(long jwtUserId)
        {

            try
            {

                return ilableRL.GetAllLable(jwtUserId);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public LableEntity GetLablesWithId(long lableId, long jwtUserId)
        {

            try
            {

                return ilableRL.GetLablesWithId(lableId, jwtUserId);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public LableResponseModel GetLableWithId(long lableId, long jwtUserId)
        {

            try
            {

                return ilableRL.GetLableWithId(lableId, jwtUserId);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public LableResponseModel UpdateLable(LableEntity updateLable, UpdateLableModel model, long jwtUserId)
        {

            try
            {

                return ilableRL.UpdateLable(updateLable, model, jwtUserId);

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
