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

    this.api.updateAlertas(token, idToUpdate).subscribe(res => {
      if (res === null || res === undefined) {
        this.successMsg = undefined;
        this.errorMsg = "Falha na atualização do tipo de alerta";
        setTimeout(() => this.errorMsg = undefined, 5000);
      } else {
        this.getAllAlerts();
        this.successMsg = "Alerta alterado com sucesso";
        this.errorMsg = undefined;
        setTimeout(() => this.successMsg = undefined, 5000);
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

    this.api.deleteAlertas(token, this.idToDel!).subscribe(res => {
      if (res === null || res === undefined) {
        this.successMsg = undefined;
        this.errorMsg = "Falha na remoção do alerta";
        setTimeout(() => this.errorMsg = undefined, 5000);
      } else {
        this.getAllAlerts();
        this.successMsg = "Alerta removido com sucesso";
        this.errorMsg = undefined;
        setTimeout(() => this.successMsg = undefined, 5000);
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
}
