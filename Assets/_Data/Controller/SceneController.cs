using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : ImpBehaviour
{
    private static SceneController _instance;
    public static SceneController Instance { get => _instance; }

    //[SerializeField] Animator transitionAnim;
    //[SerializeField] Canvas canvas;
    //[SerializeField] Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        if (SceneController._instance != null)
        {
            Debug.LogWarning("Only 1 SceneController allowed to exist");
            Destroy(gameObject);
            return;
        }
        SceneController._instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayGame()
    {
        this.LoadScene("Level");
    }

    //public void NextLevel()
    //{
    //    StartCoroutine(LoadSceneCoroutine("SelectLevel"));
    //}

    //IEnumerator LoadSceneCoroutine(string sceneName)
    //{
    //    AudioManager.Instance.StopMusic();
    //    transitionAnim.SetTrigger("End");
    //    yield return new WaitForSeconds(1);

    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
    //    yield return new WaitUntil(() => asyncLoad.isDone);

    //    yield return new WaitForSeconds(0.1f);
    //    LoadMainCamera();
    //    transitionAnim.SetTrigger("Start");
    //    AudioManager.Instance.PlayMusic(sceneName);
    //}

    //IEnumerator LoadSceneCoroutine(int sceneIndex)
    //{
    //    transitionAnim.SetTrigger("End");
    //    yield return new WaitForSeconds(1);

    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
    //    yield return new WaitUntil(() => asyncLoad.isDone);

    //    yield return new WaitForSeconds(0.1f);
    //    LoadMainCamera();
    //    transitionAnim.SetTrigger("Start");
    //}

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    public int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    //public void LoadMainCamera()
    //{
    //    mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
    //    if (mainCamera != null)
    //    {
    //        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    //        canvas.worldCamera = mainCamera;
    //    }
    //}
}
