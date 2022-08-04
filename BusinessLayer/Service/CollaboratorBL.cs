using BusinessLayer.Interface;
using CommonLayer.Model;
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
    }
}
