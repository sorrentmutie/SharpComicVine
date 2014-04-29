using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpComicVine.Models
{
    public class ComicVineVolume
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int count_of_issues { get; set; }
        public string volume_description { get; set; }
        public int start_year { get; set; }
        public ComicVinePublisher publisher { get; set; }
        public ComicVineImages images { get; set; }
        public List<ComicVineIssue> list_of_issue { get; set; }
    }
}
