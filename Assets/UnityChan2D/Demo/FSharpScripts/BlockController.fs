namespace UnityChan2D.Demo
open UnityEngine

type BlockController () =
  inherit MonoBehaviour()

  [<DefaultValue>]val mutable public whatIsPlayer : LayerMask

  [<DefaultValue>]val mutable public brokenBlock : GameObject

  [<DefaultValue>]val mutable public hitClip : AudioClip

  [<DefaultValue>]val mutable public canBreak : bool

  [<DefaultValue>]val mutable private m_boxCollider2D : BoxCollider2D

  let ( !! ) (x:LayerMask) = LayerMask.op_Implicit x
  let ( ! ) x  = Collider2D.op_Implicit x

  member this.Awake () = 
    this.m_boxCollider2D <- this.GetComponent<BoxCollider2D>()

  member this.OnCollisionEnter2D(collision2D : Collision2D) = 
    if (collision2D.gameObject.tag = "Player") then
      let pos = this.transform.position
      let groundCheck = new Vector2(pos.x, pos.y - this.transform.lossyScale.y)
      let groundArea = new Vector2(this.m_boxCollider2D.size.x * this.transform.lossyScale.y * 0.45f, 0.05f)
      let col2D = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, !!this.whatIsPlayer)

      if !col2D then
        if this.canBreak then
          let broken = MonoBehaviour.Instantiate(this.brokenBlock, this.transform.position, this.transform.rotation) :?> GameObject
          broken.transform.localScale <- this.transform.lossyScale
          MonoBehaviour.Destroy(this.gameObject)
        else
          AudioSourceController.instance.PlayOneShot(this.hitClip)



