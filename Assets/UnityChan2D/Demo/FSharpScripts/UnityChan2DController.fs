namespace UnityChan2D.Demo
open UnityEngine
open System.Collections

type State =
  | Normal
  | Damaged
  | Invincible

[<RequireComponent(typeof<Animator>, typeof<Rigidbody2D>, typeof<BoxCollider2D>)>]
type UnityChan2DController () =
  inherit MonoBehaviour()
  let mutable maxSpeed :float32 = 10.f
  let mutable jumpPower :float32 = 1000.f
  let mutable backwardForce = new Vector2(-4.5f, 5.4f)

  [<DefaultValue>]val mutable public whatIsGround : LayerMask

  [<DefaultValue>]val mutable private m_animator : Animator
  [<DefaultValue>]val mutable private m_boxcollier2D : BoxCollider2D
  [<DefaultValue>]val mutable private m_rigidbody2D : Rigidbody2D
  [<DefaultValue>]val mutable private m_isGround : bool

  [<Literal>]
  let m_centerY = 1.5f
  let mutable m_state : State = State.Normal

  let ( !! ) (x:int) = LayerMask.op_Implicit x
  let ( !!! ) (x:LayerMask) = LayerMask.op_Implicit x

  member this.Reset () =
    this.Awake()

    // UnityChan2DController
    maxSpeed <- 10.f
    jumpPower <- 1000.f
    backwardForce <- new Vector2(-4.5f, 5.4f);
    this.whatIsGround <- !!(1 <<< LayerMask.NameToLayer("Ground"))

    // Transform
    this.transform.localScale <- new Vector3(1.f, 1.f, 1.f)

    // Rigidbody2D
    this.m_rigidbody2D.gravityScale <- 3.5f
    this.m_rigidbody2D.fixedAngle <- true

    // BoxCollider2D
    this.m_boxcollier2D.size <- new Vector2(1.f, 2.5f)
    this.m_boxcollier2D.center <- new Vector2(0.f, -0.25f)

    // Animator
    this.m_animator.applyRootMotion <- false

  member this.Awake () = 
    this.m_animator <- this.GetComponent<Animator>()
    this.m_boxcollier2D <- this.GetComponent<BoxCollider2D>()
    this.m_rigidbody2D <- this.GetComponent<Rigidbody2D>()

  member this.Update () =
    if m_state <> State.Damaged then
      let x = Input.GetAxis("Horizontal")
      let jump = Input.GetButtonDown("Jump")
      this.Move(x,jump)

  member this.Move(move, jump) =
    if (Mathf.Abs(move) > 0.f) then
      let rot = this.transform.rotation
      let y = if Mathf.Sign(move) = 1.f then 0.f else 180.f
      this.transform.rotation <- Quaternion.Euler(rot.x, y, rot.z)

    this.m_rigidbody2D.velocity <- new Vector2(move * maxSpeed, this.m_rigidbody2D.velocity.y)

    this.m_animator.SetFloat("Horizontal", move)
    this.m_animator.SetFloat("Vertical", this.m_rigidbody2D.velocity.y)
    this.m_animator.SetBool("isGround", this.m_isGround)

    if (jump && this.m_isGround) then
      this.m_animator.SetTrigger("Jump")
      this.SendMessage("Jump", SendMessageOptions.DontRequireReceiver)
      this.m_rigidbody2D.AddForce(Vector2.up * jumpPower)

  member this.FixedUpdate () =
    let pos = this.transform.position
    let groundCheck = new Vector2(pos.x, pos.y - (m_centerY * this.transform.localScale.y))
    let groundArea = new Vector2(this.m_boxcollier2D.size.x * 0.49f, 0.05f)

    this.m_isGround <- Collider2D.op_Implicit(Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, !!!this.whatIsGround))
    this.m_animator.SetBool("isGround", this.m_isGround)

  member this.OnTriggerStay2D (other : Collider2D) = 
    if (other.tag = "DamageObject" && m_state = State.Normal) then
      m_state <- State.Damaged
      this.StartCoroutine(this.INTERNAL_OnDamage()) |> ignore

  member this.INTERNAL_OnDamage () : IEnumerator = 
    this.m_animator.Play(if this.m_isGround then "Damage" else "AirDamage")
    this.m_animator.Play("Idle")

    this.SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver)

    this.m_rigidbody2D.velocity <- new Vector2(this.transform.right.x * backwardForce.x, this.transform.up.y * backwardForce.y)

    let isGround () = 
      if this.m_isGround = false then
        true
      else
        this.m_animator.SetTrigger("Invincible Mode")
        m_state <- State.Invincible
        false

    seq {
      yield new WaitForSeconds(0.2f) :> YieldInstruction
      while (isGround()) do
        yield new WaitForFixedUpdate() :> YieldInstruction
    } :?> IEnumerator
    
  member this.OnFinishedInvincibleMode () = m_state <- State.Normal
    