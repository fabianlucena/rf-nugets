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
        public string AuthorizationToken { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; } = DateTime.MinValue;
        public string AutoLoginToken { get; set; } = string.Empty;
        public DateTime LastUsedAt { get; set; } = DateTime.MinValue;
        public DateTime? ClosedAt { get; set; } = null;

        public long UserId { get; set; }
        public User? User { get; set; }

        public long DeviceId { get; set; }
        public Device? Device { get; set; }

        [NotMapped]
        private DataDictionary data = [];

        public string? DataJson
        {
            get
            {
                var json = JsonSerializer.Serialize(data);
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return json;
            }

            set
            {
                if (value is null)
                    data = [];
                else
                    data = JsonSerializer.Deserialize<DataDictionary>(value) ?? [];
            }
        }

        [NotMapped]
        public IDataDictionary? Data
        {
            get => data;
            set => data = (DataDictionary)(value ?? new DataDictionary());
        }

        [NotMapped]
        public DataDictionary DataResponse = [];

        public Session() { }

        public Session(Session? session = null)
            :base(session)
        {
            if (session == null)
                return;

            AuthorizationToken = session.AuthorizationToken;
            ExpireAt = session.ExpireAt;
            AutoLoginToken = session.AutoLoginToken;
            LastUsedAt = session.LastUsedAt;
            ClosedAt = session.ClosedAt;

            UserId = session.UserId;
            User = session.User;

            DeviceId = session.DeviceId;
            Device = session.Device;

            data = new DataDictionary(session.data);
            DataResponse = new DataDictionary(session.DataResponse);
        }
    
        public override Session Clone()
            => new(this);
    }
}
