using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MaKro.Libraries.RestConsumer.Utils
{
    public static class ObjectSerializer
    {
        /// <summary>
        /// Serializes the object to a json-object containing all properties of the object with their values.
        /// </summary>
        /// <param name="aiObject"></param>
        /// <returns></returns>
        public static string Serialize(object aiObject)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (NullJsonWriter njw = new NullJsonWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer();
                ser.Formatting = Formatting.Indented;
                ser.Serialize(njw, aiObject);
            }
            return sb.ToString();
        }

        public class NullJsonWriter : JsonTextWriter
        {
            public NullJsonWriter(TextWriter writer) : base(writer)
            {
            }
            public override void WriteNull()
            {
                base.WriteValue(string.Empty);
            }
        }

    }    
}
