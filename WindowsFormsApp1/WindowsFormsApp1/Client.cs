using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Security;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace DiamondStories
{
    public partial class Client : Form
    {
        class Account
        { 
            public int Id { get; set; }
            public string Login { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Sessionid { get; set; }
            public string Sessionip { get; set; }
        }
        public static int id, age;
        string login, name, surname;

        private string installing_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DiamondStories\";
        private string game_path = Registry.GetValue(@"HKEY_CURRENT_USER\Software\SAMP", "gta_sa_exe", null).ToString().Replace("gta_sa.exe", "");
        public Client()
        {
            Debug.WriteLine(installing_path);
            InitializeComponent();
            CenterToScreen();
            MaximizeBox = false;
            PanelOpacity.BackColor = Color.FromArgb(125, Color.Black);
            SettingsButton.BackColor = Color.FromArgb(25, Color.Gray);
            LoginDim.BackColor = Color.FromArgb(125, Color.Black);

            SettingsButton.Hide();
            LowerPB.Hide();
            BiggerPB.Hide();
            PlayButton.Hide();
            PlayImage.Hide();
            StatusBar.Hide();
            DiscordButton.Hide();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            PlayImage.Visible = false;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void PanelTop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MinimalizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private async void PlayButton_Click(object sender, EventArgs e)
        {
            PlayButton.Visible = false;
            PlayImage.Visible = true;
            StatusBar.Show();
            if (!File.Exists(installing_path + "gta_sa.exe") && !File.Exists(installing_path + "samp.exe"))
            {
                CopyAsync ca = new CopyAsync();
                StatusBar.Text = "Trwa kopiowanie plików...";
                await ca.DirectoryCopy(game_path, installing_path, true, LowerPB, StatusBar);


            }
            else
            {
                StatusBar.Text = "Trwa sprawdzanie plików...";
                CheckFiles cf = new CheckFiles();
                cf.DeleteFilesExcept(installing_path, StatusBar);
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string bt = GetBearerToken();
            if (bt.Length > 15)
            {
                WhirlpoolCryptoServiceProvider hash = new WhirlpoolCryptoServiceProvider();
                string hashedpass = BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(PasswordInput.Text)));
                hashedpass = hashedpass.Replace("-", "");
                LoginUser(LoginInput.Text, hashedpass, bt);
                if(login != null)
                {
                    SignInUser();
                }
                else
                {
                    Error.ShowError("Podany login lub hasło jest błędne!");
                }
            }
            else
            {
                Error.ShowError("Wystąpił błąd podczas uwierzytelniania!");
                Environment.Exit(0);
            }
        }

        private void DiscordButton_Click(object sender, EventArgs e) { Process.Start("https://discord.gg/wUDVRTT"); }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Settings sett = new Settings();
            sett.ShowDialog();
        }

        private void PasswordInput_TextChanged(object sender, EventArgs e)
        {
            PasswordInput.PasswordChar = '*';
        }

        public static string GetBearerToken()
        {
            string result = "";
            try
            {
                var webUrl = "http://localhost:51836/api/auth/token";
                result = string.Empty;

                HttpWebRequest request = HttpWebRequest.Create(webUrl) as HttpWebRequest;
                request.Method = "POST";
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("User:IAmDiamondStoriesUser"));
                request.Headers.Add("Authorization", "Basic " + encoded);
                request.ContentLength = 0;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                }
                response.Close();
            }
            catch (Exception)
            {

            }

            return result;
        }

        void LoginUser(string Login, string Hash, string Token)
        {
            string html = string.Empty;
            string url = String.Format("http://localhost:51836/api/accounts/l={0}&p={1}", Login, Hash);
            Debug.WriteLine(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Headers.Add("Authorization", "Bearer " + Token);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
                var _Data = JsonConvert.DeserializeObject<List<Account>>(html);
                Debug.WriteLine(_Data.ToString());
                foreach (Account User in _Data)
                {
                    id = User.Id;
                    age = User.Age;
                    login = User.Login;
                    name = User.Name;
                    surname = User.Surname;
                }
            }
        }
        public void SetStatus(string text)
        {
            StatusBar.Text = text;
        }
    }
}

