using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class MenuBase : MonoBehaviour
{
    public GameObject debugMenu, MainMenu, LoadningScreen;
    
    public TMP_Text loadningScreenLog;
    public Slider loadningSlider;
    public string loadninglogText = "Loading progress: ", lastlog = "Loadning the last stuff";
    int loadningDotNumber = 1;
    public int maxLoadningDots = 3;

    public void loadScene(int num)
    {
        StartCoroutine(asynchronousLoad(num));
    }

    protected virtual void closeMenus() {
        debugMenu.SetActive(false);
        MainMenu.SetActive(false);
    }

    public virtual void exitApplication()
    {
        Debug.LogWarning("Due to being play in editor the application can not exit");
        Application.Quit();
    }

    IEnumerator asynchronousLoad(int num)
    {
        closeMenus();
        LoadningScreen.SetActive(true);

        float elapsedTime = 0f;
        string log = loadninglogText + "0% (0s)";
        loadningScreenLog.SetText(log);
        loadningSlider.value = 0;
        yield return null;
        AsyncOperation ao = SceneManager.LoadSceneAsync(num);
        ao.allowSceneActivation = false;
        yield return null;
        while (!ao.isDone)
        {

            if (ao.progress < 0.9f)
            {
                elapsedTime += Time.deltaTime;
                float realProgress = Mathf.Clamp01(ao.progress / 0.9f);
                float simulatedProgress = Mathf.Lerp(0f, 1f, elapsedTime / 10f);
                float displayProgress = Mathf.Max(realProgress, simulatedProgress);
                int progressProcent = (int)(displayProgress * 100);

                loadningSlider.value = progressProcent;
                log = $"{loadninglogText}{progressProcent}% ({Mathf.FloorToInt(elapsedTime)}s)";
                loadningScreenLog.SetText(log);
                yield return null;
            }
            else
            {
                float realProgress = Mathf.Clamp01(ao.progress / 0.9f);
                int progressProcent = (int)(realProgress * 100);
                loadningSlider.value = progressProcent;
                log = lastlog;
                for (int i = 0; i < loadningDotNumber; i++)
                {
                    log += ".";

                }
                yield return null;

                loadningScreenLog.SetText(lastlog);
                loadningDotNumber++;
                if (loadningDotNumber > maxLoadningDots)
                {
                    loadningDotNumber = 1;
                }
                yield return null;

                ao.allowSceneActivation = true;
                LoadningScreen.SetActive(false);
                yield return null;
            }

        }
    }
}
