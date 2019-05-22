using NSubstitute;
using ProjectManager.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagerService.Tests.UnitTest
{
    public class MockProjectManager
    {
        public ProjectManagerEntities MockDataSetList()
        {
            var dataProjects = new List<Project>()
            {
                new Project
                {
                    Project_ID=1,
                    Project1="Project 1",
                    Start_Date=DateTime.Now.Date,
                    End_Date=DateTime.Now.Date.AddDays(1)
                },
                new Project
                {
                    Project_ID=2,
                    Project1="Project 2",
                    Start_Date=DateTime.Now.Date,
                    End_Date=DateTime.Now.Date.AddDays(1)
                },
                new Project
                {
                    Project_ID=3,
                    Project1="Project 3",
                    Start_Date=DateTime.Now.Date,
                    End_Date=DateTime.Now.Date.AddDays(1)
                }
        }.AsQueryable();

            IDbSet<Project> mocksetProjects = Substitute.For<IDbSet<Project>>();
            mocksetProjects.Provider.Returns(dataProjects.Provider);
            mocksetProjects.Expression.Returns(dataProjects.Expression);
            mocksetProjects.ElementType.Returns(dataProjects.ElementType);
            mocksetProjects.GetEnumerator().Returns(dataProjects.GetEnumerator());

            var dataUsers = new List<User>()
            {
                new User
                {
                    User_ID=1,
                    Project_ID=1,
                    FirstName="Vinoth",
                    LastName="Kumar",
                    EmployeeID="458513"
                },
                new User
                {
                    User_ID=2,
                    Project_ID=1,
                    FirstName="Kumar",
                    LastName="Vinoth",
                    EmployeeID="315854"
                },
                new User
                {       
                    User_ID =3,
                    Project_ID =2,
                    FirstName="Vinothkumar",
                    LastName="Pitcahndi",
                    EmployeeID="345678"
                }
        }.AsQueryable();

            IDbSet<User> mocksetUsers = Substitute.For<IDbSet<User>>();
            mocksetUsers.Provider.Returns(dataUsers.Provider);
            mocksetUsers.Expression.Returns(dataUsers.Expression);
            mocksetUsers.ElementType.Returns(dataUsers.ElementType);
            mocksetUsers.GetEnumerator().Returns(dataUsers.GetEnumerator());

            var dataTasks = new List<ProjectManager.DataAccessLayer.Task>()
            {
                new ProjectManager.DataAccessLayer.Task
                {
                    Task_ID=1,
                    Task1="Task 1",
                    Project_ID=1,
                    Priority=10,
                    Start_Date=DateTime.Now.Date,
                    End_Date=DateTime.Now.Date.AddDays(1)
                },
                new ProjectManager.DataAccessLayer.Task
                {
                    Task_ID=2,
                    Task1="Task 2",
                    Project_ID=1,
                    Priority=20,
                    Start_Date=DateTime.Now.Date,
                    End_Date=DateTime.Now.Date.AddDays(1),
                    Status=true
                },
                new ProjectManager.DataAccessLayer.Task
                {
                   Task_ID=3,
                    Task1="Task 3",
                    Project_ID=2,
                    Priority=10,
                    Start_Date=DateTime.Now.Date,
                    End_Date=DateTime.Now.Date.AddDays(1)
                },
                new ProjectManager.DataAccessLayer.Task
                {
                   Task_ID=4,
                    Task1="Task 4",
                    Project_ID=2,
                    Priority=20,
                    Start_Date=DateTime.Now.Date,
                    End_Date=DateTime.Now.Date.AddDays(1),
                    Status=true
                }
        }.AsQueryable();

            IDbSet<ProjectManager.DataAccessLayer.Task> mocksetTasks = Substitute.For<IDbSet<ProjectManager.DataAccessLayer.Task>>();
            mocksetTasks.Provider.Returns(dataTasks.Provider);
            mocksetTasks.Expression.Returns(dataTasks.Expression);
            mocksetTasks.ElementType.Returns(dataTasks.ElementType);
            mocksetTasks.GetEnumerator().Returns(dataTasks.GetEnumerator());

            var dataPTasks = new List<ParentTask>()
            {
                new ParentTask
                {
                    Parent_ID=1,
                    Parent_Task="Parent Task 1"
                },
                new ParentTask
                {
                    Parent_ID=2,
                    Parent_Task="Parent Task 2"
                }
        }.AsQueryable();

            IDbSet<ParentTask> mocksetPTasks = Substitute.For<IDbSet<ParentTask>>();
            mocksetPTasks.Provider.Returns(dataPTasks.Provider);
            mocksetPTasks.Expression.Returns(dataPTasks.Expression);
            mocksetPTasks.ElementType.Returns(dataPTasks.ElementType);
            mocksetPTasks.GetEnumerator().Returns(dataPTasks.GetEnumerator());

            ProjectManagerEntities mockContext = Substitute.For<ProjectManagerEntities>();
            mockContext.Projects.Returns(mocksetProjects);
            mockContext.Users.Returns(mocksetUsers);
            mockContext.Tasks.Returns(mocksetTasks);
            mockContext.ParentTasks.Returns(mocksetPTasks);

            return mockContext;
        }

    }
}
