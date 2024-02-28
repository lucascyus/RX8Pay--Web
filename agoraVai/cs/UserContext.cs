using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace agoraVai.cs
{
    public class UserContext
    {
        public static string Username { get; private set; }
        public static string Profile { get; private set; }

        public static void SetUser(string username, string profile)
        {
            Username = username;
            Profile = profile;
        }
    }
}