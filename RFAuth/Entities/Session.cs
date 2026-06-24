using RFAuth.DTO;
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

        public string? DataJson
        {
            get
            {
                var json = JsonSerializer.Serialize(Data.ToDictionary());
                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return json;
            }

            set
            {
                if (value is not null)
                    Data = JsonSerializer.Deserialize<SessionData>(value) ?? new SessionData();
                else
                    Data = new SessionData();
            }
        }

        [NotMapped]
        public SessionData Data { get; set; } = new SessionData();

        [NotMapped]
        public SessionData DataResponse { get; set; } = new SessionData();

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

            Data = session.Data.Clone();
            DataResponse = session.Data.Clone();
        }
    
        public override Session Clone()
            => new(this);
    }
}
