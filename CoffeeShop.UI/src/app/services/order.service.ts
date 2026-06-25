import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateOrder } from '../models/orderDetail';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private apiUrl = 'https://localhost:7097/api/Order';

  constructor(private http: HttpClient) {}

  createOrder(order: CreateOrder): Observable<any> {
    return this.http.post(this.apiUrl, order);
  }

  getOrders(): Observable<any> {
    return this.http.get(this.apiUrl);
  }
}