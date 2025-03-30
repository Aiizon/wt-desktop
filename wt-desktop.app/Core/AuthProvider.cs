using System;
using System.Linq;
using wt_desktop.ef;
using wt_desktop.ef.Entity;

namespace wt_desktop.app.Core;

public class AuthProvider
{
    #region Singleton
    private static AuthProvider? _instance;

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
    private User? _CurrentUser;
    public  User? CurrentUser     => _CurrentUser;
    public  bool  IsAuthenticated => _CurrentUser != null;
    #endregion

    #region Auth
    public event EventHandler? AuthenticationStateChanged;

    private void OnAuthenticationStateChanged()
    {
        AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public bool Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
        
        var user = WtContext.Instance.User.FirstOrDefault(u => u.Email == email && u.Password == password);

        if (user == null || !VerifyPassword(password, user.Password))
        {
            return false;
        }
        
        _CurrentUser = user;
        OnAuthenticationStateChanged();
        return IsAuthenticated;
    }
    
    public void Logout()
    {
        _CurrentUser = null;
        OnAuthenticationStateChanged();
    }
    #endregion
    
    #region Password
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
    #endregion
}