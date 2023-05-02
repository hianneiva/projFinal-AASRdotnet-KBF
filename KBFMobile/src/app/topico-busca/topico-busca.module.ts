import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { TopicoBuscaPageRoutingModule } from './topico-busca-routing.module';

import { TopicoBuscaPage } from './topico-busca.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    TopicoBuscaPageRoutingModule
  ],
  declarations: [TopicoBuscaPage]
})
export class TopicoBuscaPageModule {}
