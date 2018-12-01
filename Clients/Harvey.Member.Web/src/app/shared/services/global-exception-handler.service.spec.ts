import { TestBed, inject } from '@angular/core/testing';

import { GlobalExceptionHandlerService } from './global-exception-handler.service';

describe('GlobalExceptionHandlerService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GlobalExceptionHandlerService]
    });
  });

  it('should be created', inject([GlobalExceptionHandlerService], (service: GlobalExceptionHandlerService) => {
    expect(service).toBeTruthy();
  }));
});
