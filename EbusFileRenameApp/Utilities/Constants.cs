using System.Configuration;

namespace EbusFileRenameApp.Utilities
{
    public class Constants
    {
        public static string SourceFilePath = ConfigurationManager.AppSettings["SourceFilePath"];
        public static string ExtRenameFilePath = ConfigurationManager.AppSettings["ExtRenameFilePath"];
        public static string StatusFilePath = ConfigurationManager.AppSettings["StatusFilePath"];
        public static string OthersFilePath = ConfigurationManager.AppSettings["OthersFilePath"];
        public static bool DeleteStatusFiles = ConfigurationManager.AppSettings["DeleteStatusFiles"] == "false" ? false : true;
    }
}
