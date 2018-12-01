import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateCustomerProfilePageComponent } from './update-customer-profile-page.component';

describe('UpdateCustomerProfilePageComponent', () => {
  let component: UpdateCustomerProfilePageComponent;
  let fixture: ComponentFixture<UpdateCustomerProfilePageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateCustomerProfilePageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateCustomerProfilePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
