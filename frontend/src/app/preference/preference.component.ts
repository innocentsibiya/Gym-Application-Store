import { Component, OnInit } from '@angular/core';
import { PreferenceService } from '../services/preference.service';
import { Preference } from '../Model/Preference';

@Component({
  selector: 'app-preference',
  templateUrl: './preference.component.html',
  styleUrls: ['./preference.component.less']
})
export class PreferenceComponent implements OnInit {
  prefs: Preference = {
    theme: 'light',
    itemsPerPage: 6,
    sortOrder: 'newest',
    currency: 'ZAR',
    language: 'en',
    notifications: true
  };

  constructor(private prefService: PreferenceService) {}

  ngOnInit(): void {
    const userId = Number(localStorage.getItem('userId'));
    this.prefService.loadPreferences(userId).subscribe(p => this.prefs = p);
  }

  save(): void {
    const userId = Number(localStorage.getItem('userId'));
    this.prefService.savePreferences(userId, this.prefs).subscribe(() => {
      alert('Preferences saved successfully!');
    });
  }
}