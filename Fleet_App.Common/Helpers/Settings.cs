using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Fleet_App.Common.Helpers
{
    public static class Settings
    {
        private const string _user = "User";
        private const string _isRemembered = "IsRemembered";
        private static readonly bool _boolDefault = false;

        private static readonly string _settingsDefault = string.Empty;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string User
        {
            get => AppSettings.GetValueOrDefault(_user, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_user, value);
        }

        public static bool IsRemembered
        {
            get => AppSettings.GetValueOrDefault(_isRemembered, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isRemembered, value);
        }

    }
}