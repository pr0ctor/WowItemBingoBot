using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.Model
{
    public class GearItem(int Id, int ItemId, string ItemName, string ItemImageUrl, string ItemImageName)
    {
        public int Id { get; } = Id;
        public int ItemId { get; } = ItemId;
        public string ItemName { get; } = ItemName;
        public string ItemImageUrl { get; } = ItemImageUrl;
        public string ItemImageName { get; } = ItemImageName;
    }
}
