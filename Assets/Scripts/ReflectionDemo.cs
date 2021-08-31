using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.AttributeUsage(System.AttributeTargets.Method)]
public class MarkedMethodAttribute : System.Attribute
{

}

[System.AttributeUsage(System.AttributeTargets.Class)]
public class MarkedClassAttribute : System.Attribute
{
    public string TargetFunction;

    public MarkedClassAttribute(string _TargetFn)
    {
        TargetFunction = _TargetFn;
    }
}

[MarkedClass("FunctionToRun2")]
public class ToSearchFor
{
    [MarkedMethod]
    public static int FunctionToRun()
    {
        return Random.Range(1, 13);
    }

    public static int FunctionToRun2()
    {
        return Random.Range(1, 13);
    }    
}

public class ReflectionDemo : MonoBehaviour
{
    [SerializeField] ReflectionTarget Target;

    // Start is called before the first frame update
    void Start()
    {
        // traverse all types in our assembly
        var currentAssembly = Assembly.GetCallingAssembly();
        foreach (var candidateType in currentAssembly.GetTypes())
        {
            // skip if not class
            if (!candidateType.IsClass)
                continue;

            // check if this a marked class
            var classAttribute = candidateType.GetCustomAttribute<MarkedClassAttribute>();
            if (classAttribute != null)
            {
                var foundMethod = candidateType.GetMethod(classAttribute.TargetFunction, BindingFlags.Public | BindingFlags.Static);

                if (foundMethod != null)
                {
                    Debug.Log(candidateType.Name + ":" + foundMethod.Name + ": " + foundMethod.Invoke(null, null));
                }
            }

            // traverse all methods
            var classMethods = candidateType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach(var candidateMethod in classMethods)
            {
                // attempt to retrieve our attribute
                var foundAttribute = candidateMethod.GetCustomAttribute<MarkedMethodAttribute>();
                if (foundAttribute == null)
                    continue;

                Debug.Log(candidateType.Name + ":" + candidateMethod.Name + ": " + candidateMethod.Invoke(null, null));                
            }
        }

        // find and list out all private methods in the target class
        // var allMethods = typeof(ReflectionTarget).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
        // foreach(var method in allMethods)
        // {
        //     Debug.Log(method.Name);
        // }

        // find and run our private function
        // var foundMethod = typeof(ReflectionTarget).GetMethod("TestPrivateFunction", BindingFlags.NonPublic | BindingFlags.Instance);
        // if (foundMethod != null)
        // {
        //     var result = foundMethod.Invoke(Target, new object[] {"Iain"});
        //     Debug.Log(result);
        // }

        // find and run the method with the specific attribute
        // var allMethods = typeof(ReflectionTarget).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        // foreach(var method in allMethods)
        // {
        //     var foundAttribute = method.GetCustomAttribute<MarkedMethodAttribute>();
        //     if (foundAttribute == null)
        //         continue;

        //     Debug.Log(method.Name + ": " + method.Invoke(Target, null));
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
