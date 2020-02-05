using UnityEngine;
using UnityEngine.SceneManagement;

// DEBUG ONLY SCRIPT
public class DeadZone : MonoBehaviour
{
    public CanvasGroup UI;

    private void OnTriggerStay(Collider other)
    {
        Camera.main.transform.GetComponent<CameraFollow>().enabled = false;
        ShowUI();
    }

    private void ShowUI()
    {
        UI.alpha = Mathf.Lerp(0f, 1f, 15 * Time.deltaTime);
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
