import { Component } from '@angular/core';
import { Router } from '@angular/router';
import moment from 'moment';
import { CookieService } from 'ngx-cookie-service';
import { Topico } from 'src/app/model/topico';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';

@Component({
  selector: 'app-topico-listar',
  templateUrl: './topico-listar.component.html',
  styleUrls: ['./topico-listar.component.css']
})
export class TopicoListarComponent {
  private utils: Utils;

  public textAlertTypes = ["Padrão", "Apenas notificação"];
  public idToDel?: string;
  public errorMsg?: string;
  public successMsg?: string;
  public topicos: Topico[] = [];

  constructor(private api: ApiService, cookie: CookieService, decoder: TokenDecodeService, private router: Router) {
    this.utils = new Utils(cookie, decoder, router);

    try {
      this.getAllTopicos();
    } catch (err) {
      this.errorMsg = "Falha na recuperação dos topicos";
      setTimeout(() => this.errorMsg = undefined, 5000);
    }
  }

  public setIdToDel(id?: string) {
    this.idToDel = id;
  }

  public deleteTopico() {
    if (this.utils.stringIsNullOrEmpty(this.idToDel)) {
      return;
    }

    const token: string = this.utils.getJwtToken();

    this.api.deleteTopic(token, this.idToDel!).subscribe({
      next: (res) => {
        if (!res) {
          this.showMsg("Falha na remoção do topico", false);
        } else {
          this.getAllTopicos();
          this.showMsg("Topico removido com sucesso", true);
        }
      },
      error: (err) => {
        this.showMsg("Falha ao se recuperar tópicos a serem listados: " + err.message, false);
      }
    });
  }

  private getAllTopicos() {
    const token: string = this.utils.getJwtToken();
    const id: string = this.utils.getUserDataFromToken().name!;
    this.api.listAuthorTopics(token, id).subscribe({
      next: (res) => {
        this.topicos = this.utils.arrayFromAny(res)
      },
      error: (err) => {
        this.showMsg("Falha ao se listar os tópicos: " + err.message, false);
      }
    });
  }

  public alterarTopico(id?: string) {
    this.router.navigate([`/topico/topico-alterar/${id}`]);
  }

  public formatDate(date: Date): string {
    const tzOffset = new Date().getTimezoneOffset();
    return moment(date).zone(tzOffset).format("DD/MM/YYYY HH:mm:ss");
  }

  private showMsg(msg: string, success: boolean) {
    if (success) {
      this.successMsg = msg;
      this.errorMsg = undefined;
      setTimeout(() => this.successMsg = undefined, 5000);
    } else {
      this.successMsg = undefined;
      this.errorMsg = msg;
      setTimeout(() => this.errorMsg = undefined, 5000);
    }
  }
}