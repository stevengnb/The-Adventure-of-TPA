using UnityEngine;

public class ItemsObject : MonoBehaviour
{
    private static ItemsObject instance;
    
    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
