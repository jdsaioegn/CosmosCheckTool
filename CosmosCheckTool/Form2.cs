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

namespace CosmosCheckTool
{
    public partial class Form2 : Form
    {
        /// <summary>
        /// コンストラクタ(設定ファイルの確認機能の設定画面)
        /// </summary>
        public Form2()
        {
            // 画面生成
            InitializeComponent();
        }

        /// <summary>
        /// ボタン押下時発生イベント
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト情報</param>
        /// <param name="e">イベント情報</param>
        private void ClickButton(object sender, EventArgs e)
        {
            // ボタン押下発生元オブジェクトのテキストプロパティによって処理を決定
            switch (((Button)sender).Text)
            {
                // □参照ボタン押下
                case "参照":
                    // フォルダ選択ダイアログ生成
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    // 初期選択設定(マイコンピュータ)
                    dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                    // フォルダ選択ダイアログにてOKボタンが押下された場合
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // 選択したパスをテキストボックスに表示
                        textBox1.Text = dialog.SelectedPath;
                        // 現在リストボックスに表示されている内容を初期化
                        listBox1.Items.Clear();
                        // 選択したパス直下に存在するフォルダをリストボックスに表示
                        new DirectoryInfo(dialog.SelectedPath).GetDirectories().ToList().ForEach(p => listBox1.Items.Add(p));
                    }
                    // 抜け
                    break;
                // □実行ボタン押下
                case "実行":
                    // 確認ダイアログにてYesが選択された場合
                    if (MessageBox.Show("以下のフォルダのチェック処理を開始します。\nよろしいですか？\n\n" + textBox1.Text, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        // TODO:比較結果画面表示
                    }
                    // 抜け
                    break;
                // □終了ボタン押下
                case "終了":
                    // TODO:
                    MessageBox.Show("終了ボタン押下");
                    // 抜け
                    break;
            }
        }
    }
}
