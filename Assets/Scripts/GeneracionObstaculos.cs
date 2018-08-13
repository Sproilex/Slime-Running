using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoObstaculo {Espacio, Muro }

public class GeneracionObstaculos : MonoBehaviour {

    [SerializeField]
    private int PorcentajeEspacio;
    [SerializeField]
    private GameObject PrefabMuro;
    [SerializeField]
    private int PorcentajeMuro;

    private int PorcentajeUsarMuro;
    private int PorcentajeUsarEspacio;
    private List<TipoObstaculo> TipoObstaculos = new List<TipoObstaculo>();

    private void Start()
    {
        PorcentajeUsarMuro = PorcentajeMuro;
        PorcentajeUsarEspacio = PorcentajeEspacio;
    }

    public void GenerarObstaculos(List<Transform> PosicionesCuarto, int largo)
    {
        /* for(int contador = 0; contador < PosicionesCuarto.Count; contador++)
         {
             int numeroAleatorio = Random.Range(0,101);

             if(contador == 0)
                 contador = largo * 3;





             if (numeroAleatorio <= PorcentajeUsarEspacio)
             {
                 TipoObstaculos.Add(TipoObstaculo.Espacio);
                 continue;
             }
             else
             {
                 GameObject Muro = Instantiate(PrefabMuro);
                 Vector3 posBloque = PosicionesCuarto[contador].transform.position;
                 float offsetY = PosicionesCuarto[contador].transform.localScale.y;
                 Muro.transform.position = new Vector3(posBloque.x,posBloque.y + offsetY,posBloque.z);
                 TipoObstaculos.Add(TipoObstaculo.Muro);
             }


         }*/

        int NumeroSaltos = PosicionesCuarto.Count / largo;

        for (int contador = 2; contador < NumeroSaltos; contador++)
        {
            int IndiceEspacio = Random.Range(0, largo + 1);
            for (int contadorFila = 0; contadorFila < largo; contadorFila++)
            {
                if(contadorFila == IndiceEspacio)
                {
                    TipoObstaculos.Add(TipoObstaculo.Espacio);
                    continue;
                }
                GameObject Muro = Instantiate(PrefabMuro);
                Vector3 posBloque = PosicionesCuarto[largo * contador + contadorFila].transform.position;
                float offsetY = PosicionesCuarto[contador].transform.localScale.y;
                Muro.transform.position = new Vector3(posBloque.x, posBloque.y + offsetY, posBloque.z + contador);
                TipoObstaculos.Add(TipoObstaculo.Muro);
            }
            contador++;
        }

    }
}
