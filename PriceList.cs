using System;
using System.Collections.Generic;

namespace UpdatePriceList
{
    /// <summary>
    /// 価格表クラス
    /// </summary>
    public class PriceList
    {
        public int ItemNumber { get; set; }
        public int Price { get; set; }
        public int StockLevel { get; set; }
        public String Description { get; set; }
    }
        
}
