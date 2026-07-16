
using UnityEngine;

public interface IDrainable
{
    Vector2 Position{get;}
    void DrainBravery(float amount);
    void RestoreBravery(float amount);
}
