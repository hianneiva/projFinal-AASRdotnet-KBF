import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UsusarioPage } from './ususario.page';

describe('UsusarioPage', () => {
  let component: UsusarioPage;
  let fixture: ComponentFixture<UsusarioPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(UsusarioPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
