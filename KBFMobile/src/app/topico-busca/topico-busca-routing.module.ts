import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TopicoBuscaPage } from './topico-busca.page';

const routes: Routes = [
  {
    path: '',
    component: TopicoBuscaPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TopicoBuscaPageRoutingModule {}
