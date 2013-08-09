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

        public Project Add(Project project)
        {
            project.id = Projects.Count + 1;
            Projects.Add(project);
            return project;
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
                    name = "boot strap example",
                    html = @"<div class='navbar navbar-inverse navbar-fixed-top bs-docs-nav'>
  <div class='container'>
    <button class='navbar-toggle' type='button' data-toggle='collapse' data-target='.bs-navbar-collapse'>
      <span class='icon-bar'></span>
      <span class='icon-bar'></span>
      <span class='icon-bar'></span>
    </button>
    <a href='../' class='navbar-brand'>Bootstrap 3 RC1</a>
    <div class='nav-collapse collapse bs-navbar-collapse'>
      <ul class='nav navbar-nav'>
        <li class='active'>
          <a href='../getting-started'>Getting started</a>
        </li>
        <li>
          <a href='../css'>CSS</a>
        </li>
        <li>
          <a href='../components'>Components</a>
        </li>
        <li>
          <a href='../javascript'>JavaScript</a>
        </li>
        <li>
          <a href='../customize'>Customize</a>
        </li>
      </ul>
    </div>
  </div>
</div>",
                    javascript = "alert('hello world');"
                },
                new Project()
                {
                    id =8,
                    description = "test description",
                    name = "Blink Example",
                    html = @"<h1><blink>Blinking Text</blink></h1>",
                    javascript = @"alert('javascript test');

 function blink() {
    var blinks = document.getElementsByTagName('blink');
    for (var i = blinks.length - 1; i >= 0; i--) {
      var s = blinks[i];
      s.style.visibility = (s.style.visibility === 'visible') ? 'hidden' : 'visible';
    }
    window.setTimeout(blink, 1000);
  }
  if (document.addEventListener) document.addEventListener('DOMContentLoaded', blink, false);
  else if (window.addEventListener) window.addEventListener('load', blink, false);
  else if (window.attachEvent) window.attachEvent('onload', blink);
  else window.onload = blink;

$('body').append('<blink>jquery test</blink>');;"
                },

            };

        public Project Create()
        {
            var project = new Project();
            project.id = Projects.Count + 1;
            Projects.Add(project);
            return project;
        }
    }
}