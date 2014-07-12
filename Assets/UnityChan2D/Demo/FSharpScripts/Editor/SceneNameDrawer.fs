namespace AssemblyFSharpEditor
open System
open System.Linq
open System.Collections.Generic 
open UnityEngine
open UnityEditor
open UnityChan2D.Demo

[<CustomPropertyDrawer(typeof<SceneNameAttribute>)>]
type SceneNameDrawer () =
  inherit PropertyDrawer () 

  member this.SceneNameAttribute 
    with get () = this.attribute :?> SceneNameAttribute

  override this.OnGUI (position:Rect, property:SerializedProperty, label:GUIContent) = 
    let sceneNames = this.GetEnabledSceneNames()

    if (sceneNames |> Array.length = 0) then
        EditorGUI.LabelField(position, ObjectNames.NicifyVariableName(property.name), "Scene is Empty")
    else
      let sceneNumbers = Array.create sceneNames.Length 0
      this.SetSceneNambers(sceneNumbers, sceneNames)
      if (not <| String.IsNullOrEmpty(property.stringValue)) then
          this.SceneNameAttribute.selectedValue <- this.GetIndex(sceneNames, property.stringValue)
      this.SceneNameAttribute.selectedValue <- EditorGUI.IntPopup(position, label.text, this.SceneNameAttribute.selectedValue, sceneNames, sceneNumbers)
      property.stringValue <- sceneNames.[this.SceneNameAttribute.selectedValue]

  member this.GetEnabledSceneNames () =
    let scenes = if this.SceneNameAttribute.enableOnly then EditorBuildSettings.scenes.Where(fun scene -> scene.enabled) else EditorBuildSettings.scenes |> Array.toSeq 
    let sceneNames = new HashSet<string>();
    scenes |> Seq.iter (fun scene -> sceneNames.Add(scene.path.Substring(scene.path.LastIndexOf("/") + 1).Replace(".unity", String.Empty)) |> ignore)
    sceneNames.ToArray()

  member this.SetSceneNambers(sceneNumbers : int[], sceneNames : string[]) =
    for i in [0..sceneNames.Length - 1] do
      sceneNumbers.[i] <- i

  member this.GetIndex (sceneNames :string[], sceneName:string) =
    let result = ref 0
    let break' = ref true 
    let i = ref 0
    while (!break') do
      if (!i > sceneNames.Length - 1) then
        break' := false
      if (!break' && sceneName = sceneNames.[!i]) then
        result := !i
        break' := false
      incr i
    !result

