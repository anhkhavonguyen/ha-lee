import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateSettingValueComponent } from './update-setting-value.component';

describe('UpdateSettingValueComponent', () => {
  let component: UpdateSettingValueComponent;
  let fixture: ComponentFixture<UpdateSettingValueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateSettingValueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateSettingValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
