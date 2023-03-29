import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Tag } from 'src/app/model/tag';
import { Topico } from 'src/app/model/topico';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';

@Component({
  selector: 'app-topic',
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.css']
})
export class TopicComponent {
  private utils!: Utils;
  searchHasResults: boolean = true;
  topics: Topico[];
  filter?: string;
  author?: string;
  tags: string[];
  tagsToSelect: Tag[];

  constructor(private api: ApiService, cookie: CookieService, decoder: TokenDecodeService, router: Router) {
    this.utils = new Utils(cookie, decoder, router);
    this.topics = [];
    this.tags = [];
    this.tagsToSelect = [];
    this.api.getTags(this.utils.getJwtToken()).subscribe(res => this.tagsToSelect = this.utils.arrayFromAny(res));
  }

  search(): void {
    const token: string = this.utils.getJwtToken();
    this.author = this.author == '' ? undefined : this.author;
    this.filter = this.filter == '' ? undefined : this.filter;

    this.api.searchTopics(token, this.tags, this.filter, this.author).subscribe(res => {
      this.topics = res;
      this.searchHasResults = this.topics.length > 0;
    });
  }
}
