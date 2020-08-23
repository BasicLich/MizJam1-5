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
                [MaxHealth] = 14,
                [Magic] = 0,
                [MagicDefense] = 1,
                [Stats.Range] = 0,
                [Speed] = 4
            }),
            ["Paladin"] = new UnitClass("Paladin", (Defense, MagicDefense), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 16,
                [Attack] = 3,
                [Magic] = 0,
                [Stats.Range] = 0,
                [Speed] = 4
            }),
            ["Ranger"] = new UnitClass("Ranger", (Attack, Stats.Range), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 10,
                [Defense] = 2,
                [Magic] = 0,
                [MagicDefense] = 3,
                [Speed] = 4
            }),
            ["Mage"] = new UnitClass("Mage", (Magic, Speed), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 8,
                [Attack] = 0,
                [Defense] = 1,
                [MagicDefense] = 4,
                [Stats.Range] = 2
            }),
            ["Wizard"] = new UnitClass("Wizard", (Magic, Stats.Range), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 8,
                [Attack] = 0,
                [Defense] = 1,
                [MagicDefense] = 4,
                [Speed] = 3
            }),
            ["King"] = new UnitClass("King", (Attack, Magic), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 50,
                [Attack] = 4,
                [Defense] = 1,
                [MagicDefense] = 1,
                [Speed] = 6,
                [Stats.Range] = 3
            }),
            ["Zombie"] = new UnitClass("Zombie", (Defense, MagicDefense), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 4,
                [Attack] = 2,
                [Magic] = 0,
                [Stats.Range] = 0,
                [Speed] = 3
            }),
            ["Bat"] = new UnitClass("Bat", (MagicDefense, Speed), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 2,
                [Attack] = 1,
                [Magic] = 0,
                [Defense] = 1,
                [Stats.Range] = 0
            }),
            ["Spider"] = new UnitClass("Spider", (Defense, MagicDefense), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 15,
                [Attack] = 3,
                [Magic] = 0,
                [Stats.Range] = 1,
                [Speed] = 5
            }),
            ["BigSpider"] = new UnitClass("BigSpider", (Defense, MagicDefense), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 20,
                [Attack] = 4,
                [Magic] = 0,
                [Stats.Range] = 2,
                [Speed] = 4
            }),
            ["Imp"] = new UnitClass("Imp", (Speed, Stats.Range), new Dictionary<Stats, ushort>()
            {
                [MaxHealth] = 5,
                [Attack] = 2,
                [Magic] = 0,
                [Defense] = 1,
                [MagicDefense] = 2
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
