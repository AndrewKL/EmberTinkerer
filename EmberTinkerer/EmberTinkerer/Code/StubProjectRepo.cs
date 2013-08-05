using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmberTinkerer.Models;

namespace EmberTinkerer.Code
{
    public class StubProjectRepo
    {
        public Project Get(int id)
        {
            return Projects.Single(x => x.id == id);
        }

        public IEnumerable<Project> GetAll()
        {
            return Projects;
        } 
        
        public void Add(Project project)
        {
            project.id = Projects.Count + 1;
            Projects.Add(project);
        }
        public void Update(Project project)
        {
            Projects.Remove(Projects.Single(x => x.id == project.id));
            Projects.Add(project);
        }
        public IEnumerable<Project> SearchByName(string text)
        {
            return Projects.Where(x => x.name.Contains(text));
        } 

        public static List<Project> Projects = new List<Project>()
            {
                new Project()
                {
                    id =1,
                    description = "test description",
                    name = "hello world1",
                    html = "<h1>hello world</h1>",
                    javascript = "alert('hello world');"
                },
                new Project()
                {
                    id =2,
                    description = "another test description",
                    name = "MEMEMEMEMEMEMEMEME project",
                    html = "<h1>hello world</h1>",
                    javascript = "alert('hello world');"
                },
                new Project()
                {
                    id =3,
                    description = "test description",
                    name = "RAWR",
                    html = "<h1>hello world</h1>",
                    javascript = "alert('hello world');"
                },
                new Project()
                {
                    id =4,
                    description = "a simple application",
                    name = "hello world3",
                    html = "<h1>hello world</h1>",
                    javascript = "alert('hello world');"
                },
                new Project()
                {
                    id =5,
                    description = "test description",
                    name = "hello world4",
                    html = "<h1>hello world</h1>",
                    javascript = "alert('hello world');"
                },
                new Project()
                {
                    id =6,
                    description = "test description",
                    name = "hello worl5d",
                    html = "<h1>hello world</h1>",
                    javascript = "alert('hello world');"
                },
                new Project()
                {
                    id =7,
                    description = "test description",
                    name = "hello world6",
                    html = "<h1>hello world</h1>",
                    javascript = "alert('hello world');"
                },
                new Project()
                {
                    id =8,
                    description = "test description",
                    name = "hello world7",
                    html = "<h1>hello world</h1>",
                    javascript = "alert('hello world');"
                },

            };
    }
}