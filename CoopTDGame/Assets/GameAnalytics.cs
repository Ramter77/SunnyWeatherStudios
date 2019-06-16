using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameAnalytics : Singleton<GameAnalytics>
{
    public int playerDeath, playerRevive;
    public int playerJump, playerRangedAttack, playerMeleeAttack, playerHeal, playerUltimate, playerRiseTrap, playerRiseTower;
    public int trapCombinationFire, trapCombinationIce, trapCombinationBlast, towerCombinationFire, towerCombinationIce, towerCombinationBlast;
    public int meleeEnemyCombinationFire, meleeEnemyCombinationIce, meleeEnemyCombinationBlast, rangedEnemyCombinationFire, rangedEnemyCombinationIce, rangedEnemyCombinationBlast, bossEnemyCombinationFire, bossEnemyCombinationIce, bossEnemyCombinationBlast;
    public int enemyDeath;
    public int soulsPickedUp, soulsUsed;
    public int waveReached;

    public void PlayerDeath(int player) {
        playerDeath++;

        Analytics.CustomEvent("Player Death", new Dictionary<string, object>
        {
            { "Player", player },
            { "Deaths", playerDeath }
        });
    }

    public void PlayerRevive(int player) {
        playerRevive++;

        Analytics.CustomEvent("Player Revive", new Dictionary<string, object>
        {
            { "Player", player },
            { "Deaths", playerRevive }
        });
    }

    public void PlayerJump(int player) {
        playerJump++;

        Analytics.CustomEvent("Player Jump", new Dictionary<string, object>
        {
            { "Player", player },
            { "Jumps", playerJump }
        });
    }

    public void PlayerRangedAttack(int player) {
        playerRangedAttack++;

        Analytics.CustomEvent("Player Ranged Attack", new Dictionary<string, object>
        {
            { "Player", player },
            { "Ranged Attacks", playerRangedAttack }
        });
    }

    public void PlayerMeleeAttack(int player) {
        playerMeleeAttack++;

        Analytics.CustomEvent("Player Melee Attack", new Dictionary<string, object>
        {
            { "Player", player },
            { "Melee Attacks", playerMeleeAttack }
        });
    }

    public void PlayerHeal(int player) {
        playerHeal++;

        Analytics.CustomEvent("Player Heal Ability", new Dictionary<string, object>
        {
            { "Player", player },
            { "Heals", playerHeal }
        });
    }

    public void PlayerUltimate(int player) {
        playerUltimate++;

        Analytics.CustomEvent("Player Ultimate Ability", new Dictionary<string, object>
        {
            { "Player", player },
            { "Ultimates", playerUltimate }
        });
    }

    public void PlayerRise(int player, bool isTower) {
        if (!isTower) {
            playerRiseTrap++;
        }
        else
        {
            playerRiseTower++;
        }

        Analytics.CustomEvent("Player Rise Ability", new Dictionary<string, object>
        {
            { "Player", player },
            { "Risen Traps", playerRiseTrap },
            { "Risen Towers", playerRiseTower }
        });
    }

    public void PrefabCombination(Element element, bool isTower) {
        if (!isTower) {
            if (element == Element.Fire) {
                trapCombinationFire++;
            }
            else if (element == Element.Ice) {
                trapCombinationIce++;
            }
            else if (element == Element.Blast) {
                trapCombinationBlast++;
            }
        
            Analytics.CustomEvent("Combination with Trap", new Dictionary<string, object>
            {
                { "Fire Combinations", trapCombinationFire },
                { "Ice Combinations", trapCombinationIce },
                { "Blast Combinations", trapCombinationBlast }
            });
        }
        else {
            if (element == Element.Fire) {
                towerCombinationFire++;
            }
            else if (element == Element.Ice) {
                towerCombinationIce++;
            }
            else if (element == Element.Blast) {
                towerCombinationBlast++;
            }
        
            Analytics.CustomEvent("Combination with Tower", new Dictionary<string, object>
            {
                { "Fire Combinations", towerCombinationFire },
                { "Ice Combinations", towerCombinationIce },
                { "Blast Combinations", towerCombinationBlast }
            });
        }
    }

    public void EnemyCombination(Element element, int enemyType) {
        //Melee enemy
        if (enemyType == 0) {
            if (element == Element.Fire) {
                meleeEnemyCombinationFire++;
            }
            else if (element == Element.Ice) {
                meleeEnemyCombinationIce++;
            }
            else if (element == Element.Blast) {
                meleeEnemyCombinationBlast++;
            }
        
            Analytics.CustomEvent("Combination with Melee Enemy", new Dictionary<string, object>
            {
                { "Fire Combinations", meleeEnemyCombinationFire },
                { "Ice Combinations", meleeEnemyCombinationIce },
                { "Blast Combinations", meleeEnemyCombinationBlast }
            });
        }
        //Ranged enemy
        else if (enemyType == 1) {
            if (element == Element.Fire) {
                rangedEnemyCombinationFire++;
            }
            else if (element == Element.Ice) {
                rangedEnemyCombinationIce++;
            }
            else if (element == Element.Blast) {
                rangedEnemyCombinationBlast++;
            }
        
            Analytics.CustomEvent("Combination with Ranged Enemy", new Dictionary<string, object>
            {
                { "Fire Combinations", rangedEnemyCombinationFire },
                { "Ice Combinations", rangedEnemyCombinationIce },
                { "Blast Combinations", rangedEnemyCombinationBlast }
            });
        }
        //Boss enemy
        else if (enemyType == 0) {
            if (element == Element.Fire) {
                bossEnemyCombinationFire++;
            }
            else if (element == Element.Ice) {
                bossEnemyCombinationIce++;
            }
            else if (element == Element.Blast) {
                bossEnemyCombinationBlast++;
            }
        
            Analytics.CustomEvent("Combination with Boss Enemy", new Dictionary<string, object>
            {
                { "Fire Combinations", bossEnemyCombinationFire },
                { "Ice Combinations", bossEnemyCombinationIce },
                { "Blast Combinations", bossEnemyCombinationBlast }
            });
        }
    }

    public void EnemyDeath() {
        enemyDeath++;

        Analytics.CustomEvent("Enemy Death", new Dictionary<string, object>
        {
            { "Deaths", enemyDeath }
        });
    }

    public void SoulsPickedUp() {
        soulsPickedUp++;

        Analytics.CustomEvent("Souls Picked Up", new Dictionary<string, object>
        {
            { "Souls", soulsPickedUp }
        });
    }

    public void SoulsUsed(int souls) {
        soulsUsed += souls;

        Analytics.CustomEvent("Souls Used", new Dictionary<string, object>
        {
            { "Souls", soulsUsed }
        });
    }

    public void WaveReached() {
        waveReached ++;

        Analytics.CustomEvent("Wave Reached", new Dictionary<string, object>
        {
            { "Wave", waveReached }
        });
    }




    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Reset();
    }

    void Reset()
    {
        playerDeath = 0;
        playerRevive = 0;

        playerJump = 0;
        playerRangedAttack = 0;
        playerMeleeAttack = 0;
        playerHeal = 0;
        playerUltimate = 0;
        playerRiseTrap = 0;

        trapCombinationFire = 0;
        trapCombinationIce = 0;
        trapCombinationBlast = 0;
        towerCombinationFire = 0;
        towerCombinationIce = 0;
        towerCombinationBlast = 0;

        meleeEnemyCombinationFire = 0;
        meleeEnemyCombinationIce = 0;
        meleeEnemyCombinationBlast = 0;
        rangedEnemyCombinationFire = 0;
        rangedEnemyCombinationIce = 0;
        rangedEnemyCombinationBlast = 0;
        bossEnemyCombinationFire = 0;
        bossEnemyCombinationIce = 0;
        bossEnemyCombinationBlast = 0;

        enemyDeath = 0;
        soulsPickedUp = 0;
        soulsUsed = 0;
        waveReached = 0;
    }
}
