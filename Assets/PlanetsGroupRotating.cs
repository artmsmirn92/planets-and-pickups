using DG.Tweening;
using UnityEngine;

public class PlanetsGroupRotating : MonoBehaviour
{
    [SerializeField] private float    endAngle = 90f;
    [SerializeField] private float    duration = 5f;
    [SerializeField] private Ease     ease     = Ease.InOutBack;
    [SerializeField] private LoopType loopType = LoopType.Yoyo;
    
    private void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.DORotate(endAngle, duration).SetEase(ease).SetLoops(-1, loopType);
    }
}
