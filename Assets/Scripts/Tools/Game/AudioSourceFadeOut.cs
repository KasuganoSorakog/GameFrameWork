using UnityEngine;
using System.Collections;
using DG.Tweening;


public class AudioSourceFadeOut : MonoBehaviour 
{

    private AudioSource m_audioSource;

    private Tween FadeOutTween;
	

    public void StartFadeOut(float startVolume, float fadeTime)
    {
        m_audioSource = GetComponent<AudioSource>();
        //使音量从startVolum变化, 一直到0关闭
        FadeOutTween = DOTween.To(() => startVolume, x => FadeOutUpdate(x) , 0, fadeTime).OnComplete(FadeOutOver);
    }

    private void FadeOutUpdate(float value)
    {
        m_audioSource.volume = value;
    }

    private void FadeOutOver()
    {
        m_audioSource.Stop();
    }
}

 