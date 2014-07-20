using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class BrokenBlockController : ObservableMonoBehaviour
{
    public AudioClip breakClip;

    public Vector2 force = new Vector2(250, 1000);

    private Rigidbody2D[] rigidbody2Ds;
    private Transform[] transforms;
    public override void Awake()
    {
        this.StartAsObservable()
            .Subscribe(_ =>
            {
                IEnumerable<IGrouping<float, Rigidbody2D>> groupBy = rigidbody2Ds.GroupBy(r => r.transform.localPosition.y);

                foreach (IGrouping<float, Rigidbody2D> grouping in groupBy)
                {
                    foreach (var r in grouping)
                    {
                        r.AddForce(new Vector2(Mathf.Sign(r.transform.localPosition.x) * force.x, force.y + (100 * grouping.Key)));
                    }
                }
                AudioSourceController.instance.PlayOneShot(breakClip);
                Destroy(gameObject, 3);
            
            });

        base.Awake();
        rigidbody2Ds = GetComponentsInChildren<Rigidbody2D>();
    }
}
