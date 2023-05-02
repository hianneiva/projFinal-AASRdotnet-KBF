import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
import { TokenDecoderService } from '../service/token-decoder.service';
import { UtilsService } from '../service/utils.service';
import { Alerta } from '../model/alerta';
import { ApiService } from '../service/api.service';
import moment from 'moment';
import { TopicoTag } from '../model/topico-tag';

@Component({
  selector: 'app-main',
  templateUrl: './main.page.html',
  styleUrls: ['./main.page.scss'],
})
export class MainPage implements OnInit {
  private utils: UtilsService;
  
  public failure?: string;
  public userData?: TokenData;
  public alertsForUser?: Alerta[];

  constructor(_c: CookieService, _r: Router, _t:TokenDecoderService, private api: ApiService) { 
    this.utils = new UtilsService(_c, _t, _r);
    this.userData  = this.utils.getUserDataFromToken();
  }

  ngOnInit() { 
    const token: string = this.utils.getJwtToken();
    this.api.getAlertas(token, this.userData!.name!).subscribe({
      next: (response) => {
        this.alertsForUser = this.utils.arrayFromAny(response).filter(a => a.atualizacao === true);
      },
      error: () => {
        this.failure = "Não foi possível recuperar alertas para o usuário";
      }
    });
  }

  public dismissAllAlerts(): void {
    const token: string = this.utils.getJwtToken();
    this.api.postAlertasDismiss(token, this.userData!.name!).subscribe({
      next: () =>{
        window.location.reload();
      },
      error: () => {
        this.failure = "Não foi possível marcar os alertas como lido";
      }
    });
  }

  public logoff(): void {
    this.userData = undefined;
    this.utils.logoff();
  }

  public parseDate(date: Date): string {
    return moment(date).format("DD/MM/YYYY");
  }

  public extractTags(tt: TopicoTag[]): string {
    return tt.map(entry => {
      return entry.tag!.descricao;
    }).join(', ');
  }
}
