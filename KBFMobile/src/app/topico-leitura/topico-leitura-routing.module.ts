import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TopicoLeituraPage } from './topico-leitura.page';

const routes: Routes = [
  {
    path: '',
    component: TopicoLeituraPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TopicoLeituraPageRoutingModule {}
