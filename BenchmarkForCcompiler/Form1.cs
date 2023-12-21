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
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BenchmarkForCcompiler
{
    public partial class Form1 : Form
    {
        Profile profileA = new Profile();
        Profile profileB = new Profile();

        public Form1()
        {
            InitializeComponent();

            profileA.Initialize(comboBox1, button3, button6, textBox2, textBox3, textBox6);
            profileB.Initialize(comboBox2, button9, button10, textBox7, textBox9, textBox8);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            profileA.ShowProfileInfo();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            profileB.ShowProfileInfo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            profileA.CreateProfile();
            profileA.ShowProfileList();
            profileB.ShowProfileList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            profileB.CreateProfile();
            profileA.ShowProfileList();
            profileB.ShowProfileList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            profileA.SaveProfile();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            profileB.SaveProfile();
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
            fileOpen(textBox2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            fileOpen(textBox7);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            fileOpen(textBox4);
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                groupBox1.Visible = true;
            }
            else
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void fileOpen(System.Windows.Forms.TextBox textBox)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = ofd.FileName;
            }
            return;
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
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox compilerTextBox;
        private System.Windows.Forms.TextBox optionTextBox;
        private System.Windows.Forms.TextBox executableFileNameTextBox;
        private string profilePath = @"C:\\Users\\sotar\\Desktop\\BenchmarkForCcompiler\\BenchmarkForCcompiler\\testcase\\profile\\";

        public void Initialize(
            System.Windows.Forms.ComboBox comboBox,
            System.Windows.Forms.Button createButton,
            System.Windows.Forms.Button saveButton,
            System.Windows.Forms.TextBox compilerTextBox,
            System.Windows.Forms.TextBox optionTextBox,
            System.Windows.Forms.TextBox executableFileNameTextBox
            )
        {
            this.comboBox = comboBox;
            this.createButton = createButton;
            this.saveButton = saveButton;
            this.compilerTextBox = compilerTextBox;
            this.optionTextBox = optionTextBox;
            this.executableFileNameTextBox = executableFileNameTextBox;
            ShowProfileList();
        }

        public void ShowProfileList()
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(GetList());

        }

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
            for (int i = 0; i < lists.Length; i++)
            {
                string[] line = lists[i].Split(',');
                if (line[0] == "compiler")
                {
                    profileInfo.Compiler = line[1];
                }
                else if (line[0] == "option")
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

        public void ShowProfileInfo()
        {
            RoadProfileInfo(comboBox.Text);
            compilerTextBox.Text = profileInfo.Compiler;
            optionTextBox.Text = profileInfo.Option;
            return;
        }



        public void CreateProfile()
        {
            // ファイル名の競合を確認
            string[] profileList = GetList();
            for (int i = 0; i < profileList.Length; i++)
            {
                if (profileList[i] == comboBox.Text)
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
            StreamWriter sw = new StreamWriter(profilePath + comboBox.Text);
            sw.WriteLine("");
            sw.Close();
            SaveProfile();
            return;
        }

        public void SaveProfile()
        {
            profileInfo = new ProfileInfo(comboBox.Text, compilerTextBox.Text, optionTextBox.Text, executableFileNameTextBox.Text);
            StreamWriter sw = new StreamWriter(profilePath + profileInfo.ProfileName, false, Encoding.GetEncoding("Shift_JIS"));
            sw.WriteLine("compiler," + profileInfo.Compiler);
            sw.WriteLine("option, " + profileInfo.Option);
            sw.WriteLine("executableFileName, " + profileInfo.ExecutableFileName);
            sw.Close();
            return;
        }
    }
}
