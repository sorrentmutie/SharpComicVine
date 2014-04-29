using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpComicVine.Models;
using SharpComicVine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpComicVine.Json
{
    public static class JsonReadVolumeQuery
    {
        public static ComicVineResponseData GetFirstVolumeQueryResponse(string jsonData)
        {
            ComicVineResponseData comicVineResponseData = new ComicVineResponseData();

            comicVineResponseData = JsonConvert.DeserializeObject<ComicVineResponseData>(jsonData);

            return comicVineResponseData;
        }

        public static List<ComicVineVolume> GetVolumeQueryResponse(string jsonData)
        {
            List<ComicVineVolume> volumes = new List<ComicVineVolume>();
            // Parse the xml Response into a XDocument
            JObject jObject = JObject.Parse(jsonData);

            IList<JToken> results = jObject["results"].Children().ToList();

            foreach (JToken result in results)
            {
                ComicVineVolume comicVineVolume = JsonConvert.DeserializeObject<ComicVineVolume>(result.ToString());
                volumes.Add(comicVineVolume);
            }

            return volumes;
        }

        public static ComicVineVolume GetVolume(string jsonData)
        {
            ComicVineVolume detailedComicVineVolume = new ComicVineVolume();

            JObject jObject = JObject.Parse(jsonData);

            var results = jObject["results"]["publisher"];

            detailedComicVineVolume.url = jObject["results"]["api_detail_url"].ToString();

            string countOfIssues = jObject["results"]["count_of_issues"].ToString();
            detailedComicVineVolume.count_of_issues = StringUtilFunctions.TryToParse(countOfIssues);

            detailedComicVineVolume.volume_description = jObject["results"]["description"].ToString();

            string volumeId = jObject["results"]["id"].ToString();
            detailedComicVineVolume.id = StringUtilFunctions.TryToParse(volumeId);

            detailedComicVineVolume.name = jObject["results"]["name"].ToString(); ;

            string startYear = jObject["results"]["start_year"].ToString();
            detailedComicVineVolume.start_year = StringUtilFunctions.TryToParse(startYear);

            // Get the volume images 
            //var timages = from imgs in xdoc.Descendants("image") select imgs;
            ComicVineImages images = new ComicVineImages();

            if (!String.IsNullOrEmpty(jObject["results"]["image"].ToString()))
            {
                try
                {
                    images = JsonConvert.DeserializeObject<ComicVineImages>(jObject["results"]["image"].ToString());
                }
                catch (Exception ex)
                {
                    // We don't catch this exeption.
                }
            }
            detailedComicVineVolume.images = images;

            // Get the collection of issues
            //List<ComicVineIssue> comicVineIssueList = new List<ComicVineIssue>();
            //IList<JToken> jIssues = jObject["results"]["issues"].Children().ToList();

            //foreach (JToken issues in jIssues)
            //{
            //    ComicVineIssue comicVineIssue = JsonConvert.DeserializeObject<ComicVineIssue>(issues.ToString());
            //    comicVineIssueList.Add(comicVineIssue);
            //}


            //detailedComicVineVolume.list_of_issue = comicVineIssueList;

            ComicVinePublisher publisher = new ComicVinePublisher();
            publisher = JsonConvert.DeserializeObject<ComicVinePublisher>(jObject["results"]["publisher"].ToString());

            detailedComicVineVolume.publisher = publisher;

            return detailedComicVineVolume;
        }
    }
}
