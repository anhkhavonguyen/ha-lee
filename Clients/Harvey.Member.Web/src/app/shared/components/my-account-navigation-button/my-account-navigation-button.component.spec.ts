import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyAccountNavigationButtonComponent } from './my-account-navigation-button.component';

describe('MyAccountNavigationButtonComponent', () => {
  let component: MyAccountNavigationButtonComponent;
  let fixture: ComponentFixture<MyAccountNavigationButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyAccountNavigationButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyAccountNavigationButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
