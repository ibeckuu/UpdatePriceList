## UpdatePriceList

### 特徴
- 社内ツール
- 取引先ごとの価格表マスターデータを自動更新するプログラム。
- AS/400(IBM i Series) に接続、最新データを取得、マスターデータと照合し、マスターデータを更新する。
- C# .NET Framework 4.7.2
- コマンドラインアプリ、引数は設定ファイルのフルパス・ファイル名。
- データベースへの接続情報、SQL文等は設定ファイルに記述する為、コード内に情報は無い。