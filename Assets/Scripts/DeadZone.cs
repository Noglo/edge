using UnityEngine;
using UnityEngine.SceneManagement;

// DEBUG ONLY SCRIPT
public class DeadZone : MonoBehaviour
{
    public CanvasGroup UI;

    private void OnTriggerEnter(Collider other)
    {
        Camera.main.transform.GetComponent<CameraFollow>().enabled = false;
        ShowUI();
    }

    private void ShowUI()
    {
        UI.alpha = 1f;
        UI.interactable = !UI.interactable;
        UI.blocksRaycasts = !UI.blocksRaycasts;
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
