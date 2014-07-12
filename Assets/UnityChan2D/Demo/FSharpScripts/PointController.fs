namespace UnityChan2D.Demo
open System
open UnityEngine

[<AllowNullLiteral>]
type public PointController () =
  inherit MonoBehaviour()
  [<DefaultValue>] val mutable public total : GUIText
  [<DefaultValue>] val mutable public coin : GUIText
  static let mutable m_instance : PointController = null

  static member Instance 
    with get () = PointController.FindObjectOfType<PointController>()

  member this. AddCoin () = 
    this.coin.text <- (Convert.ToInt32(this.coin.text) + 1).ToString("00")
    this.total.text <- (Convert.ToInt32(this.total.text) + 100).ToString("0000000")    