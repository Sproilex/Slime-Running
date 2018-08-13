using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour {
    public static Jugador Instancia;
    [SerializeField]
    private float Velocidad;
    [SerializeField]
    private AudioClip SonidoMuerte;
    private AudioSource ReproductorJugador;
    private Rigidbody rigidbody3D;
    private Animator AnimatorJugador;
    [SerializeField]
    private GameObject ParticulaTierra;

    private int contadorPasos = 0;
    private bool Moviendose = false;

    private void Start()
    {
        Instancia = this;
        rigidbody3D = GetComponent<Rigidbody>();
        AnimatorJugador = GetComponent<Animator>();
        ReproductorJugador = GetComponent<AudioSource>();
        StartCoroutine(VerificarParticulas());
    }

    private void Update()
    {
        if (Manejador.NoGenerar)
        {
            rigidbody3D.velocity = Vector3.zero;
            return;
        }
        Vector3 direccionMover = Vector3.zero;
        Vector3 rotacion = transform.rotation.eulerAngles;
        bool MovimientoArriba = false;
        bool MovimientoAbajo = false;
        Moviendose = false;
        if (Input.GetKey(KeyCode.W))
        {
            rotacion = Vector3.zero;
            direccionMover = new Vector3(direccionMover.x,direccionMover.y,Velocidad);
            MovimientoArriba = true;
            Moviendose = true;
        }else if (Input.GetKey(KeyCode.S))
        {
            rotacion = new Vector3(0, 180, 0);
            direccionMover = new Vector3(direccionMover.x, direccionMover.y, -Velocidad);
            MovimientoAbajo = true;
            Moviendose = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rotacion = new Vector3(0, MovimientoArriba ? 30 : MovimientoAbajo ? 120 : 90, 0);
            direccionMover = new Vector3(Velocidad, direccionMover.y, direccionMover.z);
            Moviendose = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rotacion = new Vector3(0, MovimientoArriba ? -30 : MovimientoAbajo ? -120 : -90, 0);
            direccionMover = new Vector3(-Velocidad, direccionMover.y, direccionMover.z);
            Moviendose = true;
        }

        AnimatorJugador.SetBool("Corriendo", Moviendose);

        rigidbody3D.velocity = direccionMover;
        transform.rotation = Quaternion.Euler(rotacion);
    }

    public void Posicionar(Vector3 posicion)
    {
        transform.position = posicion;
    }

    public void Morir()
    {
        Moviendose = false;
        if(Manejador.SonarEfectos)
            ReproductorJugador.PlayOneShot(SonidoMuerte);
        AnimatorJugador.SetBool("Muerto",true);
    }

    private IEnumerator VerificarParticulas()
    {
        ParticulaTierra.SetActive(Moviendose);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(VerificarParticulas());
    }
}
