using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public int vidaMax;
    public float velocidad;
    public float fuerzaSalto;
    public float distCaja;
    public Vector2 tamannoCaja;
    public LayerMask capaSuelo;
    public GameObject holis;
    public GameObject polvoSalto;

    Vector3 movimiento;
    Rigidbody2D cuerpo;
    SpriteRenderer sprite;
    Animator anim;
    bool saltando;
    Vector2 knockBack;
    int vidaRestante;
    // Start is called before the first frame update
    void Start()
    {
        knockBack = Vector2.zero;
        vidaRestante = vidaMax;
        movimiento = new Vector3();
        cuerpo = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        LeerMovimiento();
        LeerSalto();
        RotarPersonaje();
        //EstaEnElSuelo();
        ActualizarAnim();
    }

    void ActualizarAnim()
    {
        if (movimiento.x == 0)
        {
            anim.SetBool("Moviendo", false);
        } 
        else
        {
            anim.SetBool("Moviendo", true);
        }
        if (!EstaEnElSuelo())
        {
            anim.SetBool("Saltando", true);
        }
        else
        {
            anim.SetBool("Saltando", false);
        }
    }

    private void RotarPersonaje()
    {
        if (movimiento.x < 0)
        {
            sprite.flipX = true;
        }
        else if (movimiento.x > 0)
        {
            sprite.flipX = false;
        }
    }

    private void LeerSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !saltando && EstaEnElSuelo())
        {
            polvoSalto.transform.position = transform.position;
            saltando = true;
            polvoSalto.transform.parent = null;
        }
    }

    private void LeerMovimiento()
    {
        if (Input.GetKey(KeyCode.A))
        {
            // Izquierda
            movimiento = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Derecha
            movimiento = Vector3.right;
        }
        else
        {
            movimiento = new Vector3();
        }

//        Debug.Log(Input.mousePosition);
    }

    void FixedUpdate()
    {
        //transform.position += movimiento * Time.fixedDeltaTime * velocidad;
        //Vector3 posNueva = transform.position + movimiento * Time.fixedDeltaTime * velocidad;
        //cuerpo.MovePosition(posNueva);

        cuerpo.velocity = new Vector2(movimiento.x * velocidad, cuerpo.velocity.y);
        if (saltando)
        {
            cuerpo.AddForce(Vector2.up * fuerzaSalto); // QUE SALTE
            saltando = false;
        }
        if (knockBack != Vector2.zero)
        {
            cuerpo.AddForce(knockBack);
            knockBack = Vector2.zero;
        }
    }

    bool EstaEnElSuelo()
    {
        RaycastHit2D rc = Physics2D.BoxCast(
            transform.position, tamannoCaja, 0, Vector2.down, distCaja, capaSuelo);
        if (rc)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    void OnDrawGizmos()
    {
        if (EstaEnElSuelo())
        {
            Gizmos.color = Color.red;
        } 
        Gizmos.DrawWireCube(transform.position + Vector3.down * distCaja, tamannoCaja);
        Gizmos.color = Color.white;
    }

    public void RecibeDanno(int cuanto, Vector2 posEnemigo)
    {
        vidaRestante -= cuanto;
        Debug.Log("Ouch!! Vida restante: " + vidaRestante);
        if (vidaRestante <= 0) Destroy(gameObject);
        Vector2 direccionAtaque = ((Vector2) transform.position) - posEnemigo;
        direccionAtaque.Normalize();
        knockBack = direccionAtaque * 250 + 100 * Vector2.up;
    }

    // void OnMouseOver()
    // {
    //     holis.SetActive(true);
    // }

    // void OnMouseExit()
    // {
    //     holis.SetActive(false);
    // }

    void OnMouseDrag()
    {
        Vector3 mouseWP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = new Vector3(mouseWP.x, mouseWP.y, gameObject.transform.position.z);

    }
}
