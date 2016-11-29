using System.Data.SqlClient;
using Dapper;
using $ext_safeprojectname$.Api.Controllers;
using $ext_safeprojectname$.Core.Models;
using $ext_safeprojectname$.Infrastructure.Repositories;
using $safeprojectname$.TestHelp;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace $safeprojectname$
{
    public class ExampleIntegrationTest : IntegrationTestBaseClass
    {
        [Theory, Gen(TestConnectionString)]
        public async void TestExamplePing(
            [Frozen(Matching.ImplementedInterfaces)] PingRepository pingRepo,
            PingModel dummyPing,
            PingController sut)
        {
            SetupExpectedPingInDatabase(dummyPing.Ping);
            // because Gen with test connection is used, a config with the connection string
            // will be injected into the controller making it "live"
            var actual = await sut.Ping(dummyPing).Unwrap<PongModel>();
            Assert.Equal(dummyPing.Ping, actual.Pong);
        }

        private void SetupExpectedPingInDatabase(string expectedPing)
        {
            using (var conn = new SqlConnection(TestConnectionString))
            {
                conn.Execute("insert into ping (pong) values (@expected);", new { expected = expectedPing });
            }
        }

        protected override void CleanupDb()
        {
            using (var conn = new SqlConnection(TestConnectionString))
            {
                conn.Execute("delete from ping");
            }
        }
    }
}
