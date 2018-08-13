using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManejadorHUD : MonoBehaviour {

    private static ManejadorHUD _Instancia;
    public static ManejadorHUD Instancia
    {
        get
        {
            if (_Instancia == null)
                _Instancia = FindObjectOfType<ManejadorHUD>();

            return _Instancia;
        }
    }
    [SerializeField]
    private Text TextoPuntuacion;
    [SerializeField]
    private GameObject PanelPerder;
    [SerializeField]
    private Text TextoPuntuacionPerder;

    public void ColocarPuntuacion(int cantidad)
    {
        TextoPuntuacion.text = cantidad.ToString();
    }

    public void MostrarPantallaPerder(int Puntuacion)
    {
        PanelPerder.SetActive(true);
        TextoPuntuacionPerder.text = string.Format("{0} ROOMS", Puntuacion);
    }

    public void BotonVolverAJugar()
    {
        Autogeneracion.Eliminar = false;
        SceneManager.LoadScene(0);
    }
}
