using UnityEngine;
using UniRx;

[RequireComponent(typeof(UnityChan2DController), typeof(AudioSource))]
public class Demo : ObservableMonoBehaviour
{
    [SerializeField]
    private AudioClip damageVoice;

    [SerializeField]
    private AudioClip[] jumpVoices;

    [SerializeField]
    private AudioClip clearVoice;

    [SerializeField]
    private AudioClip timeOverVoice;

    private AudioSource audioSource;

    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    void OnDamage()
    {
        PlayVoice(damageVoice);
    }

    void Jump()
    {
        int i = Random.Range(0, jumpVoices.Length);
        PlayVoice(jumpVoices[i]);
    }

    void Clear()
    {
        PlayVoice(clearVoice);
    }

    void TimeOver()
    {
        PlayVoice(timeOverVoice);
    }

    void PlayVoice(AudioClip voice)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(voice);
    }
}