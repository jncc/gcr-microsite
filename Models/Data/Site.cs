using System;
using System.Collections.Generic;
using System.IO;

namespace JNCC.Microsite.GCR.Models.Data
{
    public class Site
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string UnitaryAuthority { get; set; }
        public string Country { get; set; }
        public string GridReference { get; set; }
        public string ReportFilePath { get; set; }
        public string BlockCode { get; set; }
        public string BlockName { get; set; }
        public Publication Publication { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class Block
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Publication Publication { get; set; }
        public List<Site> Sites { get; set; }
    }

    public class Publication
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string VolumeFilePath { get; set; }
    }

}