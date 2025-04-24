using System;

namespace wt_desktop.app.Core;

[Serializable]
public class NoLauncherException: Exception
{
    public NoLauncherException() { }
    
    public NoLauncherException(string message) : base(message) { }
    
    public NoLauncherException(string message, Exception inner) : base(message, inner) { }
}