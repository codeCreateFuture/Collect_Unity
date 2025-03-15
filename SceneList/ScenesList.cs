using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenesList : MonoBehaviour
{
    public float longPressThreshold = 3.0f; // ��������ֵʱ�䣬��λΪ��
    private bool isPressing = false; // �Ƿ����ڰ���
    private float pressStartTime; // ���µĿ�ʼʱ��

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
            TextSceneCount.GetComponent<Text>().text = "�������� ��"+ (sceneCount-1).ToString();
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
        // ��ⰴ�²�������������꣩
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            isPressing = true; // ���Ϊ���ڰ���
            pressStartTime = Time.time; // ��¼���µ�ʱ��
        }

        // ������ڰ��£�����Ƿ��Ѿ�����������ֵ
        if (isPressing)
        {
            float pressDuration = Time.time - pressStartTime; // ���㰴�µĳ���ʱ��

            if (pressDuration >= longPressThreshold)
            {
                OnLongPress(); // ���������¼�
                isPressing = false; // ����״̬
            }
        }

        // ����ɿ���������������꣩
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isPressing = false; // ���ð���״̬
        }
    }

    // ���������Ļص�����
    private void OnLongPress()
    {
        Debug.Log("Long Press Detected! Pressed for more than 3 seconds.");
        // ������ʵ�ֳ�������3�����߼�
        SceneManager.LoadScene(0);
    }
}
