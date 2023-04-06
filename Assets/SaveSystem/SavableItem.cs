using UnityEngine;
using NaughtyAttributes;

public abstract class SavableItem : MonoBehaviour
{
    [SerializeField,ReadOnly]
    private int id;

    [SerializeField, ReadOnly]
    private string guid;

    public int Id { get => id; set => id = value; }

    public string Guid { get => guid; set => guid = value; }

}
