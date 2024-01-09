using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BenchmarkForCcompiler
{
    public partial class Form2 : Form
    {
        public string FilePath;

        public Form2()
        {
            InitializeComponent();
        }

        public void Road()
        {
            Console.WriteLine(FilePath);
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
            this.Show();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
