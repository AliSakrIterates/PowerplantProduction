using FluentAssertions;
using PowerplantProduction.Services.ProductionPlan;
using PowerplantProduction.Tests.Utilities;
using Xunit;

namespace PowerplanProduction.Tests
{
    public class PowerPlanHandlerTest
    {
        [Theory]
        [AutoServiceData]
        public async Task Handle_WhenCalled_WillCallFindIncidents(PowerPlanHandler sut,
        ProductionPlanRequest request , CancellationToken token)
        {
            // Setup

            // Act
            var response = await sut.Handle(request, token);
            int sum = response.Outputs.Sum(obj => obj.P); 

            // Assert
            Assert.NotNull(response);
            request.Load.Should().Be(sum);
        }
    }
}