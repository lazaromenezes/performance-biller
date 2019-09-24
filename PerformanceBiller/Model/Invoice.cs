namespace PerformanceBiller.Model
{
    public class Invoice
    {
        public string Customer { get; set; }
        public Performance[] Performances { get; set; }
    }
}
