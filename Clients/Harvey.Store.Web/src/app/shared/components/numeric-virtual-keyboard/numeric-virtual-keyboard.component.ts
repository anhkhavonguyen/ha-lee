import { Component, EventEmitter, OnInit, Output, HostListener } from '@angular/core';

@Component({
  selector: 'app-numeric-virtual-keyboard',
  templateUrl: './numeric-virtual-keyboard.component.html',
  styleUrls: ['./numeric-virtual-keyboard.component.scss']
})
export class NumericVirtualKeyboardComponent implements OnInit {

  @Output() phoneNumber: EventEmitter<any> = new EventEmitter<any>();
  tempPhoneNumber = '';

  constructor() {
  }

  ngOnInit() {
    if (!this.tempPhoneNumber) {
      this.phoneNumber.emit('');
    }
  }

  @HostListener('window:keydown)', ['$event'])
  keysEvent(event) {
    const value = Number(event.key);
    if (Number.isInteger(value)) {
      this.addNumber(value);
    } else {
      if (event.key === 'Delete') {
        this.clearPhoneNumber();
      }
      if (event.key === 'Backspace') {
        this.subNumber();
      }
    }
  }

  public addNumber(number: any) {
    if (this.tempPhoneNumber === '0') {
      this.tempPhoneNumber = '';
    }
    this.tempPhoneNumber = this.tempPhoneNumber + '' + number;
    this.phoneNumber.emit(this.tempPhoneNumber);
  }

  public clearPhoneNumber() {
    this.tempPhoneNumber = '';
    this.phoneNumber.emit('');
  }

  public subNumber() {
    this.tempPhoneNumber = this.tempPhoneNumber.substr(0, (this.maxLengthPhone() - 1));
    if (this.tempPhoneNumber !== '') {
      this.phoneNumber.emit(this.tempPhoneNumber);
    } else {
      this.phoneNumber.emit('');
    }

  }

  public maxLengthPhone() {
    const maxLength: number = this.tempPhoneNumber.length;
    return maxLength;
  }

}
