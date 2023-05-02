import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TopicoBuscaPage } from './topico-busca.page';

describe('TopicoBuscaPage', () => {
  let component: TopicoBuscaPage;
  let fixture: ComponentFixture<TopicoBuscaPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(TopicoBuscaPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
