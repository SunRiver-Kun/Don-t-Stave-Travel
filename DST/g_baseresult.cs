using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DST
{
    public class g_baseresult
    {
        public static readonly string FILEPATH = @"results.txt";
        public static Form1 MainForm;
        public static void SetMainForm(Form1 form) { MainForm = form; }
        public static void GotoResult(string str)
        {
            GameOver(str);
        }
        public static void LostInDark()
        {
            GameOver(Form1.traveler.name + "迷失在了黑暗当中");
        }
        private static void GameOver(string str)
        {
            string output = str + " " + Worldtime.GetTime().ToString() + "\r\n";
            File.AppendAllText(FILEPATH, output);

            //MainForm.Print(str + "\r\n~~~!!!游戏结束!!!~~~");
            MainForm.TB_Dialogue.Text += str + "\r\n~~~!!!游戏结束!!!~~~\r\n"; //简单粗暴，去你的线程
            g_address.GotoAddress(0);
            //MainForm.Print(str + "\r\n~~~!!!游戏结束!!!~~~",()=> {
            //    g_address.GotoAddress(0);
            //});
        }
    }
}
