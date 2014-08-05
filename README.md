## 概要
- MacでPlayerPrefsデータの管理を行うエディタ拡張

## インストール方法
- current-build/unity-player-pref-editor-plugin.unitypackage をUnity Porject にimportする．以上

## 使い方
- Unity Editor の Window > Edit Player Prefs をクリックする．
- 現在登録されているPlayerPrefs のデータが key, value, type の順に並んでいるので，よしなに内容を変更し，Saveボタンを押す．
- データを新規に追加したい場合は, Addボタンを押す．
- 削除したい場合は，Deleteボタンを押す．(keyを空にしてSaveでも削除できます．)

## 備考
- PlistCSという外部ライブラリを使っています．もし他にも使っていた場合はどちらかのPlistCSを削除してください．

