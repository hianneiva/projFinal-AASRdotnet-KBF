import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Tag } from 'src/app/model/tag';
import { Topico } from 'src/app/model/topico';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-topic',
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.css']
})
export class TopicComponent {
  private utils!: Utils;

  public searchHasResults: boolean = true;
  public topics: Topico[];
  public filter?: string;
  public author?: string;
  public tags: string[];
  public tagsToSelect: Tag[];
  public recentTopics: boolean;
  public alphabetic: boolean;
  public errorMsg?: string;

  constructor(private api: ApiService, cookie: CookieService, decoder: TokenDecodeService, router: Router) {
    this.utils = new Utils(cookie, decoder, router);
    this.topics = [];
    this.tags = [];
    this.tagsToSelect = [];
    this.api.getTags(this.utils.getJwtToken()).subscribe(res => this.tagsToSelect = this.utils.arrayFromAny(res));
    this.recentTopics = true;
    this.alphabetic = true;
  }

  public search(): void {
    const token: string = this.utils.getJwtToken();
    this.author = this.author == '' ? undefined : this.author;
    this.filter = this.filter == '' ? undefined : this.filter;

    this.api.searchTopics(token, this.tags, this.filter, this.author).subscribe({
      next: (res) => {
        this.topics = res;
        this.searchHasResults = this.topics.length > 0;
      },
      error: (err) => {
        this.showMsg("Falha ao recuperar dados da busca");
        
        if (!environment.production) {
          console.log(err.message);
        }
      }
    });
  }

  private showMsg(msg: string) {
    this.errorMsg = msg;
    setTimeout(() => this.errorMsg = undefined, 5000);
  }
}
