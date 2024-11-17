using RFAuth.Entities;
using static RFDapper.Setup;

namespace RFAuthDapper
{
    public static class Setup
    {
        public static void ConfigureRFAuthDapper(IServiceProvider services)
        {
            CreateTable<Device>(services);
            CreateTable<UserType>(services);
            CreateTable<User>(services);
            CreateTable<Password>(services);
            CreateTable<Session>(services);
        }
    }
}
