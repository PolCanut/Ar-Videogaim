using UnityEngine;
using UnityEngine.InputSystem;

public class BalloonSpawner : MonoBehaviour
{
    [Header("Configuración de Spawner")]
    public GameObject balloonPrefab;
    public GameController gameController;
    public float spawnInterval = 1.5f; // Intervalo de tiempo entre la aparición de globos
    public Vector3 spawnArea = new Vector3(5f, 1f, 2f); // Área de aparición (ancho, alto, profundidad)

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBalloon();
            timer = 0f;
        }

        DetectBalloonTouch();
    }

    void SpawnBalloon()
    {
        Vector3 randomPosition = GetRandomPosition();

        GameObject newBalloon = Instantiate(balloonPrefab, randomPosition, Quaternion.identity);

        Balloon balloon = newBalloon.GetComponent<Balloon>();
        if (balloon != null && gameController != null)
        {
            balloon.OnPop += gameController.HandleBalloonPopped;
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
        float y = Random.Range(0, spawnArea.y);
        float z = Random.Range(-spawnArea.z / 2, spawnArea.z / 2);

        return transform.position + new Vector3(x, y, z);
    }

    void DetectBalloonTouch()
    {
        // Verificar si hay una pantalla táctil disponible
        if (Touchscreen.current != null)
        {
            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                ProcessTouch(touchPosition);
            }
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed) // Verificar también si hay un ratón conectado
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            ProcessTouch(mousePosition);
        }
        else
        {
            Debug.LogWarning("No input device detected: Touchscreen or Mouse.");
        }
    }


    void ProcessTouch(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Balloon")))
        {
            Balloon balloon = hit.collider.gameObject.GetComponent<Balloon>();
            if (balloon != null)
            {
                balloon.PopBalloon();
            }
        }
    }
}
