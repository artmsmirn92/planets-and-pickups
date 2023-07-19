using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace AudioYB
{
    [Serializable]
    public class ClipYB
    {
        #region nonpublic members

        private bool m_UnityFile;

        #endregion

        #region api

        public string    name;
        public string    path;
        public string    ext;
        public AudioType type;

        public AudioClip cash;
        public bool      cashing;
        public bool      IsCashing => cash != null;

        public string Name => name;

        public ClipYB(string _Path, string _Name, string _Ext, AudioType _Type, bool _Cash)
        {
            ext = _Ext;
            name = _Name[..(_Name.Length - ext.Length - 1)];
            path = _Path;
            type = _Type;
            cashing = _Cash;
            m_UnityFile = false;
        }

        public ClipYB(AudioClip _Audio, bool _Cash)
        {
            cashing = _Cash;
            m_UnityFile = true;
            cash = _Audio;
            name = _Audio.name;
        }

        public IEnumerator GetFile(Action<AudioClip> _Action = null)
        {
            if (m_UnityFile || IsCashing)
            {
                _Action?.Invoke(cash);
            }
            else
            {
                string url = Application.streamingAssetsPath + "/" + Name + "." + ext;
                var request = UnityWebRequestMultimedia.GetAudioClip(url, type);
                request.SendWebRequest();
                while (!request.isDone)
                {
                    yield return null;
                }

                var newCash = DownloadHandlerAudioClip.GetContent(request);
                if (cashing)
                {
                    cash = newCash;
                    cash.name = Name;
                }

                _Action?.Invoke(newCash);
            }
        }

        public void ClearCash()
        {
            if (m_UnityFile) cash = null;
        }

        #endregion
    }
}