using wt_desktop.ef.Entity;

namespace wt_desktop.ef;

public class AuthProvider
{
    #region Singleton
    private static AuthProvider? _instance;

    /// <summary>
    /// Instance de la classe AuthProvider
    /// </summary>
    public static AuthProvider Instance
    {
        get
        {
            return _instance ??= new AuthProvider();
        }
    }

    private AuthProvider() { }
    #endregion

    #region Properties
    private User? _currentUser;
    public  User? CurrentUser     => _currentUser;
    public  bool  IsAuthenticated => _currentUser != null;
    #endregion

    #region Auth
    public event EventHandler? AuthenticationStateChanged;

    private void OnAuthenticationStateChanged()
    {
        AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
    }
    
    /// <summary>
    /// Login de l'utilisateur
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="password">Mot de passe en clair</param>
    /// <returns>True si la connection s'est effectuée avec succès, false sinon</returns>
    /// <exception cref="Exception">Erreur lors de la connection</exception>
    public bool Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("L'email et le mot de passe ne peuvent pas être vides.");
        }
        
        var user = WtContext.Instance.User.FirstOrDefault(u => u.Email == email && u.Type == "user");

        if (user == null || !VerifyPassword(password, user.Password))
        {
            throw new Exception("L'email ou le mot de passe est incorrect.");
        }
        
        if (!user.HasRole("ROLE_ADMIN") &&
            !user.HasRole("ROLE_ACCOUNTANT"))
        {
            throw new Exception("Vous n'avez pas les droits d'accès à cette application.");
        }
        
        _currentUser = user;
        OnAuthenticationStateChanged();
        return IsAuthenticated;
    }
    
    /// <summary>
    /// Déconnexion de l'utilisateur
    /// </summary>
    public void Logout()
    {
        _currentUser = null;
        OnAuthenticationStateChanged();
    }
    #endregion
    
    #region Password
    /// <summary>
    /// Hash le mot de passe
    /// </summary>
    /// <param name="password">Mot de passe en clair</param>
    /// <returns>hash</returns>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    /// <summary>
    /// Vérifie si le mot de passe en clair correspond au hash
    /// </summary>
    /// <param name="password">Mot de passe en clair</param>
    /// <param name="hash">Hash</param>
    /// <returns>True si le mot de passe correspond, false sinon</returns>
    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
    #endregion
}