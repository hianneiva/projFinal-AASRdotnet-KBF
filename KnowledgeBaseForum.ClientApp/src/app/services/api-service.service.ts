import { environment } from 'src/environments/environment.development';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest } from '../model/login-request';
import { LoginResponse } from '../model/login-response';
import { Usuario } from '../model/usuario';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  // CHAMADAS PARA ENDPOINT AUTH:
  // Login
  public login(username: string, password: string): Observable<LoginResponse> {
    const url: string = environment.urlApi + environment.urlAuthLogin;
    const encodedPwd = `${btoa(password)}.${btoa(environment.cypher)}`;
    const jsonContent: LoginRequest = new LoginRequest(username, encodedPwd);

    return this.http.post<LoginResponse>(url, jsonContent);
  }

  // SignUp
  public signup(username: string, password: string, name: string, email: string) {
    const url: string = environment.urlApi + environment.urlAuthSignUp;
    let jsonContent: Usuario = new Usuario();
    jsonContent.email = email;
    jsonContent.login = username;
    jsonContent.nome = name;
    jsonContent.senha = `${btoa(password)}.${btoa(environment.cypher)}`;
    return this.http.post<LoginResponse>(url, jsonContent);
  }

  public listTopics(token: string): Observable<any> { // TODO: Edit to use actual model
    const url: string = environment.urlApi + environment.urlTopico;
    return this.http.get<Object>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }
}