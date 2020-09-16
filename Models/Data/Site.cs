using System;
using System.Collections.Generic;

namespace JNCC.Microsite.GCR.Models.Data
{
    public class Site
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string BlockCode { get; set; }
        public string GridRef { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string FilePath { get; set; }
    }

}