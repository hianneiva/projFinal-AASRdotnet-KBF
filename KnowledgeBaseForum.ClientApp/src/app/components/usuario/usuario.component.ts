import { Component } from '@angular/core';
import { Router } from '@angular/router';
import moment from 'moment';
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
  private utils!: Utils;
  public usuario!: Usuario;
  public creationDate!: string;
  public currentPwd: string;
  public pwdToChange!: string;
  public pwdConfirm!: string;
  public successMsg?: string;
  public errorMsg?: string;
  
  constructor(private api: ApiService, cookie: CookieService, decoder: TokenDecodeService, router: Router ){
    this.utils = new Utils(cookie, decoder, router);
    this.currentPwd = '';
    this.pwdToChange = '';
    this.pwdConfirm = '';
    this.getAtualUsuario();
  }

  public updateUser(): void {
    const token = this.utils.getJwtToken();
    
    if (this.utils.stringIsNullOrEmpty(this.currentPwd)) {
      this.displayErrMsg("Senha atual não informada.");
    } else if (!this.utils.stringIsNullOrEmpty(this.pwdConfirm) && !this.utils.stringIsNullOrEmpty(this.pwdToChange) && this.pwdConfirm != this.pwdToChange) {
      this.displayErrMsg("Senha nova não pode ser confirmada.");
    }

    this.usuario!.senha = this.pwdToChange;
    this.api.putUsuario(token, this.usuario, this.currentPwd).subscribe({
      next: (res) => {
        if (!res) {
          this.displayErrMsg("Falha ao atualizar dados de usuário");
          return;
        }

        this.usuario = res;
        this.currentPwd = '';
        this.pwdToChange = '';
        this.pwdConfirm = '';
        this.successMsg = "Dados atualizados com sucesso";
        setTimeout(() => { this.successMsg = undefined }, 5000);
      },
      error: (err) => {
        this.displayErrMsg(err.message ?? "Falha ao atualizar dados de usuário");
      }
    });
  }

  private getAtualUsuario() {
    const token: string = this.utils.getJwtToken();
    const userData: TokenData = this.utils.getUserDataFromToken();
    this.api.usuarioAtual(token, userData.name!).subscribe({
      next: (res)  => {
        this.usuario = res;
        const tzOffset = new Date().getTimezoneOffset();
        this.creationDate = moment(this.usuario!.dataCriacao!).zone(tzOffset)
                                                              .toDate()
                                                              .toLocaleDateString(window.navigator.language, { day: '2-digit', month: '2-digit', year: 'numeric' });
      },
      error: (err) => {
        this.displayErrMsg(err.message ?? "Falha ao recuperar dados de usuário");
      }
    });
  }

  private displayErrMsg(msg: string): void {
    this.errorMsg = msg;
    setTimeout(() => { this.errorMsg = undefined }, 5000);
  }
}
