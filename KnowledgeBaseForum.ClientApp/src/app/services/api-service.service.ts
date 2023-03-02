import { environment } from 'src/environments/environment.development';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest } from '../model/login-request';
import { LoginResponse } from '../model/login-response';

@Injectable({
  providedIn: 'root'
})
export class ApiServiceService {

  constructor(private http: HttpClient) { }

  public login(username: string, password: string): Observable<LoginResponse> {
    const url: string = environment.urlApi + environment.urlLogin;
    const encodedPwd = `${btoa(password)}.${btoa(environment.cypher)}`;
    const jsonContent: LoginRequest = new LoginRequest(username, encodedPwd);

    return this.http.post<LoginResponse>(url, jsonContent);
  }
}
