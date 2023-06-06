using AutoFixture;
using AutoFixture.Xunit2;


namespace PowerplantProduction.Tests.Utilities
{
    public class AutoServiceData : AutoDataAttribute
    {
        public AutoServiceData() : base(
            () => new Fixture()
                .Customize(new ServiceCustomization())
        )
        {
        }
    }

    public class InlineAutoServiceData : InlineAutoDataAttribute
    {
        public InlineAutoServiceData(params object[] values) : base(new AutoServiceData(), values)
        {
        }
    }
}
