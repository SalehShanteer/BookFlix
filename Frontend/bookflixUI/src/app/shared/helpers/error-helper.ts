export class ErrorHelper {
  public static toArray(err: any): string[] {
    let errorsArray: string[] = [];
    if (Array.isArray(err)) errorsArray = err;
    else if (Array.isArray(err?.error)) errorsArray = err.error;
    return errorsArray;
  }
}
