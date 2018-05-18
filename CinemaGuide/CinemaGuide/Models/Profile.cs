using System;
using System.Collections.Generic;
using CinemaGuide.Api;
using CinemaGuide.Models.Db;

namespace CinemaGuide.Models
{
    public class Profile
    {
        public DbProfile    UserProfile     { get; set; }
        public Credentials  UserCredentials { get; set; }
        public SearchConfig SearchConfig    { get; set; }
        public List<Type>   SourceList      { get; set; }
    }
}
