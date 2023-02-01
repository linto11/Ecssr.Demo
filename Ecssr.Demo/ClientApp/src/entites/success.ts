export interface Success<T> {
  message: string;
  httpStatusCode: number;
  refId: string;
  responseData: T;
}
