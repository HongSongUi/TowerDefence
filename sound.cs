//슬라이드 조작으로 소리 조절을 하기 위한 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class sound : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider BGMSlider;
    public Slider effectSlider;
    public Slider MasterSlider;
    // Start is called before the first frame update
    public void AudioControl()
    {
        float BGMsound = BGMSlider.value;
        float effectSound = effectSlider.value;
        float MasterSound = MasterSlider.value;
        if (BGMsound == -40f)
        {
            masterMixer.SetFloat("MainBgm",-80);
            masterMixer.SetFloat("GameBgm", -80);
        }
        else
        {
            masterMixer.SetFloat("GameBgm", BGMsound);
            masterMixer.SetFloat("MainBgm", BGMsound);
        }
        if (effectSound == -40f)
        {
            masterMixer.SetFloat("Effect", -80);
            
        }
        else
        {
            masterMixer.SetFloat("Effect", effectSound);
           
        }
        if (MasterSound == -40f)
        {
            masterMixer.SetFloat("Master", -80);

        }
        else
        {
            masterMixer.SetFloat("Master", MasterSound);

        }
    }
}
