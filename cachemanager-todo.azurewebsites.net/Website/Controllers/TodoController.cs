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
        private const string KeysKey = "todo-sample-keys";
        private const string TodoKeyPrefix = "todo-sample-item-";

        [Dependency]
        protected ICacheManager<object> todoCache { get; set; }

        private List<long> AllKeys
        {
            get
            {
                var keys = todoCache.Get<long[]>(KeysKey);

                if (keys == null)
                {
                    keys = new long[] { };
                    todoCache.Add(KeysKey, keys);
                }

                return keys.ToList();
            }
        }

        // GET: api/ToDo
        public IEnumerable<Todo> Get()
        {
            var keys = this.AllKeys;

            foreach (var key in keys)
            {
                yield return todoCache.Get<Todo>(TodoKeyPrefix + key);
            }
        }

        // GET: api/ToDo/5
        public Todo Get(long id)
        {
            return todoCache.Get<Todo>(TodoKeyPrefix + id);
        }

        // POST: api/ToDo
        public Todo Post([FromBody]Todo value)
        {
            var allKeys = this.AllKeys;

            long newId = -1;
            todoCache.Update(KeysKey, obj =>
            {
                var keys = (obj as long[]).ToList();
                newId = !keys.Any() ? 1 : keys.Max() + 1;
                keys.Add(newId);
                return keys.ToArray();
            });

            if (newId == -1)
            {
                throw new InvalidOperationException("couldn't update keys");
            }

            value.Id = newId;
            todoCache.Add(TodoKeyPrefix + newId, value);
            return value;
        }

        // PUT: api/ToDo/5
        public void Put(long id, [FromBody]Todo value)
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
        public void Delete(long id)
        {
            todoCache.Remove(TodoKeyPrefix + id);
            todoCache.Update(KeysKey, obj =>
            {
                var keys = (obj as long[]).ToList();
                keys.Remove(id);
                return keys.ToArray();
            });
        }
    }
}