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
        // prefix for the todo cache key. This is not really needed, but anyways... maybe better
        // than storing the integer only
        private const string TodoKeyPrefix = "todo-sample-item-";

        // key to store all available todos' keys.
        private const string KeysKey = "todo-sample-keys";

        // retrieves all todos' keys or adds an empty int array if the key is not set
        private IList<int> AllKeys
        {
            get
            {
                var keys = keysCache.Get(KeysKey);

                if (keys == null)
                {
                    keys = new int[] { };
                    keysCache.Add(KeysKey, keys);
                }

                return keys;
            }
        }

        [Dependency]
        protected ICacheManager<Todo> todoCache { get; set; }
        
        [Dependency]
        protected ICacheManager<IList<int>> keysCache { get; set; }

        // GET: api/ToDo
        public IEnumerable<Todo> Get()
        {
            var keys = this.AllKeys;

            foreach (var key in keys)
            {
                var value = this.Get(key);
                if (value != null)
                {
                    yield return value;
                }
            }
        }

        // GET: api/ToDo/5
        public Todo Get(int id) => todoCache.Get<Todo>(TodoKeyPrefix + id);

        // POST: api/ToDo
        public Todo Post([FromBody]Todo value)
        {
            int newId = -1;
            keysCache.Update(KeysKey, keys =>
            {
                newId = !keys.Any() ? 1 : keys.Max() + 1;
                return keys.Concat(new[] { newId }).ToList();
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
            keysCache.Update(KeysKey, obj =>
            {
                var keys = obj.ToList();
                keys.Remove(id);
                return keys;
            });
        }
    }
}