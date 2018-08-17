using Microsoft.Win32;
using System.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace DiamondStories
{
    public partial class Client
    {
        async void SignInUser()
        {
            string samp037 = "C316560C5B925874FF30D49DCAE42478";
            string samp = "";
            try
            {
                if (Registry.GetValue(@"HKEY_CURRENT_USER\Software\DStories", "created", null) == null)
                {
                    RegistryKey key;
                    key = Registry.CurrentUser.CreateSubKey(@"Software\DStories");
                    key.SetValue("created", "1");
                    key.Close();
                }
            }
            catch (SecurityException) { Error.ShowError("Brak permisji!\nUruchom program jako Administrator!"); }
            catch (ArgumentException) { Error.ShowError("Klucz nie zaczyna się od głównego prawidłowy rejestru!"); }

            try
            {
                if (Registry.GetValue(@"HKEY_CURRENT_USER\Software\SAMP", "gta_sa_exe", null) != null)
                {
                    samp = Registry.GetValue(@"HKEY_CURRENT_USER\Software\SAMP", "gta_sa_exe", null).ToString().Replace("gta_sa.exe", "samp.exe");
                    if (await MD5Checksum.CalculateMD5Async(samp) == samp037)
                    {
                        SettingsButton.Show();
                        LowerPB.Show();
                        BiggerPB.Show();
                        PlayButton.Show();
                        PlayImage.Show();
                        DiscordButton.Show();
                        LoginDim.Hide();
                    }
                    else
                    {
                        Error.ShowError("Posiadasz złą wersję SA-MP!\n Wymagana wersja to 0.3.7!");
                        System.Diagnostics.Process.Start("https://www.sa-mp.com/download.php");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    System.Diagnostics.Process.Start("https://www.sa-mp.com/download.php");
                    Environment.Exit(0);
                }
            }
            catch (SecurityException) { Error.ShowError("Brak permisji!\nUruchom program jako Administrator!"); }
            catch (ArgumentException) { Error.ShowError("Klucz nie zaczyna się od głównego prawidłowego rejestru!"); }

        }

        
    }
}
