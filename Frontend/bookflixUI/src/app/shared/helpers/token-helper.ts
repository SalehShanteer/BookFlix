export class TokenHelper {
  public static setAccessToken(accessToken: string) {
    localStorage.setItem('accessToken', accessToken);
  }

  public static setRefreshToken(refreshToken: string) {
    localStorage.setItem('refreshToken', refreshToken);
  }

  public static getAccessToken() {
    return localStorage.getItem('accessToken');
  }

  public static getRefreshToken() {
    return localStorage.getItem('refreshToken');
  }
}
