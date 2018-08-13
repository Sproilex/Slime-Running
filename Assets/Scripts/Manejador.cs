using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AccionPerderPartida();
public delegate void AccionCambiarBotonMusica(bool Sonar);

public class Manejador : MonoBehaviour {

    public static bool NoGenerar = false;
    public static bool SonarEfectos = true;
    public static bool SonarMusica = true;
    public static event AccionCambiarBotonMusica OnCambiarBotonMusica;
    public static int PuntuacionMaxima { get; private set; }
    public static List<Vector3> PosicionesColocadas = new List<Vector3>();
    public static event AccionPerderPartida OnPerderPartida;

    public static void GanarPartida()
    {
        PuntuacionMaxima++;
        ManejadorHUD.Instancia.ColocarPuntuacion(PuntuacionMaxima);
        GeneracionNivel.Instancia.Generar();
        PosicionesColocadas.Clear();
    }

    public static void PerderPartida()
    {
        Autogeneracion.Eliminar = true;
        NoGenerar = true;
        PosicionesColocadas.Clear();
        int RecordAnterior = PlayerPrefs.GetInt("PuntuacionMaxima");
        bool SuperoRecord = PuntuacionMaxima > RecordAnterior;
        if (SuperoRecord)
            PlayerPrefs.SetInt("PuntuacionMaxima",PuntuacionMaxima);
        ManejadorHUD.Instancia.MostrarPantallaPerder(PuntuacionMaxima);
        ResetearPuntuacion();
        GeneracionNivel.Instancia.Resetear();
    }

    public static void CargarPuntuacion()
    {
        if (!PlayerPrefs.HasKey("PuntuacionMaxima"))
        {
            PlayerPrefs.SetInt("PuntuacionMaxima",0);
            PuntuacionMaxima = 0;
            return;
        }

        PuntuacionMaxima = PlayerPrefs.GetInt("PuntuacionMaxima");
    }

    public static void ResetearPuntuacion()
    {
        PuntuacionMaxima = 0;
    }

    public static int ObtenerMaximaPuntuacion()
    {
        return PlayerPrefs.GetInt("PuntuacionMaxima");
    }

    public static void ResetearEvento()
    {
        OnCambiarBotonMusica = null;
    }

    public static void CambiarOpcionMusica(bool Sonar)
    {
        SonarMusica = Sonar;
        if (OnCambiarBotonMusica != null)
            OnCambiarBotonMusica(Sonar);
    }
}
