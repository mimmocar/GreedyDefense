using UnityEngine;
using UnityEngine.InputSystem;
public class AnimationScript : MonoBehaviour
{
    protected Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void OnWalk(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        anim.SetFloat("vert", movement.y);
        anim.SetFloat("hor", movement.x);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        float run = context.ReadValue<float>();
       
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        float jump = context.ReadValue<float>();
        
    }

}
