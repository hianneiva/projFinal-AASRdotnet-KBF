import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Alerta } from 'src/app/model/alerta';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-alerta',
  templateUrl: './alerta.component.html',
  styleUrls: ['./alerta.component.css']
})
export default class AlertaComponent {
  private userId!: string;
  private utils: Utils;

  public textAlertTypes = ["Padrão", "Apenas notificação"];
  public idToDel?: string;
  public errorMsg?: string;
  public successMsg?: string;
  public alertas: Alerta[] = [];

  constructor(private api: ApiService, private cookie: CookieService, private decoder: TokenDecodeService) {
    this.utils = new Utils();
    const token: string = this.tokenFromCookie();
    this.userId = this.decoder.decodeToken(token).name!;
    this.getAllAlerts(token, this.userId);
  }

  public alterAlertType(idToUpdate: string) {
    const token: string = this.tokenFromCookie();

    this.api.updateAlertas(token, idToUpdate).subscribe(res => {
      if (res === null || res === undefined) {
        this.successMsg = undefined;
        this.errorMsg = "Falha na atualização do tipo de alerta";
        setTimeout(() => this.errorMsg = undefined, 5000);
      } else {
        this.getAllAlerts(token, this.userId);
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

    const token: string = this.tokenFromCookie();

    this.api.deleteAlertas(token, this.idToDel!).subscribe(res => {
      if (res === null || res === undefined) {
        this.successMsg = undefined;
        this.errorMsg = "Falha na remoção do alerta";
        setTimeout(() => this.errorMsg = undefined, 5000);
      } else {
        this.getAllAlerts(token, this.userId);
        this.successMsg = "Alerta removido com sucesso";
        this.errorMsg = undefined;
        setTimeout(() => this.successMsg = undefined, 5000);
      }
    });
  }

  private tokenFromCookie(): string {
    return this.cookie.get(environment.cookieToken);
  }

  private getAllAlerts(token: string, userId: string) {
    this.api.getAlertas(token, userId).subscribe(res => this.alertas = this.utils.arrayFromAny(res));
  }
}
