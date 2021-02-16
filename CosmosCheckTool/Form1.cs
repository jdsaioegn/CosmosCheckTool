using IniParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CosmosCheckTool
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 保存場所(「環境設定.ini」設定値)
        /// </summary>
        public static string OutPath;

        /// <summary>
        /// コンストラクタ(初期画面)
        /// </summary>
        public Form1()
        {
            // 画面生成
            InitializeComponent();
        }

        /// <summary>
        /// 初期画面(Form1)読み込み時発生イベント
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト情報</param>
        /// <param name="e">イベント情報</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // 環境設定ファイル読み込み(※UTF8対応)
            FileIniDataParser parser = new FileIniDataParser();
            IniParser.Model.IniData data = parser.ReadFile("環境設定.ini", Encoding.UTF8);
            OutPath = data["保存場所"]["Path"];
        }
    }
}
