import { environment } from 'src/environments/environment.development';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest } from '../model/login-request';
import { LoginResponse } from '../model/login-response';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  public login(username: string, password: string): Observable<LoginResponse> {
    const url: string = environment.urlApi + environment.urlLogin;
    const encodedPwd = `${btoa(password)}.${btoa(environment.cypher)}`;
    const jsonContent: LoginRequest = new LoginRequest(username, encodedPwd);

    return this.http.post<LoginResponse>(url, jsonContent);
  }

  public listTopics(token: string): Observable<any> { // TODO: Edit to use actual model
    const url: string = environment.urlApi + environment.urlTopico;
    return this.http.get<Object>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }
}
