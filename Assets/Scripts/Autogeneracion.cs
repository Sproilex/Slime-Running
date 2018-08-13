using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autogeneracion : MonoBehaviour {

    public static bool Eliminar = false;
    private BoxCollider boxCollider;
    public Animator AnimatorMuro { get; private set; }
    private bool esMuroDestruyo = false;
    [SerializeField]
    private GameObject Particulas;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        AnimatorMuro = GetComponent<Animator>();
        StartCoroutine(GenerarCubo());
    }

    private void Update()
    {
        if (Eliminar && !esMuroDestruyo)
            Destroy(this.gameObject);
    }

    private IEnumerator GenerarCubo()
    {
        if (Manejador.NoGenerar)
        {
            StopAllCoroutines();
            yield break;
        }
        yield return new WaitForSeconds(GeneracionNivel.Instancia.CantidadVelocidadGeneracion);
        int numeroLado = 0;
        Vector3 Direccion = Vector3.zero;

            numeroLado = Random.Range(0, 4);
            switch (numeroLado)
            {
                case 0:
                    Direccion = Vector3.forward;
                    break;
                case 1:
                    Direccion = Vector3.back;
                    break;
                case 2:
                    Direccion = Vector3.right;
                    break;
                case 3:
                    Direccion = Vector3.left;
                    break;
            }
        if (VerificarEspacio(Direccion))
        {
            Direccion = RetornarDireccion();
            if(Direccion == Vector3.zero)
                yield break;
        }

        GameObject NuevoCubo = Instantiate(GeneracionNivel.Instancia.PrefabObstaculo, this.transform.parent);
        NuevoCubo.transform.position = transform.position + Direccion;
        Manejador.PosicionesColocadas.Add(NuevoCubo.transform.position);
        NuevoCubo.transform.rotation = GeneracionNivel.Instancia.RotacionAleatoria();
        StartCoroutine(GenerarCubo());
    }

    private bool VerificarEspacio(Vector3 direccion)
    {
        Ray rayo = new Ray();
        rayo.origin = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        rayo.direction = direccion;
        bool Impacto = Physics.Raycast(rayo,1, 1 << LayerMask.NameToLayer("Muro"));
        return Impacto || Manejador.PosicionesColocadas.Contains(transform.position + direccion);
    }

    private Vector3 ComprobarDireccionMover()
    {
        Vector3 posicion = transform.position;
        posicion = new Vector3(posicion.x, posicion.y, posicion.z + 1);
        bool lleno = VerificarEspacio(transform.position - posicion);
        if (lleno)
        {
            posicion = new Vector3(posicion.x, posicion.y, posicion.z - 1);
            lleno = VerificarEspacio(transform.position - posicion);
            if (lleno)
            {
                posicion = new Vector3(posicion.x + 1, posicion.y, posicion.z);
                lleno = VerificarEspacio(transform.position - posicion);
                if (lleno)
                {
                    posicion = new Vector3(posicion.x - 1, posicion.y, posicion.z);
                    lleno = VerificarEspacio(transform.position - posicion);
                    if (lleno)
                    {
                        return posicion = Vector3.zero;
                    }
                }
            }
        }
        return posicion;
    }

    private Vector3 RetornarDireccion()
    {
        Vector3 direccionProbando = Vector3.zero;
        direccionProbando = Vector3.left;
        bool Impacto = VerificarEspacio(direccionProbando);
        if (Impacto)
        {
            direccionProbando = Vector3.right;
            Impacto = VerificarEspacio(direccionProbando);
            if (Impacto)
            {
                direccionProbando = Vector3.forward;
                Impacto = VerificarEspacio(direccionProbando);
                if (Impacto)
                {
                    direccionProbando = Vector3.back;
                    Impacto = VerificarEspacio(direccionProbando);
                    if (Impacto)
                    {
                        return Vector3.zero;
                    }
                }
            }
        }
        return direccionProbando;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !Manejador.NoGenerar)
        {
            esMuroDestruyo = true;
            Jugador.Instancia.Posicionar(transform.position);
            Jugador.Instancia.Morir();
            Manejador.PerderPartida();
        }
    }

    public void FinalizacionAnimacion()
    {
        boxCollider.enabled = true;
        boxCollider.isTrigger = true;
        Particulas.SetActive(false);
    }
}
