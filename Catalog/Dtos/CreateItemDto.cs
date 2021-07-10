using System;
using System.ComponentModel.DataAnnotations;

namespace Catalogs.Dtos
{
    public record CreateItemDto
    {
        [Required]
        public string Name { get; init; }

        [Required]
        [Range(1, 1000)]
        public decimal Price { get; init; }

    }
}