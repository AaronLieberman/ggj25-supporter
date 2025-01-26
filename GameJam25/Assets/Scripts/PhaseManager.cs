using System.Collections;
using UnityEngine;

public enum PhaseSkipTo
{
    None,
    StartCinematic,
    StartOfControl,
    DivingGear,
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

    PeriodicSpawner _octosharkSpawner;

    public PhaseSkipTo SkipTo;
    public bool Invincible;

    // should have a DivingGear and a Throwable on it
    public GameObject DivingGearPrefab;

    public GameObject HelmetPrefab;

    void Awake()
    {
        _introfade = Utilities.GetRootComponent<IntroFade>();
        _camera = Utilities.GetRootComponentRecursive<CameraController>();
        _hero = Utilities.GetRootComponent<HeroController>();
        _heroMover = _hero.GetComponent<ControlledMover>();
        _player = Utilities.GetRootComponent<PlayerController>();
        _playerMover = _player.GetComponent<ControlledMover>();
        _boss = Utilities.GetRootComponent<BossController>();

        _octosharkSpawner = GameObject.Find("Terrain").GetComponentInChildren<PeriodicSpawner>();
    }

    private void Start()
    {
        StartCoroutine(RunGame());
    }

    IEnumerator RunGame()
    {
        yield return Phase0();
        yield return Phase1();
        yield return Phase2();
        Utilities.GetRootComponent<YouWin>().ShowYouWin();
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

        var heroFloating = StartCoroutine(_heroMover.FloatTo(new(-55, 0, 0)));

        yield return Utilities.WaitForSeconds(1);

        // Player character floats down from the top center
        var playerFloat = StartCoroutine(_playerMover.FloatTo(new(-60, 0, 0)));

        // Hero floats down from the top, camera centered on him. The Hero sprite has a dive gear on. He stops in the middle of the room like he has landed on the bottom.
        yield return heroFloating;
        _camera.Follow(_hero.gameObject);

        //but the hero has started walking to the right and the camera follows the hero
        var heroWalk = StartCoroutine(_heroMover.WalkTo(new(4.38f, 0.0f)));

        yield return Utilities.WaitForSeconds(1);

        //The PC touches down in the center, but is already at the far left side of the screen since it is moving. The player gains movement and dash controls as soon as they touch down.
        yield return playerFloat;
        var playerIntroWait = StartCoroutine(_playerMover.WalkTo(new(-3, 0.0f)));

        // The Hero walking right reveals The Leviathan. A Hero Dialog bubble pops up and says "Alright Leviathan, your reign of destruction is over! I'm here to slay you!" Dialog bubbles last 4 seconds then go away on their own.
        yield return heroWalk; 
        yield return _hero.Say("Hero_ReignOfDestruction");
        // 4.5 seconds later a second hero dialog pops: "Oh, and that little guy over there is going to help, too.

        //TODO: This is placeholder until the camera follow works
        //_playerMover.SnapTo(new Vector2(-4.38f, 0.0f));
        yield return playerIntroWait;
        _player.SetControlsEnabled(true);

        if (SkipTo == PhaseSkipTo.StartOfControl) Utilities.FastMode = false;

        yield return _hero.Say("Hero_IntroducePlayer");
        // 4.5 seconds later a Leviathan Dialog Bubble pops that says "ROOOOARRRRRR!!!!"
        var bossRoar = StartCoroutine(_boss.Say("Boss_Roar"));
        // Screen shake.
        yield return Utilities.WaitForSeconds(0.5f);
        // The diving gear flies off the Hero. It lands on the ground.
        _hero.LoseDivingGear();
        _hero.GetComponent<PeriodicDamager>().SetEnabled(true);
        var divingGearObject1 = Instantiate(DivingGearPrefab, _hero.gameObject.transform.position, Quaternion.identity);
        var divingGearObject2 = Instantiate(DivingGearPrefab, _hero.gameObject.transform.position, Quaternion.identity);
        var helmetObject = Instantiate(HelmetPrefab, _hero.gameObject.transform.position, Quaternion.identity);
        var divingGearObject1Wait = StartCoroutine(divingGearObject1.GetComponent<ControlledMover>().ThrowTo(new(-10, 7.5f)));
        var divingGearObject2Wait = StartCoroutine(divingGearObject2.GetComponent<ControlledMover>().ThrowTo(new(-12, -5)));
        helmetObject.GetComponent<ControlledMover>().SnapTo(_hero.transform.position + new Vector3(0, 7, 0));
        var helmetWait = StartCoroutine(helmetObject.GetComponent<ControlledMover>().ThrowTo(new(-18, -3)));
        yield return _camera.Shake(1);
        yield return bossRoar;

        if (SkipTo == PhaseSkipTo.DivingGear) Utilities.FastMode = false;
 
        yield return divingGearObject1Wait;
        yield return divingGearObject2Wait;
        yield return helmetWait;
        //Every 5 seconds a Oxygen Bubble comes out of the diving gear and begins moving slowly toward the top of the screen. The Hero Health Bar appears next to him at 100% and starts ticking down. About 30 seconds from 100% to 0%.
        divingGearObject1.GetComponent<PeriodicSpawner>().SetEnabled(true);
        divingGearObject2.GetComponent<PeriodicSpawner>().SetEnabled(true);

        // The first Oxygen Bubble has some text next to it that says OXYGEN >

        //Sea stars begin firing randomly from a few points on the Leviathan's body, generally in the direction of the hero
        //(random -10 to 10 degrees off the hero's center). About 1 a second. Sea stars do 10 damage to the hero and player.
        _boss.StartShooters();

        // Hero dialog bubble: 
        yield return _hero.Say("Hero_LostDivingGear");

        // 5 additional seconds pass.
        //yield return Utilities.WaitForSeconds(3);
    }

    IEnumerator Phase1()
    {
        if (SkipTo == PhaseSkipTo.Phase1) Utilities.FastMode = false;

        var phaseEnd = StartCoroutine(Utilities.WaitForSeconds(40.0f));

        _hero.StartBragging();

        // The Hero begins shooting his machine gun continuously.It fires bullets randomly in a 15 degree cone straight forward.
        // About 3 a second.
        _hero.SetMachineGunActive(true);

        // 10 seconds after the start of the phase the Hero says "Breathing water isn't as easy as I thought it would be."
        yield return Utilities.WaitForSeconds(10.0f);
        yield return _hero.Say("Hero_NeedsAir");

        // If the hero hasn't spoken for 12 seconds, he brags about how great he is.

        // Phase 1 ends 60 seconds after it began.
        yield return phaseEnd;
    }

    IEnumerator Phase2()
    {
        if (SkipTo == PhaseSkipTo.Phase2) Utilities.FastMode = false;

        var bossRoar = StartCoroutine(_boss.Say("Boss_Roar"));
        // Screen shake.
        yield return Utilities.WaitForSeconds(0.5f);
        yield return _camera.Shake(1);
        yield return bossRoar;

        yield return _hero.Say("Hero_StartPhase2");

        //Octoshark Minions(Half Octopus, 1 / 8th Shark) begin to spawn from the Leviathan. 
        _octosharkSpawner.SetEnabled(true);

        yield return Utilities.WaitForSeconds(45f);
    }

    // IEnumerator Phase3()
    // {
    //     if (SkipTo == PhaseSkipTo.Phase3) Utilities.FastMode = false;

    //     yield return _hero.Say("Hero_StartPhase3");
    //     yield return _hero.Say("Hero_GetFlamingSwordSpecifically");

    //     // puts his hand up in the air. He stop shooting the machine gun.

    //     //A Flaming Sword floats down from the surface and lands somewhere in the backfield.

    //     //10 seconds after the sword lands, the hero swings(whether he has the sword or not).

    //     Coroutine bossRoar;
    //     //If wrong damage type
    //     bossRoar = StartCoroutine(_boss.Say("Boss_WrongDamageType", 4.5f));
    //     yield return _hero.Say("Boss_WrongDamageType");

    //     //If correct damage type
    //     bossRoar = StartCoroutine(_boss.Say("Boss_Roar", 4.5f));
    //     // Screen shake.
    //     yield return Utilities.WaitForSeconds(0.5f);
    //     yield return _camera.Shake(1);
    //     yield return bossRoar;

    //     // hero throws the weapon somewhere random on the screen.
    //     // Then he says "Fetch Me ALL THE WEAPONS",
    //     yield return _hero.Say("Hero_SummonAllWeapons");
    //     // then weapons float down from above:


    //     // A cycle begins:
    //     //        The hero shoots for 10 seconds.
    //     //        The hero stops shooting and holds his hand in the air waiting for a weapon.
    //     //5 seconds later the hero dashes forward and swings at the Leviathan
    //     //        If the Hero has no weapon, he does no damage and says an "UnarmedElement" line
    //     yield return _hero.Say("Hero_UnarmedElement");
    //     //        If the hero has a weapons that beats the current color of the boss, the boss shakes and roars, hero says "Hero_CorrectElementAttack" line
    //     yield return _hero.Say("Hero_CorrectElementAttack");
    //     //        If the hero has the wrong color weapon, he says "Hero_WrongElementAttack" line.
    //     yield return _hero.Say("Hero_WrongElementAttack");

    //     //        The hero throws the weapon a random place on the level(potentially outside the viewport)
    //     //        The hero moves quickly to a new location(and the screen / camera moves with him)

    //     //        Each attack the boss randomly selects a color that is different than the current color, options are Black, Blue, or Green.

    //     //        After 5 correct element hits, phase 3 ends.


    // }

    // IEnumerator Phase4()
    // {
    //     if (SkipTo == PhaseSkipTo.Phase4) Utilities.FastMode = false;

    //     yield return _hero.Say("Hero_StartPhase4");
    // }

    // IEnumerator Phase5()
    // {
    //     if (SkipTo == PhaseSkipTo.Phase5) Utilities.FastMode = false;

    //     yield return _hero.Say("Hero_StartPhase5");
    // }
}
