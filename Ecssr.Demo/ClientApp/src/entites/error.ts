import { ValidationError } from "./validation-error";

export interface Error {
  message: string;
  httpStatusCode: number;
  refId: string;
  validationErrors: ValidationError[];
}
