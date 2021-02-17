using System.ComponentModel;

namespace CosmosCheckTool.model
{
    /// <summary>
    /// 検証結果用クラス
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 変更ID
        /// </summary>
        [DisplayName("変更ID")]
        public string ChangeID { get; set; }
        /// <summary>
        /// 処理
        /// </summary>
        [DisplayName("処理")]
        public string Operation { get; set; }
        /// <summary>
        /// ファイル名
        /// </summary>
        [DisplayName("ファイル名")]
        public string FilePath { get; set; }
        /// <summary>
        /// 差異
        /// </summary>
        [DisplayName("差異")]
        public string Difference { get; set; }
        /// <summary>
        /// ファイル名
        /// </summary>
        [DisplayName("ルート")]
        public string Node { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("属性")]
        public string Attribute { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("元値")]
        public string BeforeValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("値")]
        public string AfterValue { get; set; }
    }
}
