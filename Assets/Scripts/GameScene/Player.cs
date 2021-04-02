using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public string playerName = "";
    public float speed = 10;

    public Color[] damageColors;
    public Weapon equipWeapon;
    public ParticleSystem effect;

    public string id;

    public int count;

    float hAxis;
    float vAxis;
    float isPlaying = 5;

    bool isDodge;
    bool isJump;
    bool isAttack;
    bool isBorder;
    bool isDamage;
    bool isDeath;
    public bool isGameover;

    bool jDown;
    bool dDown;
    bool lDown;

    public StatusManager playerStatusUI;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Rigidbody rigid;
    Animator anim;
    Renderer[] meshs;
    public Text text;
    public int maxPlayer;

    public AudioClip clip;
    public AudioClip clip2;

    void Awake()
    {
        StopToWall();

        if (photonView.IsMine)
        {
            // # 1. 시작하면 카메라는 플레이어를 따라간다
            Camera.main.GetComponent<CameraFollow>().target = GetComponent<Transform>().Find("CamPivot").transform;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        effect = GetComponentInChildren<ParticleSystem>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        meshs = GetComponentsInChildren<Renderer>();
        id = photonView.Owner.UserId;
        if (photonView.IsMine)
        {
            playerName = IDManager.Instance.Nickname();
            text.text = IDManager.Instance.Nickname();
        }

        GameManager.Instance.AddPlayer(this);
    }

    void Update()
    {
        if (photonView.IsMine & !isGameover)
        {
            GetInput();
            Move();
            Jump();
            Dodge();
            Turn();
            photonView.RPC("Death", RpcTarget.AllBuffered, null);
            if (GameManager.Instance.state == GameState.PLAY)
            {
                isPlaying -= Time.deltaTime;
                if (isPlaying <= 0)
                    Attack();
            }
        }
    }

    private void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);

        // #1. 플레이어 앞에 Layer마스크가 Wall 인 오브젝트라면 움직임을 멈춤
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        dDown = Input.GetButtonDown("Dodge");
        lDown = Input.GetButtonDown("Fire1");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            // # 1. 구르기 움직임값 설정
            moveVec = dodgeVec;

        if (!isBorder)
            // # 2. 벽을 제외하고 움직임값 설정
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
    }

    void Jump()
    {
        // # 1. 2중점프 방지 && 점프중 구르기를 못하게 제어
        if (jDown && !isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * 6.5f, ForceMode.Impulse);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }


    void Dodge()
    {
        // # 1. 제자리 구르기 방지 && 구르기중 점프를 못하게 제어
        if (dDown && !isJump && moveVec != Vector3.zero && !isDodge && !isAttack)
        {
            dodgeVec = moveVec;
            speed *= 2f;
            isDodge = true;

            anim.SetTrigger("doDodge");

            // # 2. 구르기 종료지점 설정
            Invoke("DodgeOut", 1.0f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Turn()
    {
        // #1. 키보드에 의한 회전
        transform.LookAt(transform.position - moveVec);
    }


    void Attack()
    {
        // #1. 마우스 입력을 받아 공격
        if (lDown && !isDodge && !isAttack)
        {
            isAttack = true; // #2. 공격 모션이 끊기지 않도록 불값 설정
            equipWeapon.Use(); // #3. 공격 모션중일때 무기의 콜라이더를 가져옴
            anim.SetTrigger("doAttack");
            PlayerSoundManager.instance.SFXPlay("PlayerAttack", clip);
            Invoke("AttackOut", 1f); // #4. 조건을 다시 공격할 수 있는 상태로 초기화
        }
    }

    void AttackOut()
    {
        isAttack = false;
    }

    IEnumerator IeOnDamage()
    {
        isDamage = true;
        // # 1. 공격을 받으면 HP를 감소하며 색깔을 변경
        foreach (Renderer mesh in meshs)
        {
            mesh.material.color = damageColors[count - 1];
            anim.SetTrigger("doHit");
            PlayerSoundManager.instance.SFXPlay("PlayerDamage", clip2);
        }
        yield return new WaitForSeconds(1f);

        isDamage = false;
    }

    [PunRPC]
    private void OnDamage(int damage)
    {
        // # 1. HP가 0 이면 이펙트 & 공격 못하게 제어
        if (count > 6) return;

        count += damage;

        effect.Play();
        StartCoroutine(IeOnDamage());
    }

    private void OnDestroy()
    {
        GameManager.Instance.players.Remove(this);
        GameManager.Instance.CheckUser();
    }

    [PunRPC]
    void Death()
    {
        if (count == 7 && !isDeath)
        {
            isDeath = true;
            anim.SetTrigger("doDie");
            Destroy(gameObject, 3.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") // #1. 2중 점프 제어
            isJump = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // # 1. 상대의 무기에만 공격을 받는 조건
        if (other.gameObject.tag == "Weapon" && other.gameObject != equipWeapon && !isDamage)
        {
            Weapon hammer = other.GetComponent<Weapon>();
            photonView.RPC("OnDamage", RpcTarget.All, hammer.damage);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerName);
            stream.SendNext(text.text);
            //text.text = playerName;
        }
        else
        {
            playerName = (string)stream.ReceiveNext();
            text.text = (string)stream.ReceiveNext();
            //text.text = playerName;
        }
    }
}
