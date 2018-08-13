using UnityEngine;

public class EfectosAntorchas : MonoBehaviour {

    [SerializeField]
    private float ValorMaximo;
    [SerializeField]
    private float ValorMinimo;
    private float ultimoValor;
    bool Restar = false;
    private Light Luz;

    private void Start()
    {
        Luz = GetComponent<Light>();
        ultimoValor = Luz.intensity;
    }

    private void Update()
    {
        //Titilar
        if (ultimoValor >= ValorMaximo)
            Restar = true;
        else if (ultimoValor <= ValorMinimo)
            Restar = false;

        if (Restar)
            ultimoValor -= 0.5f * Time.deltaTime;
        else
            ultimoValor += 0.5f * Time.deltaTime;

        Luz.intensity = ultimoValor;
    }
}
