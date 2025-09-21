import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GymItem } from '../interface/GymItem';

@Injectable({
  providedIn: 'root'
})

export class GymItemService {
  private apiUrl = 'http://localhost:5074/api/GymItems';

  constructor(private http: HttpClient) {}

  getAll(): Observable<GymItem[]> {
    return this.http.get<GymItem[]>(this.apiUrl);
  }

  getById(id: number): Observable<GymItem> {
    return this.http.get<GymItem>(`${this.apiUrl}/${id}`);
  }

  create(item: GymItem): Observable<GymItem> {
    return this.http.post<GymItem>(this.apiUrl, item);
  }
}
