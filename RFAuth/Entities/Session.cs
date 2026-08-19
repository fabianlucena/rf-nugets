using RFBase.ILibs;
using RFBase.Libs;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RFAuth.Entities
{
    [Table("Sessions", Schema = "auth")]
    public sealed class Session : CreatableEntity
    {
        public string IdentityProvider { get; set; } = string.Empty;
        public string AuthorizationToken { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; } = DateTime.MinValue;
        public string AutoLoginToken { get; set; } = string.Empty;
        public DateTime LastUsedAt { get; set; } = DateTime.MinValue;
        public DateTime? ClosedAt { get; set; } = null;

        public long UserId { get; set; }
        public User? User { get; set; }

        public long DeviceId { get; set; }
        public Device? Device { get; set; }

        public string? DataJson
        {
            get
            {
                var json = JsonSerializer.Serialize(Data);
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return json;
            }

            set
            {
                Data = value is not null 
                    ? JsonSerializer.Deserialize<DataDictionary>(value) ?? []
                    : [];
            }
        }

        [NotMapped]
        public IDataDictionary Data { get; set; } = new DataDictionary();

        [NotMapped]
        public IDataDictionary InternalData { get; set; } = new DataDictionary();

        [NotMapped]
        public IDataDictionary ResponseData { get; set; } = new DataDictionary();

        public Session() { }

        public Session(Session? session = null)
            :base(session)
        {
            if (session == null)
                return;

            IdentityProvider = session.IdentityProvider;
            AuthorizationToken = session.AuthorizationToken;
            ExpireAt = session.ExpireAt;
            AutoLoginToken = session.AutoLoginToken;
            LastUsedAt = session.LastUsedAt;
            ClosedAt = session.ClosedAt;

            UserId = session.UserId;
            User = session.User;

            DeviceId = session.DeviceId;
            Device = session.Device;

            Data = new DataDictionary(session.Data);
            InternalData = new DataDictionary(session.InternalData);
            ResponseData = new DataDictionary(session.ResponseData);
        }
    
        public override Session Clone()
            => new(this);
    }
}
