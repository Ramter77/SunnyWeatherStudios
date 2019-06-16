using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameAnalytics : Singleton<GameAnalytics>
{
    #region Player1
    [System.Serializable]
    public class Player1 {
        public int playerDeath, playerRevive;
        public int playerJump, playerRangedAttack, playerMeleeAttack, playerHeal, playerUltimate, playerRiseTrap, playerRiseTower;
    }
    [SerializeField] public Player1 _Player1;
    #endregion

    #region Player2
    [System.Serializable]
    public class Player2 {
        public int playerDeath, playerRevive;
        public int playerJump, playerRangedAttack, playerMeleeAttack, playerHeal, playerUltimate, playerRiseTrap, playerRiseTower;
    }
    [SerializeField] public Player2 _Player2;
    #endregion

    


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
            _Player1.playerDeath++;

            TriggerCustomEvent("Player1 Death", "Player", player, "Deaths", _Player1.playerDeath);
        }
        else if (player == 2) {
            _Player2.playerDeath++;

            TriggerCustomEvent("Player2 Death", "Player", player, "Deaths", _Player2.playerDeath);
        }
    }

    public void PlayerRevive(int player) {
        if (player == 1) {
            _Player1.playerRevive++;

            TriggerCustomEvent("Player1 Revive", "Player", player, "Revives", _Player1.playerRevive);
        }
        else if (player == 2) {
            _Player2.playerRevive++;

            TriggerCustomEvent("Player2 Revive", "Player", player, "Revives", _Player2.playerRevive);
        }
    }

    public void PlayerJump(int player) {
        if (player == 1) {
            _Player1.playerJump++;

            TriggerCustomEvent("Player1 Jump", "Player", player, "Jumps", _Player1.playerJump);
        }
        else if (player == 2) {
            _Player2.playerJump++;

            TriggerCustomEvent("Player2 Jump", "Player", player, "Jumps", _Player2.playerJump);
        }
    }

    public void PlayerRangedAttack(int player) {
        if (player == 1) {
            _Player1.playerRangedAttack++;

            TriggerCustomEvent("Player1 Ranged Attack", "Player", player, "Ranged Attacks", _Player1.playerRangedAttack);
        }
        else if (player == 2) {
            _Player2.playerRangedAttack++;

            TriggerCustomEvent("Player2 Ranged Attack", "Player", player, "Ranged Attacks", _Player2.playerRangedAttack);
        }
    }

    public void PlayerMeleeAttack(int player) {
        if (player == 1) {
            _Player1.playerMeleeAttack++;

            TriggerCustomEvent("Player1 Melee Attack", "Player", player, "Melee Attacks", _Player1.playerMeleeAttack);
        }
        else if (player == 2) {
            _Player2.playerMeleeAttack++;

            TriggerCustomEvent("Player2 Melee Attack", "Player", player, "Melee Attacks", _Player2.playerMeleeAttack);
        }
    }

    public void PlayerHeal(int player) {
        if (player == 1) {
            _Player1.playerHeal++;

            TriggerCustomEvent("Player1 Heal", "Player", player, "Heals", _Player1.playerHeal);
        }
        else if (player == 2) {
            _Player2.playerHeal++;

            TriggerCustomEvent("Player2 Heal", "Player", player, "Heals", _Player2.playerHeal);
        }
    }

    public void PlayerUltimate(int player) {
        if (player == 1) {
            _Player1.playerHeal++;

            TriggerCustomEvent("Player1 Ultimate", "Player", player, "Ultimates", _Player1.playerHeal);
        }
        else if (player == 2) {
            _Player2.playerHeal++;

            TriggerCustomEvent("Player2 Ultimate", "Player", player, "Ultimates", _Player2.playerHeal);
        }
    }

    public void PlayerRise(int player, bool isTower) {
        if (!isTower) {
            if (player == 1) {
                _Player1.playerRiseTrap++;

                TriggerCustomEvent("Player1 Rise Trap", "Player", player, "Rise Traps", _Player1.playerRiseTrap);
            }
            else if (player == 2) {
                _Player2.playerRiseTrap++;

                TriggerCustomEvent("Player2 Rise Trap", "Player", player, "Rise Traps", _Player2.playerRiseTrap);
            }
        }
        else
        {
            if (player == 1) {
                _Player1.playerRiseTower++;

                TriggerCustomEvent("Player1 Rise Tower", "Player", player, "Rise Towers", _Player1.playerRiseTower);
            }
            else if (player == 2) {
                _Player2.playerRiseTower++;

                TriggerCustomEvent("Player2 Rise Tower", "Player", player, "Rise Towers", _Player2.playerRiseTower);
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
    void Awake()
    {
        Reset();
    }

    void Reset()
    {
        _Player1.playerDeath = 0;
        _Player1.playerRevive = 0;
        _Player1.playerJump = 0;
        _Player1.playerRangedAttack = 0;
        _Player1.playerMeleeAttack = 0;
        _Player1.playerHeal = 0;
        _Player1.playerUltimate = 0;
        _Player1.playerRiseTrap = 0;
        _Player1.playerRiseTower = 0;

        _Player2.playerDeath = 0;
        _Player2.playerRevive = 0;
        _Player2.playerJump = 0;
        _Player2.playerRangedAttack = 0;
        _Player2.playerMeleeAttack = 0;
        _Player2.playerHeal = 0;
        _Player2.playerUltimate = 0;
        _Player2.playerRiseTrap = 0;
        _Player2.playerRiseTower = 0;

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
