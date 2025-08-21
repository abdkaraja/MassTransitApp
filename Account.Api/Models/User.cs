using SharedProj;

namespace Account.Api.Models
{
    public class User: EntityBase
    {
        public string FullName { get; set; }
        public int Balance { get; set; }
    }
}
