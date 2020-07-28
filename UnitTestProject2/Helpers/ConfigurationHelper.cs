using AutoMapper;
using EmmBlog;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace UnitTestProject
{
    internal sealed class ConfigurationHelper : Singleton<ConfigurationHelper>
    {
        public IConfigurationRoot Config { get; }

        public ConfigurationHelper()
        {
            try {
                
                string fulldirectory = Directory.GetCurrentDirectory();
                Regex regex = new Regex("(.*)(?:\\\\|/)bin.*$");
                Match match = regex.Match(fulldirectory);

                string directory = match.Groups[1].Value;


                IConfigurationBuilder confBuilder = new ConfigurationBuilder()
                    .SetBasePath(directory)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
                    .AddEnvironmentVariables();
                
                Config = confBuilder.Build();

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public static IConfiguration GetApplicationConfiguration(string section = null)
        {
            if(section != null)
            {
                return Instance.Config.GetSection(section);
            }
            
            return Instance.Config;
        }

        public class BlogConfiguration
        {
            public BlogConfiguration(){}

            public ProviderSettings[] Providers {get;set;} 
        }
    }
}
