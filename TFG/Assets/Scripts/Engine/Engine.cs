using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Engine : MonoBehaviour {

    public abstract int getLevel();
    public abstract float getDifficulty();
    public abstract void setCredits(int credits);
    public abstract int getCredits();
    
    public abstract void setActivePlayers(int active);
    public abstract int getActivePlayers();
    
    public abstract void setPlayerAlive(Dictionary<string, bool> player_alive);
    public abstract Dictionary<string, bool> getPlayerAlive();

    public abstract void setPlayerScore(string controller, int score);
    public abstract int getPlayerScore(string controller);

    public abstract void setZombiePlayerKills(string controller);
    public abstract int getZombiePlayerKills(string controller);

    public abstract void setPlayerShoot(string controller);
    public abstract int getPlayerShoot(string controller);

    public abstract void setPlayerTouch(string controller);
    public abstract int getPlayerTouch(string controller);

    public abstract void setPlayerCombo(string controller, int combo);
    public abstract int getPlayerCombo(string controller);

    public abstract void ZombieEngine(int zombie);
   
}
