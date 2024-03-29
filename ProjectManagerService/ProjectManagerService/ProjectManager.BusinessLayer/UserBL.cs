﻿using ProjectManager.DataAccessLayer;
using ProjectManager.InterfaceLayer;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ProjectManager.BusinessLayer
{
    public class UserBL : IUserBL
    {
        private readonly ProjectManagerEntities _projectManager;

        public UserBL()
        {
            _projectManager = new ProjectManagerEntities();
        }

        public UserBL(ProjectManagerEntities projectManager)
        {
            _projectManager = projectManager;
        }

        public Collection<CommonEntities.Users> GetUsers()
        {

            Collection<CommonEntities.Users> userCollection = new Collection<CommonEntities.Users>();
            _projectManager.Users
                .Select(u => new CommonEntities.Users()
                {
                    UserID = u.User_ID,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    EmployeeID = u.EmployeeID
                }).ToList()
               .ForEach(y => userCollection.Add(y));

            return userCollection;
        }

        public void AddUser(CommonEntities.Users user)
        {
            User ur = new User
            {
                User_ID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmployeeID = user.EmployeeID
            };

            _projectManager.Users.Add(ur);
            _projectManager.SaveChanges();
        }

        public void UpdateUser(CommonEntities.Users user)
        {
            var ur = _projectManager.Users.Where(x => x.User_ID == user.UserID).FirstOrDefault();
            if (ur != null)
            {
                ur.User_ID = user.UserID;
                ur.FirstName = user.FirstName;
                ur.LastName = user.LastName;
                ur.EmployeeID = user.EmployeeID;
                _projectManager.SaveChanges();
            }
        }

        public void DeleteUser(CommonEntities.Users user)
        {
            User ur = new User
            {
                User_ID = user.UserID
            };
            _projectManager.Entry(ur).State = EntityState.Deleted;
            _projectManager.SaveChanges();
        }
    }
}
