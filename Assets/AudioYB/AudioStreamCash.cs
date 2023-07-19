using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AudioYB
{
    [ExecuteInEditMode]
    public class AudioStreamCash : MonoBehaviour
    {
        #region serialized fields

        public bool       cash;
        public bool       dynamicCash = true;
        public List<ClipYB> infoList  = new();

        #endregion

        #region nonpublic members

        private        AudioClip[]     m_InfoListUnity;
        private static AudioStreamCash _instance;

        #endregion

        #region api

        public AudioClip this[int _Index] => infoList[_Index].cash;
        
        public static ClipYB Find(string _Name) => _instance.infoList.Find(_X => _X.Name == _Name);
        
        #endregion

        #region engine methods

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(this);
#if UNITY_EDITOR
            Listen();
            LoadLIstUnity();
#endif
#if !UNITY_EDITOR
            foreach (var item in infoList)
                item.ClearCash();
#endif
            if (cash)
                LoadCash();
        }

        #endregion

        #region nonpublic methods

        private void Listen()
        {
            infoList.Clear();
            LoadExt("mp3", AudioType.MPEG);
            LoadExt("wav", AudioType.WAV);
            LoadExt("ogg", AudioType.OGGVORBIS);
        }

        private void LoadLIstUnity()
        {
            m_InfoListUnity = Resources.LoadAll<AudioClip>("");
            foreach (var item in m_InfoListUnity) infoList.Add(new ClipYB(item, true));
        }

        private void LoadCash()
        {
            foreach (var item in infoList)
                StartCoroutine(item.GetFile());
        }

        private void LoadExt(string _Ext, AudioType _Type)
        {
            var dir = new DirectoryInfo(Application.streamingAssetsPath);
            var info = dir.GetFiles("*." + _Ext);
            foreach (var item in info)
            {
                if (Regex.IsMatch(item.Name, @"\p{IsCyrillic}"))
                {
                    Debug.LogError($"Переименуй {item.Name}");
                }
                var clip = new ClipYB(Application.streamingAssetsPath, item.Name, _Ext, _Type, cash || dynamicCash);
                infoList.Add(clip);
            }
        }

        #endregion
    }
}