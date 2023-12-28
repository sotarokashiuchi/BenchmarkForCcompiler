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

namespace BenchmarkForCcompiler
{
    public partial class Form1 : Form
    {
        Profile profileA = new Profile();
        Profile profileB = new Profile();
        Compile CompileA = new Compile();
        Compile CompileB = new Compile();
        Asm AsmA = new Asm();
        Asm AsmB = new Asm();
        Executable ExecutableA = new Executable();
        Executable ExecutableB = new Executable();

        public Form1()
        {
            InitializeComponent();

            profileA.Initialize(comboBox1, button3, button6, textBox2, textBox3, textBox6, textBox15);
            profileB.Initialize(comboBox2, button9, button10, textBox7, textBox9, textBox8, textBox14);
            CompileA.Initialize(textBox1, textBox4);
            CompileB.Initialize(textBox11, textBox4);
            AsmA.Initialize(textBox13, textBox15);
            AsmB.Initialize(textBox12, textBox14);
            ExecutableA.Initialize(textBox5, textBox6);
            ExecutableB.Initialize(textBox10, textBox8);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            diff_match_patch dmp = new diff_match_patch();
            dmp.Diff_Timeout = 0;
            List<Diff> diff = dmp.diff_main("Goodbbbye World", "Goodbye World");
            Console.WriteLine(diff[0]);
            dmp.diff_cleanupEfficiency(diff);
            for (int i = 0; i < diff.Count; i++)
            {
                Console.WriteLine(diff[i]);
            }
            Console.WriteLine( dmp.diff_prettyHtml(diff));

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
            CompileA.Run(profileA.GetNowProfile());
        }


        private void button5_Click(object sender, EventArgs e)
        {
            ExecutableA.Run();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            CompileB.Run(profileB.GetNowProfile());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ExecutableB.Run();
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
            AsmA.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            AsmB.Show();
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

    class Compile
    {
        protected System.Windows.Forms.TextBox outputTextBox;
        protected System.Windows.Forms.TextBox inputFileNameTextBox;

        public void Initialize(
            System.Windows.Forms.TextBox outputTextBox,
            System.Windows.Forms.TextBox inputFileNameTextBox
            )
        {
            this.outputTextBox = outputTextBox;
            this.inputFileNameTextBox = inputFileNameTextBox;
        }

        public void Run(Profile.ProfileInfo profileInfo)
        {
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

    }

    class Asm : Compile
    {
        public void Show()
        {
            StreamReader sr = new StreamReader(inputFileNameTextBox.Text, Encoding.GetEncoding("UTF-8"));
            outputTextBox.Text = sr.ReadToEnd();
            sr.Close();

        }
    }

    class Executable : Compile
    {
        // 実行はコンパイルの処理も行う
        public void Run()
        {
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
}
