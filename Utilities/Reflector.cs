using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilities
{
    public static class Reflector
    {
        public enum MergeOptions
        {
            KEEP_TARGET = 0,
            KEEP_SOURCE = 1
        }

        /// <summary>
        /// MErge a source object with a target object with one rule
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="collision"></param>
        public static void Merge<T>(this T target, T source, MergeOptions collision = MergeOptions.KEEP_SOURCE)
        {
            var properties = GetProperties<T>();

            foreach (var prop in properties)
            {
                MergePropertyCompared(target, source, prop, collision);
            }
        }

        /// <summary>
        /// source : https://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyLambda"></param>
        /// <returns></returns>
        public static PropertyInfo GetPInfo<TSource, TProperty>(
            TSource source,
            Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }

        /// <summary>
        /// Merge a source object with a target object with inverted rules for
        /// certain properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="collision"></param>
        /// <param name="list"></param>
        public static void Merge<T>(this T target, T source,
            MergeOptions collision,
            List<PropertyInfo> list
        ){
            var properties = GetProperties<T>();
            var inverseOptions = MergeOptions.KEEP_TARGET == collision ?
                MergeOptions.KEEP_SOURCE : MergeOptions.KEEP_TARGET;

            foreach (var prop in properties)
            {
                if (list.Contains(prop))
                {
                    MergeOptions defined = inverseOptions;
                    MergePropertyByRule(target, source, prop, defined);
                }
                else
                {
                    MergePropertyCompared(target, source, prop, collision);
                }
            }
        }

        /// <summary>
        /// Merge a source object with a target object with defined rules
        /// for certain properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="rules"></param>
        /// <param name="collision"></param>
        public static void Merge<T>(this T target, T source,
            Dictionary<PropertyInfo, MergeOptions> rules,
            MergeOptions collision = MergeOptions.KEEP_SOURCE)
        {
            var properties = GetProperties<T>();

            foreach (var prop in properties)
            {
                if (rules.ContainsKey(prop))
                {
                    MergeOptions defined = rules[prop];
                    MergePropertyByRule(target, source, prop, defined);
                }
                else
                {
                    MergePropertyCompared(target, source, prop, collision);
                }
            }
        }

        private static void MergePropertyCompared<T>(
            this T target, T source,
            PropertyInfo prop, MergeOptions collision)
        {
            var value = prop.GetValue(target, null);
            var value2 = prop.GetValue(source, null);

            Boolean iscollision = value != null && value2 != null;
            if (iscollision)
            {
                MergePropertyByRule(target, source, prop, collision);
            }
            else
            {
                MergePropertyNullRule(target, source, prop);
            }
        }

        private static void MergePropertyByRule<T>(
            this T target, T source,
            PropertyInfo prop, MergeOptions collision)
        {
            var value = prop.GetValue(target, null);
            var value2 = prop.GetValue(source, null);

            if (collision == MergeOptions.KEEP_TARGET)
            {
                prop.SetValue(target, value, null);
            }
            else if (collision == MergeOptions.KEEP_SOURCE)
            {
                prop.SetValue(target, value2, null);
            }
        }

        private static void MergePropertyNullRule<T>(
            this T target, T source,
            PropertyInfo prop)
        {
            var value = prop.GetValue(target, null);
            var value2 = prop.GetValue(source, null);

            if (value != null)
            {
                prop.SetValue(target, value, null);
            }
            else if (value2 != null)
                prop.SetValue(target, value2, null);
        }

        private static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return typeof(T).GetProperties()
                .Where(prop => prop.CanRead && prop.CanWrite);
        }

        /// <summary>
        /// Sets the reference of a source object to a new one with the copy of all its properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">the object to have a shalowed copy with</param>
        /// <returns></returns>
        public static T ReferShalowedCopy<T>(T source) where T : class, new()
        {
            T target = new NullNormalizeFactory<T>().Instance;
            Merge(target, source);

            return target;
        }
    }

    public class NullNormalizeFactory<T> where T : class, new()
    {
        public T Instance { get; set; }

        /// <summary>
        /// Create an instance of Class T and set a null value to all Nullable properties
        /// </summary>
        /// <param name="initializer"></param>
        public NullNormalizeFactory(Action<T> initializer = null)
        {
            IEnumerable<PropertyInfo> properties = GetProperties<T>();

            Instance = new T();

            // runs initializer(Instance) if initializer is not null
            initializer?.Invoke(Instance);

            foreach (var prop in properties)
            {
                Type pt = prop.GetType();

                Boolean isNullableProperty = Nullable.GetUnderlyingType(pt) != null;

                if (isNullableProperty)
                {
                    prop.SetValue(prop.GetValue(Instance, null), null);
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return typeof(T).GetProperties()
                .Where(prop => prop.CanRead && prop.CanWrite);
        }
    }
}