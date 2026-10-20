using UnityEngine;

public class Me : MonoBehaviour
{
    void Start()
    {
        Me me = this;
        World world = new World();

        world.Execute(me);
    }
}