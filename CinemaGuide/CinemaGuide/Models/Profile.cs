using System;
using System.Collections.Generic;
using CinemaGuide.Api;

namespace CinemaGuide.Models
{
    public class Profile
    {
        public DbProfile    UserProfile     { get; set; }
        public LoginModel   UserCredentials { get; set; }
        public SearchConfig SearchConfig    { get; set; }
        public List<Type>   SourceList      { get; set; }
    }
}
