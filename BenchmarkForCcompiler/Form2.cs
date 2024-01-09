using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BenchmarkForCcompiler
{
    public partial class Form2 : System.Windows.Forms.Form
    {
        public string FilePath;
        private bool textBoxChange = false;

        public Form2()
        {
            InitializeComponent();
        }

        public void Road()
        {
            Console.WriteLine(FilePath);
            richTextBox1.Text = "";
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(@FilePath, Encoding.GetEncoding("UTF-8"));
                richTextBox1.Text = sr.ReadToEnd();
                sr.Close();
            }
            catch (ArgumentException ex)
            {
                // ファイル名を指定していない
                if (MessageBox.Show(
                    "「プログラムのパス」が入力されていません。" + Environment.NewLine + "ファイルを新規作成します。",
                    "エラー",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ) == DialogResult.No)
                {
                    this.Hide();
                    return;
                }
                Console.WriteLine(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                // ファイル名が正しくない
                if (MessageBox.Show(
                    "「プログラムのパス」が正しくありません。" + Environment.NewLine + "ファイルを新規作成します。",
                    "エラー",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ) == DialogResult.No)
                {
                    this.Hide();
                    return;
                }
                Console.WriteLine(ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                // ファイル名が正しくない
                if (MessageBox.Show(
                    "「プログラムのパス」が正しくありません。" + Environment.NewLine + "ファイルを新規作成します。",
                    "エラー",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ) == DialogResult.No)
                {
                    this.Hide();
                    return;
                }
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                // ファイル名が不正
                if (MessageBox.Show(
                    "「プログラムのパス」に使用できない文字が含まれています" + Environment.NewLine + "ファイルを新規作成します。",
                    "エラー",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                ) == DialogResult.No)
                {
                    this.Hide();
                    return;
                }
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sr?.Close();
            }

            if (FilePath == null || FilePath == "")
            {
                this.Text = "New File";
            }
            else
            {
                this.Text = FilePath;
            }
            this.Show();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 保存しますか？

            // フォームを隠す
            this.Hide();
            e.Cancel = true;
        }

        private void 上書保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 上書保存
            StreamWriter streamWriter = new StreamWriter(FilePath, false);
            streamWriter.Write(richTextBox1.Text);
            streamWriter.Close();
        }

        private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 上書保存
                FilePath = saveFileDialog.FileName;
                StreamWriter streamWriter = new StreamWriter(FilePath, false);
                streamWriter.Write(richTextBox1.Text);
                streamWriter.Close();
            }
        }

        private void ファイルを開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = openFileDialog.FileName;
                this.Road();
            }
        }

        private void 新規作成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilePath = "";
            richTextBox1.Text = "";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            textBoxChange = true;
            
            if (FilePath == null || FilePath == "")
            {
                this.Text = "New File" + " *";
            }
            else
            {
                this.Text = FilePath + " *";
            }
        }
    }
}
