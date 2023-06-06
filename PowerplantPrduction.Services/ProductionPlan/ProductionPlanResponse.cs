
namespace PowerplantProduction.Services.ProductionPlan
{
    public class ProductionPlanResponse
    {
        public List<PowerPlantOutput>Outputs { get; set; }
    }
    public class PowerPlantOutput
    {
        public string Name { get; set; }
        public int P { get; set; }
    }
}
