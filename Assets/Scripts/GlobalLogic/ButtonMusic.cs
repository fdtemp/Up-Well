using UnityEngine;

class ButtonMusic : MonoBehaviour {
    public void PlayButtonAudio() {
        AudioClip ac = Resources.Load("Decision2", typeof(AudioClip)) as AudioClip;
        AudioSource.PlayClipAtPoint(ac, Vector3.zero);
    }
}