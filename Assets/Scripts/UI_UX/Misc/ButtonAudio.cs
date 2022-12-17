using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    public void PlayClickAudio()
    {
        AudioManager.instance?.PlaySoundEffect(SFX.BUTTON_CLICK);
    }
}
