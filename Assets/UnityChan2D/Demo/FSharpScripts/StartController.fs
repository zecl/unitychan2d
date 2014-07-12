namespace UnityChan2D.Demo
open System.Collections
open UnityEngine

[<RequireComponent(typeof<AudioSource>)>]
type StartController () =
  inherit MonoBehaviour()
  [<SceneName>]
  [<DefaultValue>] val mutable public nextLevel : string

  [<SerializeField>]
  let mutable enter : KeyCode = KeyCode.X

  member this.Update () = 
    if Input.GetKeyDown(enter) then
      this.StartCoroutine(this.LoadStage()) |> ignore
      
  member this.LoadStage () : IEnumerator = 
    MonoBehaviour.FindObjectsOfType<AudioSource>() 
    |> Seq.iter (fun audioS -> audioS.volume <- 0.2f)

    let audioSource = this.GetComponent<AudioSource>()
    audioSource.volume <- 1.f
    audioSource.Play()

    seq {
      yield new WaitForSeconds(audioSource.clip.length + 0.5f)
      Application.LoadLevel(this.nextLevel)
    } :?> IEnumerator 
