using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeduCoreApp.Data.Entities;
using static TeduCoreApp.Data.EF.Extensions.ModelBuilderExtensions;

namespace TeduCoreApp.Data.EF.Configurations
{
    public class ProductConfiguration : DbEntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> entity)
        {
        }
    }
}
