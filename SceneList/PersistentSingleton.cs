using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSingleton : MonoBehaviour
{
    private static PersistentSingleton _instance;

    public float longPressThreshold = 3.0f; // ��������ֵʱ�䣬��λΪ��
    private bool isPressing = false; // �Ƿ����ڰ���
    private float pressStartTime; // ���µĿ�ʼʱ��

    public static PersistentSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PersistentSingleton>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("PersistentSingleton");
                    _instance = go.AddComponent<PersistentSingleton>();
                    DontDestroyOnLoad(go); // ȷ���糡���־û�
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // ȷ���糡���־û�
        }
        else
        {
            Destroy(gameObject); // ���ٶ����ʵ��
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