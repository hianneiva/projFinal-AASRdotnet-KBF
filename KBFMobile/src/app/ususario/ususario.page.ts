import { Component, OnInit } from '@angular/core';
import { UtilsService } from '../service/utils.service';
import { ApiService } from '../service/api.service';
import { CookieService } from 'ngx-cookie-service';
import { TokenDecoderService } from '../service/token-decoder.service';
import { Router } from '@angular/router';
import { Usuario } from '../model/usuario';
import moment from 'moment';

@Component({
  selector: 'app-ususario',
  templateUrl: './ususario.page.html',
  styleUrls: ['./ususario.page.scss'],
})
export class UsusarioPage implements OnInit {
  private utils: UtilsService;

  public user?: Usuario;
  public currentPwd?: string;
  public updatePwd?: string;
  public confirmPwd?: string;
  public formattedDate?: string;
  public email?: string;
  public name?: string;
  public username?: string;
  public failure?: string;

  constructor(private api: ApiService, _c: CookieService, _d: TokenDecoderService, _r: Router) {
    this.utils = new UtilsService(_c, _d, _r);
   }

  ngOnInit() {
    this.getCurrentUser();
  }

  public updateUser() {
    if (this.utils.stringIsNullOrEmpty(this.currentPwd)) {
      this.failure = "Senha atual não informada.";
    } else if (!this.utils.stringIsNullOrEmpty(this.confirmPwd) && !this.utils.stringIsNullOrEmpty(this.updatePwd) && this.confirmPwd != this.updatePwd) {
      this.failure = "Senha nova não pode ser confirmada.";
    }

    this.user!.email= this.email!;
    this.user!.senha = this.updatePwd ?? '';
    const token = this.utils.getJwtToken();

    this.api.putUsuario(token, this.user!, this.currentPwd!).subscribe({
      next: (res) => {
        if (!res) {
          this.failure = "Falha ao atualizar os dados do usuário";
        } else {
          this.getCurrentUser();
          this.failure = "Dados de usuário atualizados com sucesso";
          this.updatePwd = undefined;
          this.confirmPwd = undefined;
          this.currentPwd = undefined;
        }
      },
      error: (err) => {
        this.failure = "Falha ao atualizar os dados do usuário: " + err.message;
      }
    });
  }

  private getCurrentUser() {
    const token = this.utils.getJwtToken();
    const userId = this.utils.getUserDataFromToken().name!;

    this.api.usuarioAtual(token, userId).subscribe({
      next: (res) => {
        if (!res) {
          this.failure = "Não foi possível recuperar os dados do usuário";
        } else {
          this.user = res;
          this.formattedDate = moment(this.user!.dataCriacao!).format("DD/MM/YYYY HH:mm:ss");
          this.username = this.user.login;
          this.name = this.user.nome;
          this.email = this.user.email;
        }
      },
      error: (err) => {
        this.failure = "Não foi possível recuperar os dados do usuário:" + err.message;
      }
    });
  }
}
