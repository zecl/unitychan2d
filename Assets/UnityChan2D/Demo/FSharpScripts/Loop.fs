namespace UnityChan2D.Demo
open UnityEngine

type Loop () =
  inherit MonoBehaviour()
  [<DefaultValue>] val mutable public to' : Collider2D
  [<DefaultValue>] val mutable public offsetX : float32

  member this.OnTriggerEnter2D(other:Collider2D) =
    if other.tag <> "Player" then () else
    let pos = this.to'.transform.position
    other.transform.position <- new Vector3(pos.x + this.offsetX, other.transform.position.y, other.transform.position.z)

