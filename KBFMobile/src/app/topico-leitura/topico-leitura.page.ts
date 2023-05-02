import { Component, OnInit } from '@angular/core';
import { TokenData } from '../model/token-data';
import { UtilsService } from '../service/utils.service';
import { CookieService } from 'ngx-cookie-service';
import { ActivatedRoute, Router } from '@angular/router';
import { TokenDecoderService } from '../service/token-decoder.service';
import { ApiService } from '../service/api.service';
import { Topico } from '../model/topico';
import { Alerta } from '../model/alerta';
import { Comentario } from '../model/comentario';
import { marked } from 'marked';
import moment from 'moment';

@Component({
  selector: 'app-topico-leitura',
  templateUrl: './topico-leitura.page.html',
  styleUrls: ['./topico-leitura.page.scss'],
})
export class TopicoLeituraPage implements OnInit {
  private utils: UtilsService;
  private commentToEdit?: Comentario;
  private id?: string;
  private cmtIdToEdit?: string;
  private cmtIdToDel?: string;
  
  public userData?: TokenData;
  public topic?: Topico;
  public alert?: Alerta;
  public delMode: boolean;
  public editMode: boolean;
  public comment?: string;
  public failure?: string;
  public parsedContent?: string;
  public topicStatusReadable?: string;
  public commentPlaceholder: string;

  public deleteAlertBtns = [
    {
      text: 'Não',
      role: 'cancel',
      handler: () => this.cancelDel()
    },
    {
      text: 'Sim',
      role: 'confirm',
      handler: () => this.commitDel()
    }
  ];
  
  constructor(_c: CookieService, _r: Router, _t:TokenDecoderService, private api: ApiService, routerParam: ActivatedRoute) { 
    this.utils = new UtilsService(_c, _t, _r);
    this.userData  = this.utils.getUserDataFromToken();
    routerParam.params.subscribe(p => this.id = p['id']);
    this.editMode = false;
    this.delMode = false;
    this.commentPlaceholder = "Novo Comentário";
  }

  ngOnInit() {
    const token: string = this.utils.getJwtToken();
    this.api.getTopic(token, this.id!).subscribe({
      next: (res) => {
        this.topic = res;

        switch(this.topic!.tipoAcesso){
          case 1:
            this.topicStatusReadable = "Publicado";
            break;
          case 2:
            this.topicStatusReadable = "Finalizado";
            break;
          default:
            this.topicStatusReadable = "WIP";
        }

        this.parsedContent = marked(this.topic!.conteudo!);
        this.getAlertForTopic();
      },
      error: () => {
        this.failure = "Falha ao recuperar dados do tópico";
      }
    });
  }

  public logoff(): void {
    this.userData = undefined;
    this.utils.logoff();
  }

  public extractTags(): string[] {
    return this.topic!.topicoTag!.map(entry => { return entry.tag!.descricao; });
  }
  
  public parseDate(date: Date): string {
    return moment(date).format("DD/MM/YYYY HH:mm:ss");
  }

  public cancelEdit() {
    this.editMode = false;
    this.commentPlaceholder = "Novo Comentário";
    this.comment = undefined;
    this.cmtIdToEdit = undefined;
    this.commentToEdit = undefined;
  }

  public enableEdit(id: string) {
    this.cmtIdToEdit = id;
    this.editMode = true;
    this.commentPlaceholder = "Editar Comentário";
    
    const token = this.utils.getJwtToken();
    this.api.getComentario(token, this.cmtIdToEdit!).subscribe(res => {
      this.commentToEdit = res[0];
      this.comment = res[0].conteudo;
      this.editMode = true;
    });
  }

  public saveComment() {
    const token = this.utils.getJwtToken();

    if (this.utils.stringIsNullOrEmpty(this.cmtIdToEdit)) {
      let toPost = new Comentario();
      toPost.conteudo = this.comment!;
      toPost.dataCriacao = new Date();
      toPost.status = true;
      toPost.topicoId = this.id!;
      toPost.usuarioCriacao = this.userData!.name!;
      toPost.usuarioId = this.userData!.name!;

      this.api.postComentario(token, toPost).subscribe({
        next: (response) => {
          if(response) {
            this.comment = undefined;
            this.getComentarios(token);
          } else {
            this.failure = "Falha ao criar o comentário";
          }
        },
        error: () => {
          this.failure = "Falha ao criar o comentário";
        }
      });
    } else {
      this.commentToEdit!.conteudo = this.comment!;

      this.api.putComentario(token, this.commentToEdit!).subscribe({
        next: (response) => {
          if (response) {
            this.cancelEdit();
            this.getComentarios(token);
          } else {
            this.failure = "Falha ao criar o comentário";
          }
        },
        error: () => {
          this.failure = "Falha ao atualizar o comentário";
        }
      });
    }
  }

  public prepareDel(id: string) {
    this.delMode = true;
    this.cmtIdToDel = id;
  }

  public cancelDel() {
    this.delMode = false;
    this.cmtIdToDel = undefined;
  }

  public commitDel() {
    const token = this.utils.getJwtToken();
    this.api.deleteComentario(token, this.cmtIdToDel!).subscribe({
      next: (response) => {
        if (response) {
          this.getComentarios(token);
          this.delMode = false;
        } else {
          this.failure = "Falha ao remover comentário";
        }
      },
      error: () => {
        this.failure = "Falha ao remover comentário";
      }
    });
  }

  public toggleAlert() {
    const token: string = this.utils.getJwtToken();
    const userId: string = this.userData!.name!;

    if (this.alert === null || this.alert === undefined) {
      let alertToCreate: Alerta = new Alerta();
      alertToCreate.dataCriacao = new Date();
      alertToCreate.modoAlerta = 1;
      alertToCreate.topicoId = this.id!;
      alertToCreate.usuarioCriacao = userId;
      alertToCreate.usuarioId = userId;

      this.api.postAlerta(token, alertToCreate).subscribe({
        next: (res) => {
          if (!res) {
            this.failure = "Falha ao criar alerta";
          } else {
            this.getAlertForTopic();
          }
        },
        error: () => {
          this.failure = "Falha ao criar alerta";
        }
      });
    } else {
      this.api.deleteAlertas(token, this.alert!.id).subscribe({
        next: (res) => {
          if (!res) {
            this.failure = "Falha ao apagar alerta";
          } else {
            this.alert = undefined;
          }
        },
        error: () => {
          this.failure = "Falha ao apagar alerta";
        }
      });
    }
  }

  private getAlertForTopic(): void {
    const token: string = this.utils.getJwtToken();
    const userId: string = this.utils.getUserDataFromToken().name!;
    this.api.getAlerta(token, userId, this.id!).subscribe({
      next: (res) => {
        this.alert = res;
      },
      error: () => {
        this.failure = "Falha ao recuperar dados de alerta para o tópico";
      }
    });
  }

  private getComentarios(token: string): void {
    this.api.getAllComentarios(token, this.id!).subscribe({
      next: res => {
        this.topic!.comentarios = this.utils.arrayFromAny(res);
      },
      error: () => {
        this.failure = "Falha ao recuperar comentários do tópico";
      }
    });
  }
}
