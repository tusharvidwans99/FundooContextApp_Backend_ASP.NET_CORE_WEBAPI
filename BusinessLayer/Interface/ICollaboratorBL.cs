using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ICollaboratorBL
    {

        public CollabResponseModel AddCollaborate(long notesId, long jwtUserId, CollaboratedModel model);

    }
}
