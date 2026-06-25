import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginRequest } from '../models/loginRequest';
import { Observable } from 'rxjs';
import { RegisterRequest } from '../models/RegisterRequest';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7097/api/Auth';

  constructor(private http: HttpClient) {}


  login(data: LoginRequest): Observable<any> {

    return this.http.post(
      `${this.apiUrl}/login`,
      data
    );

  }


  saveToken(token: string) {

    localStorage.setItem('token', token);

  }


  getToken() {

    return localStorage.getItem('token');

  }


  logout() {

    localStorage.removeItem('token');

  }
  register(data: RegisterRequest) {

  return this.http.post(
    `${this.apiUrl}/register`,
    data
  );

}

}