using System;
using System.Collections.Generic;
using System.IO;
using JNCC.Microsite.GCR.Data;
using JNCC.Microsite.GCR.Models.Data;
using JNCC.Microsite.GCR.Helpers;
using Newtonsoft.Json;

namespace JNCC.Microsite.GCR.Data
{
    public static class DatabaseExtractor 
    {
        public static void ExtractData(string accessDbPath, string outputRoot) 
        {
            Console.WriteLine(String.Format("Updating data files using: {0}", accessDbPath));
            
            var outputBasePath = "output/json";
            
            DatabaseOperations dbOps = new DatabaseOperations(accessDbPath);
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            Console.WriteLine("Extracting main GCR list");
            List<Site> sites = dbOps.GetFullGCRList();
            FileHelper.WriteJSONToFile(FileHelper.GetActualFilePath(outputRoot, outputBasePath, "sites.json"), sites);
            Console.WriteLine(String.Format("Extracted {0} GCR sites", sites.Count));
        }
    }
}