import { Component } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.less']
})
export class ProfileComponent {
  selectedSection: string = 'info'; // default section

  navItems = [
    { label: 'Edit Personal Info', section: 'user' },
    { label: 'Change Preferences', section: 'preferences' },
    { label: 'Orders', section: 'order' },
    { label: 'Edit Address', section: 'address' },
    { label: 'Logout', section: 'logout' }
  ];

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
    // redirect to login if needed
  }
}