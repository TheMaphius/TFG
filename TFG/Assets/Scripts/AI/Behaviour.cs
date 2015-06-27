/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;

public abstract class Behaviour : MonoBehaviour {

    [Tooltip("Zombie parts.")]
    public GameObject[] parts;
    [Tooltip("Life zombie parts.")]
    public float[] life;
    [Tooltip("Strenght of zombie attack.")]
    public float attack;
    [Tooltip("Score zombie parts.")]
    public int[] score;

    public abstract void ZombieDamage(string zombie_part, float damage);
    //public abstract void ZombieAttack();
    public abstract void ZombieDead(string player);
    //public abstract void ZombieDeadEffect();

    public abstract bool[] getZombieParts();
    public abstract void setZombieTarget(int target);
    public abstract void setZombieAttack(float attack);
    public abstract float getZombieAttack();
    public abstract void setPlayerTarget(int target);
}
