using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Scene/Level Config")]
class LevelConfigSO : ScriptableObject
{
    public List<GameSceneSO> Levels;
    public GameSceneSO SafeHouse;
    public GameSceneSO Menu;

    public int LevelCount => Levels.Count;
}