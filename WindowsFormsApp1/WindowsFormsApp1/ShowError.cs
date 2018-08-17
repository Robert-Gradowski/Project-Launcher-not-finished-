using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStories
{
    public class Error
    {
        public static void ShowError(string text)
        {
            InfoMessage e = new InfoMessage();
            e.MessageSetText(text);
            e.ShowDialog();
        }
    }
}
