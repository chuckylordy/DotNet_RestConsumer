using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaKro.Libraries.RestConsumer.Implementation.Webservice.AuthClasses
{
    internal class User
    {
        private string _password;
        private string _userName;

        public User(string aiUserName, string aiPassword)
        {
            this._userName = aiUserName;
            this._password = aiPassword;
        }

        public string password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }

        public string username
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }
    }
}
