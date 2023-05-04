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

  public userData: TokenData;
  public alertsForUser: Alerta[];
  public newestTopics: Topico[];
  public errorMsg?: string;

  constructor(decoder: TokenDecodeService, cookie: CookieService, private router: Router, private api: ApiService) {
    this.utils = new Utils(cookie, decoder, router);
    this.userData = this.utils.getUserDataFromToken();
    this.alertsForUser = [];
    this.newestTopics = [];
    this.getAlertsForUser();
    this.getRecentTopics();
  }

  public dismissAllAlerts(): void {
    const token: string = this.utils.getJwtToken();
    this.api.postAlertasDismiss(token, this.userData.name!).subscribe({
      next: (res) => {
        if (res) {
          window.location.reload();
        } else {
          this.showMsg("Falha ao desconsiderar todos os alertas", false);
        }
      },
      error: (err) => {
        this.showMsg("Falha não esperada ao desconsiderar alertas: " + err.message, false);
      }
    });
  }

  public readAndRouteToTopic(id: string, topicId: string) {
    const token: string = this.utils.getJwtToken();

    this.api.updateAlertas(token, id, false, true).subscribe({
      next: (res) => {
        if (!res) {
          this.showMsg("Falha ao marcar alerta como lido", false);
        } else {
          this.router.navigate(['topico/topico-leitura/', topicId]);
        }
      },
      error: (err) => {
        this.showMsg("Falha não esperada: " + err.message, false);
      }
    });
  }

  private getAlertsForUser(): void {
    const token: string = this.utils.getJwtToken();
    this.api.getAlertas(token, this.userData.name!).subscribe({
      next: (res) => {
        if (res) {
          this.alertsForUser = this.utils.arrayFromAny(res).filter(a => a.atualizacao == true);
        } else {
          this.showMsg("Falha ao recuperar alertas do usuário", false);
        }
      },
      error: (err) => {
        if(err.status !== 401) {
          this.showMsg("Falha não esperada ao recuperar alertas: " + err.message, false);
        }
      }
    });
  }

  private getRecentTopics(): void {
    const token: string = this.utils.getJwtToken();
    this.api.recentTopics(token).subscribe({
      next: (res) => {
        if (res) {
          this.newestTopics = res;
        } else {
          this.showMsg("Falha ao recuperar tópicos mais recentes.", false);
        }
      },
      error: (err) => {
        if(err.status !== 401) {
          this.showMsg("Falha não esperada ao recuperar tópicos recentes: " + err.message, false);
        }
      }
    });
  }

  private showMsg(msg: string, success: boolean) {
    this.errorMsg = msg;
    setTimeout(() => this.errorMsg = undefined, 5000);
  }
}
