import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.less']
})
export class ProfileComponent {
  selectedSection: string = 'info';

  navItems = [
    { label: 'Edit Personal Info', section: 'user' },
    { label: 'Change Preferences', section: 'preference' },
    { label: 'Orders', section: 'order' },
    { label: 'Edit Address', section: 'address' },
    { label: 'Logout', section: 'logout' }
  ];

  constructor(private route: ActivatedRoute) {}
  
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['section']) {
        this.selectedSection = params['section'];
      }
    });
  }

  selectSection(section: string): void {
    if (section === 'logout') {
      this.logout();
    } else {
      this.selectedSection = section;
    }
  }

  logout(): void {
    sessionStorage.clear();
    localStorage.removeItem('authToken');
  }
}