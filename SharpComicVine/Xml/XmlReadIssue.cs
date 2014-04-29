using SharpComicVine.Models;
using SharpComicVine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharpComicVine.Xml
{
    static public class XmlReadIssue
    {
        public static ComicVineIssue GetIssue(string xmlString)
        {
            ComicVineIssue comicVineIssue = new ComicVineIssue();

            XDocument xDocument = XDocument.Parse(xmlString, LoadOptions.None);

            comicVineIssue.api_detail_url = XmlUtilFunctions.getNodeValue(xDocument, "api_detail_url");
            comicVineIssue.issue_description = XmlUtilFunctions.getNodeValue(xDocument, "description");
            comicVineIssue.id = StringUtilFunctions.TryToParse(XmlUtilFunctions.getNodeValue(xDocument, "id"));

            // Get the volume images 
            var images = from imgs in xDocument.Descendants("image") select imgs;
            ComicVineImages comicVineImages = new ComicVineImages();

            if (images != null | images.Count() > 0)
            {
                comicVineImages.icon_url = images.Descendants("icon_url").First().Value;
                comicVineImages.medium_url = images.Descendants("medium_url").First().Value;
                comicVineImages.screen_url = images.Descendants("screen_url").First().Value;
                comicVineImages.small_url = images.Descendants("small_url").First().Value;
                comicVineImages.super_url = images.Descendants("super_url").First().Value;
                comicVineImages.thumb_url = images.Descendants("thumb_url").First().Value;
                comicVineImages.tiny_url = images.Descendants("tiny_url").First().Value;
            }

            comicVineIssue.images = comicVineImages;
            comicVineIssue.issue_number = XmlUtilFunctions.getNodeValue(xDocument, "issue_number");
            comicVineIssue.name = XmlUtilFunctions.getNodeValue(xDocument, "name");

            string comic_tentative_date = XmlUtilFunctions.getNodeValue(xDocument, "cover_date");

            comicVineIssue.issue_year = StringUtilFunctions.TryToExtractYear(comic_tentative_date);
            comicVineIssue.issue_month = StringUtilFunctions.TryToExtractMonth(comic_tentative_date);



            // Get the volume details
            var volumes = from vols in xDocument.Descendants("volume") select vols;
            if (volumes != null & volumes.Count() > 0)
            {
                ComicVineVolume volum = new ComicVineVolume();
                volum.id = StringUtilFunctions.TryToParse(volumes.Descendants("id").First().Value);
                volum.name = volumes.Descendants("name").First().Value;
                volum.url = volumes.Descendants("api_detail_url").First().Value;
                comicVineIssue.volume = volum;
            }






 

            var persons = from rols in xDocument.Descendants("person") select rols;
            List<ComicVineCredit> comicVineCreditList = new List<ComicVineCredit>();

            if (persons.Count() > 0)
            {
                foreach (var person in persons)
                {
                    ComicVineCredit comicVineCredit = new ComicVineCredit();
                    comicVineCredit.id = StringUtilFunctions.TryToParse(person.Descendants("id").First().Value);
                    comicVineCredit.name = person.Descendants("name").First().Value;
                    comicVineCredit.api_detail_url = person.Descendants("api_detail_url").First().Value;
                    comicVineCredit.role = person.Descendants("role").First().Value;

                    comicVineCreditList.Add(comicVineCredit);
                }
            }

            comicVineIssue.credits = comicVineCreditList;

            var characters = from character in xDocument.Descendants("character") select character;
            List<ComicVineCharacter> comicVineCharacterList = new List<ComicVineCharacter>();

            if (characters.Count() > 0)
            {
                foreach (var tchar in characters)
                {
                    ComicVineCharacter comicVineCharacter = new ComicVineCharacter();
                    comicVineCharacter.id = StringUtilFunctions.TryToParse(tchar.Descendants("id").First().Value);
                    comicVineCharacter.name = tchar.Descendants("name").First().Value;
                    comicVineCharacter.api_detail_url = tchar.Descendants("api_detail_url").First().Value;
                    comicVineCharacter.site_detail_url = tchar.Descendants("site_detail_url").First().Value;
                    
                    comicVineCharacterList.Add(comicVineCharacter);
                }

            }

            comicVineIssue.characters = comicVineCharacterList;

            return comicVineIssue;
        }
    }
}
