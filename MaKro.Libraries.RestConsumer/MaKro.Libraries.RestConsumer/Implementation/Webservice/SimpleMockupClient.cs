using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MaKro.Libraries.RestConsumer.Implementation.Webservice
{
    public class SimpleMockupClient : IClient
    {
        string IClient.Get(string aiRessource)
        {
            return "MockUp: Successfully get the ressource " + aiRessource;
        }

        T IClient.Get<T>(string aiRessource)
        {
            return default(T);
        }

        public HttpRequestMessage GetPostRequest<T>(string aiRessource, T aiRequestBody)
        {
            return new HttpRequestMessage(HttpMethod.Post, aiRessource);
        }

        public Stream GetRaw(string aiRessource)
        {
            return null;
        }

        public HttpRequestMessage GetGetRequest(string aiRessource)
        {
            return new HttpRequestMessage(HttpMethod.Get, aiRessource);
        }

        public void Init()
        {}

        public void Init(string aiUsername, string aiPassword, int aiTimeout = 5)
        {}

        public bool IsConnected()
        {
            return true;
        }

        public void Init(HttpClient aiClient, int aiTimeoutMinutes = 5)
        {
            
        }

        public void Init(int aiTimeoutMinutes)
        {
          
        }

        public bool Authenticate(string aiUsername, string aiPassword)
        {
            return true;
        }

        public string Post<T>(string aiRessource, T aiRequestBody)
        {
            return "MockUp: Successfully posted to the ressource " + aiRessource;
        }

        public R Post<T, R>(string aiRessource, T aiRequestBody)
        {
            return default(R);
        }

        public Dictionary<string, string> PostAndReturnHeaders<T>(string aiRessource, T aiRequestBody)
        {
            return new Dictionary<string, string>();
        }

        public HttpStatusCode PostMultiparts(string aiRessource, IList<HttpContent> aiContentList, out string aoMessage)
        {
            aoMessage = "MockUp: Successfully posted multiparts to the ressource " + aiRessource;
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Put(string aiRessource)
        {
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Put(string aiRessource, Dictionary<string, string> aiHeaders)
        {
            return HttpStatusCode.OK;
        }

        public HttpStatusCode Put(string aiRessource, string aiQuery)
        {
            return HttpStatusCode.OK;
        }

     
    }
}
