using UnityEngine;
using System;

public class AtomosVisualAnimator : MonoBehaviour
{
    //script básico que dita as animações dos átomos, recebem eventos sinalizando toque do 
    private const string IS_BEING_HELD = "isBeingHeld";
    private const string IS_LAUNCHED = "isLaunched";

    private Animator animator;

    [SerializeField] private AtomoSO atomo; //trocar para um sricptableobject

    void Awake()
    {
        animator = GetComponent<Animator>();
    }    
    
    private void Start()
    {
        //atomo.OnLaunch += Atomo_OnLaunch;
    }

    private void Atomo_OnLaunch(object sender, System.EventArgs e)
    {
        animator.SetTrigger(IS_LAUNCHED);
    }

    private void Update()
    {
        //animator.SetBool(IS_BEING_HELD, atomo.IsBeingHeld()); //permite loop para ser segurado
    }
}
