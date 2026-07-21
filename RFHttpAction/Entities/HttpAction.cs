using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFHttpAction.Entities
{
    [Table("HttpActions", Schema = "action")]
    public class HttpAction
        : AuditableEntity
    {
        [Required]
        [ForeignKey("Type")]
        public long TypeId { get; set; }
        public HttpActionType? Type { get; set; }

        [MaxLength(-1)]
        public string? Data { get; set; }

        [MaxLength(255)]
        public string Token { get; set; } = "";

        public DateTime? ClosedAt { get; set; }

        public HttpAction() { }

        public HttpAction(HttpAction? action)
            : base(action)
        {
            if (action is null)
                return;

            TypeId = action.TypeId;
            Type = action.Type?.Clone();
            Data = action.Data;
            Token = action.Token;
            ClosedAt = action.ClosedAt;
        }

        public override HttpAction Clone()
            => new(this);
    }
}