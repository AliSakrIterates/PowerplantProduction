using AutoFixture;
using AutoFixture.AutoMoq;
using PowerplantProduction.Services.ProductionPlan;

namespace PowerplantProduction.Tests.Utilities
{
    public class ServiceCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(
               new AutoMoqCustomization
               {
                   ConfigureMembers = true
               });
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        }
    }
}
