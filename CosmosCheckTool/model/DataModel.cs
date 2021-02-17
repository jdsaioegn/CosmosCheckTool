using System.Collections.Generic;

namespace CosmosCheckTool.model
{
    /// <summary>
    /// INIファイルパース用データモデル
    /// </summary>
    public class DataModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataModel()
        {
            // リスト初期化
            FileInfos = new List<FileInfo>();
            // リスト初期化
            XmlInfos = new List<XmlInfo>();
        }
        /// <summary>
        /// 変更ID
        /// </summary>
        public string ChangeID { get; set; }
        /// <summary>
        /// ファイル情報リスト
        /// </summary>
        public List<FileInfo> FileInfos { get; set; }
        /// <summary>
        /// 検索タグ情報リスト
        /// </summary>
        public List<XmlInfo> XmlInfos { get; set; }

        /// <summary>
        /// ファイル情報
        /// </summary>
        public class FileInfo
        {
            /// <summary>
            /// ファイル名
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// ファイルフルパス
            /// </summary>
            public string FilePath { get; set; }
        }

        /// <summary>
        /// 検索タグ情報
        /// </summary>
        public class XmlInfo
        {
            /// <summary>
            /// 検索操作
            /// </summary>
            public string Operation { get; set; }
            /// <summary>
            /// 検索タグ
            /// </summary>
            public string ChangeData { get; set; }
            /// <summary>
            /// 検証タグ
            /// </summary>
            public string SearchXmlNode { get; set; }
        }
    }
}
