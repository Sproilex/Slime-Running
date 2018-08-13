using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour {

    private static Camara _Instancia;
    public static Camara Instancia
    {
        get
        {
            if (_Instancia == null)
                _Instancia = FindObjectOfType<Camara>();

            return _Instancia;
        }
    }
    public Animator AnimatorCamara { get; private set; }

    private void Start()
    {
        GeneracionNivel.OnIniciarGeneracionNivel += ComenzarAnimacionInicioCuarto;
        GeneracionNivel.OnTerminarGeneracionNivel += ComenzarAnimacionFinalizarCuarto;
    }

    public void ComenzarAnimacionFinalizarCuarto()
    {
        if (AnimatorCamara != null)
                AnimatorCamara.SetBool("Mostrar",false);
    }

    public void ComenzarAnimacionInicioCuarto()
    {
        if(AnimatorCamara != null)
            AnimatorCamara.SetBool("Mostrar", true);
    }
}
