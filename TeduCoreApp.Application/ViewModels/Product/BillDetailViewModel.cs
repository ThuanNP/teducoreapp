namespace TeduCoreApp.Application.ViewModels.Product
{
    public class BillDetailViewModel
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        //public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        //public int ColorId { get; set; }
        //public int SizeId { get; set; }

        public BillViewModel Bill { get; set; }

        public ProductViewModel Product { get; set; }

        public ColorViewModel Color { set; get; }

        public SizeViewModel Size { set; get; }
    }
}