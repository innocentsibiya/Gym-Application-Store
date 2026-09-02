namespace GymStore.Modules.Catalog.Domain;

/// <summary>A product category, with self-referencing parent/sub-categories.</summary>
public class Category
{
    public long Id { get; set; }
    public long? ParentCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
