using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IAgentInput
{
    public UnityEvent<Vector2> OnMovementKeyPress { get; set; }
    public UnityEvent<Vector2> OnPointerPositionChanged { get; set; }

    //´Ü¹ßÇü ÃÑ, 
    public UnityEvent OnFireButtonPress { get; set; }
    public UnityEvent OnFireButtonRelease { get; set; }
}
