using System;
using System.Collections.Generic;
using CinemaGuide.Api;
using CinemaGuide.Models.Db;

namespace CinemaGuide.Models
{
    public class Profile
    {
        public User User { get; set; }
        public Credentials Credentials { get; set; }
        public SearchConfig SearchConfig { get; set; }
        public List<Type> SourceList { get; set; }
    }
}
