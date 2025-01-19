using UnityEngine;

public class TargetingSideAccesion : MonoBehaviour
{
    public DummyState state;
    [SerializeField] private Transform sidePosition;
    [SerializeField] private float minPos, maxPos;

    public Vector3 GetPosition()
    {
        Vector3 pos = new(sidePosition.position.x, 163.34f, sidePosition.position.z); 

        switch (state)
        {
            case DummyState.North:
            case DummyState.South:
                pos.x = Random.Range(minPos, maxPos);
                break;
            case DummyState.West:
            case DummyState.East:
                pos.z = Random.Range(minPos, maxPos);
                break;
        }

        return pos;
    }
}
