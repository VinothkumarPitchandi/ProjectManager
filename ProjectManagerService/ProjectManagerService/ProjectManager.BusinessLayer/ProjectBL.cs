using ProjectManager.DataAccessLayer;
using ProjectManager.InterfaceLayer;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ProjectManager.BusinessLayer
{
    public class ProjectBL : IProjectBL
    {
        private readonly ProjectManagerEntities _projectManager;

        public ProjectBL()
        {
            _projectManager = new ProjectManagerEntities();
        }
        public ProjectBL(ProjectManagerEntities projectManager)
        {
            _projectManager = projectManager;
        }
        public Collection<CommonEntities.Projects> GetProjects()
        {

            Collection<CommonEntities.Projects> projCollection = new Collection<CommonEntities.Projects>();

            _projectManager.Projects.SelectMany
            (
                proj => _projectManager.Users.Where(user => proj.Project_ID == user.Project_ID).DefaultIfEmpty(),
                (x, y) => new
                {
                    Projects = x,
                    Users = y
                }
            ).ToList()
                .ForEach(y => projCollection.Add(
                    new CommonEntities.Projects
                    {
                        ProjectID = y.Projects.Project_ID,
                        Project = y.Projects.Project1,
                        StartDate = y.Projects.Start_Date,
                        EndDate = y.Projects.End_Date,
                        Priority = y.Projects.Priority??0,
                        NoofTasks = _projectManager.Tasks.Where(x => x.Project_ID == y.Projects.Project_ID).Count(),
                        NoofCompletedTasks = _projectManager.Tasks.Where(x => x.Project_ID == y.Projects.Project_ID && x.Status == true).Count(),
                        ManagerID = y.Users != null ? y.Users.User_ID : 0,
                        ManagerName = y.Users != null ? y.Users.FirstName + " " + y.Users.LastName : ""
                    }
                    ));

            return projCollection;
        }

        public void AddProject(CommonEntities.Projects project)
        {
            Project proj = new Project
            {
                Project1 = project.Project,
                Start_Date = project.StartDate??DateTime.Now,
                End_Date = project.EndDate ?? DateTime.Now,
                Priority = project.Priority
            };

            _projectManager.Projects.Add(proj);
            _projectManager.SaveChanges();
            var proId = proj.Project_ID;
            var ur = _projectManager.Users.Where(x => x.User_ID == project.ManagerID).FirstOrDefault();
            if (ur != null)
            {
                ur.Project_ID = proId;
                _projectManager.SaveChanges();
            }
        }

        public void UpdateProject(CommonEntities.Projects project)
        {
            var proj = _projectManager.Projects.Where(x => x.Project_ID == project.ProjectID).FirstOrDefault();
            var user = _projectManager.Users.Where(x => x.User_ID == project.ManagerID).FirstOrDefault();
            var extUser = _projectManager.Users.Where(x => x.Project_ID == project.ProjectID).FirstOrDefault();

            if (proj != null && user != null)
            {
                proj.Project1 = project.Project;
                proj.Start_Date = project.StartDate ?? DateTime.Now;
                proj.End_Date = project.EndDate ?? DateTime.Now;
                proj.Priority = project.Priority;
                if (extUser != null)
                {
                    extUser.Project_ID = null;
                }
                user.Project_ID = project.ProjectID;
                _projectManager.SaveChanges();
            }
        }

        public void SuspendProject(int projectID)
        {
            Project proj = new Project
            {
                Project_ID = projectID
            };
            //var friends = db.Friends.Where(f => idList.Contains(f.ID)).ToList();
            //friends.ForEach(a => a.msgSentBy = '1234');
            //db.SaveChanges();
            var user = _projectManager.Users.Where(x => x.Project_ID == projectID).ToList();
            if (user.Count>0)
            {
                user.ForEach(a => a.Project_ID = null);
                
            }
            var task = _projectManager.Tasks.Where(x => x.Project_ID == projectID).ToList();
            if (task.Count > 0)
            {
                task.ForEach(a => a.Project_ID = null);
            }
            _projectManager.Entry(proj).State = EntityState.Deleted;
            _projectManager.SaveChanges();
        }
    }
}
