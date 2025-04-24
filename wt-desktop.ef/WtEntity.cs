using Microsoft.EntityFrameworkCore;

namespace wt_desktop.ef;

public abstract class WtEntity
{
    public abstract string DisplayText { get; }
    
    public virtual string SearchText => DisplayText;

    public abstract void OnModelCreating(ModelBuilder modelBuilder);

    public virtual bool MatchSearch(string? search)
    {
        return string.IsNullOrWhiteSpace(search) || SearchText.Contains(search);
    }
}
