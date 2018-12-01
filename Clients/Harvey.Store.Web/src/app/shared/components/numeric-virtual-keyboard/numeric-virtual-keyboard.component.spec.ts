import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NumericVirtualKeyboardComponent } from './numeric-virtual-keyboard.component';

describe('NumericVirtualKeyboardComponent', () => {
  let component: NumericVirtualKeyboardComponent;
  let fixture: ComponentFixture<NumericVirtualKeyboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NumericVirtualKeyboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NumericVirtualKeyboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
