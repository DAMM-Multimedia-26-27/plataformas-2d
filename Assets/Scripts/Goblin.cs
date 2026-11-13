using System;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float tiempoPatrulla;
    public float velocidad;
    public Vector2 tamannoAtaque;
    public float distanciaAtaque;
    public LayerMask capaJugador;

    float temporizador;
    int direccion;
    Animator anim;
    SpriteRenderer sprite;
    Rigidbody2D cuerpo;

    void Awake()
    {
        direccion = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        temporizador = tiempoPatrulla;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        cuerpo = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //ActualizarMovimiento();
        Girar();
        ActualizarTemporizador();
        DetectarJugador();
        if (Input.GetKeyDown(KeyCode.C))
        {
            anim.SetBool("Atacar", true);
        }
        else
        {
            anim.SetBool("Atacar", false);
        }
    }

    private bool DetectarJugador()
    {
        RaycastHit2D rayo = Physics2D.BoxCast(
            transform.position, tamannoAtaque, 0, Vector2.right * direccion,
            distanciaAtaque, capaJugador
        );

        if (rayo) return true;
        return false;
    }

    private void Girar()
    {
        if (direccion >= 0)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    private void ActualizarTemporizador()
    {
        temporizador -= Time.deltaTime;
        if (temporizador < 0)
        {
            direccion *= -1;
            temporizador = tiempoPatrulla;
        }
    }

    void FixedUpdate()
    {
        cuerpo.velocity = new Vector2(direccion * velocidad, cuerpo.velocity.y);
    }

        void OnDrawGizmos()
    {
        if (DetectarJugador())
        {
            Gizmos.color = Color.red;
        } 
        Gizmos.DrawWireCube(transform.position + Vector3.right * direccion * distanciaAtaque,
            tamannoAtaque
        );
        Gizmos.color = Color.white;
    }
}
