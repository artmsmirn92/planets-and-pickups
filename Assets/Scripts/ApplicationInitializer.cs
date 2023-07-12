using mazing.common.Runtime.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationInitializer : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(SceneNames.Level);
    }
}