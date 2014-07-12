namespace UnityChan2D.Demo
open UnityEngine

[<RequireComponent(typeof<UnityChan2DController>, typeof<AudioSource>)>]
type Demo () =
  inherit MonoBehaviour()

  [<SerializeField>]
  [<DefaultValue>]val mutable private damageVoice : AudioClip

  [<SerializeField>]
  [<DefaultValue>]val mutable private jumpVoices : AudioClip[]

  [<SerializeField>]
  [<DefaultValue>]val mutable private clearVoice : AudioClip

  [<SerializeField>]
  [<DefaultValue>]val mutable private timeOverVoice : AudioClip

  [<DefaultValue>]val mutable private audioSource : AudioSource

  member this.Awake () =
    this.audioSource <- this.GetComponent<AudioSource>()

  member this.OnDamage () =
    this.PlayVoice(this.damageVoice)

  member this.Jump () =
    let i = Random.Range(0, this.jumpVoices.Length)
    this.PlayVoice(this.jumpVoices.[i])

  member this.Clear() =
    this.PlayVoice(this.clearVoice)

  member this.TimeOver () = 
    this.PlayVoice(this.timeOverVoice)

  member this.PlayVoice(voice:AudioClip) =
    this.audioSource.Stop()
    this.audioSource.PlayOneShot(voice)