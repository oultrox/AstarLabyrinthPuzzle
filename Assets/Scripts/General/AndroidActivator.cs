using UnityEngine;

public class AndroidActivator : MonoBehaviour
{
    void Start()
    {
        #if UNITY_ANDROID
            gameObject.SetActive(true);
        #else
            gameObject.SetActive(false);
        #endif
    }

}
