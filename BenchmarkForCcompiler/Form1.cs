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
    public partial class Form1 : System.Windows.Forms.Form
    {
        /* インスタンスの生成 */
        Profile profileA = new Profile();
        Profile profileB = new Profile();
        Compile compile = new Compile();
        Asm asm = new Asm();
        Executable executable = new Executable();
        Executable ExecutableB = new Executable();
        Form2 form2 = new Form2();

        public Form1()
        {
            /* インスタンスの初期化処理 */
            InitializeComponent();
            profileA.Initialize(comboBox1, textBox2, textBox3, textBox6, textBox15);
            profileB.Initialize(comboBox2, textBox7, textBox9, textBox8, textBox14);
            compile.Initialize(richTextBox1, textBox4, richTextBox4, textBox4, richTextBox7);
            asm.Initialize(richTextBox2, textBox15, richTextBox5, textBox14, richTextBox8);
            executable.Initialize(richTextBox3, textBox6, richTextBox6, textBox8, richTextBox9);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* 初期設定適応 */
            // temp directory 作成
            Directory.CreateDirectory(@"temp" + ProfileStatus.ProfileA);
            Directory.CreateDirectory(@"temp" + ProfileStatus.ProfileB);
            

            // 比較表示
            比較表示ToolStripMenuItem.PerformClick();

            tableLayoutPanel2.Width = 000;
            tableLayoutPanel2.Height = 000;
            flowLayoutPanel2.Width = flowLayoutPanel2.Height = 0;
            tableLayoutPanel2.Refresh();
        }

        private void Form1_Close(object sender, EventArgs e)
        {
            /* 終了処理 */
            Directory.Delete(@"temp" + ProfileStatus.ProfileA, true);
            Directory.Delete(@"temp" + ProfileStatus.ProfileB, true);
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
            compile.Run(ProfileStatus.ProfileB, profileB.GetNowProfile());
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
            asm.Run(ProfileStatus.ProfileA);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            asm.Run(ProfileStatus.ProfileB);
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

        private void button18_Click(object sender, EventArgs e)
        {
            richTextBox7.Text = "";
            compile.Run(ProfileStatus.ProfileA, profileA.GetNowProfile());
            compile.Run(ProfileStatus.ProfileB, profileB.GetNowProfile()); 
            compile.Comparison(profileA.GetNowProfile(), profileB.GetNowProfile());

        }

        private void button19_Click(object sender, EventArgs e)
        {
            richTextBox8.Text = "";
            asm.Run(ProfileStatus.ProfileA);
            asm.Run(ProfileStatus.ProfileB);
            asm.Comparison();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            richTextBox9.Text = "";
            executable.Run(ProfileStatus.ProfileA);
            executable.Run(ProfileStatus.ProfileB);
            executable.Comparison();
        }

        private void 全て実行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compile.Comparison(profileA.GetNowProfile(), profileB.GetNowProfile());
            asm.Comparison();
            executable.Comparison();
        }

        private void コンパイル実行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compile.Run(ProfileStatus.ProfileA, profileA.GetNowProfile());
            compile.Run(ProfileStatus.ProfileB, profileB.GetNowProfile());
        }

        private void アセンブラ表示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            asm.Run(ProfileStatus.ProfileA);
            asm.Run(ProfileStatus.ProfileB);
        }

        private void プログラム実行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            executable.Run(ProfileStatus.ProfileA);
            executable.Run(ProfileStatus.ProfileB);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            form2.FilePath = textBox4.Text;
            form2.Road();
        }
    }

    class Profile
    {
        /* プロファイルの全ての情報を格納する構造体の定義 */
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
        private System.Windows.Forms.TextBox compilerTextBox;
        private System.Windows.Forms.TextBox optionTextBox;
        private System.Windows.Forms.TextBox executableFileNameTextBox;
        private System.Windows.Forms.TextBox asmFileNameTextBox;
        private string profilePath = @"profile\\";

        /* 初期化 */
        public void Initialize(
            System.Windows.Forms.ComboBox comboBox,
            System.Windows.Forms.TextBox compilerTextBox,
            System.Windows.Forms.TextBox optionTextBox,
            System.Windows.Forms.TextBox executableFileNameTextBox,
            System.Windows.Forms.TextBox asmFileNameTextBox
            )
        {
            this.comboBox = comboBox;
            this.compilerTextBox = compilerTextBox;
            this.optionTextBox = optionTextBox;
            this.executableFileNameTextBox = executableFileNameTextBox;
            this.asmFileNameTextBox = asmFileNameTextBox;
            ShowProfileList();
        }

        /* comboBoxのリストに追加 */
        public void ShowProfileList()
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(GetList());

        }

        private string[] GetList()
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

        private ProfileInfo RoadProfileInfo(string filename)
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

        /* プロファイルを表示 */
        public void ShowProfileInfo()
        {
            RoadProfileInfo(comboBox.Text);
            compilerTextBox.Text = profileInfo.Compiler;
            optionTextBox.Text = profileInfo.Option;
            executableFileNameTextBox.Text = profileInfo.ExecutableFileName;
            asmFileNameTextBox.Text = profileInfo.AsmFileName;
            return;
        }


        /* プロファイル作成 */
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
                        "警告",
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

        /* 現在のプロファイル */
        public ProfileInfo GetNowProfile()
        {
            return new ProfileInfo(comboBox.Text, compilerTextBox.Text, optionTextBox.Text, executableFileNameTextBox.Text, asmFileNameTextBox.Text);
        }

        /* プロファイルの保存 */
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

    /* 抽象クラス */
    abstract class BuildBaseClass
    {
        protected System.Windows.Forms.RichTextBox outputRichTextBox;
        protected System.Windows.Forms.TextBox inputFileNameTextBox;
        protected System.Windows.Forms.RichTextBox ProfileA_OutputRichTextBox;
        protected System.Windows.Forms.TextBox ProfileA_InputFileNameTextBox;
        protected System.Windows.Forms.RichTextBox ProfileB_OutputRichTextBox;
        protected System.Windows.Forms.TextBox ProfileB_InputFileNameTextBox;
        protected System.Windows.Forms.RichTextBox ComparisonRichOutputTextBox;
        protected Comparison comparison = new Comparison();

        /* 初期化 */
        public void Initialize(
            System.Windows.Forms.RichTextBox ProfileA_OutputRichTextBox,
            System.Windows.Forms.TextBox ProfileA_InputFileNameTextBox,
            System.Windows.Forms.RichTextBox ProfileB_OutputRichTextBox,
            System.Windows.Forms.TextBox ProfileB_InputFileNameTextBox,
            System.Windows.Forms.RichTextBox ComparisonOutputRichTextBox
            )
        {
            this.ProfileA_OutputRichTextBox = ProfileA_OutputRichTextBox;
            this.ProfileA_InputFileNameTextBox = ProfileA_InputFileNameTextBox;
            this.ProfileB_OutputRichTextBox = ProfileB_OutputRichTextBox;
            this.ProfileB_InputFileNameTextBox = ProfileB_InputFileNameTextBox;
            this.ComparisonRichOutputTextBox = ComparisonOutputRichTextBox;
        }

        protected void switchProfile(ProfileStatus profileStatus)
        {
            switch (profileStatus)
            {
                case ProfileStatus.ProfileA:
                    outputRichTextBox = ProfileA_OutputRichTextBox;
                    inputFileNameTextBox = ProfileA_InputFileNameTextBox;
                    break;
                case ProfileStatus.ProfileB:
                    outputRichTextBox = ProfileB_OutputRichTextBox;
                    inputFileNameTextBox = ProfileB_InputFileNameTextBox;
                    break;
                default:
                    Console.WriteLine("Error:プロファイルを選択してください。");
                    break;
            }
        }

        protected string matchFileName(ProfileStatus profileStatus, string match)
        {
            if (match != "" && match[0] != '*')
            {
                return match;
            }
            
            string[] filename;
            filename = Directory.GetFiles(@"temp" + profileStatus);
            for (int i = 0; i < filename.Length; i++)
            {
                filename[i] = Path.GetFileName(filename[i]);
                Console.WriteLine(filename[i]);
                for (int j = 1; j <= filename[i].Length && j < match.Length; j++)
                {
                    if (filename[i][filename[i].Length - j] != match[match.Length - j])
                    {
                        break;
                    }
                    if (match.Length-1 == j)
                    {
                        return filename[i];
                    }
                }
            }

            return "";
        }
        abstract public void Run(ProfileStatus profileStatus);

        public void Comparison()
        {
            Run(ProfileStatus.ProfileA);
            Run(ProfileStatus.ProfileB);
            comparison.GitDiff(ProfileA_OutputRichTextBox.Text.Replace("\n", Environment.NewLine), ProfileB_OutputRichTextBox.Text.Replace("\n", Environment.NewLine), ComparisonRichOutputTextBox);
            //ComparisonRichOutputTextBox.Text = comparison.GNUDiff(ProfileA_OutputRichTextBox.Text, ProfileB_OutputRichTextBox.Text);
        }
    }

    class Compile : BuildBaseClass
    {
        public override void Run(ProfileStatus profileStatus)
        {
            return;
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
            psInfo.WorkingDirectory = @"temp" + profileStatus;

            Console.WriteLine(psInfo.FileName + psInfo.Arguments);
            
            Process p;
            try
            {
                p = Process.Start(psInfo); // コマンドの実行開始
            }
            catch(InvalidOperationException ex)
            {
                // ファイル名を指定していない
                MessageBox.Show(
                    "「コンパイラのパス」が入力されていません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine(ex.Message);
                return;
            }
            catch (Win32Exception ex)
            {
                // ファイル名が正しくない
                MessageBox.Show(
                    "「コンパイラのパス」が正しくありません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine(ex.Message);
                return;
            }

            string line = "";
            string lines = "";
            while ((line = p.StandardOutput.ReadLine()) != null || (line = p.StandardError.ReadLine()) != null)
            {
                lines += line + "\r\n";
            }
            outputRichTextBox.Text = lines;
        }
        
        public void Comparison(Profile.ProfileInfo profileInfoA, Profile.ProfileInfo profileInfoB)
        {
            Run(ProfileStatus.ProfileA, profileInfoA);
            Run(ProfileStatus.ProfileB, profileInfoB);
            comparison.GitDiff(ProfileA_OutputRichTextBox.Text.Replace("\n", Environment.NewLine), ProfileB_OutputRichTextBox.Text.Replace("\n", Environment.NewLine), ComparisonRichOutputTextBox);
        }
    }

    class Asm : BuildBaseClass
    {
        public override void Run(ProfileStatus profileStatus)
        {
            switchProfile(profileStatus);
            StreamReader sr = null;
            try
            {
                // 先頭だけ正規表現使える
                sr = new StreamReader(@"temp" + profileStatus + @"\" + matchFileName(profileStatus, inputFileNameTextBox.Text), Encoding.GetEncoding("UTF-8"));
                outputRichTextBox.Text = sr.ReadToEnd();
                sr.Close();
            }
            catch (ArgumentException ex)
            {
                // ファイル名を指定していない
                MessageBox.Show(
                    "「アセンブラファイル名」が入力されていません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                // ファイル名が正しくない
                MessageBox.Show(
                    "「アセンブラファイル名」が存在しません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine(ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                // ファイル名が正しくない
                MessageBox.Show(
                    "「アセンブラファイル名」が存在しません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine(ex.Message);
            }
            catch(IOException ex) {
                // ファイル名が不正
                MessageBox.Show(
                    "「アセンブラファイル名」に使用できない文字が含まれています",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sr?.Close();
            }
        }
    }

    class Executable : BuildBaseClass
    {
        // 実行はコンパイルの処理も行う
        public override void Run(ProfileStatus profileStatus)
        {
            switchProfile(profileStatus);
            // プログラムの実行
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = inputFileNameTextBox.Text == "" ? "" : @"temp" + profileStatus + @"\" + matchFileName(profileStatus, inputFileNameTextBox.Text);
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト

            Console.WriteLine(psInfo.FileName + psInfo.Arguments);

            Process p;
            try
            {
                p = Process.Start(psInfo); // コマンドの実行開始
            }
            catch (InvalidOperationException ex)
            {
                // ファイル名を指定していない
                MessageBox.Show(
                    "「実行ファイル名」が入力されていません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine(ex.Message);
                return;
            }
            catch (Win32Exception ex)
            {
                // ファイル名が正しくない
                MessageBox.Show(
                    "「実行ファイル名」が存在しません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine(ex.Message);
                return;
            }

            string line = "";
            string lines = "";
            while ((line = p.StandardOutput.ReadLine()) != null)
            {
                lines += line + "\r\n";
            }
            outputRichTextBox.Text = lines;

        }
    }

    class Comparison
    {
        diff_match_patch dmp = new diff_match_patch();
        /*
        public string GNUDiff(string textA, string textB)
        {
            return dmp.patch_toText(dmp.patch_make(textA, textB));
        }
        */

        public void GitDiff(string  textA, string textB, System.Windows.Forms.RichTextBox richTextBox)
        {
            richTextBox.Text = "";
            var diff = dmp.diff_main(textA, textB);
            dmp.diff_cleanupSemantic(diff); 
            dmp.diff_text(diff, richTextBox);
        }
    }
}
