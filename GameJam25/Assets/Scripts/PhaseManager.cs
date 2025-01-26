using System.Collections;
using UnityEngine;

public enum PhaseSkipTo
{
    None,
    StartCinematic,
    StartOfControl,
    Phase1,
    Phase2,
    Phase3,
    Phase4,
    Phase5,
    Victory,
}

public class PhaseManager : MonoBehaviour
{
    IntroFade _introfade;
    CameraController _camera;
    HeroController _hero;
    ControlledMover _heroMover;
    PlayerController _player;
    ControlledMover _playerMover;
    BossController _boss;

    public PhaseSkipTo SkipTo;

    // should have a DivingGear and a Throwable on it
    public DivingGear DivingGearPrefab;

    void Awake()
    {
        _introfade = Utilities.GetRootComponent<IntroFade>();
        _camera = Utilities.GetRootComponentRecursive<CameraController>();
        _hero = Utilities.GetRootComponent<HeroController>();
        _heroMover = _hero.GetComponent<ControlledMover>();
        _player = Utilities.GetRootComponent<PlayerController>();
        _playerMover = _player.GetComponent<ControlledMover>();
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
        _heroMover.SnapTo(new Vector3(-65, 0, -32));
        _playerMover.SnapTo(new Vector3(-70, 0, -32));

        if (SkipTo != PhaseSkipTo.None) Utilities.FastMode = true;

        _player.SetControlsEnabled(false);

        _introfade.enabled = true;
        yield return Utilities.DoAndWait(_introfade.Go());

        if (SkipTo == PhaseSkipTo.StartCinematic) Utilities.FastMode = false;

        // Sandy underwater background, basically the same left to right.

        IEnumerator heroFloating = _heroMover.FloatTo(new Vector3(-55, 0, 0));

        yield return Utilities.WaitForSeconds(0.5f);

        // Player character floats down from the top center
        var playerFloat = _playerMover.FloatTo(new Vector3(-60, 0, 0));

        // Hero floats down from the top, camera centered on him. The Hero sprite has a dive gear on. He stops in the middle of the room like he has landed on the bottom.
        yield return heroFloating;
        _camera.Follow(_hero.gameObject);

        //but the hero has started walking to the right and the camera follows the hero
        var heroWalk = _heroMover.WalkTo(new Vector2(4.38f, 0.0f)); // This si where the hero should start walking.

        //yield return Utilities.WaitForSeconds(1);

        //The PC touches down in the center, but is already at the far left side of the screen since it is moving. The player gains movement and dash controls as soon as they touch down.
        yield return playerFloat;
        _player.SetControlsEnabled(true);

        if (SkipTo == PhaseSkipTo.StartOfControl) Utilities.FastMode = false;

        // The Hero walking right reveals The Leviathan. A Hero Dialog bubble pops up and says "Alright Leviathan, your reign of destruction is over! I'm here to slay you!" Dialog bubbles last 4 seconds then go away on their own.
        yield return heroWalk;  //TODO: For some reason the hero doens't start walking until we get here. But he should have been walking since we created this
        yield return _hero.Say("Hero_ReignOfDestruction");
        // 4.5 seconds later a second hero dialog pops: "Oh, and that little guy over there is going to help, too.

        //TODO: This is placeholder until the camera follow works
        _playerMover.SnapTo(new Vector2(-4.38f, 0.0f));

        yield return _hero.Say("Hero_IntroducePlayer");
        // 4.5 seconds later a Leviathan Dialog Bubble pops that says "ROOOOARRRRRR!!!!"
        var bossRoar = _boss.Say("Boss_Roar", 4.5f);
        // Screen shake.
        yield return Utilities.WaitForSeconds(0.5f);
        yield return _camera.Shake(1);
        yield return bossRoar;

        // The diving gear flies off the Hero. It lands on the ground.
        _hero.SetDivingGear(false);
        var divingGearObject = Instantiate(DivingGearPrefab, _hero.gameObject.transform.position, Quaternion.identity);
        yield return divingGearObject.GetComponent<Throwable>().ThrowTo(-40, -20, 2);
        //Every 5 seconds a Oxygen Bubble comes out of the diving gear and begins moving slowly toward the top of the screen. The Hero Health Bar appears next to him at 100% and starts ticking down. About 30 seconds from 100% to 0%.
        // The first Oxygen Bubble has some text next to it that says OXYGEN >
        divingGearObject.GetComponent<DivingGear>().SetBubblesEnabled(true);

        // 5 additional seconds pass.
        yield return Utilities.WaitForSeconds(5);
                
        // Hero dialog bubble: 
        yield return _hero.Say("Hero_LostDivingGear");


    }

    IEnumerator Phase1()
    {
        if (SkipTo == PhaseSkipTo.Phase1) Utilities.FastMode = false;

        IEnumerator phaseEnd = Utilities.WaitForSeconds(60.0f);

        //Sea stars begin firing randomly from a few points on the Leviathan's body, generally in the direction of the hero
        //(random -10 to 10 degrees off the hero's center). About 1 a second. Sea stars do 10 damage to the hero and player.
        _boss.StartShooters();

        _hero.StartBragging();

        // The Hero begins shooting his machine gun continuously.It fires bullets randomly in a 15 degree cone straight forward.
        // About 3 a second.
        _hero.StartShootingMachineGun();


        // 10 seconds after the start of the phase the Hero says "Breathing water isn't as easy as I thought it would be."
        yield return Utilities.WaitForSeconds(10.0f);
        yield return _hero.Say("Hero_NeedsAir");


        // If the hero hasn't spoken for 12 seconds, he brags about how great he is.

        // Phase 1 ends 60 seconds after it began.
        yield return phaseEnd;
    }
}
