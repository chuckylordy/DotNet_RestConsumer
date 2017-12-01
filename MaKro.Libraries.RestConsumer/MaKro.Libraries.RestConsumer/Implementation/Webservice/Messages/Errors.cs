using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaKro.Libraries.RestConsumer.Implementation.Webservice.Messages
{
    public static class Errors
    {
        /// <summary>
        /// Not authorized
        /// </summary>
        public static string Not_Authorized
        {
            get { return "User {0} is not authorized!"; }
        }

        public static string Client_Error_AuthenticationToken_Missing
        {
            get { return "Client-Error: Authentication-Token is not set yet. Please authenticate first!"; }
        }

        
    }
}
