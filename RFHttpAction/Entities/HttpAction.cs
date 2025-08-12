using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFHttpAction.Entities
{
    [Table("HttpActions", Schema = "action")]
    public class HttpAction
        : EntityTimestampsIdUuid
    {
        [Required]
        [ForeignKey("Type")]
        public required Int64 TypeId { get; set; }
        public HttpActionType? Type { get; set; }

        [MaxLength(-1)]
        public string? Data { get; set; }

        [MaxLength(255)]
        public string Token { get; set; } = "";
        
        public DateTime? ClosedAt { get; set; }
    }
}