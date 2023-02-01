/*
 * This service is a common function to get any value from the cookie.
 */

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppCookieService {
  constructor() {}

  private parseCookies(cookies = document.cookie, key: string) {
    if (!!cookies === false) { return; }
    const cookiesArr = cookies.split(';');
    for (const cookie of cookiesArr) {
      const cookieArr = cookie.split('=');
      if (key.toLowerCase().trim() === cookieArr[0].trim().toLowerCase())
        return cookieArr[1].trim();
    }

    return null;
  }

  get(key: string) {
    return this.parseCookies(document.cookie, key);
  }
}
