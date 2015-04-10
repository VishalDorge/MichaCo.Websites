using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CacheManager.Core;
using Microsoft.Practices.Unity;
using Website.Models;

namespace Website.Controllers
{
    public class ToDoController : ApiController
    {
        // prefix for the todo cache key. 
        // This is not really needed, but anyways... maybe better than storing the integer only
        private const string TodoKeyPrefix = "todo-sample-item-";

        // key to store all available todos' keys.
        private const string KeysKey = "todo-sample-keys";
        
        // retrieves all todos' keys or adds an empty int array if the key is not set
        private List<int> AllKeys
        {
            get
            {
                var keys = todoCache.Get<int[]>(KeysKey);

                if (keys == null)
                {
                    keys = new int[] { };
                    todoCache.Add(KeysKey, keys);
                }

                return keys.ToList();
            }
        }

        [Dependency]
        protected ICacheManager<object> todoCache { get; set; }

        // GET: api/ToDo
        public IEnumerable<Todo> Get()
        {
            var keys = this.AllKeys;

            foreach (var key in keys)
            {
                yield return this.Get(key);
            }
        }

        // GET: api/ToDo/5
        public Todo Get(int id)
        {
            return todoCache.Get<Todo>(TodoKeyPrefix + id);
        }

        // POST: api/ToDo
        public Todo Post([FromBody]Todo value)
        {
            int newId = -1;
            todoCache.Update(KeysKey, obj =>
            {
                var keys = (obj as int[]).ToList();
                newId = !keys.Any() ? 1 : keys.Max() + 1;
                keys.Add(newId);
                return keys.ToArray();
            });

            value.Id = newId;
            todoCache.Add(TodoKeyPrefix + newId, value);
            return value;
        }

        // PUT: api/ToDo/5
        public void Put(int id, [FromBody]Todo value)
        {
            todoCache.Put(TodoKeyPrefix + id, value);
        }

        // DELETE ALL completed: api/ToDo
        public void Delete()
        {
            var keys = this.AllKeys;

            foreach (var key in keys)
            {
                var item = this.Get(key);
                if (item != null && item.Completed)
                {
                    this.Delete(item.Id);
                }
            }
        }

        // DELETE: api/ToDo/5
        public void Delete(int id)
        {
            todoCache.Remove(TodoKeyPrefix + id);
            todoCache.Update(KeysKey, obj =>
            {
                var keys = (obj as int[]).ToList();
                keys.Remove(id);
                return keys.ToArray();
            });
        }
    }
}