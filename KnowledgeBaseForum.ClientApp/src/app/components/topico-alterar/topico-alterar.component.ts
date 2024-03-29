import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { MarkdownService } from 'ngx-markdown';
import { Tag } from 'src/app/model/tag';
import { TokenData } from 'src/app/model/token-data';
import { Topico } from 'src/app/model/topico';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';

@Component({
  selector: 'app-topico-alterar',
  templateUrl: './topico-alterar.component.html',
  styleUrls: ['./topico-alterar.component.css']
})
export class TopicoAlterarComponent {
  private utils: Utils;
  private id: string = '';
  public errorMsg?: string;
  public markdownContent?: string;
  public successMsg?: string;
  public tagNameToAdd?: string;
  public textAlertTypes = ["Padrão", "Apenas notificação"];
  public selectedTags?: string[];
  public tagsSelect?: Tag[];
  public topico?: Topico;

  constructor(private api: ApiService, cookie: CookieService, private router: Router, decoder: TokenDecodeService, private markdown: MarkdownService, private routerParam: ActivatedRoute) {
    this.utils = new Utils(cookie, decoder, router);
    this.routerParam.params.subscribe(params => this.id = params['id']);

    try {
      this.getTopic();
    } catch (err) {
      this.errorMsg = "Falha na recuperação dos topicos";
      setTimeout(() => this.errorMsg = undefined, 5000);
    }

    this.getTags();
  }

  private getTopic() {
    this.api.getTopic(this.utils.getJwtToken(), this.id).subscribe({
      next: (res) => {
        this.topico = res;
        this.selectedTags = res.topicoTag?.map(tt => tt.tagId);
      },
      error: (err) => {
        this.showMsg("Falha ao recuperar dados do tópico: " + err.message, false);
      }
    });
  }

  public alterarTopico(topico: Topico): void {
    if (this.utils.stringIsNullOrEmpty(topico.titulo) || this.utils.stringIsNullOrEmpty(topico.conteudo)) {
      return;
    }

    const usuario: TokenData = this.utils.getUserDataFromToken();
    topico.status = true;
    topico.usuarioModificacao = usuario.name!;
    topico.dataModificacao = new Date();
    const token: string = this.utils.getJwtToken();

    this.api.modifyTopic(token, topico).subscribe({
      next: (res) => {
        if (!res) {
          this.showMsg("Falha ao atualizar novo topico", false);
        } else {
          this.showMsg("Topico atualizado com sucesso", true);
          const currentTags = this.topico!.topicoTag?.map(tt => tt.tagId);
          const toAdd = this.selectedTags?.filter(t => !currentTags?.includes(t));
          const toRemove = currentTags?.filter(t => !this.selectedTags?.includes(t));

          for (let i = 0; i < toRemove!.length; i++) {
            this.api.deleteTT(token, toRemove![i], this.topico!.id).subscribe(res => {
              if(res == true){
                console.log("Tag removed");
              } else {
                console.log("Failure removing tag");
              }
            });
          }
          
          for (let j = 0; j < toAdd!.length; j++) {
            this.api.postTT(token, toAdd![j], this.topico!.id).subscribe(res => {
              if(res == true){
                console.log("Tag registered");
              } else {
                console.log("Failure registering tag");
              }
            });
          }

          setTimeout(() => { this.cancelar(); }, 5000);
        }
      },
      error: (err) => {
        this.showMsg("Falha não esperada na atualização de tópico: " + err.message, false);
      }
    });
  }

  public cancelar(reload: boolean = false): void {
    if (reload) {
      this.router.navigate(['/'])/*.then(() => { window.location.reload(); })*/;
    }
    else {
      this.router.navigate(['/topico/topico-usuario']);
    }
  }

  public updateMarkdown() {
    this.markdown.reload();
  }
  
  public cancelTagCreation(): void {
    this.tagNameToAdd = undefined;
  }

  public addTag(): void {
    if (this.utils.stringIsNullOrEmpty(this.tagNameToAdd)) {
      this.successMsg = undefined;
      this.errorMsg = "Nova tag deve ter conteúdo";
      setTimeout(() => this.errorMsg = undefined, 5000);
      return;
    }

    let tag: Tag = new Tag();
    tag.dataCriacao = new Date();
    tag.descricao = this.tagNameToAdd!;
    tag.usuarioCriacao = this.utils.getUserDataFromToken().name!;
    const token: string = this.utils.getJwtToken();

    this.api.postTag(token, tag).subscribe({
      next: (res) => {
        if (res) {
          this.showMsg("Tag criada com sucesso", true);
          this.cancelTagCreation();
          this.getTags();
        } else {
          this.showMsg("Falha ao criar nova tag", false);
        }
      },
      error: (err) => {
        this.showMsg("Falha não esperada na criação de tags: "+ err.message, false);
      }
    });
  }

  private getTags(): void {
    const token = this.utils.getJwtToken();
    this.api.getTags(token).subscribe({
      next: (res) => {
        this.tagsSelect = res;
      },
      error: (err) => {
        this.showMsg("Falha não esperada na recuperação de tags: " + err.message, false);
      }
    });
  }

  private showMsg(msg: string, success: boolean) {
    if (success) {
      this.successMsg = msg;
      this.errorMsg = undefined;
      setTimeout(() => this.successMsg = undefined, 5000);
    } else {
      this.successMsg = undefined;
      this.errorMsg = msg;
      setTimeout(() => this.errorMsg = undefined, 5000);
    }
  }
}
