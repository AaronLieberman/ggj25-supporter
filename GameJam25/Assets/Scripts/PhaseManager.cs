using System.Collections;
using UnityEngine;

public enum PhaseSkipTo
{
    None,
    StartCinematic,
    StartOfControl
}

public class PhaseManager : MonoBehaviour
{
    IntroFade _introfade;
    CameraController _camera;
    HeroController _hero;
    PlayerController _player;
    BossController _boss;

    public PhaseSkipTo SkipTo;

    // should have a DivingGear and a Throwable on it
    public DivingGear DivingGearPrefab;

    void Awake()
    {
        _introfade = Utilities.GetRootComponent<IntroFade>();
        _camera = Utilities.GetRootComponent<CameraController>();
        _hero = Utilities.GetRootComponent<HeroController>();
        _player = Utilities.GetRootComponent<PlayerController>();
        _boss = Utilities.GetRootComponent<BossController>();

        StartCoroutine(RunGame());
    }

    void Update()
    {
        
    }

    IEnumerator RunGame()
    {
        yield return Phase0();
        yield return Phase1();
    }

    IEnumerator Phase0()
    {
        if (SkipTo != PhaseSkipTo.None) Utilities.FastMode = true;

        _player.SetControlsEnabled(false);

        _introfade.enabled = true;
        yield return Utilities.DoAndWait(_introfade.Go());

        if (SkipTo == PhaseSkipTo.StartCinematic) Utilities.FastMode = false;

        // Sandy underwater background, basically the same left to right.

        // Hero floats down from the top, camera centered on him. The Hero sprite has a dive gear on. He stops in the middle of the room like he has landed on the bottom.
        yield return _hero.FloatTo(new Vector2(0.0f, 0.0f));
        _camera.Follow(_hero.gameObject);

        // Player character floats down from the top center
        var playerFloat = _player.FloatTo(-10, 0, 3);
        yield return Utilities.WaitForSeconds(1);

        //but the hero has started walking to the right and the camera follows the hero
        var heroWalk = _hero.WalkTo(new Vector2(10.0f, 0.0f));

        //The PC touches down in the center, but is already at the far left side of the screen since it is moving. The player gains movement and dash controls as soon as they touch down.
        yield return playerFloat;
        _player.SetControlsEnabled(true);

        if (SkipTo == PhaseSkipTo.StartOfControl) Utilities.FastMode = false;

        // The Hero walking right reveals The Leviathan. A Hero Dialog bubble pops up and says "Alright Leviathan, your reign of destruction is over! I'm here to slay you!" Dialog bubbles last 4 seconds then go away on their own.
        yield return heroWalk;
        yield return _hero.Say("Alright Leviathan, your reign of destruction is over! I'm here to slay you!", 4);
        // 4.5 seconds later a second hero dialog pops: "Oh, and that little guy over there is going to help, too.
        yield return _hero.Say("Oh, and that little guy over there is going to help, too.", 4.5f);
        // 4.5 seconds later a Leviathan Dialog Bubble pops that says "ROOOOARRRRRR!!!!"
        var bossRoar = _boss.Say("ROOOOARRRRRR!!!!", 4.5f);
        // Screen shake.
        yield return Utilities.WaitForSeconds(0.5f);
        yield return _camera.Shake(1);
        yield return bossRoar;

        // The diving gear flies off the Hero. It lands on the ground.
        var divingGearObject = Instantiate(DivingGearPrefab, _hero.gameObject.transform.position, Quaternion.identity);
        yield return divingGearObject.GetComponent<Throwable>().ThrowTo(-40, -20, 2);
        //Every 5 seconds a Oxygen Bubble comes out of the diving gear and begins moving slowly toward the top of the screen. The Hero Health Bar appears next to him at 100% and starts ticking down. About 30 seconds from 100% to 0%.
        // The first Oxygen Bubble has some text next to it that says OXYGEN >
        divingGearObject.GetComponent<DivingGear>().SetBubblesEnabled(true);

        // 5 additional seconds pass.
        yield return Utilities.WaitForSeconds(5);
                
        // Hero dialog bubble: 
        yield return _hero.Say("That thing was messing up my hair anyway.", 3);


    }

    IEnumerator Phase1()
    {
        yield return null;
    }
}
