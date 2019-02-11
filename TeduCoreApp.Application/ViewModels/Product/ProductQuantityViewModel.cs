namespace TeduCoreApp.Application.ViewModels.Product
{
    public class ProductQuantityViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int SizeId { get; set; }

        public int ColorId { get; set; }

        public int Quantity { get; set; }

        public ProductViewModel Product { get; set; }

        public SizeViewModel Size { get; set; }

        public ColorViewModel Color { get; set; }
    }
}
