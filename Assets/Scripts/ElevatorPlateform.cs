using UnityEngine;

public class ElevatorPlateform : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Activate(bool value)
    {
        anim.SetBool("Active", value);
    }
}
