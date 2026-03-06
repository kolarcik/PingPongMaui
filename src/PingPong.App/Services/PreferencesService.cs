using Microsoft.Maui.Storage;

namespace PingPong.App.Services;

public class PreferencesService : IPreferencesService
{
    public T Get<T>(string key, T defaultValue) =>
        Preferences.Default.Get(key, defaultValue);

    public void Set<T>(string key, T value) =>
        Preferences.Default.Set(key, value);

    public void Remove(string key) =>
        Preferences.Default.Remove(key);
}
