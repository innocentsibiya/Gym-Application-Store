import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent {
  navItems = [
    { className: 'categories', label: 'Categories', route: '/category' },
    { className: 'register', label: 'Register', route: '/register' },
    { className: 'about', label: 'About', route: '/about' },
    { className: 'help', label: 'Help', route: '/help' },
    { className: 'cart', label: 'Cart', route: '/cart' }
  ];

}
