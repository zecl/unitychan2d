namespace UnityChan2D.Demo
open UnityEngine
open System.Runtime.InteropServices

type SceneNameAttribute () =
  inherit PropertyAttribute()

  [<DefaultValue>]val mutable public selectedValue : int
  [<DefaultValue>]val mutable public enableOnly : bool

  member this.SceneNameAttribute([<Optional;DefaultParameterValue(true)>] ?enableOnly:bool) =
    let enableOnly = defaultArg enableOnly true
    this.enableOnly <- enableOnly

