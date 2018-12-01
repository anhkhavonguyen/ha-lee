import { TestBed, inject } from '@angular/core/testing';
import { AppSettingsService } from 'src/app/shared/services/app-settings.service';



describe('AppSettingsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AppSettingsService]
    });
  });

  it('should be created', inject([AppSettingsService], (service: AppSettingsService) => {
    expect(service).toBeTruthy();
  }));
});
