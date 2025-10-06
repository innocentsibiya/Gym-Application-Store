export interface Product {
  id: number;
  categoryId: number;
  categoryName: string;
  name: string;
  slug: string;
  description: string;
  brand?: string;
  price: number;
  discountPrice?: number;
  sku: string;
  stockQuantity: number;
  weight?: number;
  dimensions?: string;
  isActive: boolean;
  imageUrls: string[];
}