namespace UnityChan2D.Demo
open System.Collections
open UnityEngine

type TimeController () =
  inherit MonoBehaviour ()

  [<DefaultValue>] val mutable public time : int
  [<SceneName>]
  [<DefaultValue>] val mutable public nextLevel : string
  [<DefaultValue>] val mutable public timer : GUIText

  member this.Update () = 
    let remainingTime = this.time - Mathf.FloorToInt(Time.timeSinceLevelLoad * 2.5f)

    if (0 <= remainingTime) then
      this.timer.text <- remainingTime.ToString("000")
    else
      let player = GameObject.FindGameObjectWithTag("Player")
      if player <> null then
        this.StartCoroutine(this.LoadNextLevel()) |> ignore
        this.enabled <- false

  member this.LoadNextLevel () : IEnumerator = 
    let player = GameObject.FindGameObjectWithTag("Player")

    if player <> null then
      player.SendMessage("TimeOver", SendMessageOptions.DontRequireReceiver)

    seq {
      yield WaitForSeconds(3.f)
      Application.LoadLevel(this.nextLevel)
    } :?> IEnumerator