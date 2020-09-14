using System;
using System.Runtime.InteropServices;

namespace PostTechnology.DataAccess.EntityFramework.Entities
{
    public class RxMessage : Entity
    {
        public DateTime Sent { get; set; }
        public string Content { get; set; }
        public string Hash { get; set; }
        public DateTime Received { get; set; }

        public override string ToString()
        {
            return $"RxMessage, Number: {Number}, Sent: {Sent}, Content: {Content}, Hash: {Hash}, Received: {Received}";
        }
    }
}
