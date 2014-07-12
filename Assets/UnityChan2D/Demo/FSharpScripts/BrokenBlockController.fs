namespace UnityChan2D.Demo
open System.Linq 
open UnityEngine

type BrokenBlockController () =
    inherit MonoBehaviour()

    [<DefaultValue>] val mutable public breakClip : AudioClip
    [<DefaultValue>] val mutable public force :Vector2 
    [<DefaultValue>] val mutable private rigidbody2Ds : Rigidbody2D[]
    [<DefaultValue>] val mutable private transforms : Transform[]

    member this.Awake () = 
      this.force <- new Vector2(250.f, 1000.f)
      this.rigidbody2Ds <- this.GetComponentsInChildren<Rigidbody2D>()

    member this.Start () = 
      let groupBy = this.rigidbody2Ds.GroupBy(fun r -> r.transform.localPosition.y)

      for grouping in groupBy do
        for r in grouping do
          r.AddForce(new Vector2(Mathf.Sign(r.transform.localPosition.x) * this.force.x, this.force.y + (100.f * grouping.Key)))
      AudioSourceController.instance.PlayOneShot(this.breakClip)
      MonoBehaviour.Destroy(this.gameObject, 3.f)
