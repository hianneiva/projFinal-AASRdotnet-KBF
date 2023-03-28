import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { MarkdownService } from 'ngx-markdown';
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
  public markdownContent?: string;

  public textAlertTypes = ["Padrão", "Apenas notificação"];
  public errorMsg?: string;
  public successMsg?: string;
  topico: Topico[] = [];

  constructor(private api: ApiService, private cookie: CookieService, private router: Router, decoder: TokenDecodeService, private markdown: MarkdownService, private routerParam: ActivatedRoute) {
    this.utils = new Utils(cookie, decoder, router);
    this.routerParam.params.subscribe(params => this.id = params['id']);

    try {
      this.getTopic();
    } catch (err) {
      this.errorMsg = "Falha na recuperação dos topicos";
      setTimeout(() => this.errorMsg = undefined, 5000);
    }
  }  

  private getTopic() {
    this.api.getTopic(this.utils.getJwtToken(),this.id).subscribe(res => {
      this.topico = this.utils.arrayFromAny(res)
    });
  }

  public alterarTopico(topico:Topico):void{
    
    if (this.utils.stringIsNullOrEmpty(topico.titulo) || this.utils.stringIsNullOrEmpty(topico.conteudo)) {
      return;
    }
    
    const usuario: TokenData = this.utils.getUserDataFromToken();     
    
    topico.status = true;
    topico.usuarioModificacao = usuario.given_name!;
    topico.dataModificacao = new Date();

    const token: string = this.utils.getJwtToken();

    this.api.modifyTopic(token, topico).subscribe(res => {
      if (res === null || res === undefined) {
        this.successMsg = undefined;
        this.errorMsg = "Falha ao atualizar novo topico";
        setTimeout(() => this.errorMsg = undefined, 5000);
      } else {
        this.successMsg = "Topico atualizado com sucesso";
        this.errorMsg = undefined;
        setTimeout(() => this.successMsg = undefined, 5000);
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

}
