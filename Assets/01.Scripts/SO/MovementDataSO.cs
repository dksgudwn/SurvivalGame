using UnityEngine;

[CreateAssetMenu(menuName = "SO/Agent/MovementData")]
public class MovementDataSO : ScriptableObject
{
    [Range(1, 10)]
    public float _maxSpeed;
}
//���� Ƚ���� �̰����� �� �ؾ��ұ�