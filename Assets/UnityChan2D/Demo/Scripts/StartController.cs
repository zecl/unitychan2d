using UnityEngine;
using System.Collections;
using UniRx;

[RequireComponent(typeof(AudioSource))]
public class StartController : ObservableMonoBehaviour
{
    [SceneName]
    public string nextLevel;

    [SerializeField]
    private KeyCode enter = KeyCode.X;

    public override void Awake()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(enter))
            .Subscribe(x => {
                StartCoroutine(LoadStage());
            });

        base.Awake();
    }

    private IEnumerator LoadStage()
    {
        foreach (AudioSource audioS in FindObjectsOfType<AudioSource>())
        {
            audioS.volume = 0.2f;
        }

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1;

        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);
        Application.LoadLevel(nextLevel);
    }
}
