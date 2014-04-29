using SharpComicVine.Models;
using SharpComicVine.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpComicVine
{
    public class ComicVineService
    {
        public ComicVineService()
        {
            this.Initialize();
        }

        ComicVineService(string comicVineKey)
        {
            this.ComicVineKey = comicVineKey;
            this.Initialize();
        }

        public string ComicVineKey { get; set; }

        public MatchType MatchType { get; set; }

        public SearchType SearchType { get; set; }

        public string SearchAddress { get; private set; }

        public string ComicVineAddress { get; private set; }

    

        private void Initialize()
        {
            this.SearchType = SearchType.Xml;
            this.MatchType = MatchType.AbsoluteMatch;
            this.SearchAddress = "http://api.comicvine.com/search/";
            this.ComicVineAddress = "http://api.comicvine.com/";
        }

        private List<ComicVineVolume> FindVolumeIdByName(string volumeName)
        {
            string query = null;

            if (this.SearchType == SearchType.Xml)
            {
                query = this.ComicVineAddress + "volumes/?api_key=" + this.ComicVineKey + "&format=xml&field_list=id,name,publisher&filter=name:" + volumeName;
            }
            else
            {
                query = this.ComicVineAddress + "volumes/?api_key=" + this.ComicVineKey + "&format=json&field_list=id,name,publisher&filter=name:" + volumeName;
            }

            Task<ComicVineResponse> comicVineResponse = ComicVineConnection.ConnectAndRequest(query);

            ConcurrentBag<List<ComicVineVolume>> comicVineVolumeLists = new ConcurrentBag<List<ComicVineVolume>>();

            if (comicVineResponse.Result.Status == "OK")
            {
                ComicVineResponseData firstData = ComicVineReader.GetFirstVolumeQueryResponse(this.SearchType, comicVineResponse.Result.Response);

                if (firstData.number_of_total_results > 0)
                {
                    int parallelThreads = SystemEnvironment.ProcessorCountOptimizedForEnvironment();

                    int numberOfIterations = (int)Math.Ceiling(((double)firstData.number_of_total_results / (double)firstData.limit));

                    Parallel.For(0, numberOfIterations, new ParallelOptions() { MaxDegreeOfParallelism = parallelThreads }, i =>
                    {
                        int offset = i * firstData.limit;
                        string secondQuery = query + "&offset=" + offset.ToString();

                        Task<ComicVineResponse> secondResponse = ComicVineConnection.ConnectAndRequest(secondQuery);
                        var volumeList = ComicVineReader.GetVolumeQueryResponse(this.SearchType, secondResponse.Result.Response);

                        comicVineVolumeLists.Add(volumeList);
                        secondResponse = null;
                    });
                }
            }

            if (this.MatchType == MatchType.AbsoluteMatch)
            {
                ConcurrentBag<List<ComicVineVolume>> filteredComicVineVolumeLists = new ConcurrentBag<List<ComicVineVolume>>();
                List<ComicVineVolume> filteredComicVineVolumeList = new List<ComicVineVolume>();

                foreach (var volumeList in comicVineVolumeLists)
                {
                    foreach (var volume in volumeList)
                    {
                        if (volume.name == volumeName)
                        {
                            filteredComicVineVolumeList.Add(volume);
                        }
                    }
                }

                filteredComicVineVolumeLists.Add(filteredComicVineVolumeList);
                comicVineVolumeLists = filteredComicVineVolumeLists;
            }

            List<ComicVineVolume> comicVineVolumeList = new List<ComicVineVolume>();

            foreach (List<ComicVineVolume> comicVineVolume in comicVineVolumeLists)
            {
                comicVineVolumeList.AddRange(comicVineVolume);
            }

            return comicVineVolumeList;
        }

        public List<ComicVineVolume> SearchVolume(string volumeName)
        {
            List<ComicVineVolume> comicVineVolumeList = this.FindVolumeIdByName(volumeName);

            ConcurrentBag<ComicVineVolume> comicVineVolumeBag = new ConcurrentBag<ComicVineVolume>();

            int parallelThreads = SystemEnvironment.ProcessorCountOptimizedForEnvironment();

            Parallel.ForEach(comicVineVolumeList, new ParallelOptions() { MaxDegreeOfParallelism = parallelThreads }, comicVineVolume =>
            {
                try
                {
                    if (comicVineVolume != null)
                    {
                        comicVineVolumeBag.Add(this.GetComicVineVolume(comicVineVolume.id));
                    }
                }
                catch (AggregateException aggregateException)
                {
                    foreach (var exception in aggregateException.InnerExceptions)
                    {
                        if (exception is ArgumentException)
                        {
                            // Don't act on this
                        }
                        else
                        {
                            throw exception;
                        }
                    }
                }
            });

            return comicVineVolumeBag.ToList();
        }

        public List<ComicVineIssue> SearchIssue(string volumeName, int issueNumber)
        {
            List<ComicVineVolume> comicVineVolumeList = this.FindVolumeIdByName(volumeName);

            ConcurrentBag<ComicVineIssue> comicVineIssueBag = new ConcurrentBag<ComicVineIssue>();

            int parallelThreads = SystemEnvironment.ProcessorCountOptimizedForEnvironment();

            Parallel.ForEach(comicVineVolumeList, new ParallelOptions() { MaxDegreeOfParallelism = parallelThreads }, comicVineVolume =>
            {
                try
                {
                    if (comicVineVolume != null)
                    {
                        ComicVineIssue detailedComicVineIssue = this.GetComicVineIssue(comicVineVolume.id, issueNumber);

                        if (detailedComicVineIssue.issue_number == issueNumber.ToString())
                        {
                            comicVineIssueBag.Add(detailedComicVineIssue);
                        }
                    }
                }
                catch (AggregateException aggregateException)
                {
                    foreach (var exception in aggregateException.InnerExceptions)
                    {
                        if (exception is ArgumentException)
                        {
                            // Don't act on this
                        }
                        else
                        {
                            throw exception;
                        }
                    }
                }
            });

            return comicVineIssueBag.ToList();
        }

        public ComicVineVolume GetComicVineVolume(int volumeId)
        {
            ComicVineVolume detailedComicVineVolume = new ComicVineVolume();

            string query = null;

            if (this.SearchType == SearchType.Xml)
            {
                query = this.ComicVineAddress + "volume/4050-" + volumeId.ToString() + "/?api_key=" + this.ComicVineKey + "&format=xml&field_list=id,api_detail_url,count_of_issues,description,image,name,publisher,start_year";
            }
            else
            {
                query = this.ComicVineAddress + "volume/4050-" + volumeId.ToString() + "/?api_key=" + this.ComicVineKey + "&format=json&field_list=id,api_detail_url,count_of_issues,description,image,name,publisher,start_year";
            }

            Task<ComicVineResponse> firstResponse = ComicVineConnection.ConnectAndRequest(query);

            detailedComicVineVolume = ComicVineReader.GetVolume(this.SearchType, firstResponse.Result.Response);

            return detailedComicVineVolume;
        }

        public ComicVineIssue GetComicVineIssue(int issueId)
        {
            ComicVineIssue comicVineIssue = new ComicVineIssue();

            string query = null;

            if (this.SearchType == SearchType.Xml)
            {
                query = this.ComicVineAddress + "issue/4000-" + issueId.ToString() + "/?api_key=" + this.ComicVineKey + "&format=xml&field_list=id,api_detail_url,description,image,issue_number,name,person_credits,character_credits,cover_date,volume";
            }
            else
            {
                query = this.ComicVineAddress + "issue/4000-" + issueId.ToString() + "/?api_key=" + this.ComicVineKey + "&format=json&field_list=id,api_detail_url,description,image,issue_number,name,person_credits,character_credits,cover_date,volume";
            }

            Task<ComicVineResponse> firstResponse = ComicVineConnection.ConnectAndRequest(query);

            comicVineIssue = ComicVineReader.GetIssue(this.SearchType, firstResponse.Result.Response, true);


            return comicVineIssue;
        }

        public ComicVineIssue GetComicVineIssue(int volumeId, int issueNumber)
        {
            ComicVineIssue comicVineIssue = new ComicVineIssue();

            string query = null;

            if (this.SearchType == SearchType.Xml)
            {
                query = this.ComicVineAddress + "issues/?api_key=" + this.ComicVineKey + "&format=xml&field_list=id,api_detail_url,issue_number,cover_date,name,image,person_credits,character_credits,volume&filter=issue_number:" + issueNumber.ToString() + ",volume:" + volumeId.ToString();
            }
            else
            {
                query = this.ComicVineAddress + "issues/?api_key=" + this.ComicVineKey + "&format=json&field_list=id,api_detail_url,issue_number,cover_date,name,image,person_credits,character_credits,volume&filter=issue_number:" + issueNumber.ToString() + ",volume:" + volumeId.ToString();
            }

            Task<ComicVineResponse> firstResponse = ComicVineConnection.ConnectAndRequest(query);

            comicVineIssue = ComicVineReader.GetIssue(this.SearchType, firstResponse.Result.Response, issueNumber, false);

            if (comicVineIssue.id > 0)
            {
                return GetComicVineIssue(comicVineIssue.id);
            }
            else
            {
                return comicVineIssue;
            }

        }

    }

    public enum MatchType
    {
        AbsoluteMatch,
        PartialMatch
    }

    public enum SearchType
    {
        Json,
        Xml
    }
}
