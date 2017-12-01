using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MaKro.Libraries.RestConsumer.Utils
{
    public static class MediaTypes
    {
        //Default-json-generic: application/json
        private const string JsonGeneric = "application/json";


        public static MediaTypeWithQualityHeaderValue JSON_GENERIC_MEDIA_TYPE
        {
            get { return new MediaTypeWithQualityHeaderValue(JsonGeneric); }
        }
    }
}
