using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private bool canMove;
    [SerializeField] private bool mustMove;

    public bool CanMove => mustMove || (canMove && Random.Range(0, 2) == 1);
}