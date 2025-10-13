export interface ProductSearch {
  id: number;
  name: string;
  slug: string;
  price: number;
  stockQuantity: number;
  brand?: string;
  description: string;
  categoryId: number;
  category?: any; 
  productImages?: { imageUrl: string }[];
}