namespace UnityChan2D.Demo
open UnityEngine

[<AllowNullLiteral>]
[<RequireComponent(typeof<AudioSource>)>]
[<Sealed>]
type public AudioSourceController () =
  inherit MonoBehaviour()
  static let mutable m_instance : AudioSourceController = null

  static member instance 
    with get () = 
      if m_instance = null then
        m_instance <- MonoBehaviour.FindObjectOfType<AudioSourceController>()
        if m_instance = null then
          let go = new GameObject("AudioSourceController")
          MonoBehaviour.DontDestroyOnLoad(go)
          m_instance <- go.AddComponent<AudioSourceController>()
      m_instance
  
  member this.PlayOneShot (clip:AudioClip) =
    AudioSourceController.instance.audio.PlayOneShot(clip)
