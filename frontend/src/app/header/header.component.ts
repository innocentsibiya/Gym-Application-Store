import { Component, HostListener } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent {
  navItems = [
    { className: 'categories', label: 'Categories', route: '/category' },
    { className: 'about', label: 'About', route: '/about' },
    { className: 'help', label: 'Help', route: '/help' },
    { className: 'register', label: 'Register', route: '/register' },
    { className: 'login', label: 'Login', route: '/login' },
    { className: 'cart', label: 'Cart', route: '/cart' }
  ];

 dropdownOpen = false;
  currentTitle = '';
  user: any = null; 

  constructor(private router: Router) {}

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentTitle = this.getTitle(this.router.url);
      }
    });

    this.user = {
      name: 'User',
      avatar: 'assets/img/anime3.png'
    };
  }

  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
  }

  logout() {

    localStorage.removeItem('token'); 
    sessionStorage.clear();

    this.user = null;
    this.dropdownOpen = false;

    this.router.navigate(['/login']);
  }

  login() {
    this.router.navigate(['/login']);
  }

  getTitle(url: string): string {
    const path = url.split('/').pop();

    switch (path) {
      case 'dashboard': return 'Dashboard';
      case 'user': return 'User Profile';
      case 'table': return 'Table List';
      default: return 'Dashboard';
    }
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const target = event.target as HTMLElement;

    if (!target.closest('.profile-dropdown')) {
      this.dropdownOpen = false;
    }
  }
}
