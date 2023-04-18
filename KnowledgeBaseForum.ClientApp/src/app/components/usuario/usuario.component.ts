import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
import { Usuario } from 'src/app/model/usuario';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';
import { FormControl, FormGroup, Validators } from '@angular/forms';





@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent {



public usuario! : Usuario;
private utils! : Utils;
public successMsg? : string;
public errorMsg? : string;
public novaSenha? : string;
public confSenha? : string;
public email : Usuario["email"];
public nome : Usuario["nome"];
public login : Usuario["login"];


 
 constructor(private api: ApiService, private cookie: CookieService, decoder: TokenDecodeService, router: Router){
  this.utils = new Utils(cookie, decoder, router);
  this.getAtualUsuario();


 }


 private getAtualUsuario() {
  const token: string = this.utils.getJwtToken();
  const userData: TokenData = this.utils.getUserDataFromToken();
  this.api.usuarioAtual(token, userData.name!).subscribe(res => {
    this.usuario = res;
    this.nome = this.usuario.nome;
    this.email = this.usuario.email;
    this.login = this.usuario.login;
  });
  
 }

 public putAtualUsuario(usuario:Usuario):void {
  const token: string = this.utils.getJwtToken();


  if(this.novaSenha != this.confSenha || this.utils.stringIsNullOrEmpty(this.novaSenha)
    || this.utils.stringIsNullOrEmpty(this.confSenha) ){
    this.errorMsg = "Falha ao atualizar cadastro!";
    setTimeout(() => this.errorMsg = undefined, 5000);
    return;
  }

  this.usuario.senha = btoa(this.novaSenha!);

  this.api.trocaSenha(token,usuario).subscribe(res => {
    if(res === null || res === undefined){
      this.successMsg = undefined;
      this.errorMsg = "Falha ao alterar a senha";
      setTimeout(() => this.errorMsg = undefined, 5000);
    }else{
      this.successMsg = "Senha alterada com sucesso!";
      this.errorMsg = undefined;
      setTimeout(() => this.successMsg = undefined, 5000);  
      this.novaSenha = ""; 
      this.confSenha = "";
      this.email = usuario.email;

    
    }
  });

 }
}
