using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollaboratorRL
    {

        public CollabResponseModel AddCollaborate(long notesId, long jwtUserId, CollaboratedModel model);

        public void DeleteCollab(CollaboratorEntity collab);
        public CollaboratorEntity GetCollabWithId(long collabId);

        public IEnumerable<CollaboratorEntity> GetCollab(long userID);

    }
}
