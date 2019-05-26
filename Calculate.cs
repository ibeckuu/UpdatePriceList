using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UpdatePriceList
{
    /// <summary>
    /// 価格表作成クラス
    /// </summary>
    static class Calculate
    {
        /// <summary>
        /// AS400からのデータを PriceList オブジェクトに変換する
        /// </summary>
        public static List<PriceList> ConvertToObjectList(List<List<String>> duplicableString)
        {
            List<PriceList> result = new List<PriceList>();
            int inputcount = 0;
            foreach (var row in duplicableString)
            {
                inputcount++;
                PriceList item = new PriceList();
                // データの最後の空白の連続を削除する為のパターン
                String pattern = @"\s+$";

                // フォーマットに合わないデータのある行はスキップする
                if (!Regex.IsMatch(row[0], @"\d{7}\s*")) continue;
                item.ItemNumber = Int32.Parse(Regex.Replace(row[0], pattern, ""));

                if (!Regex.IsMatch(row[1], @"\w.+\s*")) continue;
                item.Description = Regex.Replace(row[1], pattern, "");

                if (!Regex.IsMatch(row[2], @"\d+")) continue;
                item.StockLevel = Int32.Parse(Regex.Replace(row[2], pattern, ""));

                if (!Regex.IsMatch(row[3], @"0\.\d{3}")) continue;
                int discount = Int32.Parse(Regex.Replace(row[3], @"0\.(\d{2})\d", "$1"));

                if (!Regex.IsMatch(row[4], @"\d+\.000")) continue;
                int unitPrice = Int32.Parse(Regex.Replace(row[4], @"\.000", ""));

                // 仕切り価格 = 定価 * (1-割引率)
                // 1円未満を切り上げる為0.9円を足してから切り捨てる
                // int で計算する為100倍して計算してから100で割る
                item.Price = ((unitPrice * (100 - discount)) + 90) / 100;
                result.Add(item);

            }
            Console.WriteLine("取得データ総数 : " + inputcount + " , 有効データ数 : " + result.Count);

            return result;
        }

        /// <summary>
        /// 元データと最新データを照合し最新価格リストを作成して返す
        /// </summary>
        public static List<PriceList> Update
                 (List<PriceList> originalList, List<PriceList> newList)
        {
            // 照合処理
            // 元データ・更新データともに商品番号の昇順に並んでいる
            // 両データ商品番号を比較し
            // 1.商品番号が一致・・・更新データをコピー
            //   両方をインクリメント
            // 2.更新データの商品番号が小さい・・・（新規アイテム）更新データをコピー
            //   更新データをインクリメント
            // 3.更新データの商品番号が大きい・・・（廃番になった）元データのStockLevelを99としてコピー
            //   元データをインクリメント

            List<PriceList> updateList = new List<PriceList>();
            int orgCounter = 0;
            int newCounter = 0;
            while (orgCounter < originalList.Count && newCounter < newList.Count)
            {
                if (newList[newCounter].ItemNumber == originalList[orgCounter].ItemNumber)
                {
                    updateList.Add(newList[newCounter]);
                    orgCounter++;
                    newCounter++;
                }
                else if (newList[newCounter].ItemNumber < originalList[orgCounter].ItemNumber)
                {
                    updateList.Add(newList[newCounter]);
                    newCounter++;
                }
                else
                {
                    PriceList phaseOut = new PriceList();
                    phaseOut.ItemNumber = originalList[orgCounter].ItemNumber;
                    phaseOut.Price = originalList[orgCounter].Price;
                    phaseOut.StockLevel = 99;
                    phaseOut.Description = originalList[orgCounter].Description;
                    updateList.Add(phaseOut);
                    orgCounter++;
                }
            }

            // 元データが最終レコードに達した場合（新規追加データが残っている）
            while (newCounter < newList.Count)
            {
                updateList.Add(newList[newCounter]);
                newCounter++;
            }

            // 更新データが最終レコードに達した場合（元データが残っている）
            while (orgCounter < originalList.Count)
            {
                PriceList phaseOut = new PriceList();
                phaseOut.ItemNumber = originalList[orgCounter].ItemNumber;
                phaseOut.Price = originalList[orgCounter].Price;
                phaseOut.StockLevel = 99;
                phaseOut.Description = originalList[orgCounter].Description;
                updateList.Add(phaseOut);
                orgCounter++;
            }
            Console.WriteLine("更新後データ数 : " + updateList.Count);

            return updateList;
        }

    }
}
