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
            psInfo.Arguments = textBox3.Text + " " + @textBox4.Text;
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
            psInfo.FileName = @"./" + textBox6.Text;
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

        private void button3_Click(object sender, EventArgs e)
        {
            // ファイル名の競合を確認
            string[] profileList = profile.GetList();
            for(int i=0; i<profileList.Length; i++)
            {
                if (profileList[i] == comboBox1.Text)
                {
                    // エラー処理
                    DialogResult dialogResult = MessageBox.Show(
                        "既に同じ名前のプロファイルが存在しています。\n上書を行い続行しますか?\n",
                        "Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            // ファイル作成
            profile.CreateProfile(comboBox1.Text);
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(profile.GetList());
            profile.SaveProfile(new Profile.ProfileInfo(comboBox1.Text, textBox2.Text, textBox3.Text, textBox6.Text));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            profile.SaveProfile(new Profile.ProfileInfo(comboBox1.Text, textBox2.Text, textBox3.Text, textBox6.Text));
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) { 
                groupBox1.Visible = true;
            } else
            {
                groupBox1.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                groupBox2.Visible = true;
            }
            else
            {
                groupBox2.Visible = false;
            }
        }
    }

    class Profile
    {
        public struct ProfileInfo
        {
            public string ProfileName;
            public string Compiler;
            public string Option;
            public string ExecutableFileName;
            
            public ProfileInfo(string profileName, string compiler, string option, string executableFileName)
            {
                ProfileName = profileName;
                Compiler = compiler; 
                Option = option;
                ExecutableFileName = executableFileName;
            }
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
            profileInfo.ProfileName = filename;
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
                else if (line[0] == "executableFileName")
                {
                    profileInfo.ExecutableFileName = line[1];
                }
            }
            return profileInfo;
        }

        public void CreateProfile(string fileName)
        {
            StreamWriter sw = new StreamWriter(profilePath + fileName);
            sw.WriteLine("");
            sw.Close();
            return;
        }

        public void SaveProfile(ProfileInfo info) {
            profileInfo = info;
            StreamWriter sw = new StreamWriter(profilePath + profileInfo.ProfileName, false, Encoding.GetEncoding("Shift_JIS"));
            sw.WriteLine("compiler," + profileInfo.Compiler);
            sw.WriteLine("option, " + profileInfo.Option);
            sw.WriteLine("executableFileName, " + profileInfo.ExecutableFileName);
            sw.Close();
            return;
        }
    }
}
