using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class InvalidConfigurationException(string section)
    : HttpException(400, "Invalid configurarion: {0}", section)
{
}
