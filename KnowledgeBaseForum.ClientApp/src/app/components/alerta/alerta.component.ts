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
  private utils: Utils;

  public textAlertTypes = ["Padrão", "Apenas notificação"];
  public alertas: Alerta[] = [];

  constructor(private api: ApiService, private cookie: CookieService, private decoder: TokenDecodeService) {
    this.utils = new Utils();
    const token: string = this.cookie.get(environment.cookieToken);
    const userId = this.decoder.decodeToken(token).name!;

    this.api.getAlertas(token, userId).subscribe(res => this.alertas = this.utils.arrayFromAny(res));
  }
}
