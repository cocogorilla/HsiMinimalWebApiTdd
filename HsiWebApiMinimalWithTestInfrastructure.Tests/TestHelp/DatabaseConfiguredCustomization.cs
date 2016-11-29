using $ext_safeprojectname$.Core.Models;
using Ploeh.AutoFixture;

namespace $safeprojectname$.TestHelp
{
    public class DatabaseConfiguredCustomization : ICustomization
    {
        private readonly string _connectionString;

        public DatabaseConfiguredCustomization(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Customize(IFixture fixture)
        {
            if (!string.IsNullOrEmpty(_connectionString))
                fixture.Customize<AppConfig>(composer =>
                {
                    return composer.With(x => x.ConnectionString, _connectionString);
                });
        }
    }
}
