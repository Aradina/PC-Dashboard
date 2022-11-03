using Microsoft.Win32;

namespace PC_Dashboard.Core
{
    internal class RegistryKeyExists
    {

        /// <summary>
        /// Checks if a registrykey exists.
        /// </summary>
        /// <param name="registryPath">The path to the registry key to check.</param>
        /// <returns>True if true, false if false.</returns>
        public static bool KeyExists(string registryPath)
        {
            RegistryKey baseKey = BaseKey();
            using (RegistryKey key = baseKey.OpenSubKey(registryPath))
            {
                if (key != null) { return true; }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets the basekey.
        /// </summary>
        /// <returns>The basekey.</returns>
        static RegistryKey BaseKey()
        {
            return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        }
    }
}
