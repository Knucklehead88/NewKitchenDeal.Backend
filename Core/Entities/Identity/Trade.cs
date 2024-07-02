namespace Core.Entities.Identity
{
    public class Trade: BaseEntity
    {
        public string Name { get; set; }
        public List<string> Categories { get; set; }
    }
}