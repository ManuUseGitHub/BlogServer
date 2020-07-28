using Utilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Utilities
{
    public class Comparator
    {

        private class CompAnalyse
        {
            public string Shorter { get; set; }
            public string Longer { get; set; }
            public int IShorter { get; internal set; }
            public int ILonger { get; internal set; }
            public int Invers { get; internal set; }
        }

        private CompAnalyse PrepareLongerAndShorter(string a, string b, CompAnalyse obj)
        {
            var r = @"[\`\s\r\n\t~!@#$%^&*()_|+\-=?;:',.<>\{\}\[\]\\\/]";
            var copyASorted = a.Replace(r, "").ToLower();
            var copyBSorted = b.Replace(r, "").ToLower();

            var aLength = copyASorted.Length;
            var bLength = copyBSorted.Length;

            var shorter = copyASorted;
            var longer = copyBSorted; //

            // TODO : factorize dooble values

            if (aLength > bLength)
            {
                shorter = copyBSorted;
                longer = copyASorted;
            }
            Reflector.Merge(obj, new CompAnalyse()
            {
                Shorter = shorter,
                Longer = longer
            });

            return obj;
        }

        private double SCompare(string a, string b)
        {
            if (a.Equals(b))
            {
                return 100;
            }
            else if (a.Length == 0 || b.Length == 0)
            {
                //One is empty, second is not
                return 0;
            }
            return SComparChars(a, b);
        }

        private double SComparChars(string a, string b)
        {
            var maxLen = a.Length > b.Length ? a.Length : b.Length;
            var minLen = a.Length < b.Length ? a.Length : b.Length;
            var sameCharAtIndex = 0;

            //Compare char by char
            for (var i = 0; i < minLen; i++)
            {
                if (a[i] == b[i])
                {
                    sameCharAtIndex++;
                }
            }

            return (sameCharAtIndex / (maxLen * 1.0)) * 100;
        }

        public int SentenceCompare(string a, string b)
        {
            CompAnalyse obj = PrepareLongerAndShorter(a, b, new CompAnalyse()
            {
                IShorter = 0,
                ILonger = 0,
                Invers = 0
            });

            var keys = SequenceMatcher.MatchingSequences(obj.Shorter, obj.Longer)
              .Matches; //

            factorizeSentences(keys, obj);

            double result = SCompare(
              string.Join("", keys) + obj.Shorter,
              string.Join("", keys) + obj.Longer
            );

            // if there is an inversion between 2 words
            // a percentage will be substracted
            Console.WriteLine(obj.Invers);
            return (int)(result / (1 + obj.Invers * 0.05));
        }

        private int CountWordInversion(string k, CompAnalyse obj)
        {
            int newIndexA = obj.Shorter.IndexOf(k);
            int newIndexB = obj.Longer.IndexOf(k);

            bool hasPermutation = newIndexA < obj.IShorter != newIndexB < obj.ILonger;

            obj.IShorter = newIndexA;
            obj.ILonger = newIndexB;

            return hasPermutation ? 1 : 0;
        }

        private void factorizeSentences(List<string> keys, CompAnalyse obj)
        {
            // looping over sentences to factorize them with matching keys

            foreach (string k in keys)
            {
                obj.Invers += CountWordInversion(k, obj);

                Regex rgx = new Regex(k);

                obj.Shorter = rgx.Replace(obj.Shorter, "",1);
                obj.Longer = rgx.Replace(obj.Longer, "",1);
            }
        }
    }
}