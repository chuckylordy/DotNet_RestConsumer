using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaKro.Libraries.RestConsumer.Utils
{
    public static class UriUtil
    {
        /// <summary>
        /// Build a URI from a base path and query.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string BuildUri(string baseUri, List<KeyValuePair<string, object>> query)
        {
            UriBuilder builder = new UriBuilder(baseUri);
            String existingQuery = builder.Query.StartsWith("?") ? builder.Query.Substring(1) : builder.Query;
            StringBuilder baseQueryString = (existingQuery == null || existingQuery.Trim().Equals(""))
                ? new StringBuilder()
                : new StringBuilder(existingQuery);
            if (query != null && query.Count != 0)
                foreach (KeyValuePair<string, object> pair in query) 
                    if (pair.Value != null)
                        if (baseQueryString.ToString().Equals(""))
                            baseQueryString.Append(pair.Key).Append("=").Append(pair.Value);
                        else
                            baseQueryString.Append("&").Append(pair.Key).Append("=").Append(pair.Value);                    
            builder.Query = baseQueryString.ToString();
            return builder.ToString();
        }

    }
}
