using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpComicVine.Models;
using SharpComicVine.Utils;
using System.Xml.Linq;

namespace SharpComicVine.Xml
{
    static public class XmlReadVolumeQuery
    {
        public static ComicVineResponseData GetFirstVolumeQueryResponse(string xmlString)
        {
            ComicVineResponseData comicVineResponseData = new ComicVineResponseData();

            XDocument xDocument = XDocument.Parse(xmlString, LoadOptions.None);

            comicVineResponseData.number_of_page_results = StringUtilFunctions.TryToParse(XmlUtilFunctions.getNodeValue(xDocument, "number_of_page_results"));
            comicVineResponseData.number_of_total_results = StringUtilFunctions.TryToParse(XmlUtilFunctions.getNodeValue(xDocument, "number_of_total_results"));
            comicVineResponseData.limit = StringUtilFunctions.TryToParse(XmlUtilFunctions.getNodeValue(xDocument, "limit"));

            return comicVineResponseData;
        }

        public static List<ComicVineVolume> GetVolumeQueryResponse(string xmlString)
        {
            List<ComicVineVolume> comicVineVolumeList = new List<ComicVineVolume>();

            XDocument xDocument = XDocument.Parse(xmlString, LoadOptions.None);

            var volumes = (from volume in xDocument.Descendants("results") select volume);

            foreach (var volume in volumes)
            {
                string name = volume.Descendants("name").First().Value;
                string id = volume.Descendants("id").First().Value;

                comicVineVolumeList.Add(new ComicVineVolume()
                {
                    name = name,
                    id = StringUtilFunctions.TryToParse(id),
                });
            }

            return comicVineVolumeList;
        }

        public static ComicVineVolume GetVolume(string xmlString)
        {
            ComicVineVolume comicVineVolume = new ComicVineVolume();

            XDocument xDocument = XDocument.Parse(xmlString);

            comicVineVolume.url = XmlUtilFunctions.getNodeValue(xDocument, "api_detail_url");
            comicVineVolume.count_of_issues = StringUtilFunctions.TryToParse(XmlUtilFunctions.getNodeValue(xDocument, "count_of_issues"));
            comicVineVolume.volume_description = XmlUtilFunctions.getNodeValue(xDocument, "description");
            comicVineVolume.id = StringUtilFunctions.TryToParse(XmlUtilFunctions.getNodeValue(xDocument, "id"));

            
            ComicVineImages comicVineImages = new ComicVineImages();
            
            var images = from imgs in xDocument.Descendants("image") select imgs;
            
            if (images.Count() > 0)
            {
                try
                {
                    comicVineImages.icon_url = images.Descendants("icon_url").First().Value;
                    comicVineImages.medium_url = images.Descendants("medium_url").First().Value;
                    comicVineImages.screen_url = images.Descendants("screen_url").First().Value;
                    comicVineImages.small_url = images.Descendants("small_url").First().Value;
                    comicVineImages.super_url = images.Descendants("super_url").First().Value;
                    comicVineImages.thumb_url = images.Descendants("thumb_url").First().Value;
                    comicVineImages.tiny_url = images.Descendants("tiny_url").First().Value;
                }
                catch (Exception ex)
                {

                }
            }
            
            comicVineVolume.images = comicVineImages;

            List<ComicVineIssue> comicVineIssueList = new List<ComicVineIssue>();
            
            var issues = from iss in xDocument.Descendants("issue") select iss;

            foreach (var issue in issues)
            {
                ComicVineIssue comicVineIssue = new ComicVineIssue();
                comicVineIssue.id = StringUtilFunctions.TryToParse(issue.Descendants("id").First().Value);
                comicVineIssue.api_detail_url = issue.Descendants("api_detail_url").First().Value;
                comicVineIssue.name = issue.Descendants("name").First().Value;
                comicVineIssue.issue_number = issue.Descendants("issue_number").First().Value;

                comicVineIssueList.Add(comicVineIssue);
            }

            comicVineVolume.list_of_issue = comicVineIssueList;
            comicVineVolume.name = XmlUtilFunctions.getNodeValue(xDocument, "name");

            ComicVinePublisher comicVinePublisher = new ComicVinePublisher();

            var publishers = from publ in xDocument.Descendants("publisher") select publ;

            if (publishers != null & publishers.Count() > 0)
            {
                try
                {
                    comicVinePublisher.publisher_name = publishers.Descendants("name").First().Value;
                    comicVinePublisher.publisher_id = StringUtilFunctions.TryToParse(publishers.Descendants("id").First().Value);
                    comicVinePublisher.publisher_url = publishers.Descendants("api_detail_url").First().Value;
                }
                catch (Exception ex)
                {

                }
            }

            comicVineVolume.publisher = comicVinePublisher;
            comicVineVolume.start_year = StringUtilFunctions.TryToParse(XmlUtilFunctions.getNodeValue(xDocument, "start_year"));

            return comicVineVolume;
        }
    }
}