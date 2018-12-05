﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Tags")]
    public class Tag : DomainEntity<string>
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Type { get; set; }
    }
}