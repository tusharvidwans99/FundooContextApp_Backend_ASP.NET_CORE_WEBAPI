using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollaboratorRL
    {

        public CollabResponseModel AddCollaborate(long notesId, long jwtUserId, CollaboratedModel model);

    }
}
