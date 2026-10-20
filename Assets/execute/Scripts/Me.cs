using Unity.VisualScripting;
using UnityEngine;

public class Me : MonoBehaviour
{
    [SerializeField] public float volume = 1f;
    void Awake()
    {
        Me me = this;
        // World world = me.GetComponent<World>();

        me.AddComponent<World>().Execute(me);


        // world.Execute(me);
    }
}