using RFBase.Libs;

namespace RFOauth2Client.Entities
{
    public class Provider
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public Client Client { get; set; } = new();
        public Dictionary<string, Endpoint> Endpoints { get; set; } = [];
        public List<RolesSource>? RolesSources { get; set; }
        public Features? Features { get; set; }
    }
}
