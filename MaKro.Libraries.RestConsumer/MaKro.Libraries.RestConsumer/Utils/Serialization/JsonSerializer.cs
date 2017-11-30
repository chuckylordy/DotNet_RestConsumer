using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MaKro.Libraries.RestConsumer.Utils.Serialization
{

    public class JsonSerializer : AbstractJsonSerializer
    {
        private Newtonsoft.Json.JsonSerializer SERIALIZER;
        private Formatting _formatting = Formatting.Indented;
        /// <summary>
        /// Set/Get the formatting for the serializer. Options are:
        /// Formatting.None, Formatting.Indented
        /// </summary>
        public Formatting Formatting
        {
            get { return _formatting; }
            set { _formatting = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public JsonSerializer()
        {
            SERIALIZER = new Newtonsoft.Json.JsonSerializer();
            SERIALIZER.Converters.Add(new JavaScriptDateTimeConverter());
            SERIALIZER.NullValueHandling = NullValueHandling.Ignore;
            SERIALIZER.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            SERIALIZER.MissingMemberHandling = MissingMemberHandling.Ignore;
            //SERIALIZER.TraceWriter = new MemoryTraceWriter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public override T ReadObject<T>(Stream input)
        {
            JsonReader reader = new JsonTextReader(new StreamReader(input));
            T obj = SERIALIZER.Deserialize<T>(reader);
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="output"></param>
        /// <param name="obj"></param>
        public override void WriteObject<T>(Stream output, T obj)
        {
            JsonWriter writer = new JsonTextWriter(new StreamWriter(output));
            SERIALIZER.Serialize(writer, obj);
            writer.Flush();
            output.Position = 0;
            //Console.WriteLine(SERIALIZER.TraceWriter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public String Serialize<T>(T obj)
        {
            String json = "";
            using (MemoryStream ms = new MemoryStream())
            {
                JsonWriter writer = new JsonTextWriter(new StreamWriter(ms));
                writer.Formatting = Formatting;
                SERIALIZER.Serialize(writer, obj);
                json = Encoding.UTF8.GetString(ms.ToArray());
                return json;
            }
        }

        /// <summary>
        /// Gets a property of the given jsonString.
        /// </summary>
        /// <param name="aiJsonString"></param>
        /// <param name="aiPropertyname"></param>
        /// <returns></returns>
        public override T GetProperty<T>(string aiJsonString, string aiPropertyname)
        {
            if (HasProperty(aiJsonString, aiPropertyname) == false)
                return default(T);

            var t = JObject.Parse(aiJsonString);
            var s = t.Property(aiPropertyname);

            return s.Value.ToObject<T>();
        }

        /// <summary>
        /// Checks if the given jsonString has the specified property
        /// </summary>
        /// <param name="aiJsonString"></param>
        /// <param name="aiPropertyname"></param>
        /// <returns></returns>
        public override bool HasProperty(string aiJsonString, string aiPropertyname)
        {
            var t = JObject.Parse(aiJsonString);

            if (t == null)
                return false;

            var s = t.Property(aiPropertyname);

            if (s == null)
                return false;

            return true;

        }
    }
}
