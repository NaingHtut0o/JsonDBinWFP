namespace SmartHealthTest.Models
{
    public class CompanyMasterModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public string DisplayName
        {
            get { return $"{CompanyId}. {CompanyName}"; }
        }
    }
}
