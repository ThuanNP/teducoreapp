namespace TeduCoreApp.Application.ViewModels.Product
{
    public class ProductTagViewModel
    {
        public string Id { get; set; }
        public int ProductId { get; set; }
        public string TagId { get; set; }
        public ProductViewModel Product { get; set; }
        public TagViewModel Tag { get; set; }
    }
}