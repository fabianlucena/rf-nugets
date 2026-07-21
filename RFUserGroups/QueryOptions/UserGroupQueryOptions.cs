using RFIServices.QueryOptions;

namespace RFUserGroups.QueryOptions;

public sealed class UserGroupQueryOptions : CommonJoinQueryOptions
{
    public bool IncludeUser { get; set; } = false;
    public bool IncludeGroup { get; set; } = false;

    public long? UserId { get; set; }
    public long? GroupId { get; set; }
    public IEnumerable<long>? UserIds { get; set; }    

    public UserGroupQueryOptions() { }

    public UserGroupQueryOptions(UserGroupQueryOptions? options)
        : base(options)
    {
        if (options == null)
            return;

        IncludeUser = options.IncludeUser;
        IncludeGroup = options.IncludeGroup;

        UserId = options.UserId;
        GroupId = options.GroupId;
        UserIds = options.UserIds != null ? [.. options.UserIds] : null;
    }

    public override UserGroupQueryOptions Clone()
        => new(this);
}
