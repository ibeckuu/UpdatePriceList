using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;


namespace UpdatePriceList
{
    /// <summary>
    /// Json 形式の設定ファイルを読み込むクラス
    /// </summary>
    class ReadJSON
    {
        public String MasterFilePath { get; private set; }
        public String MasterFileName { get; private set; }
        public String ConnectString { get; private set; }
        public String QueryString { get; private set; }

        public ReadJSON()
        {
            MasterFileName = "";
            MasterFilePath = "";
            ConnectString = "";
            QueryString = "";
        }

        /// <summary>
        /// 設定ファイルを読み込みインスタンスに保持する
        /// </summary>
        public void SetParameters(System.String paramFilePath, System.String paramFileName)
        {
            try
            {
                string target;
                string fullFileName = paramFilePath + "\\" + paramFileName;
                StreamReader sr = new StreamReader(fullFileName, System.Text.Encoding.GetEncoding("shift_jis"));
                target = sr.ReadToEnd();
                sr.Close();
                var loadedFile = JsonConvert.DeserializeObject<Parameter>(target);
                MasterFilePath = loadedFile.FilePath;
                MasterFileName = loadedFile.FileName;
                ConnectString = loadedFile.ConnectString;
                StringBuilder sb = new StringBuilder("");
                for (var i = 0; i < loadedFile.QueryStrings.Length; i++)
                {
                    sb.Append(loadedFile.QueryStrings[i]);
                }
                QueryString = sb.ToString();
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
