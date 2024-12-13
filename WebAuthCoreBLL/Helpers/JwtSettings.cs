namespace WebAuthCoreBLL.Helpers
{
  /// <summary>
  /// Настройки для конфигурации JWT (JSON Web Token)
  /// </summary>
  public class JwtSettings
  {
    /// <summary>
    /// Секретный ключ для подписи токенов
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    /// Издатель токена (Issuer)
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Аудитория токена (Audience)
    /// </summary>
    public string Audience { get; set; }
  }
}
