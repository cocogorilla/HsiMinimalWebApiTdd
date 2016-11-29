using System;
using System.Net;
using System.Web.Http;
using $ext_safeprojectname$.Api.Controllers;
using $ext_safeprojectname$.Core.Abstractions;
using $ext_safeprojectname$.Core.Models;
using $safeprojectname$.TestHelp;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace $safeprojectname$
{
    public class ExamplePingTests
    {
        [Theory, Gen]
        public async void CanGetPing(
            [Frozen] Mock<IReceiveAsync<PingModel, PongModel>> pingrepo,
            PingModel ping,
            PongModel pong,
            PingController sut)
        {
            pingrepo.Setup(x => x.GetDataAsync(ping)).ReturnsAsync(pong);
            var result = await sut.Ping(ping).Unwrap();
            var actual = await result.Unwrap<PongModel>();
            Assert.Equal(pong, actual);
        }

        [Theory, Gen]
        public async void CanGetPingFail(
            [Frozen] Mock<IReceiveAsync<PingModel, PongModel>> pingrepo,
            PingModel dummyPing,
            Exception expected,
            PingController sut)
        {
            pingrepo.Setup(x => x.GetDataAsync(It.IsAny<PingModel>())).ThrowsAsync(expected);
            var result = await sut.Ping(dummyPing).Unwrap();
            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var actual = await result.Unwrap<HttpError>();
            Assert.Equal(expected.Message, actual.Message);
        }
    }
}
