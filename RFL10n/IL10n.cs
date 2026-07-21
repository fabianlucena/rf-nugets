namespace RFL10n;

public interface IL10n
{
    Task<string> _(string text, params string[] args);

#pragma warning disable IDE1006 // Naming Styles
    Task<string> _c(string context, string text, params string[] args);
#pragma warning restore IDE1006 // Naming Styles
}
