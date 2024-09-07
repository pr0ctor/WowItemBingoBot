using DSharpPlus.Commands.ContextChecks.ParameterChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.ParameterChecks
{
    public sealed class FilenameFiletypeAttribute : ParameterCheckAttribute
    {
        public List<string> FileMimeTypes { get; private set; } = new List<string>();
        // Comma Demilited List -> Make them lowercase
        public FilenameFiletypeAttribute(string fileTypes) =>
             FileMimeTypes = fileTypes
                .Split(',')
                .Select(v => MimeTypes.GetMimeType(v.Trim().ToLower()))
                .Distinct()    
                .ToList();
    }
}
