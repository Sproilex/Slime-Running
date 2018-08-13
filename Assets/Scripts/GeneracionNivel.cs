using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AccionGeneracion();

public class GeneracionNivel : MonoBehaviour {

    private static GeneracionNivel _Instancia;
    public static GeneracionNivel Instancia
    {
        get
        {
            if (_Instancia == null)
                _Instancia = FindObjectOfType<GeneracionNivel>();

            return _Instancia;
        }
    }

    [SerializeField]
    private Transform PosicionInicial;
    [SerializeField]
    private Transform AgrupadorCuarto;
    [SerializeField]
    private GeneracionObstaculos generacionObstaculos;
    [SerializeField]
    private GeneracionCuarto generacionCuarto;
    public GameObject PrefabObstaculo;
    [SerializeField]
    private GameObject PrefabPuertaEntrada;
    [SerializeField]
    private GameObject PrefabPuertaSalida;
    [SerializeField]
    private GameObject PrefabJugador;
    private int CantidadBloquesIniciales = 7;
    public int ContadorActualBloquesIniciales;
    private float VelocidadGeneracion = 1f;
    public float CantidadVelocidadGeneracion;

    [SerializeField]
    private float[] RotacionesDisponibles;

    [SerializeField]
    InformacionHabitacion InfoHabitacionActual;

    public static event AccionGeneracion OnIniciarGeneracionNivel;
    public static event AccionGeneracion OnTerminarGeneracionNivel;

    private bool Pasar = false;

    private void Start()
    {
        ContadorActualBloquesIniciales = CantidadBloquesIniciales;
        CantidadVelocidadGeneracion = VelocidadGeneracion;
        Generar();
    }

    public void Generar()
    {
        if (OnIniciarGeneracionNivel != null)
            OnIniciarGeneracionNivel();

        if(ContadorActualBloquesIniciales < 16)
            ContadorActualBloquesIniciales += Random.Range(0,2);

        if (CantidadVelocidadGeneracion > .5f)
            CantidadVelocidadGeneracion -= 0.05f;

        Limpiar();
        InfoHabitacionActual = generacionCuarto.GenerarCuarto(PosicionInicial, AgrupadorCuarto);

        GenerarPuertas();
        Manejador.NoGenerar = false;

        if (OnTerminarGeneracionNivel != null)
            OnTerminarGeneracionNivel();

        Pasar = true;
    }

    public void Resetear()
    {
        ContadorActualBloquesIniciales = CantidadBloquesIniciales;
        CantidadVelocidadGeneracion = VelocidadGeneracion;
    }

    private void GenerarObstaculosIniciales()
    {
        List<int> EspaciosYaColocados = new List<int>();
        for (int contador = 0; contador < ContadorActualBloquesIniciales; contador++)
        {
            int indice = 0;
            do
            {
                indice = Random.Range(0, InfoHabitacionActual.EspaciosObstaculos.Count);
            } while (EspaciosYaColocados.Contains(indice) || NoAparecer(indice) || TapaPuerta(indice));
            EspaciosYaColocados.Add(indice);
        }
        foreach (int CIndice in EspaciosYaColocados)
        {
            GameObject Obstaculo = Instantiate(PrefabObstaculo,AgrupadorCuarto);
            Vector3 posInicial = InfoHabitacionActual.EspaciosObstaculos[CIndice].transform.position;
            Obstaculo.transform.position = new Vector3(posInicial.x, posInicial.y + .5f, posInicial.z);
            Obstaculo.transform.rotation = RotacionAleatoria();
        }
    }

    private void GenerarPuertas()
    {
        int IndiceCortar = InfoHabitacionActual.EspaciosPuertas.Count / 2 - 2;

        List<Transform> PosEntradas = new List<Transform>();
        List<Transform> PosSalidas = new List<Transform>();
        bool SaltarPrimero = true;

        for (int contador = 0; contador < InfoHabitacionActual.EspaciosPuertas.Count; contador++)
        {
            if(contador <= IndiceCortar)
                PosEntradas.Add(InfoHabitacionActual.EspaciosPuertas[contador]);
            else
            {
                if (SaltarPrimero || contador + 1 == InfoHabitacionActual.EspaciosPuertas.Count)
                {
                    SaltarPrimero = false;
                    continue;
                }
                PosSalidas.Add(InfoHabitacionActual.EspaciosPuertas[contador]);
            }
        }
        int IndiceEntrada = Random.Range(0, PosEntradas.Count);
        int IndiceSalida = Random.Range(0, PosSalidas.Count);
        GameObject PuertaEntrada = Instantiate(PrefabPuertaEntrada, AgrupadorCuarto);
        Vector3 posColocarEntrada = PosEntradas[IndiceEntrada].position;
        PuertaEntrada.transform.position = new Vector3(posColocarEntrada.x, posColocarEntrada.y - .49f, posColocarEntrada.z);

        GameObject PuertaSalida = Instantiate(PrefabPuertaSalida, AgrupadorCuarto);
        Vector3 posColocarSalida = PosSalidas[IndiceSalida].position;
        PuertaSalida.transform.position = new Vector3(posColocarSalida.x, posColocarSalida.y - .49f, posColocarSalida.z);
        PuertaSalida.transform.rotation = Quaternion.Euler(-90,270,0);
        InfoHabitacionActual.PosicionPuertaSalida = PuertaEntrada.transform.position;

        Destroy(PosEntradas[IndiceEntrada].gameObject);
        Destroy(PosSalidas[IndiceSalida].gameObject);

        GenerarJugador(PuertaEntrada.transform.position);
    }

    private void GenerarJugador(Vector3 PosicionPuertaEntrada)
    {
        GameObject Jugador = Instantiate(PrefabJugador,AgrupadorCuarto);
        Jugador.transform.position = new Vector3(PosicionPuertaEntrada.x,PosicionPuertaEntrada.y,PosicionPuertaEntrada.z + 1);
        InfoHabitacionActual.PosicionJugador = Jugador.transform.position;

        GenerarObstaculosIniciales();
    }

    public void Limpiar()
    {
        int cantidadHijos = AgrupadorCuarto.childCount;
        for (int contador = 0; contador < cantidadHijos; contador++)
            Destroy(AgrupadorCuarto.GetChild(contador).gameObject);
    }

    public bool NoAparecer(int indiceBloque)
    {
        float distanciaJugador = Vector3.Distance(InfoHabitacionActual.PosicionJugador, InfoHabitacionActual.EspaciosObstaculos[indiceBloque].position);
        Vector3 posicion = InfoHabitacionActual.EspaciosObstaculos[indiceBloque].position;
        bool Impacto = Physics.Raycast(new Vector3(posicion.x, posicion.y + 1, posicion.z), new Vector3(0, 0, 1), 3,LayerMask.NameToLayer("Salida"));
        return distanciaJugador < 2 || Impacto;
    }

    private bool TapaPuerta(int indice)
    {
        List<Vector3> posicionesBloqueadas = PuertaSalida.Instancia.PosicionesBloqueadas();
        Vector3 posicionBloque = InfoHabitacionActual.EspaciosObstaculos[indice].transform.position;
        posicionBloque = new Vector3(posicionBloque.x, posicionBloque.y + 1, posicionBloque.z);
        return posicionesBloqueadas.Contains(posicionBloque);
    }

    public Quaternion RotacionAleatoria()
    {
        Quaternion rotacion = new Quaternion();
        float y = Random.Range(0, RotacionesDisponibles.Length);
        rotacion = new Quaternion(0,y,0,0);
        return rotacion;
    }
}
