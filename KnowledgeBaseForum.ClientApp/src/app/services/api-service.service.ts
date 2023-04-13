import { environment } from 'src/environments/environment.development';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest } from '../model/login-request';
import { LoginResponse } from '../model/login-response';
import { Usuario } from '../model/usuario';
import { Alerta } from '../model/alerta';
import { Topico } from '../model/topico';
import { Tag } from '../model/tag';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  // AUTH
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

  // Topicos
  // GET
  public listTopics(token: string): Observable<Topico> {
    const url: string = environment.urlApi + environment.urlTopico;
    return this.http.get<Topico>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // GET by ID
  public getTopic(token: string, id: string): Observable<Topico> {
    const url: string = `${environment.urlApi}${environment.urlTopico}/${id}`;
    return this.http.get<Topico>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Post
  public createTopic(token: string, topico: Topico): Observable<Topico> {
    const url: string = `${environment.urlApi}${environment.urlTopico}`;
    return this.http.post<Topico>(url, topico, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Put
  public modifyTopic(token: string, topico: Topico): Observable<Topico> {
    const url: string = `${environment.urlApi}${environment.urlTopico}/${topico.id}`;
    return this.http.put<Topico>(url, topico, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Delete
  public deleteTopic(token: string, id: string): Observable<Topico> {
    const url: string = `${environment.urlApi}${environment.urlTopico}/${id}`;
    return this.http.delete<Topico>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Search
  public searchTopics(token: string, tags: string[], filter?: string | null, author?: string | null): Observable<Topico[]> {
    const url = `${environment.urlApi}${environment.urlTopico}/${environment.urlTopicoSearch}`;
    const jsonData = {
      filter: filter,
      author: author,
      tags: tags
    }
    return this.http.post<Topico[]>(url, jsonData, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Author only
  public listAuthorTopics(token: string, id: string): Observable<Topico> {
    const url: string = `${environment.urlApi}${environment.urlTopico}/${environment.urlTopicoAutor}?login=${id}`;
    return this.http.get<Topico>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Recent
  public recentTopics(token: string): Observable<Topico[]> {
    const url = `${environment.urlApi}${environment.urlTopico}/${environment.urlTopicoRecent}`;
    return this.http.get<Topico[]>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // ALERTA
  // Get
  public getAlertas(token: string, idUser: string): Observable<Alerta[]> {
    const url: string = `${environment.urlApi}${environment.urlAlerta}/${idUser}`;
    return this.http.get<Alerta[]>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Put
  public updateAlertas(token: string, id: string, toggle: boolean, dismiss: boolean): Observable<Alerta> {
    const url: string = `${environment.urlApi}${environment.urlAlerta}/${id}?toggleMode=${toggle}&dismiss=${dismiss}`;
    return this.http.put<Alerta>(url, {}, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Delete
  public deleteAlertas(token: string, id: string): Observable<Alerta> {
    const url: string = `${environment.urlApi}${environment.urlAlerta}/${id}`;
    return this.http.delete<Alerta>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Post - Marca todos os alertas do usuário como lidos
  public postAlertasDismiss(token: string, id: string): Observable<any> {
    const url: string = `${environment.urlApi}${environment.urlAlerta}/${id}`;
    return this.http.post<any>(url, {}, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // TAGS
  public getTags(token: string): Observable<Tag[]> {
    const url: string = environment.urlApi + environment.urlTag;
    return this.http.get<Tag[]>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }

  // Usuário Atual
  public usuarioAtual(token: string, userName: string): Observable<Usuario> {
    const url: string = `${environment.urlApi}${environment.urlUsuario}/${userName}`;
    return this.http.get<Usuario>(url, { headers: new HttpHeaders({ 'Authorization': `Bearer ${token}` }) });
  }
}
