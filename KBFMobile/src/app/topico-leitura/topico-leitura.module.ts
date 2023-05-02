import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TopicoLeituraPageRoutingModule } from './topico-leitura-routing.module';

import { TopicoLeituraPage } from './topico-leitura.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TopicoLeituraPageRoutingModule
  ],
  declarations: [TopicoLeituraPage]
})
export class TopicoLeituraPageModule {}
