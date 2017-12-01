using MaKro.Libraries.RestConsumer.Utils;
using MaKro.Libraries.RestConsumer.Utils.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MaKro.Libraries.RestConsumer.Implementation.Webservice
{
    public class RestClient_Spring_Template : IClient
    {
        private string _baseUri;
        private string _userName;
        private bool _disposed;
        private HttpClient _httpClient;
        private string _token;

        /// <summary>
        /// Provides a generic contructor for "bring you own HttpClient" which should allow 
        /// bringing your own authentication. This constuctor is also need to provide a COM 
        /// interface for this class.
        /// </summary>
        public RestClient_Spring_Template() { }

        /// <summary>
        /// This constructor will use the current process owner's kerberos credentials to login.
        /// This is useful for VBA, CommandLine utilities, or services that will use a service
        /// account to login. It is not for Kerberos delegated credentials.
        /// </summary>
        /// <param name="timeOutMinutes"></param>
        public RestClient_Spring_Template(int timeOutMinutes, string aiBaseUri)
        {
            _baseUri = aiBaseUri;
            Init(timeOutMinutes);
        }

        /// <summary>
        /// Initilizes a BasicAuth controller using the provided username and password. Uses the default 
        /// 5 minute timeout for http responses.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public RestClient_Spring_Template(string userName, string password, string aiBaseUri)
        {
            _baseUri = aiBaseUri;

            // Default of 5 minutes for a http response timeout.
            if (String.IsNullOrWhiteSpace(userName) || (password == null))
                throw new Exception("Username and/or password should not be empty!");
            else Init(userName, password, 5);
        }

        /// <summary>
        /// Initializes the specified client with the specified timeout. Default timeout = 5 min if not specified
        /// </summary>
        /// <param name="aiClient"></param>
        /// <param name="aiTimeoutMinutes"></param>
        public void Init(HttpClient aiClient, int aiTimeoutMinutes = 5)
        {
            this._httpClient = aiClient;
            _httpClient.Timeout = new TimeSpan(0, aiTimeoutMinutes, 0);
        }

        public void Init(int aiTimeoutMinutes)
        {
            WebRequestHandler hdl = new WebRequestHandler();
            hdl.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;//SSL
            hdl.UseDefaultCredentials = true;
            _httpClient = new HttpClient(hdl);
            _httpClient.Timeout = new TimeSpan(0, aiTimeoutMinutes, 0);
        }
    
        public void Init(string aiUsername, string aiPassword, int aiTimeout = 5)
        {
            WebRequestHandler hdl = new WebRequestHandler();
            hdl.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(hdl);
            _userName = aiUsername;
            _httpClient.Timeout = new TimeSpan(0, aiTimeout, 0);
            if (Authenticate(aiUsername, aiPassword) == false)
                throw new Exception("Could not authenticate: Username or password seem to be wrong.");
        }

        public bool Authenticate(string aiUsername, string aiPassword)
        {
            try
            {
                var tokenObj = PostAndReturnHeaders<AuthClasses.User>(BaseUri + LinkRelations.AUTHENTICATE, new AuthClasses.User(aiUsername, aiPassword));

                if (tokenObj == null)
                    return false;

                var entry = tokenObj.FirstOrDefault(dicEntry => dicEntry.Key == "Authorization");

                if (entry.Key == null)
                    return false;
                _userName = aiUsername;
                _token = entry.Value;//Set the token for authentication.
            }
            catch( Exception e)
            {
                return false;
            }        
            return true;
        }

        public string Get(string aiRessource)
        {
            string obj = default(string);
            HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
            HttpRequestMessage request = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Get, aiRessource));
            Task<HttpResponseMessage> response = _httpClient.SendAsync(request, option);
            HttpResponseMessage message = response.Result;
            if (message.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(String.Format(Messages.Errors.Not_Authorized, UserName));
            message.EnsureSuccessStatusCode();
            var result = message.Content.ReadAsStringAsync();
            obj = result.Result;
            return obj;
        }

        public T Get<T>(string aiRessource)
        {
            T obj = default(T);
            HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
            HttpRequestMessage request = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Get, aiRessource));
            Task<HttpResponseMessage> response = _httpClient.SendAsync(request, option);
            HttpResponseMessage message = response.Result;
            if (message.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(String.Format(Messages.Errors.Not_Authorized, UserName));
            message.EnsureSuccessStatusCode();
            Task<Stream> result = message.Content.ReadAsStreamAsync();
            obj = SerializationContainer.JsonSerializer.ReadObject<T>(result.Result);
            return obj;
        }

        public Stream GetRaw(string aiRessource)
        {
            HttpRequestMessage request = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Get, aiRessource));
            HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
            Task<HttpResponseMessage> response = _httpClient.SendAsync(request, option);
            HttpResponseMessage message = response.Result;
            return message.Content.ReadAsStreamAsync().Result;
        }

        public bool IsConnected()
        {
            if (String.IsNullOrEmpty(_token))
                return false;
            return true;
        }

        public string Post<T>(string aiRessource, T aiRequestBody)
        {
            string obj = default(string);
            using (MemoryStream ms = new MemoryStream())
            {
                SerializationContainer.JsonSerializer.WriteObject(ms, aiRequestBody);
                byte[] requestInJson = ms.ToArray();
                HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
                HttpRequestMessage request = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Post, aiRessource).AddContentToRequest(aiRequestBody));
                Task<HttpResponseMessage> response = _httpClient.SendAsync(request, option);
                HttpResponseMessage message = response.Result;
                string test = message.Content.ReadAsStringAsync().Result;//Save content temporarly for messaging-process.
                message.EnsureSuccessStatusCode();
                if (message.Content != null)
                {
                    var result = message.Content.ReadAsStringAsync();
                    obj = result.Result;
                }
            }
            return obj;
        }

        public R Post<T, R>(string aiRessource, T aiRequestBody)
        {
            R obj = default(R);
            using (MemoryStream ms = new MemoryStream())
            {
                SerializationContainer.JsonSerializer.WriteObject(ms, aiRequestBody);                byte[] requestInJson = ms.ToArray();
                HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
                HttpRequestMessage request = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Post, aiRessource).AddContentToRequest(aiRequestBody));
                Task<HttpResponseMessage> response = _httpClient.SendAsync(request, option);
                HttpResponseMessage message = response.Result;
                string contentForDebug = message.Content.ReadAsStringAsync().Result;//Save content temporarly for messaging-process.
                message.EnsureSuccessStatusCode();
                if (message.Content != null)
                {
                    var result = message.Content.ReadAsStreamAsync();
                    obj = SerializationContainer.JsonSerializer.ReadObject<R>(result.Result);
                }
            }
            return obj;
        }

        public Dictionary<string, string> PostAndReturnHeaders<T>(string aiRessource, T aiRequestBody)
        {
            Dictionary<string, string> aoDic = new Dictionary<string, string>();
            using (MemoryStream ms = new MemoryStream())
            {
                SerializationContainer.JsonSerializer.WriteObject(ms, aiRequestBody);
                byte[] requestInJson = ms.ToArray();
                HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
                HttpRequestMessage request = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Post, aiRessource).AddContentToRequest(aiRequestBody));
                Task<HttpResponseMessage> response = _httpClient.SendAsync(request, option);
                try
                {
                    HttpResponseMessage message = response.Result;
                    message.EnsureSuccessStatusCode();
                    foreach (var header in message.Headers)
                        aoDic.Add(header.Key, header.Value.First());
                }
                catch (Exception e)
                {
                    //ToDo: Logging.
                    return null;
                }
            }
            return aoDic;
        }

        public HttpStatusCode PostMultiparts(string aiRessource, IList<HttpContent> aiContentList, out string aoMessage)
        {
            using (var multiPartStream = new MultipartFormDataContent())
            {
                foreach (var content in aiContentList)
                {
                    multiPartStream.Add(content);
                }
                HttpRequestMessage requestMessage = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Post, aiRessource));
                requestMessage.Content = multiPartStream;
                HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
                Task<HttpResponseMessage> response = _httpClient.SendAsync(requestMessage, option);
                HttpResponseMessage message = response.Result;
                aoMessage = message.Content.ReadAsStringAsync().Result;//Save content temporarly and return it.
                return message.StatusCode;
            }
        }

        public HttpStatusCode Put(string aiRessource)
        {
            HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
            HttpRequestMessage request = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Put,aiRessource));
            Task<HttpResponseMessage> response = _httpClient.SendAsync(request, option);
            HttpResponseMessage message = response.Result;
            return message.StatusCode;
        }

        public HttpStatusCode Put(string aiRessource, Dictionary<string, string> aiHeaders)
        {
            HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
            HttpRequestMessage request = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Put, aiRessource));
            foreach (var header in aiHeaders)
                request.Headers.Add(header.Key, header.Value);
            Task<HttpResponseMessage> response = _httpClient.SendAsync(request, option);
            HttpResponseMessage message = response.Result;
            return message.StatusCode;
        }

        public HttpStatusCode Put(string aiRessource, string aiQuery)
        {
            HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
            HttpRequestMessage request = AddBasicAuthentication(HttpClientUtils.CreateBlankRequestMessage(HttpMethod.Put, aiRessource));
            Task<HttpResponseMessage> response = _httpClient.SendAsync(request, option);
            HttpResponseMessage message = response.Result;
            return message.StatusCode;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _httpClient.Dispose();
            _disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        #region Getter/Setter

   
        /// <summary>
        /// Gets the service uri.
        /// </summary>
        public string BaseUri { get { return _baseUri; } set { _baseUri = value; } }

        /// <summary>
        /// Gets the current username.
        /// </summary>
        public string UserName { get { return _userName; } }
        #endregion;



        #region CustomCode

        private HttpRequestMessage AddBasicAuthentication(HttpRequestMessage aiRequestMessage)
        {
            if(_token != null)
                return aiRequestMessage.AddHeaderToRequest("Authorization", new List<string>() { _token });
            return aiRequestMessage;
        }

        #endregion

    }
}
