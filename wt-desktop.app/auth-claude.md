# Login System with BCrypt Authentication

I'll create a login page that authenticates users against the database using BCrypt hashing.

## 1. First, add BCrypt.Net-Next package:

```bash
dotnet add package BCrypt.Net-Next
```

## 2. Create the LoginViewModel:

```csharp
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;
using wt_desktop.ef;
using BCrypt.Net;
using System.Linq;

namespace wt_desktop.app
{
    public class LoginViewModel : ReactiveObject
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Veuillez saisir un nom d'utilisateur et un mot de passe.";
                return;
            }

            var user = WtContext.Instance.User
                .FirstOrDefault(u => u.Username == Username);

            if (user == null || !BCrypt.Verify(Password, user.PasswordHash))
            {
                ErrorMessage = "Nom d'utilisateur ou mot de passe incorrect.";
                return;
            }

            // Store current user information if needed
            App.CurrentUser = user;
            
            // Open main window
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // Close login window
            if (App.Current?.ApplicationLifetime is Avalonia.ApplicationLifetime.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow.Close();
                desktop.MainWindow = mainWindow;
            }
        }
    }
}
```

## 3. Create the LoginWindow.axaml:

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:suki="clr-namespace:SukiUI.Controls;assembly=SukiUI"
        xmlns:local="clr-namespace:wt_desktop.app"
        mc:Ignorable="d" d:DesignWidth="400" d:DesignHeight="500"
        x:Class="wt_desktop.app.LoginWindow"
        x:DataType="local:LoginViewModel"
        Title="WorkTogether - Connexion"
        Width="400" Height="500"
        WindowStartupLocation="CenterScreen"
        Background="Transparent"
        ExtendClientAreaToDecorationsHint="True"
        TransparencyLevelHint="AcrylicBlur">

    <Design.DataContext>
        <local:LoginViewModel />
    </Design.DataContext>

    <Border CornerRadius="8" Margin="20">
        <Grid RowDefinitions="Auto,*,Auto">
            <!-- Header -->
            <TextBlock Grid.Row="0" 
                       Text="WorkTogether" 
                       HorizontalAlignment="Center" 
                       FontSize="24"
                       Margin="0,40,0,0" />

            <!-- Login Form -->
            <StackPanel Grid.Row="1" 
                        Spacing="20" 
                        Width="300"
                        VerticalAlignment="Center">
                
                <TextBlock Text="Connexion" 
                           FontSize="18" 
                           HorizontalAlignment="Center" 
                           Margin="0,0,0,20" />
                
                <TextBox Text="{Binding Username}" 
                         Watermark="Nom d'utilisateur"
                         UseFloatingWatermark="True" />
                
                <TextBox Text="{Binding Password}" 
                         PasswordChar="•"
                         Watermark="Mot de passe"
                         UseFloatingWatermark="True" />
                
                <TextBlock Text="{Binding ErrorMessage}" 
                           Foreground="Red"
                           TextWrapping="Wrap"
                           IsVisible="{Binding !!ErrorMessage}" />
                
                <Button Content="Se connecter"
                        Command="{Binding LoginCommand}"
                        HorizontalAlignment="Stretch"
                        HorizontalContentAlignment="Center"
                        Margin="0,20,0,0" />
            </StackPanel>
            
            <!-- Footer -->
            <TextBlock Grid.Row="2" 
                       Text="© 2023 WorkTogether" 
                       HorizontalAlignment="Center"
                       Opacity="0.7"
                       Margin="0,0,0,20" />
        </Grid>
    </Border>
</Window>
```

## 4. Create the LoginWindow.axaml.cs:

```csharp
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace wt_desktop.app
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
```

## 5. Modify the User entity to include password hash:

```csharp
// Add this property to your User entity class
public string PasswordHash { get; set; }
```

## 6. Modify App.axaml.cs to start with the login window:

```csharp
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using wt_desktop.ef.Entity;

namespace wt_desktop.app
{
    public class App : Application
    {
        // Store current user for app-wide access
        public static User CurrentUser { get; set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new LoginWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
```

## 7. Utility method for hashing passwords (when creating users):

```csharp
// Method to use when creating/updating user passwords
public static string HashPassword(string password)
{
    return BCrypt.Net.BCrypt.HashPassword(password);
}
```

This implementation provides a login system that:
- Shows a login page at startup
- Authenticates users against the database using BCrypt
- Redirects to the main application window after successful login
- Stores the current user for access throughout the application