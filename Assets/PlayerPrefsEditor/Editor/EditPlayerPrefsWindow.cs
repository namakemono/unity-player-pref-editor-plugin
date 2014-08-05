using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PlayerPrefsEditor
{
    class PlayerPrefDataForEdit
    {
        public bool enabled = true;
        public string key = "";         // PlayerPrefのkey
        public PlayerPrefsType type = PlayerPrefsType.String;    // PlayerPrefのDataの型
        public int intValue;            // 型がintの場合の代入値
        public float floatValue;        // 型がfloatの場合の代入値
        public string strValue;         // 型がstringの場合の代入値
        public string tempValue = "";   // Editorで入力するために使う一時的な値
    }

    public enum PlayerPrefsType{
        Integer,
        Float,
        String
    }
    
    public class EditPlayerPrefsWindow : EditorWindow
    {
        static int textFieldWidthForKey = 200;
        static int textFieldWidthForValue = 400;
        bool needRefresh = true;
        Dictionary<string, PlayerPrefDataForEdit> dict = new Dictionary<string, PlayerPrefDataForEdit> ();
        Vector2 scrollPosition = Vector2.zero;
        #if UNITY_EDITOR_OSX
        [MenuItem ("Window/Edit Player Prefs")]
        static void Init ()
        {
            EditPlayerPrefsWindow window = (EditPlayerPrefsWindow)EditorWindow.GetWindow (typeof(EditPlayerPrefsWindow));
        }
        #endif

        Dictionary<string, PlayerPrefDataForEdit> Load ()
        {
            var res = new Dictionary<string, PlayerPrefDataForEdit> ();
            foreach (var key in PlayerPrefsSupport.GetAllKeys ()) {
                var data = new PlayerPrefDataForEdit ();
                data.key = key;
                data.strValue = PlayerPrefs.GetString (key);
                data.intValue = PlayerPrefs.GetInt (key);
                data.floatValue = PlayerPrefs.GetFloat (key);
                if (data.strValue.Length > 0) {
                    data.type = PlayerPrefsType.String;
                    data.tempValue = data.strValue;
                } else if (data.floatValue != 0) {
                    data.type = PlayerPrefsType.Float;
                    data.tempValue = data.floatValue.ToString ();
                } else {
                    data.type = PlayerPrefsType.Integer;
                    data.tempValue = data.intValue.ToString ();
                }
                res [key] = data;
            }
            return res;
        }
        
        void Save (Dictionary<string, PlayerPrefDataForEdit> dict)
        {
            PlayerPrefs.DeleteAll ();
            foreach (var data in dict.Values) {
                if (data.key.Length == 0) {
                    data.enabled = false;
                    continue;
                }
                if (!data.enabled) continue;
                if (data.type == PlayerPrefsType.Integer) {
                    PlayerPrefs.SetInt (data.key, data.intValue);
                } else if (data.type == PlayerPrefsType.Float) {
                    PlayerPrefs.SetFloat (data.key, data.floatValue);
                } else if (data.type == PlayerPrefsType.String) {
                    PlayerPrefs.SetString (data.key, data.strValue);
                }
            }
            PlayerPrefs.Save ();
        }

        void OnGUI ()
        {
            if (needRefresh) {
                dict = Load();
                needRefresh = false;
            }
            bool isValid = true;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(1000), GUILayout.Height(400));
            foreach (PlayerPrefDataForEdit data in dict.Values) {
                if (!data.enabled) continue;
                GUILayout.BeginHorizontal ();
                data.key = EditorGUILayout.TextField (data.key, GUILayout.Width(textFieldWidthForKey));    // key
                data.tempValue = GUILayout.TextField (data.tempValue, GUILayout.Width(textFieldWidthForValue)); // value
                data.type = (PlayerPrefsType)EditorGUILayout.EnumPopup(data.type); // type
                // Parse value
                if (data.type == PlayerPrefsType.String) {
                    data.strValue = data.tempValue;
                } else if (data.type == PlayerPrefsType.Float) {
                    isValid &= float.TryParse (data.tempValue, out data.floatValue);
                } else {
                    isValid &= int.TryParse (data.tempValue, out data.intValue);
                }
                if (GUILayout.Button("Delete")) {
                    data.enabled = false;
                    if (PlayerPrefs.HasKey(data.key)) {
                        PlayerPrefs.DeleteKey(data.key);
                        PlayerPrefs.Save();
                    }
                }
                GUILayout.EndHorizontal ();
            }
            GUILayout.EndScrollView ();
            if (isValid) {
                if (GUILayout.Button ("Save")) {
                    Save(dict);
                }
            } else {
                GUILayout.Label("There are some invalid values. Please modify them." );
            }

            if (GUILayout.Button ("Add")) {
                dict["new-record-" + dict.Count] = new PlayerPrefDataForEdit();    
            }
        }
    }
}