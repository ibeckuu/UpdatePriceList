using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;

namespace UpdatePriceList
{
    /// <summary>
    /// AS400 に接続しデータを取得するクラス
    /// </summary>
    class ConnectAS400
    {
        /// <summary>
        /// AS400にQueryを発行し、取得したデータをString の二次元リストとして返す
        /// </summary>
        public List<List<String>> Read(String connectString, String queryString)
        {

            List<List<String>> result = new List<List<String>>();

            OdbcCommand command = new OdbcCommand(queryString);

            try
            {
                using (OdbcConnection connection = new OdbcConnection(connectString))
                {
                    command.Connection = connection;
                    connection.Open();

                    OdbcDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        List<String> buf = new List<String>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            buf.Add(reader[i].ToString());
                        }
                        result.Add(buf);
                    }
                    reader.Close();
                }
                return result;

            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (OdbcException)
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
