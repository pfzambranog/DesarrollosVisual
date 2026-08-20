// Utilities/ConfigProtect.cs
using System.Configuration;

public static class ConfigProtect
{
    public static void ProtectConnectionStrings()
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        var section = config.GetSection("connectionStrings");
        if (section != null && !section.SectionInformation.IsProtected)
        {
            section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }

    public static void UnprotectConnectionStrings()
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        var section = config.GetSection("connectionStrings");
        if (section != null && section.SectionInformation.IsProtected)
        {
            section.SectionInformation.UnprotectSection();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
