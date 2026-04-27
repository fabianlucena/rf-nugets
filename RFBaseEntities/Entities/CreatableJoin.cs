namespace RFBaseEntities.Entities
{
    public class CreatableJoin : Join
    {
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public long CreatedById { get; set; } = 0;
        public User? CreatedBy { get; set; } = null;
    }
}
