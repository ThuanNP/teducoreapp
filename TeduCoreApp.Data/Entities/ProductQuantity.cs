using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("ProductQuantities")]
    public class ProductQuantity : DomainEntity<int>
    {
        public ProductQuantity()
        {
        }

        public ProductQuantity(int id, int productId, int sizeId, int colorId, int quantity)
        {
            ProductId = productId;
            SizeId = sizeId;
            ColorId = colorId;
            Quantity = quantity;
        }

        public ProductQuantity(int productId, int sizeId, int colorId, int quantity)
        {
            ProductId = productId;
            SizeId = sizeId;
            ColorId = colorId;
            Quantity = quantity;
        }

        [Column(Order = 1)]
        public int ProductId { get; set; }

        [Column(Order = 2)]
        public int SizeId { get; set; }

        [Column(Order = 3)]
        public int ColorId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("SizeId")]
        public virtual Size Size { get; set; }

        [ForeignKey("ColorId")]
        public virtual Color Color { get; set; }
    }
}