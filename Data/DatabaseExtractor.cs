using System;
using System.Collections.Generic;
using System.IO;
using JNCC.Microsite.GCR.Data;
using JNCC.Microsite.GCR.Models.Data;
using JNCC.Microsite.GCR.Helpers;
using Newtonsoft.Json;
using System.Linq;

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

            Console.WriteLine("Extracting site list");
            List<Site> sites = dbOps.GetSiteList();
            FileHelper.WriteJSONToFile(FileHelper.GetActualFilePath(outputRoot, outputBasePath, "sites.json"), sites);
            Console.WriteLine(String.Format("Extracted {0} GCR sites", sites.Count));

            Console.WriteLine("Extracting block list");
            List<Block> blocks = dbOps.GetBlockList();
            List<Block> blocksWithSites = GetBlocksWithSiteLists(blocks, sites);
            FileHelper.WriteJSONToFile(FileHelper.GetActualFilePath(outputRoot, outputBasePath, "blocks.json"), blocksWithSites);
            Console.WriteLine(String.Format("Extracted {0} GCR blocks", blocksWithSites.Count));

        }

        private static List<Block> GetBlocksWithSiteLists(List<Block> blocks, List<Site> sites)
        {
            var blocksWithSites = new List<Block>();

            var groupedSites = sites.GroupBy(site => site.BlockCode);
            foreach (var group in groupedSites)
            {
                var block = blocks.Single(b => b.Code == group.Key);
                block.Sites = group.ToList();
                blocksWithSites.Add(block);
            }
            return blocksWithSites;
        }
    }
}