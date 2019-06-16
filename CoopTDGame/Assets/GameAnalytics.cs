using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameAnalytics : Singleton<GameAnalytics>
{
    public int player1Death, player1Revive;
    public int player1Jump, player1RangedAttack, player1MeleeAttack, player1Heal, player1Ultimate, player1RiseTrap, player1RiseTower;

    public int player2Death, player2Revive;
    public int player2Jump, player2RangedAttack, player2MeleeAttack, player2Heal, player2Ultimate, player2RiseTrap, player2RiseTower;


    public int trapCombinationFire, trapCombinationIce, trapCombinationBlast, towerCombinationFire, towerCombinationIce, towerCombinationBlast;
    public int meleeEnemyCombinationFire, meleeEnemyCombinationIce, meleeEnemyCombinationBlast, rangedEnemyCombinationFire, rangedEnemyCombinationIce, rangedEnemyCombinationBlast, bossEnemyCombinationFire, bossEnemyCombinationIce, bossEnemyCombinationBlast;
    public int enemyDeath;
    public int soulsPickedUp, soulsUsed;
    public int waveReached;

    void TriggerCustomEvent(string eventName, string param1, int _param1, string param2, int _param2) {
        Analytics.CustomEvent(eventName, new Dictionary<string, object>
        {
            { param1, _param1 },
            { param2, _param2 }
        });
    }

    public void PlayerDeath(int player) {
        if (player == 1) {
            player1Death++;

            TriggerCustomEvent("Player1 Death", "Player", player, "Deaths", player1Death);
        }
        else if (player == 2) {
            player2Death++;

            TriggerCustomEvent("Player2 Death", "Player", player, "Deaths", player2Death);
        }
    }

    public void PlayerRevive(int player) {
        if (player == 1) {
            player1Revive++;

            TriggerCustomEvent("Player1 Revive", "Player", player, "Revives", player1Revive);
        }
        else if (player == 2) {
            player2Revive++;

            TriggerCustomEvent("Player2 Revive", "Player", player, "Revives", player2Revive);
        }
    }

    public void PlayerJump(int player) {
        if (player == 1) {
            player1Jump++;

            TriggerCustomEvent("Player1 Jump", "Player", player, "Jumps", player1Jump);
        }
        else if (player == 2) {
            player2Jump++;

            TriggerCustomEvent("Player2 Jump", "Player", player, "Jumps", player2Jump);
        }
    }

    public void PlayerRangedAttack(int player) {
        if (player == 1) {
            player1RangedAttack++;

            TriggerCustomEvent("Player1 Ranged Attack", "Player", player, "Ranged Attacks", player1RangedAttack);
        }
        else if (player == 2) {
            player2RangedAttack++;

            TriggerCustomEvent("Player2 Ranged Attack", "Player", player, "Ranged Attacks", player2RangedAttack);
        }
    }

    public void PlayerMeleeAttack(int player) {
        if (player == 1) {
            player1MeleeAttack++;

            TriggerCustomEvent("Player1 Melee Attack", "Player", player, "Melee Attacks", player1MeleeAttack);
        }
        else if (player == 2) {
            player2MeleeAttack++;

            TriggerCustomEvent("Player2 Melee Attack", "Player", player, "Melee Attacks", player2MeleeAttack);
        }
    }

    public void PlayerHeal(int player) {
        if (player == 1) {
            player1Heal++;

            TriggerCustomEvent("Player1 Heal", "Player", player, "Heals", player1Heal);
        }
        else if (player == 2) {
            player2Heal++;

            TriggerCustomEvent("Player2 Heal", "Player", player, "Heals", player2Heal);
        }
    }

    public void PlayerUltimate(int player) {
        if (player == 1) {
            player1Heal++;

            TriggerCustomEvent("Player1 Ultimate", "Player", player, "Ultimates", player1Heal);
        }
        else if (player == 2) {
            player2Heal++;

            TriggerCustomEvent("Player2 Ultimate", "Player", player, "Ultimates", player2Heal);
        }
    }

    public void PlayerRise(int player, bool isTower) {
        if (!isTower) {
            if (player == 1) {
                player1RiseTrap++;

                TriggerCustomEvent("Player1 Rise Trap", "Player", player, "Rise Traps", player1RiseTrap);
            }
            else if (player == 2) {
                player2RiseTrap++;

                TriggerCustomEvent("Player2 Rise Trap", "Player", player, "Rise Traps", player2RiseTrap);
            }
        }
        else
        {
            if (player == 1) {
                player1RiseTower++;

                TriggerCustomEvent("Player1 Rise Tower", "Player", player, "Rise Towers", player1RiseTower);
            }
            else if (player == 2) {
                player2RiseTower++;

                TriggerCustomEvent("Player2 Rise Tower", "Player", player, "Rise Towers", player2RiseTower);
            }
        }
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
    void Start()
    {
        Reset();
    }

    void Reset()
    {
        player1Death = 0;
        player1Revive = 0;
        player1Jump = 0;
        player1RangedAttack = 0;
        player1MeleeAttack = 0;
        player1Heal = 0;
        player1Ultimate = 0;
        player1RiseTrap = 0;
        player1RiseTower = 0;

        player2Death = 0;
        player2Revive = 0;
        player2Jump = 0;
        player2RangedAttack = 0;
        player2MeleeAttack = 0;
        player2Heal = 0;
        player2Ultimate = 0;
        player2RiseTrap = 0;
        player2RiseTower = 0;

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
