import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PointSummaryComponent } from './point-summary.component';

describe('PointSummaryComponent', () => {
  let component: PointSummaryComponent;
  let fixture: ComponentFixture<PointSummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PointSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PointSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
