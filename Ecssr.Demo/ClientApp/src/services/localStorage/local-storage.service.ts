/*
 * This is a common service to set and get values from local storage
 */

import { Injectable, Type } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }

  //set value to storage
  set(key: string, value: string) {
    localStorage.setItem(key, value);
  }

  //get value from storage based on type
  get(key: string, type: string): any {
    let data = localStorage.getItem(key);
    if (!data) return null;

    switch (type) {
      case "number":
        return Number(data);
      case "boolean":
        return data === "true";
      case "object":
        try {
          return JSON.parse(data);
        } catch (e) {
          console.error(e);
          return null;
        }
      case "array":
        try {
          return JSON.parse(data);
        } catch (e) {
          console.error(e);
          return null;
        }
      default:
        return data;
    }
  }

  //remove storage
  remove(key: string) {
    localStorage.removeItem(key);
  }
}
