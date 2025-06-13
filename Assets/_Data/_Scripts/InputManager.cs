//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InputManager : ImpBehaviour
//{
//    [Header("InputManager")]
//    private static InputManager _instance;
//    public static InputManager Instance
//    {
//        get
//        {
//            if (_instance == null) Debug.LogError("Singleton instance has not been created yet!");
//            return _instance;
//        }
//    }

//    [SerializeField] protected Vector3 mouseWorldPos;
//    public Vector3 MouseWorldPos { get => mouseWorldPos; }

//    protected override void Awake()
//    {
//        base.Awake();
//        if (_instance == null)
//        {
//            _instance = this;
//            if (transform.parent == null) DontDestroyOnLoad(gameObject);
//            return;
//        }

//        if (_instance != this)
//        {
//            Debug.LogWarning("Another instance of InputManager already exists!");
//            Destroy(gameObject);
//            return;
//        }
//    }

//    private void FixedUpdate()
//    {
//        this.GetMousePos();
//    }

//    protected virtual void GetMousePos()
//    {
//        this.mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//    }
//}
