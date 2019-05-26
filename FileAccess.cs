using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UpdatePriceList
{
    /// <summary>
    /// ファイル読み込み、書き込みクラス
    /// </summary>
    class FileAccess
    {
        private System.String _path;
        private System.String _fileName;

        public FileAccess(String path, String fileName)
        {
            _path = path;
            _fileName = fileName;
        }

        /// <summary>
        /// マスターデータファイルを読み込んで List に詰め直して返す
        /// </summary>
        public List<PriceList> ReadMasterFile()
        {
            List<PriceList> MasterPriceList = new List<PriceList>();
            String fullFileName = _path + "\\" + _fileName;
            try
            {
                using (var reader = new StreamReader
                (fullFileName, Encoding.GetEncoding("shift_jis")))
                {
                    while (!reader.EndOfStream)
                    {
                        PriceList priceList = new PriceList();
                        String str = reader.ReadLine();
                        String[] values = str.Split(',');
                        priceList.ItemNumber = Int32.Parse(values[0]);
                        priceList.Price = Int32.Parse(values[1]);
                        priceList.StockLevel = Int32.Parse(values[2]);
                        priceList.Description = values[3];
                        MasterPriceList.Add(priceList);
                    }
                    return MasterPriceList;
                }

            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 最新リストをファイルに書き込む（上書き）
        /// </summary>
        public void WriteMasterFile(List<PriceList> updateList)
        {
            String fullFileName = _path + "\\" + _fileName;
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(fullFileName, false, Encoding.GetEncoding("shift-jis")))
                {
                    foreach (var priceData in updateList)
                    {
                        String str = priceData.ItemNumber.ToString() + ","
                                   + priceData.Price.ToString() + ","
                                   + priceData.StockLevel.ToString() + ","
                                   + priceData.Description;
                        streamWriter.WriteLine(str);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
