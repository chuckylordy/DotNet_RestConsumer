using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaKro.Libraries.RestConsumer.Utils
{
    public class LinkRelations
    {
        /// <summary>
        /// The location to authenticate with user and password.
        /// </summary>
        public static string AUTHENTICATE
        {
            get { return "/authenticate"; }
        }

        /// <summary>
        /// The location to append to the repositoryBaseUri for the availability service.
        /// </summary>
        public static string AVAILABLE
        {
            get { return "/rest/health"; }
        }

        /// <summary>
        /// The location to append to the repositoryBaseUri for the upload service.
        /// </summary>
        public static string UPLOAD
        {
            get { return "/upload"; }
        }

    }
}
