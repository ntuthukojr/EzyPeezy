namespace EzyPeezy.Models
{
    public class ProductDictionary
    {
        public int ID { get; set; }
        public virtual Product Key { get; set; }
        public int Value { get; set; }
    }
}