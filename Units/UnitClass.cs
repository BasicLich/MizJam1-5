using System;
using System.Collections.Generic;

using static MizJam1.Units.Stats;

namespace MizJam1.Units
{
    public class UnitClass
    {
        public static readonly Dictionary<string, UnitClass> UnitClasses = new Dictionary<string, UnitClass>()
        {
            ["Warrior"] = new UnitClass("Warrior", (Attack, Defense), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 10,
                [Magic] = 0,
                [MagicDefense] = 3,
                [Stats.Range] = 0,
                [Speed] = 4
            }),
            ["Ranger"] = new UnitClass("Ranger", (Attack, Stats.Range), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 10,
                [Defense] = 0,
                [Magic] = 0,
                [MagicDefense] = 3,
                [Speed] = 4
            }),
            ["Zombie"] = new UnitClass("Zombie", (Defense, MagicDefense), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 4,
                [Attack] = 2,
                [Magic] = 0,
                [Stats.Range] = 0,
                [Speed] = 3
            })
        };


        public UnitClass(string name, (Stats, Stats) oppositeStats, Dictionary<Stats, ushort> defaultStatsValue)
        {
            Name = name;
            OppositeStats = oppositeStats;
            DefaultStatsValue = defaultStatsValue;
        }

        public string Name { get; set; }
        public (Stats, Stats) OppositeStats { get; set; }
        public Dictionary<Stats, ushort> DefaultStatsValue { get; set; }

        public Dictionary<Stats, ushort> GetStats(ushort leftOppositeStatValue)
        {
            Dictionary<Stats, ushort> stats = new Dictionary<Stats, ushort>();

            foreach (var stat in DefaultStatsValue)
            {
                stats.Add(stat.Key, stat.Value);
            }
            stats.Add(OppositeStats.Item1, leftOppositeStatValue);
            stats.Add(OppositeStats.Item2, (ushort)(7 - leftOppositeStatValue));

            return stats;
        }
    }
}
