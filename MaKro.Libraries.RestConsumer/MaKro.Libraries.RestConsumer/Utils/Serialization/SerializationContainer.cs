using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaKro.Libraries.RestConsumer.Utils.Serialization
{
    public static class SerializationContainer
    {
        private static AbstractJsonSerializer _jsonSerializer;

        /// <summary>
        /// Defines the class used for Json Serialization
        /// </summary>
        public static AbstractJsonSerializer JsonSerializer
        {
            get
            {
                if (_jsonSerializer == null)
                    _jsonSerializer = new Utils.Serialization.JsonSerializer();
                return _jsonSerializer;
            }
            set
            {
                _jsonSerializer = value;
            }
        }
    }
}
