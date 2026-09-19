using RFService.Repo;

namespace RFDapper
{
    public class UpdateQueryOptions
    {
        public string Schema { get; set; } = "";
        public string TableName { get; set; } = "";
        public string TableAlias { get; set; } = "";
        public string Set { get; set; } = "";
        public string Joins { get; set; } = "";
        public string Where { get; set; } = "";
        public string TruncatedJoins { get; set; } = "";
        public string FirstJoinCondition { get; set; } = "";
    }
}
