using UnityEngine;
using System.Collections;
using UniRx;
using System.Threading;

[RequireComponent(typeof(AudioSource))]
public class IntroCameraController : ObservableMonoBehaviour
{
    public Transform target;

    private Vector3 pos;

    [SceneName]
    public string nextLevel;
    private bool next = false;

    public override void Awake()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
                {
                    float newPosition = Mathf.SmoothStep(pos.x, target.position.x, Time.timeSinceLevelLoad / audio.clip.length);
                    transform.position = new Vector3(newPosition, pos.y, pos.z);
                });

        this.UpdateAsObservable()
            .Where(_ => next)
            .Subscribe(_ => Application.LoadLevel(nextLevel));

        var interval = audio.clip.length + 1;
        var wait = Observable.Start(() => Thread.Sleep(System.TimeSpan.FromSeconds(interval)));
        this.StartAsObservable()
            .Subscribe(_ => 
            {
                pos = transform.position;
                wait.Subscribe(x => next = true);
            });

        base.Awake();
    }
}
