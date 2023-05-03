import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Alerta } from 'src/app/model/alerta';
import { TokenData } from 'src/app/model/token-data';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';

@Component({
  selector: 'app-alerta',
  templateUrl: './alerta.component.html',
  styleUrls: ['./alerta.component.css']
})
export class AlertaComponent {
  private utils: Utils;

  public textAlertTypes = ["Padrão", "Apenas notificação"];
  public idToDel?: string;
  public errorMsg?: string;
  public successMsg?: string;
  public alertas: Alerta[] = [];

  constructor(private api: ApiService, cookie: CookieService, decoder: TokenDecodeService, router: Router) {
    this.utils = new Utils(cookie, decoder, router);

    try {
      this.getAllAlerts();
    } catch (err) {
      this.errorMsg = "Falha na recuperação dos alertas";
      setTimeout(() => this.errorMsg = undefined, 5000);
    }
  }

  public alterAlertType(idToUpdate: string) {
    const token: string = this.utils.getJwtToken();

    this.api.updateAlertas(token, idToUpdate, true, false).subscribe({
      next: (res) => {
        if (!res) {
          this.showMsg("Falha na atualização do tipo de alerta", false);
        } else {
          this.getAllAlerts();
          this.showMsg("Alerta alterado com sucesso", false);
        }
      },
      error: (err) => {
        this.showMsg("Falha não esperada: " + err.message, false);
      }
    });
  }

  public readAlert(idToUpdate: string) {
    const token: string = this.utils.getJwtToken();

    this.api.updateAlertas(token, idToUpdate, false, true).subscribe({
      next: (res) => {
        if (!res) {
          this.showMsg("Falha ao marcar alerta como lido", false);
        } else {
          this.getAllAlerts();
          this.showMsg("Alerta lido", true);
        }
      },
      error: (err) => {
        this.showMsg("Falha não esperada: " + err.message, false);
      }
    });
  }

  public setIdToDel(id?: string) {
    this.idToDel = id;
  }

  public deleteAlert() {
    if (this.utils.stringIsNullOrEmpty(this.idToDel)) {
      return;
    }

    const token: string = this.utils.getJwtToken();

    this.api.deleteAlertas(token, this.idToDel!).subscribe({
      next: (res) => {
        if (!res) {
          this.showMsg("Falha na remoção do alerta", false);
        } else {
          this.getAllAlerts();
          this.showMsg("Alerta removido com sucesso", true);
        }
      },
      error: (err) => {
        this.showMsg("Falha não esperada: " + err.message, false);
      }
    });
  }

  private getAllAlerts() {
    const token: string = this.utils.getJwtToken();
    const userData: TokenData = this.utils.getUserDataFromToken();
    this.api.getAlertas(token, userData.name!).subscribe(res => {
      this.alertas = this.utils.arrayFromAny(res)
    });
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
