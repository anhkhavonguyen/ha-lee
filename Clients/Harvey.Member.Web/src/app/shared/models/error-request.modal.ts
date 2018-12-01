export class ErrorRequest {
  constructor(error?: any) {
    if (!error) {
      return;
    }
    this.UserId = error.UserId;
    this.ErrorMessage = error.ErrorMessage;
    this.ErrorCaption = error.ErrorCaption;
    this.Source = error.Source;
  }

  public UserId = '';
  public ErrorMessage = '';
  public ErrorCaption = '';
  public Source = '';
}
