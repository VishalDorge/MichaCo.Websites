using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CacheManager.Core;
using Microsoft.Practices.Unity;
using Website.Models;

namespace Website.Controllers
{
    public class TodoController : ApiController
    {
        [Dependency]
        protected ICacheManager<Todo[]> todoCache { get; set; }

        public ICollection<Todo> Get()
        {
            var todos = todoCache.Get("todos");
            if (todos == null)
            {
                todos = new Todo[] { };
                todoCache.Add("todos", todos);
            }

            return todos;
        }

        [HttpPost]
        public void Post(ICollection<Todo> todos)
        {
            todoCache.Update("todos", val =>
            {
                return todos.ToArray();
            });
        }
    }
}
