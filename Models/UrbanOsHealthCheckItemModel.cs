namespace SmartHealthTest.Models
{
    public class UrbanOsHealthCheckItemModel
    {
        public int Id { get; set; }
        public int HealthCheckId { get; set; }
        public int ItemId { get; set; }
        public string ItemValue { get; set; }
    }
}
