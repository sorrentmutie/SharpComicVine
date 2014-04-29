using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpComicVine.Models
{
    public class ComicVineIssue
    {
        public int id { get; set; }
        
        public string api_detail_url { get; set; }
        
        public string issue_number { get; set; }
        
        public string name { get; set; }
        
        public int issue_month { get; set; }
        
        public int issue_year { get; set; }

        public string issue_description { get; set; }

        public string issue_title { get; set; }
        
        public ComicVineImages images { get; set; }

        public List<ComicVineCredit> credits { get; set; }
        
        public List<ComicVineCharacter> characters { get; set; }
        
        public ComicVineVolume volume { get; set; }
    }
}
