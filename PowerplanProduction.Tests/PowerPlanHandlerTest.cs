using FluentAssertions;
using Newtonsoft.Json;
using PowerplantProduction.Services.ProductionPlan;
using PowerplantProduction.Tests.Utilities;
using System.Threading;
using Xunit;

namespace PowerplanProduction.Tests
{
    public class PowerPlanHandlerTest
    {
        [Theory]
        [InlineData("payloads/payload1.json")]
        [InlineData("payloads/payload2.json")]
        [InlineData("payloads/payload3.json")]
        public async Task Handle_WhenCalled_WillReturnLoad(string payload)
        {
            // Setup
            var request = JsonConvert.DeserializeObject<ProductionPlanRequest>(File.ReadAllText(payload));
            var handler = new PowerPlanHandler();

            // Act
            var response = await handler.Handle(request, new CancellationToken());
            int sum = response.Outputs.Sum(obj => obj.P); 

            // Assert
            Assert.NotNull(response);
            request.Load.Should().Be(sum);
        }

        [Theory]
        [InlineData("payloads/payload4.json")]
        public async Task Handle_WhenCalledWithEmptyPowerPlans_WillReturnException(string payload)
        {
            // Setup
            var request = JsonConvert.DeserializeObject<ProductionPlanRequest>(File.ReadAllText(payload));
            var handler = new PowerPlanHandler();

            // Act
            var actual = () => handler.Handle(request, new CancellationToken());

            // Assert
            await actual.Should().ThrowAsync<InvalidOperationException>();
        }
           
    }
}