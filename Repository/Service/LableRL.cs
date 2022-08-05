using CommonLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class LableRL : ILableRL
    {

        FundooContext fundooContext;

        public LableRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }




        public LableResponseModel CreateLable(long notesId, long jwtUserId, LableModel model)
        {
            try
            {
                var validNotesAndUser = this.fundooContext.UserTable.Where(e => e.UserId == jwtUserId);

                if(validNotesAndUser != null)
                {
                    LableEntity lable = new LableEntity();

                    lable.noteID = notesId;
                    lable.UserId = jwtUserId;
                    lable.LabelName = model.LabelName;

                    this.fundooContext.Add(lable);
                    this.fundooContext.SaveChanges();

                    LableResponseModel responseModel = new LableResponseModel();

                    responseModel.LabelID = lable.LabelID;
                    responseModel.NoteID = lable.noteID;
                    responseModel.UserID = lable.UserId;
                    responseModel.LabelName = lable.LabelName;

                    return responseModel;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public IEnumerable<LableEntity> GetAllLable(long jwtUserId)
        {
            try
            {

                var result = this.fundooContext.lableTable.Where(x => x.UserId == jwtUserId);
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public LableResponseModel GetLableWithId(long lableId, long jwtUserId)
        {
            try
            {
                var validUserId = this.fundooContext.UserTable.Where(e => e.UserId == jwtUserId);

                var response = this.fundooContext.lableTable.FirstOrDefault(e => e.LabelID == lableId && e.UserId == jwtUserId);

                if (validUserId != null && response != null)
                {
                    

                    LableResponseModel model = new LableResponseModel();

                    model.LabelID = response.LabelID;
                    model.NoteID = response.noteID;
                    model.UserID = response.UserId;
                    model.LabelName = response.LabelName;
                
                    return model;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public LableEntity GetLablesWithId(long lableId, long jwtUserId)
        {
            try
            {
                var validUserId = this.fundooContext.UserTable.Where(e => e.UserId == jwtUserId);
                if (validUserId != null)
                {
                    return this.fundooContext.lableTable.FirstOrDefault(e => e.LabelID == lableId);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public LableResponseModel UpdateLable(LableEntity updateLable, UpdateLableModel model, long jwtUserId)
        {
            try
            {
                var validUserId = this.fundooContext.UserTable.Where(e => e.UserId == jwtUserId);

                var response = this.fundooContext.lableTable.FirstOrDefault(e => e.LabelID == updateLable.LabelID);

                if (validUserId != null && response != null)
                {
                    updateLable.LabelName = model.LabelName;
                    updateLable.noteID = model.NoteID;
                   
                    this.fundooContext.SaveChanges();

                    
                    LableResponseModel models = new LableResponseModel();

                    models.LabelID = response.LabelID;
                    models.NoteID = response.noteID;
                    models.UserID = response.UserId;
                    models.LabelName = response.LabelName;

                    return models;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public void DeleteLable(LableEntity lable, long jwtUserId)
        {
            try
            {
                var validUserId = this.fundooContext.UserTable.Where(e => e.UserId == jwtUserId);
                if (validUserId != null)
                {
                    this.fundooContext.lableTable.Remove(lable);
                    this.fundooContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
