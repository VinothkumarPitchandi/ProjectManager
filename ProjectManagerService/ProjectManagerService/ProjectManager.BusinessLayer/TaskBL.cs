using ProjectManager.DataAccessLayer;
using ProjectManager.InterfaceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.BusinessLayer
{
    public class TaskBL : ITaskBL
    {
        private readonly ProjectManagerEntities _projectManager;

        public TaskBL()
        {
            _projectManager = new ProjectManagerEntities();
        }

        public TaskBL(ProjectManagerEntities projectManager)
        {
            _projectManager = projectManager;
        }

        public Collection<CommonEntities.ParentTasks> GetParentTasks()
        {

            Collection<CommonEntities.ParentTasks> taskCollection = new Collection<CommonEntities.ParentTasks>();
            _projectManager.ParentTasks
                .Select(u => new CommonEntities.ParentTasks()
                {
                    ParentTaskID = u.Parent_ID,
                    ParentTask = u.Parent_Task
                }).ToList()
               .ForEach(y => taskCollection.Add(y));

            return taskCollection;
        }

        public Collection<CommonEntities.Tasks> GetTasks(int projectID)
        {

            Collection<CommonEntities.Tasks> taskCollection = new Collection<CommonEntities.Tasks>();

            _projectManager.Tasks.Where(c => c.Parent_ID == null && c.Project_ID == projectID)
                .Select(s => new CommonEntities.Tasks
                {
                    Task = s.Task1,
                    ProjectID = s.Project_ID,
                    Project = _projectManager.Projects.Where(u => u.Project_ID == s.Project_ID)
                    .Select(u => u.Project1).FirstOrDefault(),
                    ParentTask = "",
                    Priority = s.Priority??0,
                    StartDate = s.Start_Date,
                    EndDate = s.End_Date,
                    TaskID = s.Task_ID,
                    Status = s.Status,
                    UserID = _projectManager.Users.Where(u => u.Task_ID == s.Task_ID)
                    .Select(u => u.User_ID).FirstOrDefault(),
                    UserName = _projectManager.Users.Where(u => u.Task_ID == s.Task_ID)
                    .Select(u => u.FirstName + " " + u.LastName).FirstOrDefault()
                }).ToList()
                    .ForEach(x => taskCollection.Add(x));

            _projectManager.Tasks.Where(c => c.Parent_ID != null && c.Project_ID == projectID)
                .Join(_projectManager.ParentTasks, f => f.Parent_ID, s => s.Parent_ID,
                (f, s) => new CommonEntities.Tasks
                {
                    Task = f.Task1,
                    ProjectID = f.Project_ID,
                    Project = _projectManager.Projects.Where(u => u.Project_ID == f.Project_ID)
                    .Select(u => u.Project1).FirstOrDefault(),
                    ParentTask = s.Parent_Task,
                    Priority = f.Priority??0,
                    StartDate = f.Start_Date,
                    EndDate = f.End_Date,
                    ParentTaskID = s.Parent_ID,
                    TaskID = f.Task_ID,
                    Status = f.Status,
                    UserID = _projectManager.Users.Where(u => u.Task_ID == f.Task_ID)
                    .Select(u => u.User_ID).FirstOrDefault(),
                    UserName = _projectManager.Users.Where(u => u.Task_ID == f.Task_ID)
                    .Select(u => u.FirstName + " " + u.LastName).FirstOrDefault()
                }).ToList()
                 .ForEach(x => taskCollection.Add(x));

            return taskCollection;
        }


        public void AddTask(CommonEntities.Tasks task)
        {
           
           DataAccessLayer.Task tk = new DataAccessLayer.Task
           {
                Task1 = task.Task,
                Project_ID = task.ProjectID,
                Priority = task.Priority,
                Start_Date = task.StartDate,
                End_Date = task.EndDate,
                Status = false
            };
            if (task.ParentTaskID == 0)
            {
                tk.Parent_ID = null;
            }
            else
            {
                tk.Parent_ID = task.ParentTaskID;
            }


            _projectManager.Tasks.Add(tk);
            _projectManager.SaveChanges();
            var taskId = tk.Task_ID;
            var ur = _projectManager.Users.Where(x => x.User_ID == task.UserID).FirstOrDefault();
            if (ur != null)
            {
                ur.Task_ID = taskId;
                _projectManager.SaveChanges();
            }
        }

        public void UpdateTask(CommonEntities.Tasks task)
        {
            var tk = _projectManager.Tasks.Where(x => x.Task_ID == task.TaskID).FirstOrDefault();

            if (tk != null)
            {
                tk.Task1 = task.Task;
                tk.Project_ID = task.ProjectID;
                tk.Priority = task.Priority;
                tk.Start_Date = task.StartDate;
                tk.End_Date = task.EndDate;
                if (task.ParentTaskID == 0)
                {
                    tk.Parent_ID = null;
                }
                else
                {
                    tk.Parent_ID = task.ParentTaskID;
                }

                _projectManager.SaveChanges();
                var ur = _projectManager.Users.Where(x => x.User_ID == task.UserID).FirstOrDefault();
                if (ur != null)
                {
                    ur.Task_ID = tk.Task_ID;
                    _projectManager.SaveChanges();
                }
            }
        }

        public void EndTask(CommonEntities.Tasks task)
        {
            var tk = _projectManager.Tasks.Where(x => x.Task_ID == task.TaskID).FirstOrDefault();

            if (tk != null)
            {
                tk.Status = true;
                _projectManager.SaveChanges();
            }
        }

        public void AddParentTask(CommonEntities.ParentTasks pTask)
        {
            ParentTask tk = new ParentTask
            {
                Parent_Task = pTask.ParentTask
            };

            _projectManager.ParentTasks.Add(tk);
            _projectManager.SaveChanges();
        }
    }
}
