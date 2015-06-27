/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;

public abstract class AI : MonoBehaviour {

    [Tooltip("Initial state name.")]
    public string state;
    [Tooltip("Initial time state.")]
    public float timeState;
    [Tooltip("Enemy speed when walk or run.")]
    public float speed;
    [Tooltip("Enemy speed rotation")]
    public float rotation;
    [Tooltip("Minimum distance to attack the player")]
    public float distance;
    [Tooltip("Target of zombie.")]
    public Transform target;

    public abstract void StateMachine();
    public abstract void IA();
    public abstract void HitState(RaycastHit hit, string player);
    public abstract void setAIState(string ai_state);
    public abstract void setTimeRun(float time_run);
    public abstract void setTimeAttack(float time_attack);

}
