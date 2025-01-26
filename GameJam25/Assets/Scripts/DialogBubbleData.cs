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
        new ("Hero_NeedsAir", 1f, "Breathing water is harder than I thought it would be." ),
        new ("Hero_NeedsAir", 1f, "I can just hold my breath, I'll be fine" ),
        new ("Hero_NeedsAir", 0.1f, "*gasp*" ),
        new ("Hero_NeedsAir", 0.4f, "What's the synonym for \"thirsty\", but for air?" ),
        new ("HeroWantsWeapon", 1f, "Fetch my sword!" ),
        new ("HeroWantsWeapon", 1f, "Arm me!" ),
        new ("HeroWantsWeapon", 1f, "Bring me a melee weapon!" ),
        new ("HeroWantsWeapon", 1f, "Time to attack up close!" ),
        new ("HeroWantsWeapon", 0.2f, "Mjölnir! I summon thee!" ),
        new ("HeroWantsWeapon", 1f, "I summon my blade!" ),
        new ("Hero_ReignOfDestruction", 1f, "Alright Leviathan, your reign of destruction is over! I'm here to slay you!" ),
        new ("Hero_IntroducePlayer", 1f, "Oh, and that little guy over there is going to help, too." ),
        new ("Boss_Roar", 1f, "ROOOOARRRRRR!!!" ),
        new ("Hero_LostDivingGear", 1f, "That thing was messing up my hair anyway." ),
        new ("Hero_LostDivingGear", 1f, "That thing was cramping my ears anyway." ),
        new ("Hero_NeedsAir", 1f, "Breathing water isn't as easy as I thought it would be." ),
        new ("Hero_Brag", 1f, "I'm going to crush you like clam shells!" ),
        new ("Hero_Brag", 1f, "I've got more abs than you have eyeballs!" ),
        new ("Hero_Brag", 1f, "Your squishy body is about to be calamari!" ),
        new ("Hero_Brag", 1f, "I'm going to make fish sauce out of you!" ),
        new ("Hero_Brag", 1f, "I'm the alpha wolf!" ),
        new ("Hero_Brag", 1f, "Bring it on! Try harder! Feel the burn!" ),
        new ("Hero_Brag", 1f, "I'm not impressed by your writhing mass of tentacles!" ),
        new ("Hero_Brag", 1f, "This is great, I don't even need to sweat down here." ),
        new ("Hero_Brag", 1f, "They'll sing sea shanties about how hard I'm going to hit you!" ),
        new ("Hero_Brag", 1f, "WARCRYYYYYY!!!!!" ),
        new ("Hero_StartPhase2", 1f, "I'm just getting started, little fish!" ),
        new ("Hero_StartPhase3", 1f, "Hmph. This gun isn't cutting it. Time to get personal!" ),
        new ("Hero_StartPhase4", 1f, "" ),
        new ("Hero_StartPhase5", 1f, "" ),
        new ("Hero_GetFlamingSwordSpecifically", 1f, "Fetch Me The Flaming Sword!" ),
        new ("Boss_WrongDamageType", 1f, "..." ),
        new ("Hero_AdmintsWrongDamageType", 1f, "That must not be the right damage type..." ),
        new ("Hero_SummonAllWeapons", 1f, "Fetch Me ALL THE WEAPONS" ),
        new ("Hero_UnarmedElement", 1f, "Too slippery for fists..." ),
        new ("Hero_CorrectElementAttack", 1f, "That's the right one!" ),
        new ("Hero_CorrectElementAttack", 1f, "That sinks in!" ),
        new ("Hero_CorrectElementAttack", 1f, "Don't like that, eh?" ),
        new ("Hero_CorrectElementAttack", 1f, "Does the baddy not like the elemental attacks?" ),
        new ("Hero_WrongElementAttack", 1f, "Gah! Bring me the right one next time!" ),
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
