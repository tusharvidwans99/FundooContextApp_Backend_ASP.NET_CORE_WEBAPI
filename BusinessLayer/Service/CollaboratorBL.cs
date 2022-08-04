using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class CollaboratorBL : ICollaboratorBL
    {


        public ICollaboratorRL icollaboratorRL;
        
        public CollaboratorBL(ICollaboratorRL icollaboratorRL)
        {
            this.icollaboratorRL = icollaboratorRL;
        }


        public CollabResponseModel AddCollaborate(long notesId, long jwtUserId, CollaboratedModel model)
        {

            try
            {
                return icollaboratorRL.AddCollaborate(notesId, jwtUserId, model);
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

                icollaboratorRL.DeleteCollab(collab);
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public CollaboratorEntity GetCollabWithId(long collabId)
        {

            try
            {
               return icollaboratorRL.GetCollabWithId(collabId);
            }
            catch (Exception)
            {

                throw;
            }

        }



        public IEnumerable<CollaboratorEntity> GetCollab(long userID)
        {
            try
            {
                return icollaboratorRL.GetCollab(userID);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
