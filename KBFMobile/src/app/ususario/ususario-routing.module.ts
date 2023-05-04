import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UsusarioPage } from './ususario.page';

const routes: Routes = [
  {
    path: '',
    component: UsusarioPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UsusarioPageRoutingModule {}
