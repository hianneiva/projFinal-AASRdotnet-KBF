import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import moment from 'moment';
import { CookieService } from 'ngx-cookie-service';
import { MarkdownService } from 'ngx-markdown';
import { Alerta } from 'src/app/model/alerta';
import { Comentario } from 'src/app/model/comentario';
import { Topico } from 'src/app/model/topico';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';

@Component({
  selector: 'app-topico-leitura',
  templateUrl: './topico-leitura.component.html',
  styleUrls: ['./topico-leitura.component.css']
})
export class TopicoLeituraComponent {
  private utils: Utils;
  private id: string = '';
  private commentToEdit?: Comentario;

  public currentTopic?: Topico;
  public alertFortopic?: Alerta;
  public editMode: boolean = false;
  public comment?: string;
  public errorMsg?: string;
  public successMsg?: string;
  public userId: string = '';

  constructor(private api: ApiService, cookie: CookieService, router: Router, decoder: TokenDecodeService, private markdown: MarkdownService, private routerParam: ActivatedRoute) {
    this.utils = new Utils(cookie, decoder, router);
    this.userId = this.utils.getUserDataFromToken().name!;
    this.routerParam.params.subscribe(params => this.id = params['id']);

    try {
      this.getTopic();
    } catch (err) {
      this.errorMsg = "Falha na recuperação dos topicos";
      setTimeout(() => this.errorMsg = undefined, 5000);
    }
  }

  public formattedDate(date: Date): string {
    return moment(date).format('DD/MM/YYYY HH:mm:ss');
  }

  public addComment(): void {
    if (this.utils.stringIsNullOrEmpty(this.comment)) {
      this.errorMsg = "Comentário não pode ser vazio";
      setTimeout(() => { this.errorMsg = undefined; }, 5000);
    }

    let commentToAdd: Comentario = new Comentario();
    commentToAdd.conteudo = this.comment!;
    commentToAdd.dataCriacao = new Date();
    commentToAdd.status = true;
    commentToAdd.topicoId = this.id;
    commentToAdd.usuarioCriacao = this.userId;
    commentToAdd.usuarioId = this.userId;
    const token: string = this.utils.getJwtToken();

    this.api.postComentario(token, commentToAdd).subscribe(res => {
      if (res) {
        this.comment = undefined;
        this.getComentarios(token);
      }
    });
  }

  public deleteComentario(id: string): void {
    const token: string = this.utils.getJwtToken();
    this.api.deleteComentario(token, id).subscribe(res => {
      if(res) {
        this.getComentarios(token);
      }
    });
  }

  public cancelEdit(): void {
    this.editMode = false;
    this.comment = undefined;
  }

  public editComment(id: string): void {
    const token: string = this.utils.getJwtToken();
    this.api.getComentario(token, id).subscribe(res => {
      this.commentToEdit = res[0];
      this.comment = res[0].conteudo;
      this.editMode = true;
    });
  }

  public applyEdit(): void {
    const token: string = this.utils.getJwtToken();
    this.commentToEdit!.conteudo = this.comment!;
    this.api.putComentario(token, this.commentToEdit!).subscribe(res => {
      if (res) {
        this.editMode = false;
        this.comment = undefined;
        this.getComentarios(token);
      }
    });
  }

  public deleteAlert(): void {
    const token: string = this.utils.getJwtToken();
    this.api.deleteAlertas(token, this.alertFortopic!.id).subscribe(res => {
      if (res) {
        this.successMsg = "Alerta removido";
        setTimeout(() => { this.successMsg = undefined; }, 5000);
        this.alertFortopic = undefined;
      }
    });
  }

  public createAlert(): void {
    const token: string = this.utils.getJwtToken();
    const userId: string = this.utils.getUserDataFromToken().name!;
    let alert: Alerta = new Alerta();
    alert.dataCriacao = new Date();
    alert.modoAlerta = 1;
    alert.topicoId = this.id;
    alert.usuarioCriacao = userId;
    alert.usuarioId = userId;
    this.api.postAlerta(token, alert).subscribe(res => {
      if (res) {
        this.successMsg = "Alerta criado";
        setTimeout(() => { this.successMsg = undefined; }, 5000);
        this.alertFortopic = res;
      }
    });
  }

  private getAlertForTopic(): void {
    const token: string = this.utils.getJwtToken();
    const userId: string = this.utils.getUserDataFromToken().name!;
    this.api.getAlerta(token, userId, this.id).subscribe(res => {
      this.alertFortopic = res;
    });
  }

  private getComentarios(token: string): void {
    this.api.getAllComentarios(token, this.id).subscribe(res => {
      this.currentTopic!.comentarios = this.utils.arrayFromAny(res);
    });
  }

  private getTopic(): void {
    this.api.getTopic(this.utils.getJwtToken(), this.id).subscribe(res => {
      this.currentTopic = res;
      this.getAlertForTopic();
    });
  }
}
