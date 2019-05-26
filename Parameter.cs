using System;
using System.Collections.Generic;

namespace UpdatePriceList
{
    /// <summary>
    /// 設定ファイルクラス
    /// </summary>
    class Parameter
    {
        public String FilePath { get; set; }
        public String FileName { get; set; }
        public String ConnectString { get; set; }
        public System.String[] QueryStrings { get; set; }
    }
}
