using UnityEngine;
using Unity.Cinemachine;
using System.Collections; // necessário para IEnumerator

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera[] cameras; 
    private int currentIndex = 0;
    public float switchTime = 5f; // tempo de exibição da câmera
    public float pauseTime = 4f;  // tempo de pausa após cada troca

    void Start()
    {
        if (cameras.Length > 0)
            StartCoroutine(SwitchCameras());
    }

    IEnumerator SwitchCameras()
    {
        while (true) // loop infinito
        {
            ActivateCamera(currentIndex);
            yield return new WaitForSeconds(switchTime); // mostra a câmera por X segundos

            // pausa extra
            yield return new WaitForSeconds(pauseTime);

            // troca para a próxima câmera
            currentIndex = (currentIndex + 1) % cameras.Length;
        }
    }

    void ActivateCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = (i == index) ? 10 : 0;
        }
    }
}
