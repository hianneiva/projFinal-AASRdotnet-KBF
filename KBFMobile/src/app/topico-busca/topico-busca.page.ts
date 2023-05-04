import { Component, OnInit } from '@angular/core';
import { UtilsService } from '../service/utils.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { TokenDecoderService } from '../service/token-decoder.service';
import { ApiService } from '../service/api.service';
import { TokenData } from '../model/token-data';
import { Topico } from '../model/topico';
import { TopicoTag } from '../model/topico-tag';
import moment from 'moment';

@Component({
  selector: 'app-topico-busca',
  templateUrl: './topico-busca.page.html',
  styleUrls: ['./topico-busca.page.scss'],
})
export class TopicoBuscaPage implements OnInit {
  private utils: UtilsService;
  
  public userData?: TokenData;
  public found?: Topico[];
  public tagsOptions?: string[];
  public tagsSelected?: string[];
  public failure?: string;
  public topicName?: string;
  public authorName?: string;
  public recent: boolean;
  public abOrdered: boolean;

  constructor(_c: CookieService, _r: Router, _t:TokenDecoderService, private api: ApiService) { 
    this.utils = new UtilsService(_c, _t, _r);
    this.userData = this.utils.getUserDataFromToken();
    this.recent = true;
    this.abOrdered = true;
  }

  ngOnInit() {
    this.api.getTags(this.utils.getJwtToken()).subscribe({
      next: (res) => {
        this.tagsOptions = this.utils.arrayFromAny(res).map(t => {
          return t.descricao;
        });
      },
      error: (err) => {
        this.failure = "Não foi possível recuperar as tags para busca: " + err.message;
      }
    });
  }
  
  public parseDate(date: Date): string {
    return moment(date).format("DD/MM/YYYY");
  }
  
  public extractTags(tt: TopicoTag[]): string {
    return tt.map(entry => {
      return entry.tag!.descricao;
    }).join(', ');
  }

  public search() {
    const token = this.utils.getJwtToken();
    const author = this.utils.stringIsNullOrEmpty(this.authorName) ? undefined : this.authorName;
    const filter = this.utils.stringIsNullOrEmpty(this.topicName) ? undefined : this.topicName;

    this.api.searchTopics(token, this.tagsSelected!, filter, author).subscribe({
      next: (res) => {
        this.found = this.utils.arrayFromAny(res);
      },
      error: (err) => {
        this.failure = "Falha ao obter resultado da busca: " + err.message;
      }
    });
  }
}
