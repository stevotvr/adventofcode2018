using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2018
{
    class Day24 : ISolution
    {
        private string[] input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
        {
            ParseInput(input, out var immune, out var infection, out var groups);
            Simulate(immune, infection, groups);
            return groups.Sum(x => x.Units);
        }

        public object Part2()
        {
            var i = 1;
            while (true)
            {
                ParseInput(input, out var immune, out var infection, out var groups);
                immune.ForEach(x => x.attackDamage += i);
                Simulate(immune, infection, groups);
                if (infection.Count == 0)
                {
                    return immune.Sum(x => x.Units);
                }

                i++;
            }
        }

        private static void Simulate(List<Group> immune, List<Group> infection, List<Group> groups)
        {
            while (immune.Count > 0 && infection.Count > 0)
            {
                foreach (var attacker in groups.OrderBy(x => -x.Power).ThenBy(x => -x.initiative))
                {
                    var targets = immune.Contains(attacker) ? infection : immune;
                    var sortedTargets = targets.Where(x => !x.Targeted).OrderBy(x => -x.CalcDamage(attacker)).ThenBy(x => -x.Power).ThenBy(x => -x.initiative);
                    if (sortedTargets.Count() > 0)
                    {
                        attacker.SetTarget(sortedTargets.First());
                    }
                }

                var units = groups.Sum(x => x.Units);
                foreach (var attacker in groups.OrderBy(x => -x.initiative))
                {
                    attacker.AttackTarget();
                }

                immune.RemoveAll(x => !x.IsAlive);
                infection.RemoveAll(x => !x.IsAlive);
                groups.RemoveAll(x => !x.IsAlive);

                if (units == groups.Sum(x => x.Units))
                {
                    break;
                }
            }
        }

        private static void ParseInput(string[] input, out List<Group> immune, out List<Group> infection, out List<Group> groups)
        {
            immune = new List<Group>();
            infection = new List<Group>();
            groups = new List<Group>();
            List<Group> current = null;
            foreach (var l in input)
            {
                if (string.IsNullOrEmpty(l))
                {
                    continue;
                }

                if (l[1] == 'm')
                {
                    current = immune;
                    continue;
                }

                if (l[1] == 'n')
                {
                    current = infection;
                    continue;
                }

                var group = GetGroup(l);
                current.Add(group);
                groups.Add(group);
            }
        }

        private static Group GetGroup(string description)
        {
            var m = Regex.Match(description, @"^([0-9]+) units each with ([0-9]+) hit points ?\(?(.*?)\)? with an attack that does ([0-9]+) ([a-z]+) damage at initiative ([0-9]+)$");
            var units = int.Parse(m.Groups[1].Value);
            var hp = int.Parse(m.Groups[2].Value);
            var attackDamage = int.Parse(m.Groups[4].Value);
            var attackType = damageMap[m.Groups[5].Value];
            var initiative = int.Parse(m.Groups[6].Value);

            Damage[] weaknesses = null;
            Damage[] immunities = null;
            if (!string.IsNullOrEmpty(m.Groups[3].Value))
            {
                foreach (var weakImmune in m.Groups[3].Value.Split(';').Select(x => x.Trim()))
                {
                    if (weakImmune[0] == 'w')
                    {
                        weaknesses = weakImmune.Substring(8).Split(',').Select(x => damageMap[x.Trim()]).ToArray();
                    }
                    else
                    {
                        immunities = weakImmune.Substring(10).Split(',').Select(x => damageMap[x.Trim()]).ToArray();
                    }
                }
            }

            return new Group(units, hp, weaknesses, immunities, attackType, attackDamage, initiative);
        }

        private class Group
        {
            private readonly int hp;
            private readonly Damage[] weaknesses;
            private readonly Damage[] immunities;

            private int units;
            private bool targeted;
            private Group target;

            public readonly Damage attackType;
            public readonly int initiative;

            public int attackDamage;

            public bool IsAlive { get => this.units > 0; }
            public int Units { get => this.units; }
            public bool Targeted { get => this.targeted; }
            public int Power { get => this.units * this.attackDamage; }

            public Group(int units, int hp, Damage[] weaknesses, Damage[] immunities, Damage attackType, int attackDamage, int initiative)
            {
                this.units = units;
                this.hp = hp;
                this.weaknesses = weaknesses ?? (new Damage[0]);
                this.immunities = immunities ?? (new Damage[0]);
                this.attackType = attackType;
                this.attackDamage = attackDamage;
                this.initiative = initiative;
            }

            public int CalcDamage(Group attacker)
            {
                if (this.immunities.Contains(attacker.attackType))
                {
                    return 0;
                }

                if (this.weaknesses.Contains(attacker.attackType))
                {
                    return attacker.Power * 2;
                }

                return attacker.Power;
            }

            public void SetTarget(Group target)
            {
                if (target.CalcDamage(this) <= 0)
                {
                    return;
                }

                this.target = target;
                target.targeted = true;
            }

            public void AttackTarget()
            {
                if (this.target != null)
                {
                    if (this.IsAlive && this.target.IsAlive)
                    {
                        var effectiveDamage = this.target.CalcDamage(this);
                        this.target.units -= effectiveDamage / this.target.hp;
                    }

                    this.target.targeted = false;
                    this.target = null;
                }
            }
        }

        private static readonly Dictionary<string, Damage> damageMap = new Dictionary<string, Damage>
        {
            { "cold", Damage.Cold },
            { "fire", Damage.Fire },
            { "radiation", Damage.Radiation },
            { "slashing", Damage.Slashing },
            { "bludgeoning", Damage.Bludgeoning },
        };

        private enum Damage
        {
            Cold,
            Fire,
            Radiation,
            Slashing,
            Bludgeoning,
        }
    }
}
