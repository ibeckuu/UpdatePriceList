using System;
using System.Collections.Generic;
using System.IO;

namespace UpdatePriceList
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
                    String msg = "Usage : コマンドライン引数が2つ必要です。(設定ファイルのフルパス ファイル名)";
                    Console.WriteLine(msg);
                    return;
                }
                String filePath = args[0];
                String fileName = args[1];

                // 設定値の読み込み
                ReadJSON readJSON = new ReadJSON();
                readJSON.SetParameters(filePath, fileName);

                // AS400データを読み込み最新価格リストを作成
                List<List<String>> as400data = new List<List<String>>();
                ConnectAS400 connect = new ConnectAS400();
                as400data = connect.Read(readJSON.ConnectString, readJSON.QueryString);
                List<PriceList> newPriceList = new List<PriceList>();
                newPriceList = Calculate.ConvertToObjectList(as400data);

                // 元ファイルを開く
                FileAccess masterFile = new FileAccess(readJSON.MasterFilePath, readJSON.MasterFileName);

                // 元ファイルを読み込み元データリストを作成
                List<PriceList> originalMasterList = new List<PriceList>();
                originalMasterList = masterFile.ReadMasterFile();

                // リストを照合し更新価格データリストを作成
                List<PriceList> UpdatePriceList = new List<PriceList>();
                UpdatePriceList = Calculate.Update(originalMasterList, newPriceList);

                // 更新価格ファイルに書き込む
                masterFile.WriteMasterFile(UpdatePriceList);

                Console.WriteLine("更新処理完了");

            }
            catch (Exception ex)
            {
                String logFileName = "errorLog.txt";
                String fullFileName = Environment.CurrentDirectory + "\\" + logFileName;
                String errorMsg = "エラーが発生しました。\n詳細は "
                    + fullFileName + " を参照下さい。";
                Console.WriteLine(errorMsg);

                string appendText = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\n"
                    + ex.Message + "\n"
                    + ex.StackTrace + "\n";
                File.AppendAllText(fullFileName, appendText);
            }
        }
    }
}
