using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockback
{
    void Knockback(Vector2 direction, float power, float duration);
}
