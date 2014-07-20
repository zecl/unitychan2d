using UnityEngine;
using UniRx;

public class Loop : ObservableMonoBehaviour
{
    public Collider2D to;
    public float offsetX;

    public override void Awake()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(other => other.tag != "Player")
            .Subscribe(other =>
            {
                var pos = to.transform.position;
                other.transform.position = new Vector2(pos.x + offsetX, other.transform.position.y);
            });

        base.Awake();
    }
}
