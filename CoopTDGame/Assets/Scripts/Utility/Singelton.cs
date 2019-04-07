using UnityEngine;
 
/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// 
/// <see> http://wiki.unity3d.com/index.php/Singleton </see>
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour{

    // Check to see if singelton is about to be destroyed.
    private static bool willBeDestroyed = false;
    private static object lockObject = new object(); // Container to lock in the singelton while getting instances
    private static T _Instance; // Reference placeholder to handle instances of the singelton
 
    /// <summary> Access singleton instance through this propriety.</summary>
    public static T Instance{

        get{
            if (willBeDestroyed){ // Prevent accessing destroyed objects & refferences
                Debug.LogWarning("[Singleton] Instance " + typeof(T) + " already destroyed. Returning null.");
                return null;
            }
 
            lock (lockObject){ // Preventing other scripts (& threads to access the current locked object)

                if (_Instance == null){ // if there is no current running instance

                    _Instance = (T)FindObjectOfType(typeof(T)); // Find the existing instance
 
                    if (_Instance == null){ // Create new instance if none is found
                    
                        var singletonObject = new GameObject(); // Create new GameObject to attach singleton
                        _Instance = singletonObject.AddComponent<T>(); // attaching singelton
                        singletonObject.name = typeof(T).ToString() + " (Singleton)"; // naming singelton accordingly
 
                        DontDestroyOnLoad(singletonObject); // Make singelton instance persistent.
                    }
                } return _Instance; //returning new singelton instance
            } // releasing lock to grant access again
        }
    }
 
    
    private void OnApplicationQuit(){ // trigger enter destroyed prevention when quitting game
        willBeDestroyed = true;
    }
    
    private void OnDestroy(){  // trigger enter destroyed prevention when destroying object singelton is attached to
        //willBeDestroyed = true;
    }
}