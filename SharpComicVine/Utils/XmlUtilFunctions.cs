using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SharpComicVine.Utils
{
    public static class XmlUtilFunctions
    {
        public static string getNodeValue(XDocument xDocument, string name)
        {
            string result = string.Empty;
            
            var lv1s = from lv1 in xDocument.Descendants(name) select lv1;

            if (lv1s != null & lv1s.Count() > 0)
            {
                result = lv1s.First().Value;
            }

            lv1s = null;

            return result;
        }
    }
}
