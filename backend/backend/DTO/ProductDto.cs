﻿namespace backend.DTO
{
    public class ProductDto
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? Brand { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string SKU { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public decimal? Weight { get; set; }
        public string? Dimensions { get; set; }

        public bool IsActive { get; set; }

        public List<string> ImageUrls { get; set; } = new();
    }

}
