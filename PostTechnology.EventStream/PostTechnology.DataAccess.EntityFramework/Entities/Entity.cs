using MessagePack;

namespace PostTechnology.DataAccess.EntityFramework.Entities
{
    [MessagePackObject]
    public class Entity
    {
        [IgnoreMember]
        public int Id { get; set; }
        [Key(0)] 
        public int Number { get; set; }
    }
}
