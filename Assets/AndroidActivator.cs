using UnityEngine;

public class AndroidActivator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    #if UNITY_ANDROID
        gameObject.SetActive(true);
    #else
        gameObject.SetActive(false);
    #endif
    }

}
