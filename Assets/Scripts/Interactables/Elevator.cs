using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, IInteractable
{
    [Header("Internal Data")]
    [SerializeField] Position position;
    [SerializeField] Animator animator;


    enum Position { Up, Down }

    void Awake() => SetUp();

    public void Interact() => ToggleElevator();

    void ToggleElevator()
    {
        switch (position)
        {
            case Position.Up:
                animator.CrossFade("Elevator_Down_Anim", 0.5f);
                position = Position.Down;
                break;
            case Position.Down:
                animator.CrossFade("Elevator_Up_Anim", 0.5f);
                position = Position.Up;
                break;
            default:
                break;
        }
    }

    void SetUp()
    {
        animator = GetComponent<Animator>();
        ToggleElevator();
    }

}
