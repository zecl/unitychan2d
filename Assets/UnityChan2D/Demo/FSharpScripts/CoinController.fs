namespace UnityChan2D.Demo
open UnityEngine

type CoinController () =
  inherit MonoBehaviour()

  [<DefaultValue>] val mutable public getCoin : AudioClip

  member public this.OnTriggerEnter2D(other:Collider2D) =
    if (other.tag = "Player") then
      PointController.Instance.AddCoin()
      AudioSourceController.instance.PlayOneShot(this.getCoin)
      MonoBehaviour.Destroy(this.gameObject)
