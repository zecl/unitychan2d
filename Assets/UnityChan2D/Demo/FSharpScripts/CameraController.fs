namespace UnityChan2D.Demo
open System
open System.Collections 
open UnityEngine

[<RequireComponent(typeof<Camera>)>]
type CameraController () =
  inherit MonoBehaviour()
  
  [<DefaultValue>] val mutable public target : Transform
  [<DefaultValue>] val mutable public stopPosition : Transform
  
  [<SceneName>]
  [<DefaultValue>] val mutable public nextLevel : string

  [<DefaultValue>] val mutable private  m_camera : Camera

  member this.Awake () = 
    this.m_camera <- this.GetComponent<Camera>()

  member this.LateUpdate () = 
    let right = this.m_camera.ViewportToWorldPoint(Vector3.right)
    let center = this.m_camera.ViewportToWorldPoint(Vector3.one * 0.5f)

    if (center.x < this.target.position.x) then
      let pos = this.m_camera.transform.position

      if (Math.Abs(pos.x - this.target.position.x) >= 0.0000001f) then
          this.m_camera.transform.position <- new Vector3(this.target.position.x, pos.y, pos.z)

    if (this.stopPosition.position.x - right.x < 0.f) then
      this.StartCoroutine(this.INTERNAL_Clear()) |> ignore
      this.enabled <- false


  member this.INTERNAL_Clear () : IEnumerator =
    let player = GameObject.FindGameObjectWithTag("Player")

    if (player <> null) then
      player.SendMessage("Clear", SendMessageOptions.DontRequireReceiver)

    seq {
      yield new WaitForSeconds(3.f)
      Application.LoadLevel(this.nextLevel)
    } :?> IEnumerator

    