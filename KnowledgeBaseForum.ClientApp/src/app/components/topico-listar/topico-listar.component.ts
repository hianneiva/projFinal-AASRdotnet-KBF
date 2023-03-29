import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
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

    this.api.deleteTopic(token, this.idToDel!).subscribe(res => {
      if (res === null || res === undefined) {
        this.successMsg = undefined;
        this.errorMsg = "Falha na remoção do topico";
        setTimeout(() => this.errorMsg = undefined, 5000);
      } else {
        this.getAllTopicos();
        this.successMsg = "Topico removido com sucesso";
        this.errorMsg = undefined;
        setTimeout(() => this.successMsg = undefined, 5000);
      }
    });
  }

  private getAllTopicos() {
    const token: string = this.utils.getJwtToken();
    const id: string = this.utils.getUserDataFromToken().name!;
    this.api.listAuthorTopics(token, id).subscribe(res => {
      this.topicos = this.utils.arrayFromAny(res)
    });
  }

  public alterarTopico(id?: string) {
    this.router.navigate([`/topico/topico-alterar/${id}`]);
  }
}