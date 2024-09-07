using DiscordBingoBot.Model;
using DSharpPlus.Commands.ContextChecks.ParameterChecks;
using DSharpPlus.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBingoBot.ParameterChecks
{
    internal class FilenameFiletypeCheck : IParameterCheck<FilenameFiletypeAttribute>
    {
        public ValueTask<string?> ExecuteCheckAsync(FilenameFiletypeAttribute attribute, ParameterCheckInfo info, CommandContext context)
        {
            if (info.Value is null)
            {
                return ValueTask.FromResult<string?>(Messages.InvalidDiscordAttachmentParameter);
            }
            if (info.Value is not DiscordAttachment)
            {
                return ValueTask.FromResult<string?>(Messages.InvalidDiscordAttachmentParameter);
            }

            var attachment = info.Value as DiscordAttachment;
            var filename = attachment!.FileName ?? "";

            var filetype = (filename).Substring(filename.LastIndexOf('.')).ToLower();

            var fileMimeType = MimeTypes.GetMimeType(filetype.Trim().ToLower());

            if (!attribute.FileMimeTypes.Contains(fileMimeType)) return ValueTask.FromResult<string?>(Messages.InvalidFileTypeUploaded(filename, attribute.FileMimeTypes));

            return ValueTask.FromResult<string?>(null);
        }
    }
}
