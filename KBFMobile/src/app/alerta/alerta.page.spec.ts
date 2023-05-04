import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AlertaPage } from './alerta.page';

describe('AlertaPage', () => {
  let component: AlertaPage;
  let fixture: ComponentFixture<AlertaPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(AlertaPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
