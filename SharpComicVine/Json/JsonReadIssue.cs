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
    static public class JsonReadIssue
    {

        private static ComicVineIssue Get(JObject j_ob, bool details)
        {

            ComicVineIssue comicVineIssue = new ComicVineIssue();
            comicVineIssue.id = (int)j_ob["id"];
            comicVineIssue.api_detail_url = j_ob["api_detail_url"].ToString();

            ComicVineImages images = new ComicVineImages();

            if (!String.IsNullOrEmpty(j_ob["image"].ToString()))
            {
                try
                {
                    images = JsonConvert.DeserializeObject<ComicVineImages>(j_ob["image"].ToString());
                }
                catch (Exception ex)
                {
                    // We don't catch this exeption.
                }
            }

            comicVineIssue.images = images;
            comicVineIssue.issue_number = j_ob["issue_number"].ToString();
            comicVineIssue.name = j_ob["name"].ToString();

            string coverdate = j_ob["cover_date"].ToString();

            comicVineIssue.issue_year = StringUtilFunctions.TryToExtractYear(coverdate);
            comicVineIssue.issue_month = StringUtilFunctions.TryToExtractMonth(coverdate);
            comicVineIssue.issue_title = j_ob["name"].ToString();

            ComicVineVolume volum = new ComicVineVolume();
            if (!String.IsNullOrEmpty(j_ob["volume"].ToString()))
            {
                volum = JsonConvert.DeserializeObject<ComicVineVolume>(j_ob["volume"].ToString());
            }

            comicVineIssue.volume = volum;

            if (details)
            {
                IList<JToken> jCharacters = j_ob["character_credits"].Children().ToList();
                IList<JToken> jCredits = j_ob["person_credits"].Children().ToList();
                List<ComicVineCharacter> comicVineCharacterList = new List<ComicVineCharacter>();
                List<ComicVineCredit> comicVineCreditList = new List<ComicVineCredit>();


                foreach (JToken character in jCharacters)
                {
                    ComicVineCharacter comicVineCharacter = JsonConvert.DeserializeObject<ComicVineCharacter>(character.ToString());
                    comicVineCharacterList.Add(comicVineCharacter);
                }

                foreach (JToken credit in jCredits)
                {
                    ComicVineCredit cc = JsonConvert.DeserializeObject<ComicVineCredit>(credit.ToString());
                    comicVineCreditList.Add(cc);
                }

                comicVineIssue.credits = new List<ComicVineCredit>();
                comicVineIssue.credits = comicVineCreditList;

                comicVineIssue.characters = new List<ComicVineCharacter>();
                comicVineIssue.characters = comicVineCharacterList;
            }



            return comicVineIssue;
        }

        public static ComicVineIssue GetIssue(string jsonData, bool details)
        {

            JObject jObject = JObject.Parse(jsonData);

            string how_many_string = jObject["number_of_total_results"].ToString();
            bool found = false;
            int how_many_number = 0;

            found = int.TryParse(how_many_string, out how_many_number);

            ComicVineIssue comicVineIssue = new ComicVineIssue();

            if (how_many_number > 0)
            {
                if (!details)
                {

                    string res = jObject["results"].ToString();
                    JArray a = JArray.Parse(res);
                    return Get((JObject)a[0], false);
                }
                else
                {
                    return Get((JObject)jObject["results"], true);
                }            
            }
            return comicVineIssue;            
        }
    }
}
