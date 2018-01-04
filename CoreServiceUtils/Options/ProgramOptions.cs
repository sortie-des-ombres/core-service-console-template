using CommandLine;
using CoreServiceUtils.Models;

namespace CoreServiceUtils.Options
{
    public class ProgramOptions : BaseOptions
    {
        [Option('c', "configuration", Required = true,
             HelpText = "Path to the application configuration file.")]
        public string ConfigurtaionPath { get; set; }

        [Option('p', "prompt", Required = false,
             HelpText = "Prompt before exiting")]
        public bool Prompt { get; set; }
    }
}
