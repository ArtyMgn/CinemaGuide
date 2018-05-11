using System;
using System.Collections.Generic;
using CinemaGuide.Api;

namespace CinemaGuide.Models
{
    public class Profile
    {
        public bool         IsAuth       { get; set; }
        public SearchConfig SearchConfig { get; set; }
        public List<Type>   SourceList   { get; set; }
    }
}
