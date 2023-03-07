import { Component } from '@angular/core';
import { MarkdownService } from 'ngx-markdown';

@Component({
  selector: 'app-markdown-edit',
  templateUrl: './markdown-edit.component.html',
  styleUrls: ['./markdown-edit.component.css']
})
export class MarkdownEditComponent {
  public markdownContent?: string;

  constructor(private markdown: MarkdownService) { }

  updateMarkdown() {
    this.markdown.reload();
  }
}
