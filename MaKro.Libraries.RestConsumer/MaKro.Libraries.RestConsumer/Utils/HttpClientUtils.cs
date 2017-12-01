using MaKro.Libraries.RestConsumer.Utils.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MaKro.Libraries.RestConsumer.Utils
{
    public static class HttpClientUtils
    {

        public static HttpRequestMessage CreateBlankRequestMessage(HttpMethod aiMethod, string aiRessource)
        {
            return new HttpRequestMessage(aiMethod, aiRessource);
        }

        public static HttpRequestMessage AddContentToRequest<T>(this HttpRequestMessage aiRequestMessage,T aiContent)
        {
            HttpRequestMessage aoRequest = aiRequestMessage;
            using (MemoryStream ms = new MemoryStream())
            {
                SerializationContainer.JsonSerializer.WriteObject(ms, aiContent);
                byte[] requestInJson = ms.ToArray();
                aoRequest.Content = new ByteArrayContent(requestInJson);
                aoRequest.Content.Headers.ContentType = MediaTypes.JSON_GENERIC_MEDIA_TYPE;
                aoRequest.Headers.Accept.Add(MediaTypes.JSON_GENERIC_MEDIA_TYPE);
            }
            return aoRequest;
        }

        public static HttpRequestMessage AddHeaderToRequest(this HttpRequestMessage aiRequestMessage, string aiKey, IList<string> aiValue)
        {
            HttpRequestMessage aoRequest = aiRequestMessage;
            if (aiValue.Count == 1)
                aoRequest.Headers.Add(aiKey, aiValue[0]);
            else if (aiValue.Count > 1)
                aoRequest.Headers.Add(aiKey, aiValue);
            else//Add an emtpy string as value to the given key
                aoRequest.Headers.Add(aiKey, String.Empty);
            return aoRequest;
        }
    }
}
