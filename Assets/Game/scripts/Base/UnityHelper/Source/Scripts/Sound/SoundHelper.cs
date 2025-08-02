using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnityHelper
{
    public class SoundHelper : MonoSingleton<SoundHelper>, IDisposable
    {
        [Serializable]
        public class Audio
        {
            public AudioSource source = null;
            public float oriVolume { get; set; }
        }
        [SerializeField] Audio m_bgm = new Audio();
        [SerializeField] Audio m_shareSfx = new Audio();
        [SerializeField] List<Audio> m_singleSfxes = new List<Audio>();
        [SerializeField] float m_bgmFadeInOutTime = 3.0f;
        [SerializeField] float m_sfxFadeInOutSpeed = 1.0f;

        private int m_playBgmId = 0;
        private AudioClip m_bgmClip = null;
        private Dictionary<int, AudioClip> m_sfxClips = new Dictionary<int, AudioClip>();
        private float m_sfxVolume = 1.0f;
        private float m_bgmVolume = 1.0f;
        private float m_oldSfxVolume = 0.0f;
        private float m_oldBgmVolume = 0.0f;
        private Coroutine m_coPlayBgm = null;
        private Coroutine m_coPlaySfxCombine = null;

        public int playBgmId => m_playBgmId;
        public float sfxVolume => m_sfxVolume;
        public float bgmVolume => m_bgmVolume;
        public float bgmLength => (null == m_bgmClip) ? 0.0f : m_bgmClip.length;

        protected override void Awake()
        {
            base.Awake();

            m_oldBgmVolume = m_bgm.source.volume;
        }

        public virtual void initialize()
        {

        }

        protected virtual string getClipPath(int soundId)
        {
            if (Logx.isActive)
                Logx.error("Need override getClipPath");

            return null;
        }

        protected virtual string getClipName(int soundId)
        {
            if (Logx.isActive)
                Logx.error("Need override getClipName");

            return null;
        }

        protected virtual float getVolume(int soundId)
        {
            return 1.0f;
        }

        protected virtual bool isLoop(int soundId)
        {
            if (Logx.isActive)
                Logx.error("Need override isLoop");

            return false;
        }

        private Audio findAudio(string clipName)
        {
            if (string.IsNullOrEmpty(clipName))
                return null;

            foreach (var audio in m_singleSfxes)
            {
                if (null == audio.source.clip)
                    continue;

                if (audio.source.clip.name.Equals(clipName))
                    return audio;
            }

            return null;
        }

        public bool isPlayingSingleSfx()
        {
            foreach (var audio in m_singleSfxes)
            {
                if (!audio.source.isPlaying || null == audio.source.clip)
                    return false;
            }

            return true;
        }

        protected int getPlayingSingleSfxCount(string clipName)
        {
            int count = 0;
            foreach (var audio in m_singleSfxes)
            {
                if (audio.source.isPlaying && null != audio.source.clip)
                {
                    if (audio.source.clip.Equals(clipName))
                        ++count;
                }
            }

            return count;
        }

        public string getSinglePlayingSfxNames()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_singleSfxes.Count; ++i)
            {
                var audio = m_singleSfxes[i];
                if (audio.source.isPlaying && null != audio.source.clip)
                {
                    sb.Append(audio.source.clip.name);
                    if (i != m_singleSfxes.Count - 1)
                        sb.Append(",");
                }
            }

            return sb.ToString();
        }

        private Audio findEmptySfxAudio(bool isForce)
        {
            if (0 == m_singleSfxes.Count)
            {
                if (Logx.isActive)
                    Logx.error("Failed findSingleSfxSource, empty list");

                return null;
            }

            foreach (var audio in m_singleSfxes)
            {
                if (!audio.source.isPlaying || null == audio.source.clip)
                    return audio;
            }

            if (isForce)
                return m_singleSfxes[0];

            if (Logx.isActive)
                Logx.warn("sfx sound is all plyaing!");

            return null;
        }

        private AudioClip getSfxClip(int soundId)
        {
            AudioClip clip;
            if (m_sfxClips.TryGetValue(soundId, out clip))
                return clip;

            clip = loadClip(soundId);
            if (null != clip)
                m_sfxClips.Add(soundId, clip);

            return clip;
        }

        private AudioClip getBgmClip(int soundId)
        {
            return loadClip(soundId);
        }

        private AudioClip loadClip(int soundId)
        {
            string clipPath = getClipPath(soundId);
            AudioClip audioClip = Resources.Load<AudioClip>(clipPath);

            if (null == audioClip)
            {
                if (Logx.isActive)
                    Logx.error("Failed load audio clip {0}", clipPath);
            }

            return audioClip;
        }

        public AudioClip playShare(int soundId)
        {
            if (0 == soundId)
                return null;

            if (null == m_shareSfx)
                return null;

            m_shareSfx.oriVolume = getVolume(soundId);

            AudioClip clip = getSfxClip(soundId);
            m_shareSfx.source.loop = false;
            m_shareSfx.source.PlayOneShot(clip, m_shareSfx.oriVolume * m_sfxVolume);

            return clip;
        }

        public AudioClip playSingle(int soundId, bool isForce = false, float customVolume = 1.0f)
        {
            if (0 == soundId)
                return null;

            return playSingle(soundId, isLoop(soundId), isForce, customVolume);
        }

        protected AudioClip playSingle(int soundId, bool isLoop, bool isForce = false, float customVolume = 1.0f)
        {
            if (0 == soundId)
                return null;

            var clipName = getClipName(soundId);
            var audio = findAudio(clipName);

            if (null != audio && audio.source.isPlaying)// TODO : 2024-07-26 by pms //같은 소리 중복 재생 차단
                return null;

            audio = findEmptySfxAudio(isForce);

            if (null == audio)
                return null;

            audio.oriVolume = getVolume(soundId) * customVolume;

            AudioClip clip = getSfxClip(soundId);
            audio.source.loop = isLoop;
            audio.source.volume = audio.oriVolume * m_sfxVolume;
            audio.source.clip = clip;
            audio.source.Play();

            return clip;
        }

        public void playCombine(List<int> soundIds, float combineDelay, float endDelay, float sfxVolume, Action cbEnd = null)
        {
            if (0 == soundIds.Count)
                return;

            var audio = findEmptySfxAudio(true);
            if (null == audio)
                return;

            stopCombine();

            m_coPlaySfxCombine = StartCoroutine(coPlayCombine(audio, soundIds, combineDelay, endDelay, sfxVolume, cbEnd));
        }

        public void stopCombine(List<int> soundIds = null)
        {
            if (null != soundIds)
            {
                foreach (var sfxId in soundIds)
                    stop((int)sfxId);
            }

            if (m_coPlaySfxCombine != null)
            {
                StopCoroutine(m_coPlaySfxCombine);
                m_coPlaySfxCombine = null;
            }
        }

        public void stop(int soundId, bool isFadeIn = false)
        {
            var clipName = getClipName(soundId);
            var audio = findAudio(clipName);
            if (null == audio)
                return;

            if (isFadeIn)
            {
                StartCoroutine(coStopFadeIn(audio));
            }
            else
            {
                audio.source.Stop();
                audio.source.clip = null;
            }
        }

        protected IEnumerator coStopFadeIn(Audio audio)
        {
            var volume = audio.source.volume;

            while (0.0f < volume)
            {
                float s = m_sfxFadeInOutSpeed * Time.deltaTime;
                volume -= s;
                if (0.0f > volume)
                    volume = 0.0f;

                audio.oriVolume = volume;
                audio.source.volume = volume * sfxVolume;

                yield return null;
            }

            audio.source.Stop();
            audio.source.clip = null;
        }

        protected IEnumerator coPlayFadeOut(Audio audio, int soundId)
        {
            var curVolume = 0.0f;

            AudioClip clip = getSfxClip(soundId);
            audio.source.loop = isLoop(soundId);
            audio.source.volume = curVolume;
            audio.source.clip = clip;
            audio.source.Play();

            var volume = getVolume(soundId);
            while (curVolume < volume)
            {
                float s = m_sfxFadeInOutSpeed * Time.deltaTime;
                curVolume += s;
                if (curVolume > volume)
                    curVolume = volume;

                audio.oriVolume = curVolume;
                audio.source.volume = curVolume * sfxVolume;

                yield return null;
            }
        }

        public bool isPlayingLoop(int soundId)
        {
            var clipName = getClipName(soundId);
            var audio = findAudio(clipName);
            if (null == audio)
                return false;

            if (!audio.source.isPlaying)
                return false;

            return audio.source.loop;
        }

        public bool isPlaying(int soundId)
        {
            var clipName = getClipName(soundId);
            var audio = findAudio(clipName);
            if (null == audio)
                return false;

            return audio.source.isPlaying;
        }

        public void playBgm(int soundId)
        {
            if (0 == soundId)
                return;

            if (m_playBgmId == soundId)
                return;

            if (null != m_coPlayBgm)
            {
                StopCoroutine(m_coPlayBgm);
                m_coPlayBgm = null;
            }
            m_coPlayBgm = StartCoroutine(coPlayBgm(soundId));
        }

        IEnumerator coPlayBgm(int soundId)
        {
            yield return StartCoroutine(coFadeInBgm());

            if (Logx.isActive)
                Logx.trace("PlayBgm {0}", soundId);

            m_playBgmId = soundId;
            m_bgmClip = getBgmClip(soundId);

            m_bgm.oriVolume = getVolume(soundId);
            float volume = m_bgm.oriVolume * m_bgmVolume;

            if (isLoop(soundId))
            {
                m_bgm.source.clip = m_bgmClip;
                m_bgm.source.volume = volume;
                m_bgm.source.Play();
            }
            else
            {
                m_bgm.source.Stop();
                m_bgm.source.PlayOneShot(m_bgmClip, volume);
            }

            yield return StartCoroutine(coFadeOutBgm(volume));
        }

        IEnumerator coFadeInBgm()
        {
            if (!m_bgm.source.isPlaying)
                yield break;
            if (0.0f >= m_bgmFadeInOutTime)
                yield break;

            float curVolume = m_bgm.source.volume;
            float fadeInOutSpeed = curVolume / m_bgmFadeInOutTime;
            while (0.0f < curVolume)
            {
                float s = fadeInOutSpeed * Time.smoothDeltaTime;
                curVolume -= s;
                curVolume = Mathf.Max(0.0f, curVolume);
                m_bgm.source.volume = curVolume;
                yield return null;
            }

            m_bgm.source.Stop();
        }

        IEnumerator coFadeOutBgm(float maxVolume)
        {
            if (0.0f >= m_bgmFadeInOutTime)
            {
                yield break;
            }
            m_bgm.source.Play();
            m_bgm.source.volume = 0.0f;

            float curVolume = 0.0f;
            float fadeInOutSpeed = maxVolume / m_bgmFadeInOutTime;
            while (maxVolume > curVolume)
            {
                float s = fadeInOutSpeed * Time.smoothDeltaTime;
                curVolume += s;
                curVolume = Mathf.Min(maxVolume, curVolume);
                m_bgm.source.volume = curVolume;
                yield return null;
            }

            m_bgm.source.volume = maxVolume;
        }

        IEnumerator coPlayCombine(Audio audio, List<int> soundIds, float combineDelay, float endDelay, float sfxVolume, Action cbEnd = null)
        {
            foreach (var sId in soundIds)
            {
                audio.oriVolume = getVolume(sId);

                AudioClip clip = getSfxClip(sId);
                audio.source.loop = false;
                audio.source.clip = clip;
                audio.source.volume = audio.oriVolume * sfxVolume;
                audio.source.Play();

                float length = clip.length + combineDelay;

                yield return new WaitForSeconds(length);
            }

            if (0.0f < endDelay)
                yield return new WaitForSeconds(endDelay);

            cbEnd?.Invoke();
        }

        public void fadeInBgm()
        {
            m_playBgmId = 0;

            StartCoroutine(coFadeInBgm());

            if (null != m_coPlayBgm)
            {
                StopCoroutine(m_coPlayBgm);
                m_coPlayBgm = null;
            }
        }

        private void unload(ref AudioClip clip)
        {
            if (null == clip)
                return;

            clip.UnloadAudioData();
            //AudioClip.Destroy(clip);
            clip = null;
        }

        public void setPauseSfx(bool isPause)
        {
            if (isPause)
            {
                m_oldSfxVolume = m_sfxVolume;
                m_sfxVolume = 0.0f;
            }
            else
            {
                m_sfxVolume = m_oldSfxVolume;
            }

            foreach (var audio in m_singleSfxes)
            {
                if (audio.source.isPlaying)
                    audio.source.volume = m_sfxVolume;
            }

            if (m_shareSfx.source.isPlaying)
                m_shareSfx.source.volume = m_sfxVolume;
        }

        public void setPauseBgm(bool isPause)
        {
            if (isPause)
            {
                m_oldBgmVolume = m_bgmVolume;
                m_bgmVolume = 0.0f;
            }
            else
            {
                m_bgmVolume = m_oldBgmVolume;
            }

            m_bgm.source.volume = m_bgmVolume;
        }

        public virtual void setBgmVolume(float volume)
        {
            m_bgmVolume = volume;
            m_bgm.source.volume = m_bgm.oriVolume * volume;
        }

        public virtual void setSfxVolume(float volume)
        {
            m_sfxVolume = volume;

            foreach (var audio in m_singleSfxes)
            {
                if (audio.source.isPlaying)
                    audio.source.volume = audio.oriVolume * volume;
            }

            if (null == m_shareSfx.source)
                return;

            if (m_shareSfx.source.isPlaying)
                m_shareSfx.source.volume = m_shareSfx.oriVolume * volume;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                /* 씬 전환 할 때 사운드가 끊겨 들려서 주석 처리함
                 * 릭이 발생되는지 확인 해보긴 해야 된다.
                if (null != m_bgm)
                    m_bgm.Stop();

                unload(ref m_bgmClip);

                m_playBgmId = 0;
                */

                foreach (var audio in m_singleSfxes)
                {
                    audio.source.Stop();
                    audio.source.clip = null;
                }

                if (null != m_shareSfx)
                {
                    m_shareSfx.source.Stop();
                    m_shareSfx.source.clip = null;
                }

                foreach (var pair in m_sfxClips)
                {
                    pair.Value.UnloadAudioData();
                    //AudioClip.Destroy(pair.Value);
                }

                m_sfxClips.Clear();
            }
        }
    }
}