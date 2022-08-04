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
    public class CollaboratorRL : ICollaboratorRL
    {

        private readonly FundooContext fundooContext;

        public CollaboratorRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }


        public CollabResponseModel AddCollaborate(long notesId, long jwtUserId, CollaboratedModel model)
        {
            try
            {
                var validNotesAndUser = this.fundooContext.UserTable.Where(e => e.UserId == jwtUserId);

                if (validNotesAndUser != null)
                {
                    CollaboratorEntity collaborate = new CollaboratorEntity();

                    collaborate.noteID = notesId;
                    collaborate.UserId = jwtUserId;
                    collaborate.CollaboratedEmail = model.Collaborated_Email;

                    fundooContext.Add(collaborate);
                    fundooContext.SaveChanges();

                    CollabResponseModel responseModel = new CollabResponseModel();

                    responseModel.CollaboratorID = collaborate.CollaboratorID;
                    responseModel.noteID = collaborate.noteID;
                    responseModel.UserId = collaborate.UserId;
                    responseModel.CollaboratedEmail = collaborate.CollaboratedEmail;

                    return responseModel;
                }
                else
                {
                    return null;
                }

                
            }
            catch (Exception)
            {
                throw;
            }
        }



        public void DeleteCollab(CollaboratorEntity collab)
        {
            try
            {

                this.fundooContext.CollaboratorTable.Remove(collab);
                this.fundooContext.SaveChanges();
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public CollaboratorEntity GetCollabWithId(long collabId)
        {
            try
            {
                var result = this.fundooContext.CollaboratorTable.FirstOrDefault(e => e.CollaboratorID == collabId);

                if (result != null)
                {
                    return result;
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


        public IEnumerable<CollaboratorEntity> GetCollab(long userID)
        {
            try
            {
                var result = fundooContext.CollaboratorTable.Where(x => x.UserId == userID);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
