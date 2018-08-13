using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaSalida : MonoBehaviour {

    private static PuertaSalida _Instancia;
    public static PuertaSalida Instancia
    {
        get
        {
            if (_Instancia == null)
                _Instancia = FindObjectOfType<PuertaSalida>();

            return _Instancia;
        }
    }

    private bool YaGanada = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !YaGanada)
        {
            Manejador.GanarPartida();
            YaGanada = true;
        }
    }

    public List<Vector3> PosicionesBloqueadas()
    {
        List<Vector3> Resultado = new List<Vector3>();

        Vector3 posicionPuerta = transform.position;
        Vector3 PosicionInicial = new Vector3(posicionPuerta.x,posicionPuerta.y,posicionPuerta.z - 1);
        Resultado.Add(PosicionInicial);
        Resultado.Add(new Vector3(PosicionInicial.x - 1, PosicionInicial.y, PosicionInicial.z));
        Resultado.Add(new Vector3(PosicionInicial.x + 1, PosicionInicial.y, PosicionInicial.z));
        return Resultado;
    }

}
