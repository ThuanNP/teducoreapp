using System;
using System.ComponentModel.DataAnnotations;
using TeduCoreApp.Data.Enums;

namespace TeduCoreApp.Application.ViewModels.Common
{
    public class SystemConfigViewModel
    {
        public string Id { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }

        public string Value1 { get; set; }
        public int? Value2 { get; set; }
        public bool? Value3 { get; set; }
        public DateTime? Value4 { get; set; }
        public decimal? Value5 { get; set; }
        public Status Status { get; set; }
    }
}