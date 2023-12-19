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
            //ProcessStartInfoのオブジェクトを生成
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = @"C:\\Users\\sotar\\Desktop\\BenchmarkForCcompiler\\testcase\\a.exe"; //コマンド
            // psInfo.Arguments = @"/c dir d:\ /s"; //引数
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト

            Process p = Process.Start(psInfo); // コマンドの実行開始
            string line = "";
            while((line = p.StandardOutput.ReadLine()) != null)
            {
                textBox1.Text = line;
            }
        }
    }
}
