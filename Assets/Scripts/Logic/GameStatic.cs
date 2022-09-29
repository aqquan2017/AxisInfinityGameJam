using System.Collections.Generic;
using AxieMixer.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AnimationState{
    IDLE,
    ATTACK1,
    ATTACK2,
    HIT1,
    HIT2,
    WIN1,
    WIN2,
    LOSE,
    START_GAME,
    MOVE_FORWARD,
}

public class GameStatic : BaseManager<GameStatic>
{
    public int _curLevel = 0;
    public GameObject CurrentPlayer;

    private Dictionary<AnimationState, string> AnimationMapper;
    [SerializeField] private List<Sprite> AxieRescue = new List<Sprite>();
    [SerializeField] private List<string> AxieRescueQuote = new List<string>();
    
    private void Awake()
    {
        Mixer.Init();
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        AnimationMapper = new Dictionary<AnimationState, string>()
        {
            {AnimationState.IDLE , "action/idle/normal"},
            {AnimationState.ATTACK1 , "attack/melee/multi-attack"},
            {AnimationState.ATTACK2 , "attack/ranged/cast-high"},
            {AnimationState.HIT1 , "battle/get-debuff"},
            {AnimationState.HIT2 , "defense/hit-by-normal-crit"},
            {AnimationState.WIN1 , "battle/get-buff"},
            {AnimationState.WIN2 , "activity/evolve"},
            {AnimationState.START_GAME , "activity/appear"},
            {AnimationState.MOVE_FORWARD , "action/move-forward"},
        };
    }

    public void OnWinGame()
    {
        CurrentPlayer.GetComponent<PlayerMovement>().PlayerFrozen();
        SoundManager.Instance.Play(Sounds.WIN_LV);
        
        if (SceneController.Instance.CurrentScene + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            OnFinishTheGame();
            return;
        }

        int currentLv = (SceneController.Instance.CurrentScene - 1);
        if (currentLv == 5
            || currentLv == 12
            || currentLv == 18)
        {
            //Axie rescue
            OnFinishRescueLv();
            return;
        }

        OnNextLevel();
    }

    void OnNextLevel()
    {
        CircleTransition.Instance.FadeIn(onMidFadeIn:() => SoundManager.Instance.Play(Sounds.FadeIn)
            ,onEndFadeIn:() =>
            {
                SceneController.Instance.NextScene();
            });
    }

    void OnFinishTheGame()
    {
        UIManager.Instance.GetPanel<TextPopupPanel>().SetInfo("You have finished the game !!!" ,
            "Thanks for playing, Hero, you lead Axie to success!",
             ExitToGameMenu);
        UIManager.Instance.ShowPanelWithDG(typeof(TextPopupPanel));
    }

    private int _countMeetNewCharacter = 0;
    void OnFinishRescueLv()
    {
        int index = _countMeetNewCharacter;
        UIManager.Instance.GetPanel<AxieRescuePanel>().SetInfo("AXIE UNLOCK!" ,
            AxieRescueQuote[index], AxieRescue[index],
            OnNextLevel);
        UIManager.Instance.ShowPanelWithDG(typeof(AxieRescuePanel));
        _countMeetNewCharacter++;
    }
    
    public void OnHowToPlay()
    {
        UIManager.Instance.GetPanel<TextPopupPanel>().SetInfo("You have finished the game !!!" ,    
            "Thanks for playing, Hero, you lead Axie to success!",
            ExitToGameMenu);
        UIManager.Instance.ShowPanelWithDG(typeof(TextPopupPanel));
    }

    public void OnLoseGame()
    {
        CurrentPlayer.GetComponent<PlayerMovement>().PlayerFrozen();
        SoundManager.Instance.Play(Sounds.LOSE_LV);
        CircleTransition.Instance.FadeIn(onMidFadeIn:() => SoundManager.Instance.Play(Sounds.FadeIn)
            , onEndFadeIn:() => SceneController.Instance.ReloadScene());
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);        
        SceneController.Instance.OnChangeScene += OnChangeScene;
        SoundManager.Instance.Play(Sounds.LOSE_LV);
        SceneController.Instance.ChangeScene(1);
    }

    private void Update()
    {
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Escape))
        {
            ExitToGameMenu();
        }
        
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }
    }

    private bool canChangeScene = true;
    public void ExitToGameMenu()
    {
        if (SceneController.Instance.CurrentScene > 1 && canChangeScene)
        {
            canChangeScene = false;
            CircleTransition.Instance.FadeIn(onMidFadeIn:() => SoundManager.Instance.Play(Sounds.FadeIn),
                onEndFadeIn:() =>
                {
                    canChangeScene = true;
                    SceneController.Instance.ChangeScene(1);
                });
        }
    }

    private bool canResetScene = true;
    public void ResetGame()
    {
        if (SceneController.Instance.CurrentScene > 1 && canResetScene)
        {
            canResetScene = false;
            CircleTransition.Instance.FadeIn(onMidFadeIn:() => SoundManager.Instance.Play(Sounds.FadeIn),
                onEndFadeIn:() =>
                {
                    TimerManager.Instance.AddTimer(2f ,() => canResetScene = true);
                    SceneController.Instance.ReloadScene();
                });
        }
    }

    private void OnChangeScene(int sceneId)
    {
        CurrentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (CurrentPlayer)
        {
            CircleTransition.Instance._playerPos = CurrentPlayer.transform;
        }
        if(sceneId > 1)
            CircleTransition.Instance.FadeOut(onMidFadeOut:() => SoundManager.Instance.Play(Sounds.FadeOut));
    }

    public override void Init()
    {
        
    }
}
