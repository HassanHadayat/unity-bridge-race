using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
   public void OnClick_RestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
