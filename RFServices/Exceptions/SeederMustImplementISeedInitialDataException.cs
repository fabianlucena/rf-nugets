using RFBase.Exceptions;

namespace RFServices.Exceptions;

public class SeederMustImplementISeedInitialDataException(string name)
    : HttpException(500, "{0} seeder must implement ISeedInitialData", name)
{}