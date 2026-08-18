import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Preference } from '../Model/Preference';


@Injectable({
  providedIn: 'root'
})
export class PreferenceService {
  private apiUrl = 'http://localhost:5074/api/Preference';

  constructor(private http: HttpClient) {}

  loadPreferences(userId: number): Observable<Preference> {
    return this.http.get<Preference>(`${this.apiUrl}/user/${userId}`).pipe(
      tap(prefs => localStorage.setItem('theme', prefs.theme))
    );
  }

  savePreferences(userId: number, prefs: Preference): Observable<Preference> {
    localStorage.setItem('theme', prefs.theme);
    return this.http.post<Preference>(`${this.apiUrl}/user/${userId}`, prefs);
  }

  getTheme(): string {
    return localStorage.getItem('theme') || 'light';
  }
}