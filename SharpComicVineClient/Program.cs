using SharpComicVine;
using SharpComicVine.Models;
using SharpComicVine.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SharpComicVineClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Suppose we want extract details about "The Amazing Spider-Man 175" published in December 1977.
            // First we need to extract a list of Volume candidates. 

            string searchedVolume = "The Amazing Spider-Man";
            int issueNumber = 175;

            List<ComicVineIssue> comicVineIssueList = new List<ComicVineIssue>();

            ComicVineService comicVineService = new ComicVineService();
            comicVineService.ComicVineKey = "2afb272275ea5aa2ecc4fe3d2bf68c49c3473b92";

            // Xml Working

            comicVineService.SearchType = SearchType.Json;
            comicVineService.MatchType = MatchType.AbsoluteMatch;

            // Query the service
            List<ComicVineVolume> comicVinevolumes = new List<ComicVineVolume>();
            comicVinevolumes = comicVineService.SearchVolume(searchedVolume);

            if (comicVinevolumes.Count > 0)
            {
                Console.WriteLine("The query returned " + comicVinevolumes.Count.ToString() +   " results");
                foreach (ComicVineVolume volu in comicVinevolumes)
                {
                    Console.WriteLine(volu.id.ToString() + " - " + volu.name);


                    ComicVineIssue issue = new ComicVineIssue();
                    issue = comicVineService.GetComicVineIssue(volu.id, issueNumber);

                    if (issue.id >0)
                    {
                        Console.WriteLine("We have a full match!");
                        Console.WriteLine("");
                        Console.WriteLine("Title: " + issue.name);
                        Console.WriteLine("URL: " + issue.api_detail_url);
                        Console.WriteLine("Issue number: " + issue.issue_number);
                        Console.WriteLine("Id: " + issue.id);

                        Console.WriteLine("----------");
                        Console.WriteLine("# of Credits:" + issue.credits.Count.ToString());
                        foreach (ComicVineCredit credit in issue.credits)
                        {
                            Console.WriteLine(credit.name + ": " + credit.role);
                        }

                        Console.WriteLine("----------");
                        Console.WriteLine("# of Characters:" + issue.characters.Count.ToString());
                        foreach (ComicVineCharacter character in issue.characters)
                        {
                            Console.WriteLine(character.name);
                        }


                    }



                }

            }
            else
            {
                Console.WriteLine("The query returned 0 results");
            }

          
            stopWatch.Stop();
            
            // Get the elapsed time as a TimeSpan value.
            TimeSpan timeSpan = stopWatch.Elapsed;

            // Format and display the TimeSpan value. 
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            Console.WriteLine("");
            Console.WriteLine("RunTime " + elapsedTime);
            Console.WriteLine("");

            Console.ReadLine(); 
        }
    }
}
