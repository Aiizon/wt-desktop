using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using wt_desktop.tools;

namespace wt_desktop.launcher;

[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
class Program
{
    private static readonly string AppPath            = "wt-desktop.app";
    private static readonly string PipeName           = "wt-desktop-error-pipe";
    private static readonly string ErrorArgument      = "--error-file=";
    private static readonly string AuthorizedArgument = "--authorized";
    
    private static int     _errorCount;
    private static string? _lastErrorFile;
    
    /// <summary>
    /// Point d'entrée pour l'application. Le launcher sert de gestionnaire de l'application afin de gérer les erreurs.
    /// </summary>
    public static void Main(string[] args)
    {
        ConsoleHandler.WriteInfo("Démarrage de l'application wt-desktop...");

        while (true)
        {
            using var tokenSource = new CancellationTokenSource();
            using var pipeServer  = new NamedPipeServerStream(PipeName, PipeDirection.InOut);

            if (_errorCount == 3)
            {
                ConsoleHandler.WriteError("Erreur : L'application wt-desktop a échoué 3 fois consécutivement. Fermeture du launcher.");
                Environment.Exit(1);
            }

            // Récupère le chemin de l'exécutable de l'application (.exe ou sans extension en fonction de l'OS)
            var appPath = GetExecutablePath();

            if (!File.Exists(appPath))
            {
                ConsoleHandler.WriteError($"Impossible de trouver l'application wt-desktop à l'emplacement {appPath}.");
                break;
            }
            
            if (!EnsureExecutionPermissions())
            {
                ConsoleHandler.WriteError($"Impossible de définir les permissions d'exécution pour {appPath}.");
                break;
            }

            // Démarre l'application wt-desktop
            Process appProcess;
            if (_lastErrorFile != null)
            {
                ConsoleHandler.WriteWarning($"Lancement de l'application wt-desktop en mode erreur. Le fichier d'erreur utilisé est localisé au chemin : {_lastErrorFile}");
                appProcess = Process.Start(appPath, $"{ErrorArgument}{_lastErrorFile} {AuthorizedArgument}");
            }
            else
            {
                appProcess = Process.Start(appPath, AuthorizedArgument);
            }
            
            appProcess.EnableRaisingEvents = true;
            ConsoleHandler.WriteDebug($"L'application wt-desktop a démarré avec le PID {appProcess.Id}.");
            
            // Deux tâches simultanées sont démarrées.
            // La première sert au gestionnaire d'erreurs, afin qu'il puisse communiquer lors d'une erreur.
            
            bool errorReceived = false;
            var pipeConnectionTask = Task.Run(() =>
            {
                ConsoleHandler.WriteInfo($"Attente de la connexion au pipe {PipeName}...");
            
                pipeServer.WaitForConnection();
                ConsoleHandler.WriteSuccess($"Connexion au pipe {PipeName} réceptionnée.");
            
                using var reader = new StreamReader(pipeServer);
                var errorJsonString = reader.ReadToEnd();

                if (!string.IsNullOrEmpty(errorJsonString))
                {
                    ConsoleHandler.WriteDebug($"Erreur reçue de l'application wt-desktop : {errorJsonString}");

                    _lastErrorFile = Path.Combine(Path.GetTempPath(), $"wt-desktop-error-{Guid.NewGuid()}.json");
                    File.WriteAllText(_lastErrorFile, errorJsonString);
                    
                    ConsoleHandler.WriteDebug($"Erreur traitée. Le fichier d'erreur est enregistré au chemin : {_lastErrorFile}");
                    
                    errorReceived = true;
                    return null;
                }

                return null;
            });
            
            // La seconde tâche attend la fermeture de l'application.
            // Si l'application se ferme normalement, le code de sortie est 0. Dans ce cas, le launcher se termine.
            
            var processExitedTask = Task.Run(() =>
            {
                appProcess.WaitForExit();
                
                if (errorReceived)
                {
                    return false;
                }
                
                ConsoleHandler.WriteDebug($"L'application wt-desktop s'est terminée avec le code de sortie {appProcess.ExitCode}.");

                try
                {
                    tokenSource.Cancel();
                    pipeServer.Disconnect();
                }
                catch
                {
                    // ignoré
                }
                
                return appProcess.ExitCode == 0;
            });
            
            // Attendre que l'une des deux tâches se termine
            var completedTask = Task.WhenAny(pipeConnectionTask, processExitedTask).Result;

            if (completedTask == processExitedTask && processExitedTask.Result && !errorReceived)
            {
                // Sortie normale de l'application
                ConsoleHandler.WriteSuccess("L'application wt-desktop s'est terminée normalement.");
                Environment.Exit(0);
            }
            else if (completedTask == pipeConnectionTask)
            {
                // Erreur reçue de l'application, redémarrage de l'application
                ConsoleHandler.WriteInfo("Redémarrage de l'application.");

                if (!appProcess.HasExited)
                {
                    appProcess.Kill();
                }

                _errorCount++;
                continue;
            }
            
            // Si aucune des deux tâches ne se termine, cela signifie que l'application a été fermée de manière inattendue. Dans ce cas, le launcher se termine.
            ConsoleHandler.WriteError("Arrêt inattendu du launcher.");
            Environment.Exit(1);
        }
    }

    /// <summary>
    /// Récupère le chemin de l'exécutable de l'application.
    /// </summary>
    /// <returns>Chemin de l'application</returns>
    private static string GetExecutablePath()
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Path.Combine(basePath, $"{AppPath}.exe");
        }
        
        return Path.Combine(basePath, AppPath);
    }

    /// <summary>
    /// Vérifie si l'application a les permissions d'exécution.
    /// </summary>
    /// <returns>Vrai si l'application est executable, false sinon</returns>
    private static bool EnsureExecutionPermissions()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "chmod",
                        Arguments = $"+x {GetExecutablePath()}",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                
                process.Start();
                process.WaitForExit();
                
                return process.ExitCode == 0;
            }
            catch (Exception e)
            {
                ConsoleHandler.WriteWarning($"Impossible de définir les permissions d'exécution. {e.Message}");
                return false;
            }
        }

        return true;
    }
}