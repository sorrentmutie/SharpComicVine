using SharpComicVine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharpComicVine
{
    static class ComicVineConnection
    {
        public async static Task<ComicVineResponse> ConnectAndRequest(string query)
        {
            ServicePointManager.Expect100Continue = false;
           
            ComicVineResponse comicVineResponse = new ComicVineResponse();

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(query);
            httpWebRequest.UserAgent = ".NET Framework Test Client";
            httpWebRequest.Proxy = null;

            using (WebResponse webResponse = await httpWebRequest.GetResponseAsync())
            using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                comicVineResponse.Status = "OK";
                comicVineResponse.Response = streamReader.ReadToEnd();
                
                webResponse.Close();
            }

            return comicVineResponse;
        }
    }
}
