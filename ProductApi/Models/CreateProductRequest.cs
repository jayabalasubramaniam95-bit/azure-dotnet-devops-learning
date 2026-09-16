using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class CreateProductRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; } = string.Empty;
        [Range(0.01, 1000000)]
         public decimal Price { get; set; }
    }
}