using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSingleton : MonoBehaviour
{
    private static PersistentSingleton _instance;

    public float longPressThreshold = 3.0f; // 长按的阈值时间，单位为秒
    private bool isPressing = false; // 是否正在按下
    private float pressStartTime; // 按下的开始时间

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
                    DontDestroyOnLoad(go); // 确保跨场景持久化
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
            DontDestroyOnLoad(gameObject); // 确保跨场景持久化
        }
        else
        {
            Destroy(gameObject); // 销毁多余的实例
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