namespace UnityChan2D.Demo
open System.Collections
open UnityEngine

[<RequireComponent(typeof<AudioSource>)>]
type public IntroCameraController () =
  inherit MonoBehaviour()
  [<DefaultValue>] val mutable target : Transform
  let mutable pos : Vector3 = Vector3.zero

  [<SceneName>]
  [<DefaultValue>] val mutable public nextLevel : string

  member this.Start () = 
    pos <- this.transform.position
    seq {
      yield WaitForSeconds(this.audio.clip.length + 1.f)
      Application.LoadLevel(this.nextLevel)
    } :?> IEnumerator

  member this.Update () = 
    let newPosition = Mathf.SmoothStep(pos.x, this.target.position.x, Time.timeSinceLevelLoad / this.audio.clip.length)
    this.transform.position <- new Vector3(newPosition, pos.y, pos.z)
