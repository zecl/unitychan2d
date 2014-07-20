using System.Collections;
using UnityEngine;
using UniRx;

public class CoinController : ObservableMonoBehaviour
{
    public AudioClip getCoin;

    public override void Awake()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(other => other.tag == "Player")
            .Subscribe(other =>
            {
                PointController.instance.AddCoin();
                AudioSourceController.instance.PlayOneShot(getCoin);
                Destroy(gameObject);
            });

        base.Awake();
    }
}
