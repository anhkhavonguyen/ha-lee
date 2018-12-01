import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-update-setting-color',
  templateUrl: './update-setting-color.component.html',
  styleUrls: ['./update-setting-color.component.scss']
})
export class UpdateSettingColorComponent implements OnInit {
  @Input() appSettingColorValue = {
    backgroundColor: '',
    textColor: ''
  };

  @Output() change: EventEmitter<any> = new EventEmitter();

  public backgroundColor: any;
  public textColor: any;

  constructor() { }

  ngOnInit() {
    this.backgroundColor = this.appSettingColorValue.backgroundColor ? this.appSettingColorValue.backgroundColor : '#fffff';
    this.textColor = this.appSettingColorValue.textColor ? this.appSettingColorValue.textColor : '#fffff';
  }

  onChangeBackgroundColor() {
    this.backgroundColor = this.parseRgbColorTohex(this.backgroundColor);

    this.change.emit({
      backgroundColor: this.backgroundColor,
      textColor: this.textColor
    });
  }

  onChangeTextColor() {
    this.textColor = this.parseRgbColorTohex(this.textColor);

    this.change.emit({
      backgroundColor: this.backgroundColor,
      textColor: this.textColor
    });
  }

  onInputChange(typeChange: String, event: any) {
    if (typeChange === 'background-color') {
      this.backgroundColor = event.color;
      this.onChangeBackgroundColor();
    }

    if (typeChange === 'text-color') {
      this.textColor = event.color;
      this.onChangeTextColor();
    }
  }

  private parseRgbColorTohex(color: string) {
    if (!color) {
      return '';
    }

    const rgb = color.match(/^rgba?[\s+]?\([\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?/i);

    return (rgb && rgb.length === 4) ? '#' +
      ('0' + parseInt(rgb[1], 10).toString(16)).slice(-2) +
      ('0' + parseInt(rgb[2], 10).toString(16)).slice(-2) +
      ('0' + parseInt(rgb[3], 10).toString(16)).slice(-2) : '';
  }
}
