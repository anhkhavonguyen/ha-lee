import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadFileService } from 'src/app/shared/components/upload-file/upload-file.service';
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Observable, zip } from 'rxjs';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.scss']
})
export class UploadFileComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private uploadService: UploadFileService) {
  }
  selectedFiles!: FileList;
  currentFileUpload!: File | null;
  public isUploading = false;
  public message = '';
  public error = '';
  public percentDone = 0;

  ngOnInit() {
  }

  selectFile(event: { target: { files: FileList; }; }) {
    this.selectedFiles = event.target.files;
    if (this.selectedFiles !== null) {
      this.currentFileUpload = this.selectedFiles.item(0);
    }
    this.resetUploadInfo();
  }
  upload() {
    this.isUploading = true;
    this.resetUploadInfo();
    let messageResponse = '';

    const pushFileToStorage$ = this.uploadService.pushFileToStorage(this.currentFileUpload);
    const pushFileToIds$ = this.uploadService.pushFileToIds(this.currentFileUpload);

    const uploadQueue = zip(pushFileToStorage$, pushFileToIds$);
    uploadQueue.subscribe((event: any) => {
      if (event[0].type === HttpEventType.UploadProgress && event[1].type === HttpEventType.UploadProgress) {
        this.percentDone = Math.round(100 * (event[0].loaded + event[1].loaded) / (event[0].total + event[1].total));
      } else if (event[0].partialText && event[1].partialText) {
        this.isUploading = false;
        if (event[0].partialText) {
          messageResponse += event[0].partialText;
        }
        if (event[1].partialText) {
          messageResponse += event[1].partialText;
        }
        this.message = messageResponse !== '' ? 'Upload success with message: ' + messageResponse : 'Upload success!';
        this.message = this.message.replace(/\n/g, '<br/>');
      }
    }, err => {
      this.message = err;
    });
  }

  onClose(): void {
    this.activeModal.close('closed');
  }

  onDismiss(reason: String): void {
    this.activeModal.dismiss(reason);
  }

  resetUploadInfo() {
    this.message = '';
    this.error = '';
    this.percentDone = 0;
  }

}
