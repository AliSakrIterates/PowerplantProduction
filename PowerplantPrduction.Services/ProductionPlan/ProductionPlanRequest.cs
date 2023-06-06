using MediatR;
using PowerplantProduction.Services.Enums;
using System.Text.Json.Serialization;

namespace PowerplantProduction.Services.ProductionPlan
{
    public class ProductionPlanRequest : IRequest<ProductionPlanResponse>
    {
        [JsonPropertyName("load")]
        public int Load { get; set; }
        [JsonPropertyName("fuels")]
        public Fuels Fuels { get; set; }
        [JsonPropertyName("powerplants")]
        public List<PowerPlant> PowerPlants { get; set; }
    }

    public class Fuels
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public double Gas { get; set; }
        [JsonPropertyName("kerosine(euro/MWh)")]
        public double Kerosine { get; set; }
        [JsonPropertyName("co2(euro/ton)")]
        public double Co2 { get; set; }
        [JsonPropertyName("wind(%)")]
        public int Wind { get; set; }
    }

    public class PowerPlant
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public PowerplantType Type { get; set; }
        [JsonPropertyName("efficiency")]
        public double Efficiency { get; set; }
        [JsonPropertyName("pmin")]  
        public int Pmin { get; set; }
        [JsonPropertyName("pmax")]
        public int Pmax { get; set; }
    }
}