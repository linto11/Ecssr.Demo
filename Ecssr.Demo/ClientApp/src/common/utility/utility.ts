/*
 * This file is used to maintain all the common functionalities requried thorughout the application
 */

export class Utility {
  public createRange(pageNumber: number) {
    return new Array(pageNumber).fill(0)
      .map((n, index) => index + 1);
  }
}
