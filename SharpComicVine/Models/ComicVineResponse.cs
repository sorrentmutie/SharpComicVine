using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpComicVine.Models
{
    public class ComicVineResponse
    {
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string Response { get; set; }
    }

    public class ComicVineResponseData
    {
        public int number_of_total_results { get; set; }
        public int number_of_page_results { get; set; }
        public int limit { get; set; }
    }
}
