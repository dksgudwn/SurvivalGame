using UnityEngine;

[CreateAssetMenu(menuName = "SO/Agent/MovementData")]
public class MovementDataSO : ScriptableObject
{
    [Range(1, 10)]
    public float _maxSpeed;
}
//점프 횟수랑 이것저것 뭐 해야할까