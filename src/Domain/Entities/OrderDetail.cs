namespace Domain.Entities
{
    public class OrderDetail : BaseEntity<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
