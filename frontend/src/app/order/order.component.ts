import { Component, OnInit } from '@angular/core';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.less']
})
export class OrderComponent implements OnInit {
  orders: any[] = [];
  selectedYear: number | 'all' = new Date().getFullYear();
  years: (number | 'all')[] = [];
  expandedOrderId: number | null = null;

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    //const userId = Number(localStorage.getItem('userId'));
    const userId = 1;

    // Generate last 5 years + "All"
    const currentYear = new Date().getFullYear();
    this.years = ['all', ...Array.from({ length: 5 }, (_, i) => currentYear - i)];

    this.loadOrders(userId, this.selectedYear);
  }

  loadOrders(userId: number, year: number | 'all'): void {
    if (year === 'all') {
      this.orderService.getOrdersByUserId(userId).subscribe((res: any) => {
        this.orders = res;
      });
    } else {
      this.orderService.getOrdersByYear(userId, year).subscribe((res: any) => {
        this.orders = res;
      });
    }
  }

  onYearChange(year: any): void {
    const userId = Number(localStorage.getItem('userId'));
    this.loadOrders(userId, year);
  }

  toggleExpand(orderId: number): void {
    this.expandedOrderId = this.expandedOrderId === orderId ? null : orderId;
  }
}