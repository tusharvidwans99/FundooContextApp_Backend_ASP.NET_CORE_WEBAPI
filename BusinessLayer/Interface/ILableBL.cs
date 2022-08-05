using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ILableBL
    {

        public LableResponseModel CreateLable(long notesId, long jwtUserId, LableModel model);
        public IEnumerable<LableEntity> GetAllLable(long jwtUserId);
        public LableResponseModel GetLableWithId(long lableId, long jwtUserId);
        public LableEntity GetLablesWithId(long lableId, long jwtUserId);
        public LableResponseModel UpdateLable(LableEntity updateLable, UpdateLableModel model, long jwtUserId);
        public void DeleteLable(LableEntity lable, long jwtUserId);

    }
}
