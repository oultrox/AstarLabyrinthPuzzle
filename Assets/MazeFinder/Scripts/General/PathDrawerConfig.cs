using UnityEngine;

[CreateAssetMenu(fileName = "PathDrawerConfig", menuName = "PathDrawerConfig")]
public class PathDrawerConfig : ScriptableObject
{
    [SerializeField] private float _visualScale = 0.15f;
    [SerializeField] private GameObject _pathRender;
    [SerializeField] private int _skipNodes = 2;
    
    public float VisualScale => _visualScale;
    public GameObject PathRender => _pathRender;
    public int SkipNodes => _skipNodes;
}