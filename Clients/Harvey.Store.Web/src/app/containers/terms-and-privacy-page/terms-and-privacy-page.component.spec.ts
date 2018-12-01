import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TermsAndPrivacyPageComponent } from './terms-and-privacy-page.component';

describe('TermsAndPrivacyPageComponent', () => {
  let component: TermsAndPrivacyPageComponent;
  let fixture: ComponentFixture<TermsAndPrivacyPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TermsAndPrivacyPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TermsAndPrivacyPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
