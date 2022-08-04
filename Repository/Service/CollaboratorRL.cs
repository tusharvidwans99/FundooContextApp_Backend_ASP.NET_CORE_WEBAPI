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
            catch (Exception)
            {
                throw;
            }
        }


    }
}
