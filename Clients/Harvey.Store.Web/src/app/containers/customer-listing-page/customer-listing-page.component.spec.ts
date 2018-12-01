import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerListingPageComponent } from './customer-listing-page.component';

describe('CustomerListingPageComponent', () => {
  let component: CustomerListingPageComponent;
  let fixture: ComponentFixture<CustomerListingPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerListingPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerListingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
