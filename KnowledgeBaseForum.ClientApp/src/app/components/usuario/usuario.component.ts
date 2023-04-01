import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
import { Usuario } from 'src/app/model/usuario';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';



@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent {
  
public usuario! : Usuario;
private utils! : Utils;
  
 constructor(private api: ApiService, private cookie: CookieService, decoder: TokenDecodeService, router: Router ){
  this.utils = new Utils(cookie, decoder, router);
  this.getAtualUsuario();

 }
 private getAtualUsuario() {
  const token: string = this.utils.getJwtToken();
  const userData: TokenData = this.utils.getUserDataFromToken();
  this.api.usuarioAtual(token, userData.name!).subscribe(res => {
    this.usuario = res;
  });
 }

}
