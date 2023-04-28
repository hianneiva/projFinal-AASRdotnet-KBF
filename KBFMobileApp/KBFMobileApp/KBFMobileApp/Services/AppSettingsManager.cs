using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KBFMobileApp.Services
{
    public class AppSettingsManager
    {
        private static AppSettingsManager instance;
        private readonly JObject settings;

        private const string Filename = "appsettings.json";
#if __ANDROID__
        private const string Namespace = "KBFMobileApp.Droid";
#else
        private const string Namespace = "KBFMobileApp";
#endif

        private AppSettingsManager()
        {
            try
            {
                Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(AppSettingsManager)).Assembly;
                Stream stream = assembly.GetManifestResourceStream($"{Namespace}.{Filename}");
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    settings = JObject.Parse(json);
                }
            }
            catch
            {
                Debug.WriteLine("Unable to load app settings");
            }
        }

        public static AppSettingsManager Settings
        {
            get 
            {
                if (instance == null)
                {
                    instance = new AppSettingsManager();
                }

                return instance;
            }
        }

        public string this[string property]
        {
            get 
            {
                try
                {
                    JToken node = settings[property];

                    return node.ToString();
                }
                catch
                {
                    Debug.WriteLine($"Unable to retrieve setting: '{property}'");
                    return string.Empty;
                }
            }
        }
    }
}
