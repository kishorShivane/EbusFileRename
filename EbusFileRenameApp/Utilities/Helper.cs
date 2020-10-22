using System;
using System.Collections.Generic;
using System.IO;

namespace EbusFileRenameApp.Utilities
{
    public class Helper
    {
        public Helper()
        {
        }

        public bool IsFileLocked(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            //file is not locked
            return false;
        }

        public List<string> DirSearch(string sDir)
        {
            List<string> files = new List<string>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return files;
        }


        public void MoveFile(string sourcepath, string destinationPath)
        {
            try
            {
                string file = Path.GetFileName(sourcepath);
                string destinationFilePath = destinationPath + "//" + file;

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                //Checks if files exists and deletes if they do
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
                File.Copy(sourcepath, destinationFilePath);
                if (File.Exists(sourcepath))
                {
                    File.Delete(sourcepath);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public bool Exists(string sourcepath, string destinationPath)
        {
            try
            {
                string file = Path.GetFileName(sourcepath);
                string destinationFilePath = destinationPath + "//" + file;

                //Checks if files exists and deletes if they do
                if (File.Exists(destinationFilePath))
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
