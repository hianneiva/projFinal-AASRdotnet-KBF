import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
import { Topico } from 'src/app/model/topico';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';
import { MarkdownService } from 'ngx-markdown';
import { Tag } from 'src/app/model/tag';
import { TopicoTag } from 'src/app/model/topico-tag';

@Component({
  selector: 'app-topico-novo',
  templateUrl: './topico-novo.component.html',
  styleUrls: ['./topico-novo.component.css']
})
export class TopicoNovoComponent {
  private utils: Utils;
  public textAlertTypes = ["Padrão", "Apenas notificação"];
  public errorMsg?: string;
  public markdownContent?: string;
  public successMsg?: string;
  public tagNameToAdd?: string;
  public selectedTags: string[];
  public tagsSelect?: Tag[];
  public topico: Topico = new Topico();

  constructor(private api: ApiService, cookie: CookieService, private router: Router, private decoder: TokenDecodeService, private markdown: MarkdownService) {
    this.utils = new Utils(cookie, decoder, router);
    this.topico.tipoAcesso = 0;
    this.getTags();
    this.selectedTags = [];
  }
  
  public cadastrarTopico(topico:Topico): void{
    
    if (this.utils.stringIsNullOrEmpty(topico.titulo) || this.utils.stringIsNullOrEmpty(topico.conteudo)) {
      return;
    }
    
    const usuario: TokenData = this.utils.getUserDataFromToken(); 
    topico.status = true;
    topico.usuarioId = usuario.name!;
    topico.usuarioCriacao = usuario.name!;
    topico.dataCriacao = new Date();
    const token: string = this.utils.getJwtToken();
    
    this.api.createTopic(token, topico).subscribe({
      next: (res) => {
        if (!res) {
          this.showMsg("Falha ao cadastrar novo topico", false);
        } else {
          
          this.showMsg("Topico cadastrado com sucesso", true);
          const topicId = res.id;

          for (let i = 0; i < this.selectedTags.length; i++) {
            this.api.postTT(token, this.selectedTags[i], topicId).subscribe({
              next: (res) => {
                if(res == true){
                  console.log("Tag registered");
                } else {
                  console.log("Failure registering tag");
                }
              },
              error: (err) => {
                console.log("Unexpected failure registering tag: " + err.message);
              }
            });
          }
          
          setTimeout(() => this.cancelar(), 2500);
        }
      }
    });
  }

  public cancelar(reload: boolean = false): void {
    if (reload) {
      this.router.navigate(['/'])/*.then(() => { window.location.reload(); })*/;
    }
    else {
      this.router.navigate(['topico']);
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
        this.showMsg("Falha não esperada na criação de tópicos: " + err.message, false);
      }
    });
  }

  private getTags(): void {
    const token = this.utils.getJwtToken();
    this.api.getTags(token).subscribe(res => {
      this.tagsSelect = res;
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