using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 *Copyright(C) 2021 by DefaultCompany 
 *All rights reserved. 
 *作者:         Sora 
 *Unity版本：   2019.4.9f1 
 *日期:         2021-05-24 
 *说明:    音效管理器:所有音效，音乐都放到此处管理
 * 用法：1. 创建一个空物体，挂载该脚本，并将需要的音乐/音效拖到数组中
 *       2. 在GameAudioType的枚举中，定义好调用的名字；其对应关系为数组的下标。在拖入音乐的时候要注意顺序
 *       3. 使用GameAudioManager.PlayOnceAudio()/PlayLoopAudio()播放音乐；使用GameAudioManager.StopLoopAudio()/StopOnceAudio()停止音乐;
**/

namespace MyDemo
{
    public enum GameAudioType
    {
        test1,
        test2,
        test3
    }

    public class GameAudioManager : MonoBehaviour
    {
        //在外部放入音乐
        public AudioClip[] m_audio;

        private Dictionary<GameAudioType, AudioClip> m_audioDic;

        private Dictionary<string, AudioSource> m_audioSourceList = new Dictionary<string, AudioSource>();

        private static GameAudioManager m_instance;
        public static GameAudioManager Instance()
        {
            return m_instance;
        }

        void Awake()
        {
            m_instance = this;
            AudioListener listener = gameObject.GetComponent<AudioListener>();
            if (listener == null)
            {
                listener = gameObject.AddComponent<AudioListener>();
            }

            m_audioDic = new Dictionary<GameAudioType, AudioClip>();

            for (int i = 0; i < m_audio.Length; i++)
            {
                m_audioDic.Add((GameAudioType)i, m_audio[i]);
            }
        }

        //播放一次
        public void PlayOnceAudio(GameAudioType type, float delayTime = 0, float volume = 1f, float pitch = 1f)
        {
            if (delayTime == 0)
            {

                GameObject myAudio = new GameObject("OnceAudio");
                myAudio.transform.SetParent(transform);
                myAudio.transform.localPosition = Vector3.zero;
                AudioSource mySource = myAudio.AddComponent<AudioSource>();
                if (mySource != null)
                {
                    mySource.loop = false;
                    mySource.clip = m_audioDic[type];
                    mySource.volume = volume;
                    mySource.Play();
                    Destroy(myAudio, mySource.clip.length);
                }
            }
            else
            {
                StartCoroutine(PlayOnceAudioDelay(type, delayTime, volume, pitch));
            }
        }

        private IEnumerator PlayOnceAudioDelay(GameAudioType type, float delayTime = 0, float volume = 1f, float pitch = 1f)
        {
            yield return new WaitForSeconds(delayTime);
            GameObject myAudio = new GameObject("Audio");
            myAudio.transform.SetParent(transform);
            myAudio.transform.localPosition = Vector3.zero;
            AudioSource mySource = myAudio.AddComponent<AudioSource>();
            if (mySource != null)
            {
                mySource.loop = false;
                mySource.clip = m_audioDic[type];
                mySource.volume = volume;
                mySource.Play();
                Destroy(myAudio, mySource.clip.length);
            }
        }

        //循环播放
        public void PlayLoopAudio(GameAudioType type, float delayTime = 0, float volume = 1f, float pitch = 1f)
        {
            if (delayTime == 0)
            {
                if (!m_audioSourceList.ContainsKey(type.ToString()))
                {
                    GameObject myAudio = new GameObject("LoopAudio");
                    myAudio.transform.SetParent(transform);
                    myAudio.transform.localPosition = Vector3.zero;
                    AudioSource mySource = myAudio.AddComponent<AudioSource>();
                    if (mySource != null)
                    {
                        mySource.loop = true;
                        mySource.clip = m_audioDic[type];
                        mySource.volume = volume;
                        mySource.Play();
                        m_audioSourceList.Add(type.ToString(), mySource);
                    }
                }
                else
                {
                    AudioSource mySource = m_audioSourceList[type.ToString()];
                    if (mySource != null)
                    {
                        mySource.loop = true;
                        mySource.volume = volume;
                        mySource.Play();
                    }
                }
            }
            else
            {
                m_delayCoroutine = StartCoroutine(PlayLoopAudioDelay(type, delayTime, volume, pitch));
                m_delayType = type;
            }
        }


        private IEnumerator PlayLoopAudioDelay(GameAudioType type, float delayTime = 0, float volume = 1f, float pitch = 1f)
        {
            yield return new WaitForSeconds(delayTime);
            if (!m_audioSourceList.ContainsKey(type.ToString()))
            {
                GameObject myAudio = new GameObject("LoopAudio");
                myAudio.transform.SetParent(transform);
                myAudio.transform.localPosition = Vector3.zero;
                AudioSource mySource = myAudio.AddComponent<AudioSource>();
                if (mySource != null)
                {
                    mySource.loop = true;
                    mySource.clip = m_audioDic[type];
                    mySource.volume = volume;
                    mySource.pitch = pitch;
                    mySource.Play();
                    m_audioSourceList.Add(type.ToString(), mySource);
                }
            }
            else
            {
                AudioSource mySource = m_audioSourceList[type.ToString()];
                if (mySource != null)
                {
                    mySource.loop = true;
                    mySource.volume = volume;
                    mySource.pitch = pitch;
                    mySource.Play();
                }
            }
        }


        private Coroutine m_delayCoroutine;
        private GameAudioType m_delayType;
        //停止音乐播放
        public void StopLoopAudio(GameAudioType type, bool fade)
        {
            if (m_delayType == type && m_delayCoroutine != null)
            {
                StopCoroutine(m_delayCoroutine);
                m_delayCoroutine = null;
            }

            if (m_audioSourceList.ContainsKey(type.ToString()))
            {
                if (m_audioSourceList[type.ToString()] == null)
                {
                    m_audioSourceList.Remove(type.ToString());
                }

                else
                {
                    if (fade)
                    {
                        float fromValue = m_audioSourceList[type.ToString()].volume;
                        AudioSourceFadeOut fadeOut = m_audioSourceList[type.ToString()].GetComponent<AudioSourceFadeOut>();
                        if (fadeOut == null)
                        {
                            fadeOut = m_audioSourceList[type.ToString()].gameObject.AddComponent<AudioSourceFadeOut>();
                        }
                        fadeOut.StartFadeOut(fromValue, 1f);
                    }
                    else
                    {
                        m_audioSourceList[type.ToString()].Stop();
                    }
                    Destroy(m_audioSourceList[type.ToString()].gameObject, 1f);
                    m_audioSourceList.Remove(type.ToString());
                }
            }
        }

        //停止音效播放
        public void StopOnceAudio(GameAudioType type)
        {
            AudioSource[] sources = GetComponentsInChildren<AudioSource>();
            for (int i = 0; i < sources.Length; i++)
            {
                AudioClip clip = m_audioDic[type];
                if (sources[i].clip == clip)
                {
                    Destroy(sources[i].gameObject);
                }
            }
        }
    }

}
