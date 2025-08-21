using SharedProj;

namespace Contract.Api.Models
{
    public class Commission: EntityBase
    {
        public int FromAmount { get; set; }
        public int ToAmount { get; set; }
        public int Percentage { get; set; }
    }
}
