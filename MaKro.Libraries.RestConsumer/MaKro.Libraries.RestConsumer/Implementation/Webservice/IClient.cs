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
    public interface IClient
    {
        #region Initialization

        void Init(HttpClient aiClient, int aiTimeoutMinutes = 5);

        void Init(string aiUsername, string aiPassword, int aiTimeout = 5);

        void Init(int aiTimeoutMinutes);
        #endregion

        //ToDo: Authentication with IClient interface or for any client alone?
        bool Authenticate(string aiUsername, string aiPassword);

        #region Get-Methods

        T Get<T>(string aiRessource);
        
        string Get(string aiRessource);

        Stream GetRaw(string aiRessource);

        #endregion

        #region Post-Methods

        R Post<T, R>(string aiRessource, T aiRequestBody);

        Dictionary<string, string> PostAndReturnHeaders<T>(string aiRessource, T aiRequestBody);

        string Post<T>(string aiRessource, T aiRequestBody);

        HttpStatusCode PostMultiparts(string aiRessource, IList<HttpContent> aiContentList, out string aoMessage);

        #endregion

        #region Put-Methods

        HttpStatusCode Put(string aiRessource);

        HttpStatusCode Put(string aiRessource, string aiQuery);

        HttpStatusCode Put(string aiRessource, Dictionary<string, string> aiHeaders);
        #endregion

        bool IsConnected();
    }
}
