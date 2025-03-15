using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenesList : MonoBehaviour
{
   

    public Transform SceneListScrollView;
    // Start is called before the first frame update
    void Start()
    {

        if (SceneListScrollView == null) return;

        GameObject sceneItem = SceneListScrollView.Find("SceneItem").gameObject;
        Transform SceneListContent = SceneListScrollView.Find("Viewport/Content").transform;



        int sceneCount = SceneManager.sceneCountInBuildSettings;
        Debug.Log("Total Scenes in Build Settings: " + sceneCount);

        GameObject TextSceneCount = SceneListScrollView.Find("TextSceneCount").gameObject;
        if(TextSceneCount != null)
        {
            TextSceneCount.GetComponent<Text>().text = "³¡¾°¸öÊý £º"+ (sceneCount-1).ToString();
        }


        for (int i = 0; i < sceneCount; i++)
        {
            if (i != 0)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                Debug.Log("Scene Name: " + sceneName + ", Path: " + scenePath);

                Button item = Instantiate(sceneItem, SceneListContent).GetComponent<Button>();
                item.gameObject.SetActive(true);
                item.GetComponentInChildren<Text>().text = sceneName;
                item.onClick.AddListener(() => {
                    SceneManager.LoadScene(sceneName);
                });
            }
 
        }
    }


   

   
}
