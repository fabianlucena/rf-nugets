using RFService.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFHttpAction.Entities
{
    [Table("HttpActionsTypes", Schema = "action")]
    public class HttpActionType : EntityTimestampsIdUuidEnabledNameTitleTranslatable
    {
    }
}