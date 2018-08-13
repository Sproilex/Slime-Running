using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorMusica : MonoBehaviour {

    [SerializeField]
    private AudioClip MusicaSonar;
    private AudioSource Reproductor;
    [SerializeField]
    private AudioSource ReproductorEfecto;
    [SerializeField]
    private AudioClip SonidoBoton;

    private void Start()
    {
        Debug.Log(Manejador.SonarMusica);
        Reproductor = GetComponent<AudioSource>();
        Reproductor.clip = MusicaSonar;
        Manejador.OnCambiarBotonMusica += AlCambiarOpcion;
        if (Manejador.SonarMusica)
            Reproductor.Play();
    }

    public void AlCambiarOpcion(bool Sonar)
    {
        if (Sonar)
            Reproductor.Play();
        else
            Reproductor.Stop();
    }

    public void SonarBoton()
    {
        if (Manejador.SonarEfectos)
            ReproductorEfecto.PlayOneShot(SonidoBoton);
    }
}
