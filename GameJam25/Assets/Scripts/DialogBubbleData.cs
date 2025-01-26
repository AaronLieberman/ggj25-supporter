using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public static class DialogBubbleData
{
    static readonly List<Line> _lineData = new()
    {
        new ("Boss_Roar", 1, "ROOOOARRRRRR!!!" ),
        new ("Hero_IntroducePlayer", 1, "Oh, and that little guy over there is going to help, too." ),
        new ("Hero_LostDivingGear", 1, "That thing was cramping my ears anyway." ),
        new ("Hero_LostDivingGear", 1, "That thing was messing up my hair anyway." ),
        new ("Hero_NeedsAir", 1, "Breathing water isn't as easy as I thought it would be." ),
        new ("Hero_ReignOfDestruction", 1, "Alright Leviathan, your reign of destruction is over! I'm here to slay you!" ),
        new ("HeroOxygenLow", 0.1f, "*gasp*" ),
        new ("HeroOxygenLow", 0.4f, "What's the synonym for \"thirsty\", but for air?" ),
        new ("HeroOxygenLow", 1, "Breathing water is harder than I thought it would be." ),
        new ("HeroOxygenLow", 1, "I can just hold my breath, I'll be fine" ),
        new ("HeroWantsWeapon", 0.2f, "Mjölnir! I summon thee!" ),
        new ("HeroWantsWeapon", 1, "Arm me!" ),
        new ("HeroWantsWeapon", 1, "Bring me a melee weapon!" ),
        new ("HeroWantsWeapon", 1, "Fetch my sword!" ),
        new ("HeroWantsWeapon", 1, "I summon my blade!" ),
        new ("HeroWantsWeapon", 1, "Time to attack up close!" ),
    };

    struct Line
    {
        public readonly string Key;
        public readonly float Weight;
        public readonly string Text;

        public Line(string key, float weight, string text)
        {
            Key = key;
            Weight = weight;
            Text = text;
        }
    }
    
    static readonly Dictionary<string, List<Line>> _linesByKey = new();

    static DialogBubbleData()
    {
        var q =
            from line in _lineData
            group line by line.Key into g
            select new { g.Key, Lines = g };
        _linesByKey = q.ToDictionary(g => g.Key, g => g.Lines.ToList());
    }

    public static string GetLine(string key)
    {
        var lineGroup = _linesByKey.TryGetValue(key, out List<Line> lines) ? lines : null;
        if (lineGroup == null) return $"<unknown line: {key}>";

        var totalWeight = lineGroup.Sum(l => l.Weight);
        if (totalWeight <= 0) return $"<line weight <= 0: {key}>";

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        foreach (var line in lineGroup)
        {
            randomValue -= line.Weight;
            if (randomValue <= 0f)
            {
                return line.Text;
            }
        }

        throw new NotSupportedException("Shouldn't be possible to reach here");
    }
}
