export class TokenHelper {
  public static setAccessToken(accessToken: string) {
    localStorage.setItem('accessToken', accessToken);
  }

  public static setRefreshToken(refreshToken: string) {
    localStorage.setItem('refreshToken', refreshToken);
  }
}
