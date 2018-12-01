import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateSettingColorComponent } from './update-setting-color.component';

describe('UpdateSettingColorComponent', () => {
  let component: UpdateSettingColorComponent;
  let fixture: ComponentFixture<UpdateSettingColorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateSettingColorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateSettingColorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
