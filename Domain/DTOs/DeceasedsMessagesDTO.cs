using Domain.Models;

namespace Domain.DTOs
{
    public class DeceasedsMessagesDTO
    {
        public ICollection<Message> MessageList { get; set; } = new List<Message>();
        public short NrOfFlowers { get; set; }
        public short NrOfWreaths { get; set; }
        public short NrOfCandles { get; set; }

        public static DeceasedsMessagesDTO FromDeceased(Deceased deceased)
        {
            var nrOfWreaths = deceased.MessageList.Count(m => m.ItemType == 1);
            var nrOfFlowers = deceased.MessageList.Count(m => m.ItemType == 2);
            var nrOfCandles = deceased.MessageList.Count(m => m.ItemType == 3);

            return new DeceasedsMessagesDTO
            {
                MessageList = deceased.MessageList,
                NrOfFlowers = (short)nrOfFlowers,
                NrOfWreaths = (short)nrOfWreaths,
                NrOfCandles = (short)nrOfCandles
            };
        }
    }
}
