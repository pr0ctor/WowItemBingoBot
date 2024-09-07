using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBingoBot.Model
{
    public class BingoCard(int Id, string SessionCode, string Layout, string BoardStateCode, bool Completed, ulong Owner)
    {
        public int Id { get; set; } = Id;
        public string SessionCode { get; set; } = SessionCode;
        public string Layout { get; set; } = Layout;
        public string BoardStateCode { get; set; } = BoardStateCode;
        public bool Completed { get; set; } = Completed;
        public ulong Owner {  get; set; } = Owner;

    }
}
