using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenesList : MonoBehaviour
{
    public float longPressThreshold = 3.0f; // 长按的阈值时间，单位为秒
    private bool isPressing = false; // 是否正在按下
    private float pressStartTime; // 按下的开始时间

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
            TextSceneCount.GetComponent<Text>().text = "场景个数 ："+ (sceneCount-1).ToString();
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


   

    void Update()
    {
        // 检测按下操作（触摸或鼠标）
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            isPressing = true; // 标记为正在按下
            pressStartTime = Time.time; // 记录按下的时间
        }

        // 如果正在按下，检查是否已经长按超过阈值
        if (isPressing)
        {
            float pressDuration = Time.time - pressStartTime; // 计算按下的持续时间

            if (pressDuration >= longPressThreshold)
            {
                OnLongPress(); // 触发长按事件
                isPressing = false; // 重置状态
            }
        }

        // 检测松开操作（触摸或鼠标）
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isPressing = false; // 重置按下状态
        }
    }

    // 长按操作的回调函数
    private void OnLongPress()
    {
        Debug.Log("Long Press Detected! Pressed for more than 3 seconds.");
        // 在这里实现长按超过3秒后的逻辑
        SceneManager.LoadScene(0);
    }
}
