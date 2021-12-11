using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Reflection;
//using System.Xml.Serialization;
using UnityEngine;

namespace LevelInfo
{
    [CreateAssetMenu(fileName = "Data", menuName = " ScriptableObjects/LevelInfo", order = 1)]
    public class LevelContainer : ScriptableObject
    {
        [SerializeField]
        List<Tier> difficultyTiers = null;
        [SerializeField]
        List<Level> levels = null;

        public List<Tier> DifficultyTiers { get { return difficultyTiers; } }
        public List<Level> Levels { get { return levels; } }

    }

    [System.Serializable]
    public struct Tier
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public Enemy[] enemyPrefabs;
        [SerializeField]
        public Enemy[] bossPrefabs;
        [SerializeField]
        public Squad[] squads;
    }

    [System.Serializable]
    public struct Squad
    {
        [System.Serializable]
        public struct SquadEnemy
        {
            public Enemy enemy;
            public Vector2 spawnPosition;
        }

        public int squadID;
        public SquadEnemy[] squadMembers;
    }

    [System.Serializable]
    public struct EnemyInfo
    {
        public Enemy enemy;
        public int delay;
    }

    //Level information container
    [System.Serializable]
    public struct Level
    {
        public int difficulty;
        public int length;
        public bool containsBoss;
        [Tooltip("Frames between each spawn, higher numbers means slower spawn times.")]
        public int averageTime;

        public Level(int difficulty, int length, bool containsBoss, int averageTime)
        {
            this.difficulty = difficulty;
            this.length = length;
            this.containsBoss = containsBoss;
            this.averageTime = averageTime;
        }
    }

}