using MessagePack;
using System;

namespace PostTechnology.DataAccess.EntityFramework.Entities
{
    [MessagePackObject]
    public class TxMessage : Entity
    {
        [Key(1)]
        public DateTime Sent { get; set; }
        [Key(2)]
        public string Content { get; set; }
        [Key(3)]
        public string Hash { get; set; }

        public override string ToString()
        {
            return $"TxMessage, Number: {Number}, Sent: {Sent}, Content: {Content}, Hash: {Hash}";
        }
    }
}
