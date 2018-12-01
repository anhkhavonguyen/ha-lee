import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OutletInfoCardComponent } from './outlet-info-card.component';

describe('OutletInfoCardComponent', () => {
  let component: OutletInfoCardComponent;
  let fixture: ComponentFixture<OutletInfoCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OutletInfoCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OutletInfoCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
