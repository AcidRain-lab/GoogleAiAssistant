namespace WebAuthCoreBLL.Helpers
{
  public class AuthenticationSettings
  {
    public bool EnableJwt { get; set; }
    public bool EnableCookies { get; set; }

    public string ServerName { get; set; } // Host, Alex или Vlad
  }
}
