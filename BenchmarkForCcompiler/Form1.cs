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
using DiffMatchPatch;
using System.Web.Configuration;

namespace BenchmarkForCcompiler
{
    public partial class Form1 : Form
    {
        Profile profileA = new Profile();
        Profile profileB = new Profile();
        Compile compile = new Compile();
        Asm asm = new Asm();
        Executable executable = new Executable();
        Executable ExecutableB = new Executable();

        public Form1()
        {
            InitializeComponent();

            profileA.Initialize(comboBox1, button3, button6, textBox2, textBox3, textBox6, textBox15);
            profileB.Initialize(comboBox2, button9, button10, textBox7, textBox9, textBox8, textBox14);
            compile.Initialize(textBox1, textBox4, textBox11, textBox4, textBox16);
            asm.Initialize(textBox13, textBox15, textBox12, textBox14, textBox17);
            executable.Initialize(textBox5, textBox6, textBox10, textBox8, textBox18);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Comparison comparison = new Comparison();
            Console.WriteLine(comparison.GNUDiff("Hello World", "Helllo World Japan."));
            tableLayoutPanel2.Width = 000;
            tableLayoutPanel2.Height = 000;
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

        private void button1_Click(object sender, EventArgs e)
        {
            compile.Run(ProfileStatus.ProfileA, profileA.GetNowProfile());
        }


        private void button5_Click(object sender, EventArgs e)
        {
            executable.Run(ProfileStatus.ProfileA);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            compile.Run(ProfileStatus.ProfileB, profileA.GetNowProfile());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            executable.Run(ProfileStatus.ProfileB);
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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            asm.Show(ProfileStatus.ProfileA);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            asm.Show(ProfileStatus.ProfileB);
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void 比較表示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            比較表示ToolStripMenuItem.Checked = true;
            diff表示ToolStripMenuItem.Checked = false;
            changeViewMode(true);

        }

        private void diff表示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diff表示ToolStripMenuItem.Checked = true;
            比較表示ToolStripMenuItem.Checked = false;
            changeViewMode(false);
            compile.Comparison();
            asm.Comparison();
        }

        private void changeViewMode(bool mode)
        {
            // modeがtrueの時、比較モード、falseの時、diffモード
            panel4.Visible = true == mode;
            panel6.Visible = true == mode;

            panel5.Visible = false == mode;
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

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
            public string AsmFileName;

            public ProfileInfo(string profileName, string compiler, string option, string executableFileName, string asmFileName)
            {
                ProfileName = profileName;
                Compiler = compiler;
                Option = option;
                ExecutableFileName = executableFileName;
                AsmFileName = asmFileName;
            }
        }
        private ProfileInfo profileInfo = new ProfileInfo();
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox compilerTextBox;
        private System.Windows.Forms.TextBox optionTextBox;
        private System.Windows.Forms.TextBox executableFileNameTextBox;
        private System.Windows.Forms.TextBox asmFileNameTextBox;
        private string profilePath = @"C:\\Users\\sotar\\Desktop\\BenchmarkForCcompiler\\BenchmarkForCcompiler\\testcase\\profile\\";

        public void Initialize(
            System.Windows.Forms.ComboBox comboBox,
            System.Windows.Forms.Button createButton,
            System.Windows.Forms.Button saveButton,
            System.Windows.Forms.TextBox compilerTextBox,
            System.Windows.Forms.TextBox optionTextBox,
            System.Windows.Forms.TextBox executableFileNameTextBox,
            System.Windows.Forms.TextBox asmFileNameTextBox
            )
        {
            this.comboBox = comboBox;
            this.createButton = createButton;
            this.saveButton = saveButton;
            this.compilerTextBox = compilerTextBox;
            this.optionTextBox = optionTextBox;
            this.executableFileNameTextBox = executableFileNameTextBox;
            this.asmFileNameTextBox = asmFileNameTextBox;
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
            StreamReader sr = new StreamReader(profilePath + filename, Encoding.GetEncoding("UTF-8"));
            string[] lists = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //string[] lists = sr.ReadToEnd().Split('\n');
            sr.Close();
            profileInfo = new ProfileInfo();
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
                else if (line[0] == "asmFileName")
                {
                    profileInfo.AsmFileName = line[1];
                }
            }
            return profileInfo;
        }

        public void ShowProfileInfo()
        {
            RoadProfileInfo(comboBox.Text);
            compilerTextBox.Text = profileInfo.Compiler;
            optionTextBox.Text = profileInfo.Option;
            executableFileNameTextBox.Text = profileInfo.ExecutableFileName;
            asmFileNameTextBox.Text = profileInfo.AsmFileName;
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
            StreamWriter sw = new StreamWriter(profilePath + comboBox.Text, false);
            sw.WriteLine("");
            sw.Close();
            SaveProfile();
            return;
        }

        public ProfileInfo GetNowProfile()
        {
            return new ProfileInfo(comboBox.Text, compilerTextBox.Text, optionTextBox.Text, executableFileNameTextBox.Text, asmFileNameTextBox.Text);
        }

        public void SaveProfile()
        {
            profileInfo = GetNowProfile();
            StreamWriter sw = new StreamWriter(profilePath + profileInfo.ProfileName, false, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine("compiler," + profileInfo.Compiler);
            sw.WriteLine("option," + profileInfo.Option);
            sw.WriteLine("executableFileName," + profileInfo.ExecutableFileName);
            sw.WriteLine("asmFileName," + profileInfo.AsmFileName);
            sw.Close();
            return;
        }
    }

    public enum ProfileStatus
    {
        ProfileA,
        ProfileB,
    }

    class Compile
    {
        protected System.Windows.Forms.TextBox outputTextBox;
        protected System.Windows.Forms.TextBox inputFileNameTextBox;
        protected System.Windows.Forms.TextBox ProfileA_OutputTextBox;
        protected System.Windows.Forms.TextBox ProfileA_InputFileNameTextBox;
        protected System.Windows.Forms.TextBox ProfileB_OutputTextBox;
        protected System.Windows.Forms.TextBox ProfileB_InputFileNameTextBox;
        protected System.Windows.Forms.TextBox ComparisonOutputTextBox;
        protected Comparison comparison = new Comparison();

        public void Initialize(
            System.Windows.Forms.TextBox ProfileA_OutputTextBox,
            System.Windows.Forms.TextBox ProfileA_InputFileNameTextBox,
            System.Windows.Forms.TextBox ProfileB_OutputTextBox,
            System.Windows.Forms.TextBox ProfileB_InputFileNameTextBox,
            System.Windows.Forms.TextBox ComparisonOutputTextBox
            )
        {
            this.ProfileA_OutputTextBox = ProfileA_OutputTextBox;
            this.ProfileA_InputFileNameTextBox = ProfileA_InputFileNameTextBox;
            this.ProfileB_OutputTextBox = ProfileB_OutputTextBox;
            this.ProfileB_InputFileNameTextBox = ProfileB_InputFileNameTextBox;
            this.ComparisonOutputTextBox = ComparisonOutputTextBox;
        }

        protected void switchProfile(ProfileStatus profileStatus)
        {
            switch (profileStatus)
            {
                case ProfileStatus.ProfileA:
                    outputTextBox = ProfileA_OutputTextBox;
                    inputFileNameTextBox = ProfileA_InputFileNameTextBox;
                    break;
                case ProfileStatus.ProfileB:
                    outputTextBox = ProfileB_OutputTextBox;
                    inputFileNameTextBox = ProfileB_InputFileNameTextBox;
                    break;
                default:
                    Console.WriteLine("Error:プロファイルを選択してください。");
                    break;
            }
        }

        public void Run(ProfileStatus profileStatus, Profile.ProfileInfo profileInfo)
        {
            switchProfile(profileStatus);
            // コンパイラの実行
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = profileInfo.Compiler;
            psInfo.Arguments = profileInfo.Option + " " + inputFileNameTextBox.Text;
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト
            psInfo.RedirectStandardError = true;

            Console.WriteLine(psInfo.FileName + psInfo.Arguments);

            Process p = Process.Start(psInfo); // コマンドの実行開始
            string line = "";
            string lines = "";
            while ((line = p.StandardOutput.ReadLine()) != null || (line = p.StandardError.ReadLine()) != null)
            {
                lines += line + "\r\n";
            }
            outputTextBox.Text = lines;
        }

        public void Comparison()
        {
            string diff =  comparison.GNUDiff(ProfileA_OutputTextBox.Text, ProfileB_OutputTextBox.Text);
            ComparisonOutputTextBox.Text = diff.Replace("\n", Environment.NewLine);
        }

    }

    class Asm : Compile
    {
        public void Show(ProfileStatus profileStatus)
        {
            switchProfile(profileStatus);
            StreamReader sr = new StreamReader(inputFileNameTextBox.Text, Encoding.GetEncoding("UTF-8"));
            outputTextBox.Text = sr.ReadToEnd();
            sr.Close();

        }
    }

    class Executable : Compile
    {
        // 実行はコンパイルの処理も行う
        public void Run(ProfileStatus profileStatus)
        {
            switchProfile(profileStatus);
            // プログラムの実行
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = @"./" + inputFileNameTextBox.Text;
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト

            Console.WriteLine(psInfo.FileName + psInfo.Arguments);

            Process p = Process.Start(psInfo); // コマンドの実行開始
            string line = "";
            string lines = "";
            while ((line = p.StandardOutput.ReadLine()) != null)
            {
                lines += line + "\r\n";
            }
            outputTextBox.Text = lines;

        }
    }

    class Comparison
    {
        diff_match_patch dmp = new diff_match_patch();

        public string GNUDiff(string textA, string textB)
        {
            return dmp.patch_toText(dmp.patch_make(textA, textB));
        }
    }
}
