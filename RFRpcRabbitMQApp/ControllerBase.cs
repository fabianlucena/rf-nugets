using RFRpcRabbitMQApp.Types;

namespace RFRpcRabbitMQApp
{
    public class ControllerBase
    {
        public static Result Ok(object? value = null)
        {
            return new Result()
            {
                Ok = true,
                Value = value
            };
        }
    }
}
