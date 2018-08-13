using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InformacionHabitacion
{
    public List<Transform> EspaciosObstaculos;
    public List<Transform> EspaciosPuertas;
    public Vector3 PosicionJugador;
    public Vector3 PosicionPuertaSalida;
}

public class GeneracionCuarto : MonoBehaviour {

    [SerializeField]
    private int AnchuraMaxima = 20;
    public int LargoMaximo = 20;
    [SerializeField]
    private int AlturaBordes = 2;
    [SerializeField]
    private GameObject PrefabPiso;
    [SerializeField]
    private GameObject PrefabDelimitador;
    [SerializeField]
    private Material[] TexturasPisos;
    [SerializeField]
    private Material[] TexturasParedes;

    public InformacionHabitacion GenerarCuarto(Transform PosicionadorInicial, Transform PadreColocar)
    {
        int AnchuraCuarto = 16;
        int LargoCuarto =16;

        //GenerarTamaño(out AnchuraCuarto, out LargoCuarto);

        float offsetA = 0;
        int contadorPisos = 0;
        int contadorParedes = 0;
        List<Transform> EspaciosBloqueos = new List<Transform>();
        List<Transform> EspaciosPuertas = new List<Transform>();
        for (int contadorAncho = 0; contadorAncho < AnchuraCuarto; contadorAncho++)
        {
            float offsetL = 0;
            for(int contadorLargo = 0; contadorLargo < LargoCuarto; contadorLargo++)
            {
                GameObject Cubo = Instantiate(PrefabPiso, PadreColocar);
                Cubo.transform.position = new Vector3(PosicionadorInicial.position.x + offsetL, PosicionadorInicial.position.y, PosicionadorInicial.position.z + offsetA);
                Cubo.transform.rotation = GeneracionNivel.Instancia.RotacionAleatoria();
                Cubo.name = "Piso " + contadorPisos;
                Cubo.GetComponent<MeshRenderer>().material = ObtenerTexturaPiso();
                contadorPisos++;
                EspaciosBloqueos.Add(Cubo.transform);

                if (((contadorLargo == 0 || contadorLargo == LargoCuarto - 1)) || (contadorAncho == 0 || contadorAncho == (AnchuraCuarto - 1)))
                {
                    float offsetAB = 0;
                    for (int contadorAB = 0; contadorAB < AlturaBordes; contadorAB++)
                    {
                        GameObject Pared = Instantiate(PrefabDelimitador, PadreColocar);
                        Pared.transform.position = new Vector3(PosicionadorInicial.position.x + offsetL, PosicionadorInicial.position.y + PrefabPiso.transform.localScale.y + offsetAB, PosicionadorInicial.position.z + offsetA);
                        Pared.transform.rotation = GeneracionNivel.Instancia.RotacionAleatoria();
                        Pared.GetComponent<MeshRenderer>().material = ObtenerTexturaPared();
                        Pared.name = "Pared " + contadorParedes;
                        contadorParedes++;
                        offsetAB += PrefabDelimitador.transform.localScale.y;
                        EspaciosBloqueos.Remove(Cubo.transform);
                        if (contadorAB == 0 && ( (contadorLargo > 0 && contadorLargo < (LargoCuarto - 1)) && contadorAncho == 0 || contadorAncho == AnchuraCuarto - 1  ))
                            EspaciosPuertas.Add(Pared.transform);
                    }
                }

                offsetL += Cubo.transform.localScale.x;

            }
            offsetA += PrefabPiso.transform.localScale.z;
        }

        InformacionHabitacion Info = new InformacionHabitacion();
        Info.EspaciosObstaculos = EspaciosBloqueos;
        Info.EspaciosPuertas = EspaciosPuertas;

        return Info;

    }

    private Material ObtenerTexturaPiso()
    {
        int[] Porcentajes = new int[] { 5,10,35,68,80};
        int numero = Random.Range(0,101);

        if (numero <= Porcentajes[0])
            return TexturasPisos[0];
        else if (numero <= Porcentajes[1])
            return TexturasPisos[1];
        else if (numero <= Porcentajes[2])
            return TexturasPisos[2];
        else if (numero <= Porcentajes[3])
            return TexturasPisos[3];
        else
            return TexturasPisos[4];
    }

    private Material ObtenerTexturaPared()
    {
        int indice = Random.Range(0, TexturasParedes.Length);
        return TexturasParedes[indice];
    }
}
