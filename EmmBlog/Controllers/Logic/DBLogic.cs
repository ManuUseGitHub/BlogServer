using Microsoft.AspNetCore.Mvc;
using System;

namespace EmmBlog.Controllers.Logic
{
    public abstract class DBLogic
    {
        public DBController Ctrl { protected get; set; }

        internal IActionResult ResultOrNotFound<T, DTO>(T item, Object id) where T : class
        {
            if (item == null)
            {
                Ctrl.Logger.LogError($"No Object of type {typeof(T).Name} with key[{id}], hasn't been found in the db.");
                return Ctrl.NotFound();
            }
            else
            {
                Ctrl.Logger.LogInfo($"Returned Object of type {typeof(T).Name} with key[{id}] found : {id}");

                var result = Ctrl.Mapper.Map<DTO>(item);
                return Ctrl.Ok(result);
            }
        }
    }
}