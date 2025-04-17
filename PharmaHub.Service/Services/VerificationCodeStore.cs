namespace PharmaHub.Service.Services;

public class VerificationCodeStore
{
    private static readonly Dictionary<string, (string Code, DateTime ExpiresAt)> _store = new();

    public void Store(string userId, string code)
    {
        _store[userId] = (code, DateTime.UtcNow.AddMinutes(5));
    }

    public bool Verify(string userId, string enteredCode)
    {
        if (!_store.ContainsKey(userId)) return false;

        var (code, expiresAt) = _store[userId];
        if (DateTime.UtcNow > expiresAt || code != enteredCode) return false;

        _store.Remove(userId); // Mark as used
        return true;
    }
}