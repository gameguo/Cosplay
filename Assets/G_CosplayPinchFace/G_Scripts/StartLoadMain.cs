using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoadMain : MonoBehaviour
{
    void Start()
    {
#if UNITY_STANDALONE_WIN
        //int height = Display.displays[0].renderingHeight;
        //float sca = 9f / 16f;
        //int width = (int)(height * sca);
        //Debug.Log(width + " " + height);
        //transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = width + " " + height;
        Screen.SetResolution(1280, 720, false);
        sceneName = "Main_PC";
#endif
        StartCoroutine(Load());
    }
    private string sceneName = "Main";
    IEnumerator Load()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
