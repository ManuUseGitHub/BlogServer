using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Exceptions
{
    public abstract class EmmBlogException <T> where T : class
    {
        protected static string ExpliciteNotFoundMessage(T entity,object[] keys)
        {

            StringBuilder sb = InitializeIdentifying(entity, keys);
            string identifying = sb.ToString();

            sb.Length = 0; // reset the string builder with no element

            return sb
                .Append($"There is no T:{typeof(T).Name} identified by\r\n")
                .Append($"[key:{identifying}]\r\n\r\n")
                .ToString();
        }

        protected static string ExpliciteDuplicationFoundMessage(T entity, object[] keys)
        {
            StringBuilder sb = InitializeIdentifying(entity, keys);
            string identifying = sb.ToString();

            sb.Length = 0; // reset the string builder with no element

            return sb
                .Append($"There is already a T:{typeof(T).Name} identified by\r\n")
                .Append($"[key:{identifying}]\r\n\r\n")
                .ToString();
        }

        private static StringBuilder InitializeIdentifying(T entity, object[] keys)
        {
            var nextKeys = new ArraySegment<object>(keys, 1, keys.Length - 1)
                .ToArray();

            StringBuilder sb = new StringBuilder().Append(keys[0]);
            foreach (object key in nextKeys)
            {
                sb.Append('|').Append(key);
            }

            return sb;
        }
    }
}
