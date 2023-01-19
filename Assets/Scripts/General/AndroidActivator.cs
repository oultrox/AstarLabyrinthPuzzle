using UnityEngine;

public class AndroidActivator : MonoBehaviour
{
    [SerializeField] private GameObject prefabToEnable;
    void Start()
    {
#if UNITY_ANDROID
            prefabToEnable. gameObject.SetActive(true);
#else
        prefabToEnable.gameObject.SetActive(false);
#endif
    }

}
