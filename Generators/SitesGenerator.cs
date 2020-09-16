using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using JNCC.Microsite.GCR.Helpers;

namespace JNCC.Microsite.GCR.Generators
{
    public static class SitesGenerator
    {
        public static void Generate(IServiceScopeFactory serviceScopeFactory, GeneratorConfig config, string basePath, bool generateSearchDocuments, string searchIndex)
        {
            FileHelper.WriteToFile(FileHelper.GetActualFilePath(basePath, "output/html/index.html"), "Hello world 2");

            Console.WriteLine("Generated index page");
        }
    }
}