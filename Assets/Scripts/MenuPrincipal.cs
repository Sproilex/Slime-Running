using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour {

    [SerializeField]
    private Text TextoRecord;
    [SerializeField]
    private Image ImagenSonido;
    [SerializeField]
    private Sprite IconoSonido;
    [SerializeField]
    private Sprite IconoSonidoDesactivado;
    [SerializeField]
    private Image ImagenEfecto;
    [SerializeField]
    private Sprite IconoEfecto;
    [SerializeField]
    private Sprite IconoEfectoDesactivado;


    private void Start()
    {
        Manejador.CargarPuntuacion();
        Manejador.ResetearPuntuacion();
        Manejador.ResetearEvento();
        TextoRecord.text = string.Format("RECORD {0} ROOMS",Manejador.ObtenerMaximaPuntuacion());
        ManejarIconoMusica(Manejador.SonarMusica);
        ManejarIconoEfecto(Manejador.SonarEfectos);
    }

    public void BotonJugar()
    {
        SceneManager.LoadScene(1);
    }

    public void ManejarMusica()
    {
        bool Sonando = Manejador.SonarMusica;
        Sonando = !Sonando;
        Manejador.CambiarOpcionMusica(Sonando);
        ManejarIconoMusica(Sonando);
    } 

    public void ManejarEfectos()
    {
        bool Sonando = Manejador.SonarEfectos;
        Sonando = !Sonando;
        Manejador.SonarEfectos = Sonando;
        ManejarIconoEfecto(Sonando);
    }

    private void ManejarIconoMusica(bool Activo)
    {
        if (Activo)
            ImagenSonido.sprite = IconoSonido;
        else
            ImagenSonido.sprite = IconoSonidoDesactivado;
    }

    private void ManejarIconoEfecto(bool Activo)
    {
        if (Activo)
            ImagenEfecto.sprite = IconoEfecto;
        else
            ImagenEfecto.sprite = IconoEfectoDesactivado;
    }
}
