using UnityEngine;

public class AndroidActivator : MonoBehaviour
{
    [SerializeField] private GameObject prefabToEnable;
    private void Start()
    {
        #if UNITY_ANDROID
            prefabToEnable.SetActive(true);
        #else
            prefabToEnable.SetActive(false);
        #endif
    }

}
