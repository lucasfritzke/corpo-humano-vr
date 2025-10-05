using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Header("Configurações de Orientação")]
    [Tooltip("Inverte o eixo X se o texto aparecer espelhado")]
    public bool invertX = false;

    [Tooltip("Inverte o eixo Y se o texto aparecer de cabeça para baixo")]
    public bool invertY = false;

    [Header("Travamento de Eixos")]
    [Tooltip("Trava rotação no eixo X (pitch - cima/baixo)")]
    public bool lockX = false;

    [Tooltip("Trava rotação no eixo Y (yaw - esquerda/direita)")]
    public bool lockY = false;

    [Tooltip("Trava rotação no eixo Z (roll - inclinação)")]
    public bool lockZ = false;

    private Camera mainCamera;

    void Start()
    {
        // Tenta encontrar a câmera principal
        FindMainCamera();
    }

    void FindMainCamera()
    {
        // Primeira tentativa: Camera.main
        mainCamera = Camera.main;

        // Segunda tentativa: procura por tag "MainCamera"
        if (mainCamera == null)
        {
            GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
            if (cameraObj != null)
                mainCamera = cameraObj.GetComponent<Camera>();
        }

        // Terceira tentativa: pega a primeira câmera da cena
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }

        if (mainCamera == null)
        {
            Debug.LogWarning($"Billboard em {gameObject.name}: Nenhuma câmera encontrada!");
        }
    }

    void LateUpdate()
    {
        // Se não tem câmera, tenta encontrar novamente
        if (mainCamera == null)
        {
            FindMainCamera();
            return;
        }

        // Calcula direção da câmera para este objeto
        Vector3 lookDirection = mainCamera.transform.position - transform.position;

        // Aplicar inversões se necessário (para corrigir orientação)
        if (invertX) lookDirection.x = -lookDirection.x;
        if (invertY) lookDirection.y = -lookDirection.y;

        // Bloquear eixos se necessário (para efeitos específicos)
        if (lockX) lookDirection.x = 0;
        if (lockY) lookDirection.y = 0;
        if (lockZ) lookDirection.z = 0;

        // Aplica a rotação apenas se há direção válida
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            // -lookDirection faz o objeto olhar PARA a câmera, não LONGE dela
            transform.rotation = Quaternion.LookRotation(-lookDirection);
        }
    }

    // Método para trocar a câmera alvo manualmente
    public void SetTargetCamera(Camera newCamera)
    {
        mainCamera = newCamera;
    }
}
