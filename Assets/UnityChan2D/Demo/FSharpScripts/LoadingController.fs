namespace UnityChan2D.Demo
open System.Collections
open UnityEngine

type LoadingController () =
  inherit MonoBehaviour()

  [<SceneName>]
  [<DefaultValue>] val mutable public nextLevel : string

  member this.Start () = 
    seq {
      yield WaitForSeconds(3.f)
      Application.LoadLevel(this.nextLevel)
    } :?> IEnumerator
