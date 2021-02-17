using CosmosCheckTool.model;
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
using System.Xml;

namespace CosmosCheckTool
{
    public partial class Form3 : Form
    {
        /// <summary>
        /// COSMOS機能フォルダパス
        /// </summary>
        public string CosmosDir = string.Empty;

        /// <summary>
        /// Workデータリスト
        /// </summary>
        private List<DataModel> data;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form3()
        {
            // 画面生成
            InitializeComponent();
        }

        /// <summary>
        /// 差異画面(Form3)読み込み時発生イベント
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト情報</param>
        /// <param name="e">イベント情報</param>
        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO:外部設定値による読み込み対象の変更(V2.0)
            string path = @".\チェック対象ファイル.ini";
            // チェック対象ファイル.iniの全行取り込み
            string[] lines = File.ReadAllLines(path);
            // Workデータリスト初期化
            data = new List<DataModel>();
            // XML情報登録タグ用文字列
            string tag = string.Empty;
            // チェック対象ファイル.iniから取り込んだ内容を1行ずつ確認
            foreach (string line in lines)
            {
                // △ファイル情報セクション(文字列の開始が「[(角括弧)」)の場合
                if (line.First() == '[')
                {
                    // □Files項目のセクションの場合
                    if (line.Contains("Files]"))
                    {
                        // 検索モデル追加
                        data.Add(new DataModel()
                        {
                            // 変更IDの設定
                            ChangeID = line.Trim('[').Replace("Files]", "")
                        });
                    }
                    // □Check項目のセクションの場合
                    else if (line.Contains("Check]"))
                    {
                        // 〇該当する変更IDが1つ存在する場合のみXML情報登録タグ用文字列を設定
                        if (data.Where(p => p.ChangeID == line.Trim('[').Replace("Check]", "")).Count() == 1)
                        {
                            // XML情報登録タグ用文字列を設定
                            tag = line.Trim('[').Replace("Check]", "");
                        }
                        // 〇該当する変更IDが1つ存在する以外の場合はメッセージを出力し処理を継続
                        else
                        {
                            // ワーニングメッセージを出力
                            MessageBox.Show($"検索ファイルと紐づかない検索情報が存在します。\n対象変更ID：{line}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    // □上記以外のセクションが登録されている場合はメッセージを出力しツールを終了
                    else
                    {
                        // エラーメッセージを出力
                        MessageBox.Show($"異常なセクションが登録されています。\n対象変更ID：{line}", "異常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // アプリケーションを強制終了
                        Environment.Exit(0);
                    }
                }
                // △XML情報セクション(文字列の開始が「[(角括弧)」)以外の場合
                else
                {
                    // □先頭がFileの場合
                    if (line.StartsWith("\tFile"))
                    {
                        // ファイル情報登録
                        data.Last().FileInfos.Add(new model.DataModel.FileInfo()
                        {
                            // 完全パス
                            FilePath = line.Split('=').ElementAt(1).Split('|').ElementAt(0),
                            // ファイル名
                            FileName = Path.GetFileName(line.Split('=').ElementAt(1).Split('|').ElementAt(0))
                        });
                    }
                    // □先頭がFile以外の場合
                    else
                    {
                        // 登録対象となる変更IDが1つ存在する場合のみ登録
                        if (data.Where(p => p.ChangeID == tag).Count() == 1)
                        {
                            // 先頭タブを削除
                            string work = line.Trim('\t');
                            // XML情報登録
                            data.Where(p => p.ChangeID == tag).Single().XmlInfos.Add(new DataModel.XmlInfo()
                            {
                                // 操作を設定
                                Operation = work.Split('|').Last().Contains("del=") ? "del" : "edi",
                                // 検索値及び属性値を設定
                                ChangeData = work.Split('|').Last().Split('=').Last(),
                                // 検索情報設定
                                SearchXmlNode = GetSearchXmlNode(work)
                            });
                        }
                    }
                }
            }
            // 検索モデルごとに繰り返し
            foreach (DataModel item in data)
            {
                // ファイル情報ごとに繰り返し
                foreach (DataModel.FileInfo fileinfo in item.FileInfos)
                {
                    // MXL登録情報ごとに繰り返し
                    foreach (DataModel.XmlInfo xmlinfo in item.XmlInfos)
                    {
                        // 例外検知
                        try
                        {
                            // Xml解析インスタンス生成
                            XmlDocument xml = new XmlDocument();
                            // 解析ファイル読み込み
                            xml.Load(CosmosDir + fileinfo.FilePath);
                            // 解析
                            XmlNodeList node = xml.SelectNodes(xmlinfo.SearchXmlNode);
                        }
                        // 例外発生時
                        catch (Exception)
                        {
                            // メッセージ表示(処理は続行)
                            MessageBox.Show($"以下のファイルを検索中に例外が発生しました。\nファイル情報：{fileinfo.FilePath}\n検索ルート{xmlinfo.SearchXmlNode}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 検索情報解析
        /// </summary>
        /// <param name="line">解析文字列</param>
        /// <returns>解析結果</returns>
        private string GetSearchXmlNode(string line)
        {
            // 戻り値用
            string rtn = string.Empty;
            // 設定値取得
            string value = line.Split('=')?.ElementAt(2);
            // ノード設定
            string node = line.Split('=').ElementAt(0).Replace("p:", "");
            // 設定値及び属性値設定
            string[] attr = GetValue(line.Split('=').ElementAt(1), value);
            // 階層設定
            node.Split('|').ToList().ForEach(p => rtn += GetNode(p, attr));
            // 戻り値
            return rtn;
        }
        /// <summary>
        /// 設定値(属性値)解析
        /// </summary>
        /// <param name="line">解析文字列</param>
        /// <param name="value">設定値</param>
        /// <returns>解析結果</returns>
        private string[] GetValue(string line, string value)
        {
            // 戻り値用
            string[] sp = line.Split('|').ElementAt(0).Split(',').Where(p => !string.IsNullOrEmpty(p)).ToArray();
            // 属性値検索の場合(attr=が設定されている場合)
            if (line.Split('|').ElementAt(1).EndsWith("attr"))
            {
                // 戻り値用配列要素数を1追加
                Array.Resize(ref sp, sp.Count() + 1);
                // 要素検索値を設定
                sp[sp.Count() - 1] = value;
            }
            // 戻り値
            return sp;
        }
        /// <summary>
        /// ノード解析
        /// </summary>
        /// <param name="line">解析文字列</param>
        /// <param name="value">設定値</param>
        /// <returns>解析結果</returns>
        private string GetNode(string line, string[] value)
        {
            // 戻り値用
            string rtn = string.Empty;
            // 処理用
            string[] sp = line.Split('|');
            // 要素登録
            rtn = "/" + sp.First();
            // 要素以外の設定がある場合
            if (sp.Count() != 1)
            {
                // 属性値設定①
                rtn += "[";
                // 属性値設定②
                for (int i = 1; i < sp.Count(); i++)
                {
                    // 属性値設定③
                    rtn += "@" + sp[i] + "='" + value[i - 1] + "'";
                    // 属性値設定④
                    if (i < sp.Count() - 1)
                    {
                        // 属性値設定⑤
                        rtn += " and ";
                    }
                }
                // 属性設定⑥
                rtn += "]";
            }
            // 戻り値
            return rtn;
        }


        /// <summary>
        /// ボタン押下時発生イベント
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト情報</param>
        /// <param name="e">イベント情報</param>
        private void ButtonClick(object sender, EventArgs e)
        {
            // ボタン押下発生元オブジェクトのテキストプロパティによって処理を決定
            switch (((Button)sender).Text)
            {
                // □Text出力ボタン押下
                case "Text出力":
                    // TODO:
                    MessageBox.Show("Text出力ボタン押下");
                    // 抜け
                    break;
                // □Excel出力ボタン押下
                case "Excel出力":
                    // TODO:
                    MessageBox.Show("Excel出力ボタン押下");
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
