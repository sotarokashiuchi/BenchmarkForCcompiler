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
using System.Text.Json;
using System.Windows.Forms.VisualStyles;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BenchmarkForCcompiler
{
    public partial class Form1 : Form
    {
        Profile profile = new Profile();
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(profile.GetList());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        

        private void button1_Click(object sender, EventArgs e)
        {
            // コンパイラの実行
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = @textBox2.Text;
            psInfo.Arguments = @textBox4.Text;
            psInfo.Arguments = @textBox4.Text + " " + textBox3.Text;
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト
            psInfo.RedirectStandardError = true;

            Process p = Process.Start(psInfo); // コマンドの実行開始
            string line = "";
            string lines = "";
            while ((line = p.StandardOutput.ReadLine()) != null || (line = p.StandardError.ReadLine()) != null)
            {
                lines += line + "\r\n";
            }
            textBox1.Text = lines;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = ofd.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // プログラムの実行
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = "./a.exe";
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト
            Process p = Process.Start(psInfo); // コマンドの実行開始
            string line = "";
            string lines = "";
            while ((line = p.StandardOutput.ReadLine()) != null)
            {
                lines += line + "\r\n";
            }
            textBox5.Text = lines;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Profile.ProfileInfo profileInfo = profile.RoadProfileInfo(comboBox1.Text);
            textBox2.Text = profileInfo.Compiler;
            textBox3.Text = profileInfo.Option;
        }
    }

    class Profile
    {
        // private Dictionary<string, string> profile = new Dictionary<string, string>();
        public struct ProfileInfo
        {
            public string Compiler;
            public string Option;
        }
        private ProfileInfo profileInfo = new ProfileInfo();
        private string profilePath = @"C:\\Users\\sotar\\Desktop\\BenchmarkForCcompiler\\BenchmarkForCcompiler\\testcase\\profile\\";

        public string[] GetList()
        {
            // Profileディレクトリのファイルを一覧取得
            string[] filename;
            filename = Directory.GetFiles(profilePath);
            for (int i = 0; i < filename.Length; i++)
            {
                filename[i] = Path.GetFileName(filename[i]);
            }
            return filename;
        }

        public ProfileInfo RoadProfileInfo(string filename)
        {
            StreamReader sr = new StreamReader(profilePath + filename);
            string[] lists = sr.ReadToEnd().Split('\n');
            sr.Close();
            Console.WriteLine(lists);
            for(int i=0; i < lists.Length; i++)
            {
                string[] line = lists[i].Split(',');
                if (line[0] == "compiler")
                {
                    profileInfo.Compiler = line[1];
                } else if (line[0] == "option")
                {
                    profileInfo.Option = line[1];
                }
            }
            return profileInfo;
        }
    }
}
