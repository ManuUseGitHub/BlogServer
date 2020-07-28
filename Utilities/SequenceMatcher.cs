using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    internal static class SequenceMatcher
    {
        public class MatchesObject
        {
            public int Cpt { get; internal set; }
            public List<string> Matches { get; set; }
        }

        public static MatchesObject MatchingSequences(
            string shorter,
            string longer,
            int minimumCount = 0
        )
        {
            if (minimumCount == 0)
            {
                minimumCount = 2;
            }

            
            MatchesObject result = new MatchesObject() {
                // for stats only ----
                Cpt = 0,
                // -------------------
                Matches = new List<string>()
            };

            bool joinctMatches;
            int sl = shorter.Length;

            StringBuilder windowc = new StringBuilder();

            for (var j = 0; j < sl; ++j)
            {
                joinctMatches = new Regex(windowc.ToString()).IsMatch(longer);
                var isTheLastReached = j == (sl - 1);

                Console.WriteLine(windowc.ToString());
                if (!joinctMatches || isTheLastReached)
                {
                    // the windows does not match anymore

                    if (!isTheLastReached)
                    {
                        // remove the character in the window causing it to not match
                        --windowc.Length;
                    }
                    else if (joinctMatches)
                    {
                        windowc.Append(shorter[j]);
                    }

                    // skip if the window become empty
                    if (windowc.Length != 0)
                    {
                        // add the match if it is of the minimum length defined
                        if (windowc.Length >= minimumCount)
                        {
                            result.Matches.Add(windowc.ToString());
                        }

                        // set the previous character in the window
                        windowc.Length = 0;
                        windowc.Append(shorter[j - 1]);

                        // if this is not the last annalysis, we need to ovoid skipping the trouble maker
                        // so we set the index on the previous position to counter the incrementation
                        if (!isTheLastReached)
                        {
                            --j; //
                        }
                    }
                    else
                    {
                        windowc.Append(shorter[j]);
                    }
                }
                else
                {
                    // there is still a match for the window
                    windowc.Append(shorter[j]);
                }
                ++result.Cpt;
            }

            return result;
        }
    }
}