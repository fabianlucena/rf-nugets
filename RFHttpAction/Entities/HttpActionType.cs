using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFHttpAction.Entities
{
    [Table("HttpActionsTypes", Schema = "action")]
    public class HttpActionType
        : LocalizableEntity
    {
        public HttpActionType() { }

        public HttpActionType(LocalizableEntity? entity)
            : base(entity)
        { }

        public override HttpActionType Clone()
            => new(this);
    }
}