import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Alerta } from 'src/app/model/alerta';
import { TokenData } from 'src/app/model/token-data';
import { Topico } from 'src/app/model/topico';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  private utils: Utils;
  userData: TokenData;
  alertsForUser: Alerta[];
  newestTopics: Topico[];

  constructor(decoder: TokenDecodeService, cookie: CookieService, _: Router, private api: ApiService) {
    this.utils = new Utils(cookie, decoder, _);
    this.userData = this.utils.getUserDataFromToken();
    this.alertsForUser = [];
    this.newestTopics = [];
    this.getAlertsForUser();
    this.getRecentTopics();
  }

  private getAlertsForUser(): void {
    const token: string = this.utils.getJwtToken();
    this.api.getAlertas(token, this.userData.name!).subscribe(res => {
      this.alertsForUser = this.utils.arrayFromAny(res).filter(a => a.atualizacao == true);
    });
  }

  private getRecentTopics(): void {
    const token: string = this.utils.getJwtToken();
    this.api.recentTopics(token).subscribe(res => {
      this.newestTopics = res;
    });
  }
}
