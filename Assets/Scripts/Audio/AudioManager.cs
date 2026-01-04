using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] AudioSource[] _sfxAudio;
    [SerializeField] AudioSource[] _bgmAudio;

    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="sfxIndex"></param>
    public void PlaySFX(int sfxIndex)
    {
        if (sfxIndex < _sfxAudio.Length)
        {
            _sfxAudio[sfxIndex].Play();
        }
    }

    public void PlayBGM(int bgIndex)
    {
        StopAllBGM();
        if(bgIndex < _bgmAudio.Length)
        {
            _bgmAudio[bgIndex].Play();
        }
    }

    /// <summary>
    /// 停止所有音乐
    /// </summary>
    private void StopAllBGM()
    {
        foreach(var bgm in _bgmAudio)
        {
            bgm.Stop();
        }
    }
}
