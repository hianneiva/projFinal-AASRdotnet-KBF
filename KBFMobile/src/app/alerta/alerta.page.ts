import { Component, OnInit } from '@angular/core';
import { UtilsService } from '../service/utils.service';
import { TokenData } from '../model/token-data';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { TokenDecoderService } from '../service/token-decoder.service';
import { ApiService } from '../service/api.service';
import { Alerta } from '../model/alerta';

@Component({
  selector: 'app-alerta',
  templateUrl: './alerta.page.html',
  styleUrls: ['./alerta.page.scss'],
})
export class AlertaPage implements OnInit {
  private utils: UtilsService;

  public alertTypeDictionary = ["Padrão", "Apenas notificação"];
  public alerts: Alerta[];
  public failure?: string;
  public alertIdToDel?: string;
  public userData?: TokenData;

  public deleteAlertBtns = [
    {
      text: 'Não',
      role: 'cancel',
      handler: () => this.cancelDelete()
    },
    {
      text: 'Sim',
      role: 'confirm',
      handler: () => this.commitDelete()
    }
  ];
  
  constructor(_c: CookieService, _r: Router, _t:TokenDecoderService, private api: ApiService) { 
    this.utils = new UtilsService(_c, _t, _r);
    this.userData  = this.utils.getUserDataFromToken();
    this.alerts = [];
  }

  ngOnInit() {
    this.getAllAlerts();
  }

  public logoff(): void {
    this.userData = undefined;
    this.utils.logoff();
  }
  
  public toggleAlert(idToUpdate: string) {
    const token: string = this.utils.getJwtToken();

    this.api.updateAlertas(token, idToUpdate, true, false).subscribe({
      next: (res) => {
        if (res === null || res === undefined) {
          this.failure = "Falha ao recuperar dados de alertas pós-atualização";
        } else {
          this.getAllAlerts();
        }
      },
      error: () => {
        this.failure = "Falha ao recuperar dados de alertas pós-atualização";
      }
    });
  }

  public prepDelete(id: string) {
    this.alertIdToDel = id;
  }

  public cancelDelete() {
    this.alertIdToDel = undefined;
  }

  public commitDelete() {
    const token: string = this.utils.getJwtToken();

    this.api.deleteAlertas(token, this.alertIdToDel!).subscribe({
      next: (res) => {
        if (res === null || res === undefined) {
          this.failure = "Falha ao recuperar dados de alertas pós-deleção";
        } else {
          this.getAllAlerts();
        }
      },
      error: () => {
        this.failure = "Falha ao recuperar dados de alertas pós-deleção";
      }
    });
  }

  private getAllAlerts() {
    const token: string = this.utils.getJwtToken();
    this.api.getAlertas(token, this.userData!.name!).subscribe({
      next:  (res) => {
        this.alerts = this.utils.arrayFromAny(res)
      },
      error: () => {
        this.failure = "Não foi possível pegar os alertas para tópicos inscritos"
      }
    });
  }
}
