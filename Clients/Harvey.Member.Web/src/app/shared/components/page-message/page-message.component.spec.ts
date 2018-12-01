import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageMessageComponent } from './page-message.component';

describe('PageMessageComponent', () => {
  let component: PageMessageComponent;
  let fixture: ComponentFixture<PageMessageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageMessageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
