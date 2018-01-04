using System;
using System.IO;
using CoreServiceUtils.Extensions;
using CoreServiceUtils.Interfaces;
using CoreServiceUtils.Models;
using CoreServiceUtils.Options;
using CoreServiceUtils.Services;
using Tridion.ContentManager.CoreService.Client;

namespace CoreServiceUtils
{
    class Program
    {
        private static readonly IHostWriter Writer = new ConsoleWriterService();
        private static ITridionClientService _clientService;


        static void Main(string[] args)
        {
            var programOptions = new ProgramOptions();

            var parser = new CommandLine.Parser(s =>
            {
                s.IgnoreUnknownArguments = false;
            });


            if (parser.ParseArguments(args, programOptions)
                && Init(programOptions))
            {
                Run();

                if (programOptions.Prompt)
                {
                    Writer.Write($"", false);
                    Writer.Write("Press any key to exit...", false);
                    Console.ReadKey();
                }

            }
            else
            {
                Writer.Write(programOptions.GetUsage());
            }

        }

        private static void Run()
        {
            _clientService.Using(service =>
            {
                // Execute Tridion Actions e.g.

                var component = service.Get<ComponentData>("{TcmId or Webdav Path}");

                var componentList = service.GetList<ComponentData>("{TcmId or Webdav Path}");

                var schemaFields = service.ReadSchemaFields("{TcmId or Webdav Path}");

                var componentDefaultData = service.GetDefaultData<ComponentData>(ItemType.Component, "{TcmId or Webdav Path}");

                var checkedInComponent = service.CheckIn<VersionedItemData>("{TcmId or Webdav Path}");

                var savedComponent = service.Save(component);

                var checkedOutComponent = service.CheckOut<VersionedItemData>("{TcmId or Webdav Path}", true);

                var existing = service.IsExistingObject("{TcmId or Webdav Path}");

                var keywordListFromKey = service.FindKeywordFromKey("keyword-key", "{TcmId or Webdav Path}");

                var keywordListFromTitle = service.FindKeywordFromTitle("Keyword Title", "{TcmId or Webdav Path}");

                service.Delete("{TcmId or Webdav Path}");
            });
        }

        private static bool Init(ProgramOptions options)
        {
            if (File.Exists(options.ConfigurtaionPath))
            {
                var serverConfiguration = options.ConfigurtaionPath.LoadFromFile<ServerConfiguration>();

                _clientService = new TridionClientService(serverConfiguration);
            }
            else
            {
                Writer.Error("No source server specified");
                return false;
            }

            return true;
        }

    }
}
