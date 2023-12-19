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
using System.Windows.Forms.VisualStyles;

namespace BenchmarkForCcompiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            //psInfo.Arguments = @textBox4.Text + " " + textBox3;
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト
            //psInfo.RedirectStandardError = true;

            Process p = Process.Start(psInfo); // コマンドの実行開始
            string line = "";
            while((line = p.StandardOutput.ReadLine()) != null)
            {
                textBox1.Text = line;
            }
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
            //psInfo.RedirectStandardError = true;

            Process p = Process.Start(psInfo); // コマンドの実行開始
            string line = "";
            string lines = "";
            while ((line = p.StandardOutput.ReadLine()) != null)
            {
                lines += line + "\r\n";
            }
            textBox5.Text = lines;
        }
    }
}
