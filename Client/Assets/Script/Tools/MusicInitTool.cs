using UnityEngine;

public class MusicInitTool : MonoBehaviour
{
    void Start()
    {
        if (AudioController.DoesInstanceExist() != null)
        {
            AudioController.SetCategoryVolume("Music", PlayerPrefsUtils.GetMusicState() ? 1 : 0.00001f);
            AudioController.SetCategoryVolume("Sound", PlayerPrefsUtils.GetSoundState() ? 1 : 0.00001f);
        }
    }

}
