using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {

        public UserEntity Registration(UserRegistrationModel userRegistrationModel);

        public string Login(UserLogin userlogin);


    }
}
