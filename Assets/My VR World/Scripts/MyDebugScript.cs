using UnityEngine;

public class MyDebugScript : MonoBehaviour
{

    public GameObject SimulatorObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
#if UNITY_EDITOR
        SimulatorObject.SetActive(true);
#endif

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
