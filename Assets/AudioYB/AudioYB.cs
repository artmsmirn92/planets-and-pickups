//2.2

using System.Collections;
using mazing.common.Runtime;
using UnityEngine;

namespace AudioYB
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioYb : MonoBehaviour
    {
        #region serialized fields

        public bool pause;
        
        #endregion

        #region nonpublic members

        private bool        m_PlayLoop;
        private AudioSource m_Source;
        private bool        m_Load;
        private bool        m_Play;
        private bool        m_Focus;
        private float       m_PausedTime;

        #endregion

        #region engine methods

        private void Awake()
        {
            m_Source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (m_Source.loop)
            { 
                m_Source.loop = false; 
                Loop = true; 
            }
            SetLoop(Loop);      
        }

        #endregion


        #region api
        
        public bool   IsPlaying   => m_Source.isPlaying;
        public bool   Focus       => m_Focus;
        public bool   Loop        { get;                         set; }
        public float  Volume      { get => m_Source.volume;      set => m_Source.volume = value; }
        public bool   Mute        { get => m_Source.mute;        set => m_Source.mute = value; }
        public float  Time        { get => m_Source.time;        set => m_Source.time = value; }
        public bool   Enabled     { get => m_Source.enabled;     set => m_Source.enabled = value; }
        public string Clip        { get => m_Source.clip.name;   set => m_Source.clip = AudioStreamCash.Find(value).cash; }
        public int    TimeSamples { get => m_Source.timeSamples; set => m_Source.timeSamples = value; }

        public float ClipLength()
        {
            float length;
            if (m_Source.clip != null)
            {
                length = m_Source.clip.length;
            }
            else length = 0f;
            return length;
        }

        public IEnumerator EndFile(string _Name)
        {
            yield return new WaitForSeconds(0.1f);
            if (m_Source.time > 0f)
            {
                SourceTime();
                ZeroTime();
            }
        
            yield return new WaitForSeconds(0.02f);

            var clipYb = AudioStreamCash.Find(_Name);
            if (clipYb == null) 
                Dbg.LogError($"Клип нулевой - {_Name}");
            m_Load = false;
            m_Play = true;
            if (clipYb != null)
                StartCoroutine(clipYb.GetFile(LoadAfter));
        }
        
        public void Play(string _Name)
        {
            StartCoroutine(EndFile(_Name));
        }
        
        public void Play()
        {
            if (m_Load) 
            {
                m_Source.Play();
                m_PlayLoop = true;
            }
            else m_Play = true;
        }


        public void PlayOneShot(string _ClipName, float _VolumeScale = 1f)
        {
            var clip = AudioStreamCash.Find(_ClipName);
            if (clip == null)
            {
                Debug.LogError("Íåò òàêîãî êëèïà! Ïðîâåðü íàçâàíèå " + "Name:" + " " + name);
                Debug.Break();
                return;
            }
            StartCoroutine(clip.GetFile(delegate (AudioClip _AudioClip) { LoadShotAfter(_AudioClip, _VolumeScale); }));
        }
        
        public void Pause()
        {
            pause = true;
            if(m_Source.time !=0) m_PausedTime = m_Source.time;
            SourceTime();
        }

        public void UnPause()
        {
            if (m_PausedTime != 0f)
            {
                pause = false;
                m_Source.time = m_PausedTime;
                m_PausedTime = 0;
                m_Source.Play();
            }
            else
            {
                pause = false;
                m_Source.Play();
            }
       
        }
        public void Stop()
        {
            m_PlayLoop = false;
            SourceTime();
        }

        #endregion

        #region nonpublic methods

        private void PlayAfter()
        {
            m_Play = false;
            m_PlayLoop = true;
            ZeroTime();
            m_Source.Play();
        }

        private void LoadAfter(AudioClip _Clip)
        {
            m_Source.clip = _Clip;
            m_Load = true;
            if (m_Play) PlayAfter();
        }
        private void ZeroTime()
        {
            m_Source.time = 0f;
            m_PausedTime = 0f;
        }
        
        private void LoadShotAfter(AudioClip _Clip, float _VolumeScale)
        {
            m_Source.PlayOneShot(_Clip, _VolumeScale);
        }
        private void SetLoop(bool _Enable)
        {
            if (m_Source.time != 0f || !_Enable || !m_PlayLoop || pause || !m_Focus)
                return;
            ZeroTime(); 
            m_Source.Play();
        }
        
        private void SourceTime()
        {
            if(m_Source.clip != null) m_Source.time = m_Source.clip.length - 0.01f;
        }

        #endregion

        
        private void OnApplicationFocus(bool _Focus)
        {
            m_Focus = _Focus;
        }
    }
}

