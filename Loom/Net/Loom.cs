﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;


/* 使用案例教程
 
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading;
public class testLoom : MonoBehaviour
{

    public Text mText;
    void Start () 
    {

        // 用Loom的方法调用一个线程
        Loom.RunAsync(
            () =>
            {
                Thread thread = new Thread(RefreshText);
                thread.Start();
            }
            );
    }
    private void RefreshText()
    {  
        // 用Loom的方法在Unity主线程中调用Text组件
        Loom.QueueOnMainThread((param) =>
            {
                mText.text = "Hello Loom!";
            },null);
    }
}

*/

/* 案例2
void ScaleMesh(Mesh mesh, float scale)  
{  
    //Get the vertices of a mesh  
    var vertices = mesh.vertices;  
    //Run the action on a new thread  
    Loom.RunAsync(()=>{  
        //Loop through the vertices  
        for(var i = 0; i < vertices.Length; i++)  
        {  
            //Scale the vertex  
            vertices[i] = vertices[i] * scale;  
        }  
        //Run some code on the main thread  
        //to update the mesh  
        Loom.QueueOnMainThread(()=>{  
            //Set the vertices  
            mesh.vertices = vertices;  
            //Recalculate the bounds  
            mesh.RecalculateBounds();  
        });  
   
    });  
}   
*/


public class Loom : MonoBehaviour
{
    public static int maxThreads = 8;
    static int numThreads;

    private static Loom _current;
    //private int _count;
    public static Loom Current
    {
        get
        {
            Initialize();
            return _current;
        }
    }

    //void Awake()
    //{
    //    _current = this;
    //    initialized = true;
    //}

    static bool initialized;

    public static void Initialize()
    {
        if (!initialized)
        {

            if (!Application.isPlaying)
                return;
            initialized = true;
            GameObject g = new GameObject("Loom");
            _current = g.AddComponent<Loom>();

            UnityEngine.Object.DontDestroyOnLoad(g);
        }

    }
    public struct NoDelayedQueueItem
    {
        public Action<object> action;
        public object param;
    }

    private List<NoDelayedQueueItem> _actions = new List<NoDelayedQueueItem>();
    public struct DelayedQueueItem
    {
        public float time;
        public Action<object> action;
        public object param;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    public static void QueueOnMainThread(Action<object> taction, object tparam)
    {
        QueueOnMainThread(taction, tparam, 0f);
    }
    public static void QueueOnMainThread(Action<object> taction, object tparam, float time)
    {
        if (time != 0)
        {
            lock (Current._delayed)
            {
                Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = taction, param = tparam });
            }
        }
        else
        {
            lock (Current._actions)
            {
                Current._actions.Add(new NoDelayedQueueItem { action = taction, param = tparam });
            }
        }
    }

    public static Thread RunAsync(Action a)
    {
        Initialize();
        while (numThreads >= maxThreads)
        {
            Thread.Sleep(100);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    }

    private static void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch
        {
        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }

    }


    void OnDisable()
    {
        if (_current == this)
        {

            _current = null;
        }
    }

    List<NoDelayedQueueItem> _currentActions = new List<NoDelayedQueueItem>();

    // Update is called once per frame
    void Update()
    {
        if (_actions.Count > 0)
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }
            for (int i = 0; i < _currentActions.Count; i++)
            {
                _currentActions[i].action(_currentActions[i].param);
            }
        }

        if (_delayed.Count > 0)
        {
            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
                for (int i = 0; i < _currentDelayed.Count; i++)
                {
                    _delayed.Remove(_currentDelayed[i]);
                }
            }

            for (int i = 0; i < _currentDelayed.Count; i++)
            {
                _currentDelayed[i].action(_currentDelayed[i].param);
            }
        }
    }
}