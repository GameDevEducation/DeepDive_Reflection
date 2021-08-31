using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MarkedMethod]
    private int MethodToFind()
    {
        return Random.Range(1, 7);
    }

    [MarkedMethod]
    private int OtherMethodToFind()
    {
        return Random.Range(1, 7);
    }
    
    private string TestPrivateFunction(string personsName)
    {
        Debug.Log("TestPrivateFunction ran");

        return "Hello " + personsName;
    }
}
