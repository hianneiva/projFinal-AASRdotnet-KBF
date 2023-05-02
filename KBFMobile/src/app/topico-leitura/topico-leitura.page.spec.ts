import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TopicoLeituraPage } from './topico-leitura.page';

describe('TopicoLeituraPage', () => {
  let component: TopicoLeituraPage;
  let fixture: ComponentFixture<TopicoLeituraPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(TopicoLeituraPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
