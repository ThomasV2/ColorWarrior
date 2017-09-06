using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsManager : MonoBehaviour {

    [SerializeField]
    private Image musicMute;
    [SerializeField]
    private Sprite imgMusicOn;
    [SerializeField]
    private Sprite imgMusicOff;
    private bool mMute;

    [SerializeField]
    private Image sfxMute;
    [SerializeField]
    private Sprite imgSFXOn;
    [SerializeField]
    private Sprite imgSFXOff;
    private bool sMute;
    


    public void MusicToggle()
    {
        mMute = !mMute;
        musicMute.sprite = (mMute ? imgMusicOff : imgMusicOn);
    }

    public void SFXToggle()
    {
        sMute = !sMute;
        sfxMute.sprite = (sMute ? imgSFXOff : imgSFXOn);
    }
}
