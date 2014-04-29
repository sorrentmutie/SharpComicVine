using Newtonsoft.Json;
using SharpComicVine.Json;
using SharpComicVine.Models;
using SharpComicVine.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpComicVine
{
    public static class ComicVineReader
    {
        public static ComicVineResponseData GetFirstVolumeQueryResponse(SearchType searchType, string data)
        {
            if (searchType == null)
            {
                throw new ArgumentNullException("searchType");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            ComicVineResponseData comicVineResponseData = null;

            if (searchType == SearchType.Xml)
            {
                comicVineResponseData = XmlReadVolumeQuery.GetFirstVolumeQueryResponse(data);
            }
            else
            {
                comicVineResponseData = JsonReadVolumeQuery.GetFirstVolumeQueryResponse(data);
            }

            return comicVineResponseData;
        }

        public static List<ComicVineVolume> GetVolumeQueryResponse(SearchType searchType, string data)
        {
            if (searchType == null)
            {
                throw new ArgumentNullException("searchType");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            List<ComicVineVolume> comicVineVolumeList = null;

            if (searchType == SearchType.Xml)
            {
                comicVineVolumeList = XmlReadVolumeQuery.GetVolumeQueryResponse(data);
            }
            else
            {
                comicVineVolumeList = JsonReadVolumeQuery.GetVolumeQueryResponse(data);
            }

            return comicVineVolumeList;
        }

        public static ComicVineVolume GetVolume(SearchType searchType, string data)
        {
            if (searchType == null)
            {
                throw new ArgumentNullException("searchType");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            ComicVineVolume detailedComicVineVolume = new ComicVineVolume();

            if (searchType == SearchType.Xml)
            {
                detailedComicVineVolume = XmlReadVolumeQuery.GetVolume(data);
            }
            else
            {
                detailedComicVineVolume = JsonReadVolumeQuery.GetVolume(data);
            }

            return detailedComicVineVolume;
        }

        public static ComicVineIssue GetIssue(SearchType searchType, string data, bool details)
        {
            if (searchType == null)
            {
                throw new ArgumentNullException("searchType");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

           
            ComicVineIssue comicVineIssue = null;

            if (searchType == SearchType.Xml)
            {
                comicVineIssue = XmlReadIssue.GetIssue(data);
            }
            else
            {
                comicVineIssue = JsonReadIssue.GetIssue(data, details);
            }

            return comicVineIssue;
        }

        public static ComicVineIssue GetIssue(SearchType searchType, string data, int issueNumber, bool details)
        {
            if (searchType == null)
            {
                throw new ArgumentNullException("searchType");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (issueNumber == null)
            {
                throw new ArgumentNullException("issueNumber");
            }

            ComicVineIssue ComicVineIssue = null;

            if (searchType == SearchType.Xml)
            {
                ComicVineIssue = XmlReadIssue.GetIssue(data);
            }
            else
            {
                ComicVineIssue = JsonReadIssue.GetIssue(data, details);
            }

            return ComicVineIssue;
        }
    }
}
