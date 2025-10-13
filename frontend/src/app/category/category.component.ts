import { Component, ViewChild, OnInit } from '@angular/core';
import { CardComponent } from '../card/card.component';
import { CartComponent } from '../cart/cart.component';
import { CartService } from '../services/cart.service';
import { ProductService } from '../services/product.service';
import { Product } from '../interface/Product';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.less']
})
export class CategoryComponent implements OnInit {
  @ViewChild(CardComponent) childComponent!: CardComponent;
  @ViewChild(CartComponent) cartComponent!: CartComponent;

  cartItems: Product[] = [];
  allItems: Product[] = [];
  items: Product[] = [];

  searchTerm: string = '';
  sortField: string = 'name';
  sortDirection: string = 'asc';

  currentPage: number = 1;
  pageSize: number = 6;
  totalPages: number = 1;

  constructor(
    private productService: ProductService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  private loadProducts(): void {
    this.productService.getAllProducts().subscribe({
      next: data => {
        this.allItems = data;
        this.applyFilters();
      },
      error: err => console.error(err)
    });
  }

  private applyFilters(): void {
    let filtered = [...this.allItems];

    // Search
    if (this.searchTerm.trim()) {
      const lower = this.searchTerm.toLowerCase();
      filtered = filtered.filter(item =>
        item.name.toLowerCase().includes(lower) ||
        item.description.toLowerCase().includes(lower) ||
        (item.brand ?? '').toLowerCase().includes(lower)
      );
    }

    // Sort
    filtered.sort((a, b) => {
      const valA = a[this.sortField as keyof Product];
      const valB = b[this.sortField as keyof Product];

      if (typeof valA === 'string' && typeof valB === 'string') {
        return this.sortDirection === 'asc'
          ? valA.localeCompare(valB)
          : valB.localeCompare(valA);
      }

      if (typeof valA === 'number' && typeof valB === 'number') {
        return this.sortDirection === 'asc'
          ? valA - valB
          : valB - valA;
      }

      return 0;
    });

    // Pagination
    this.totalPages = Math.ceil(filtered.length / this.pageSize);
    this.currentPage = Math.min(this.currentPage, this.totalPages || 1);
    const start = (this.currentPage - 1) * this.pageSize;
    this.items = filtered.slice(start, start + this.pageSize);
  }

  onSearchChange(term: string): void {
    this.searchTerm = term;
    this.currentPage = 1;
    this.applyFilters();
  }

  onSortChange(field: string, direction: string): void {
    this.sortField = field;
    this.sortDirection = direction;
    this.applyFilters();
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.applyFilters();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.applyFilters();
    }
  }

  showItemDetails(item: Product): void {
    this.childComponent.openModal(item); 
  }

  addItemToCart(item: Product): void {
    this.cartService.addToCart(item).subscribe({
      next: cartItems => this.cartItems = cartItems,
      error: err => console.error(err)
    });
  }
}