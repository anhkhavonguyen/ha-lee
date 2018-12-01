import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSettingValueComponent } from './add-setting-value.component';

describe('AddSettingValueComponent', () => {
  let component: AddSettingValueComponent;
  let fixture: ComponentFixture<AddSettingValueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddSettingValueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddSettingValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
