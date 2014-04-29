using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpComicVine.Utils
{
    public static class StringUtilFunctions
    {
        public static int TryToParse(string value)
        {
            int number;
            bool result = Int32.TryParse(value, out number);
            if (result)
            {
                return number;
            }
            else
            {
                return -1;
            }
        }

        public static Single TryToParseSingle(string value)
        {
            // This method assumes that the decimal separator is ,

            Single number;
            bool result = Single.TryParse(value.Replace(".", ","), out number);
            if (result)
            {
                return number;
            }
            else
            {
                return -1;
            }
        }

        public static int TryToExtractYear(string cover_date)
        {
            string __year = string.Empty;

            if (cover_date.IndexOf("-") > 0)
            {
                __year = cover_date.Substring(0, cover_date.IndexOf("-"));
            }

            return TryToParse(__year);

        }
        public static int TryToExtractMonth(string cover_date)
        {
            string __month = string.Empty;


            if (cover_date.IndexOf("-") > 0)
            {
                __month = cover_date.Substring(cover_date.IndexOf("-") + 1, cover_date.Length - cover_date.IndexOf("-") - 1);

                if (__month.IndexOf("-") > 0)
                {
                    __month = __month.Substring(0, __month.IndexOf("-"));
                }

            }

            return TryToParse(__month);

        }
   
    }
}
