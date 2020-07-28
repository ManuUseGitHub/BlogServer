using Entities.EmmBlog.DataModelObjects;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class Util
    {
        public static Article GetSharedCopy(Article article, Blog blog2)
        {
            Article copy = GetCopyOf(article);
            copy.BlogId = blog2.Id;
            copy.Blog = blog2;
            return copy;
        }

        public static T GetCopyOf<T>(T source) where T : class, new()
        {
            T target = new NullNormalizeFactory<T>(_=>{ }).Instance;
            Reflector.Merge(target, source);
            return target;
        }


        /// <summary>
        /// Get a modifiable version of an object. Database objects with EF
        /// need to use copy to be modified if you plan on manipulating keys.
        /// here you got a semi transiant object that is the copy of what entity
        /// you want but not linked to the database since it is a new object
        /// null - normalized, merged with existant data and references
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">Use that to initialize the new instance</param>
        /// <param name="source">The object to merge with the target</param>
        /// <returns></returns>
        public static T GetModifiable<T>(T source, Action<T> action) where T : class, new()
        {
            // get a new instance of a class T where every nullable Members 
            // are set to null value ... to make merging more compliant
            T target = new NullNormalizeFactory<T>(action).Instance;

            // If there are collisions for properties, keep target values
            var mergeType = Reflector.MergeOptions.KEEP_TARGET;

            // Merge the target with the source.
            Reflector.Merge(target, source, mergeType);
            return target;
        }

        /// <summary>
        /// http://predicatet.blogspot.com/2009/04/improved-c-slug-generator-or-how-to.html
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        /// <summary>
        /// https://www.c-sharpcorner.com/code/2855/remove-accentsdiacritics-from-a-string-with-C-Sharp.aspx
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveAccent(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string GetSlugBasedOnString(string title)
        {
            string generated = GenerateSlug(title).ToUpper();

            // return singled dashed generated
            return Regex.Replace(generated, @"[\-]{2,}", "-"); ;
        }
    }
}
