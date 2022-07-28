using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {

        public UserEntity Registration(UserRegistrationModel userRegistrationModel);

        public string Login(UserLogin userlogin);

    }
}
