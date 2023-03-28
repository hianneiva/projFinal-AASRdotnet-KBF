import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
import { Topico } from 'src/app/model/topico';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';
import { MarkdownService } from 'ngx-markdown';

@Component({
  selector: 'app-topico-novo',
  templateUrl: './topico-novo.component.html',
  styleUrls: ['./topico-novo.component.css']
})
export class TopicoNovoComponent {

  private utils: Utils;
  public markdownContent?: string;

  public textAlertTypes = ["Padrão", "Apenas notificação"];
  public errorMsg?: string;
  public successMsg?: string;
  topico: Topico = new Topico();

  constructor(private api: ApiService, private cookie: CookieService, private router: Router, decoder: TokenDecodeService, private markdown: MarkdownService) {
    this.utils = new Utils(cookie, decoder, router);
  }
  
  public cadastrarTopico(topico:Topico):void{
    
    if (this.utils.stringIsNullOrEmpty(topico.titulo) || this.utils.stringIsNullOrEmpty(topico.conteudo)) {
      return;
    }
    
    const usuario: TokenData = this.utils.getUserDataFromToken(); 
    
    topico.status = true;
    topico.usuarioId = usuario.name!;
    topico.usuarioCriacao = usuario.given_name!;
    topico.dataCriacao = new Date();

    const token: string = this.utils.getJwtToken();
    
    this.api.createTopic(token, topico).subscribe(res => {
      if (res === null || res === undefined) {
        this.successMsg = undefined;
        this.errorMsg = "Falha ao cadastrar novo topico";
        setTimeout(() => this.errorMsg = undefined, 5000);
      } else {
        this.successMsg = "Topico cadastrado com sucesso";
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