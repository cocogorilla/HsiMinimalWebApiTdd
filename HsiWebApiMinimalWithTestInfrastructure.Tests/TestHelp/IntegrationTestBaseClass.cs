using System;

namespace $safeprojectname$.TestHelp
{
    public abstract class IntegrationTestBaseClass : IDisposable
    {
        protected const string TestConnectionString =
            @"Data Source=(localdb)\ProjectsV13;Initial Catalog=scratch;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        protected IntegrationTestBaseClass()
        {
            CleanupDb();
        }

        public void Dispose()
        {
            CleanupDb();
        }

        protected abstract void CleanupDb();
    }
}
