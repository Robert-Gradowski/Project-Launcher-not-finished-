using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace DiamondStories
{
    class CopyAsync
    {
        public async Task DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, ProgressBar pb, Label la)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            float files = Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories).Count();
            float count = 0;

            foreach (string dirPath in Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDirName, destDirName));
                foreach (string filename in Directory.EnumerateFiles(dirPath))
                {
                    try
                    {
                        using (FileStream SourceStream = File.Open(filename, FileMode.Open))
                        {
                            using (FileStream DestinationStream = File.Create(filename.Replace(sourceDirName, destDirName)))
                            {
                                count++;
                                float percentage = count / files * 100;
                                pb.Value = (int)percentage;
                                await SourceStream.CopyToAsync(DestinationStream);
                            }
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Error.ShowError("Odznacz atrybut Tylko do Odczytu w pliku: \n" + filename);
                        Environment.Exit(0);
                    }
                }
            }

            foreach (string filename in Directory.EnumerateFiles(sourceDirName))
            {
                using (FileStream SourceStream = File.Open(filename, FileMode.Open))
                {
                    using (FileStream DestinationStream = File.Create(destDirName + filename.Substring(filename.LastIndexOf('\\'))))
                    {
                        try
                        {
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Error.ShowError("Wyłącz atrybut Tylko do Odczytu w pliku: " + filename);
                        }
                        
                    }
                }
            }
            CheckFiles cf = new CheckFiles();
            la.Text = "Sprawdzanie plików...";
            cf.DeleteFilesExcept(destDirName, la);
        }
    }
}
