using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [HideInInspector]
    public BoardFiller BoardFiller;
    [HideInInspector]
    public BallController BallController;
    [HideInInspector]
    public BoardInput BoardInput;
    [HideInInspector]
    public BoardQuery BoardQuery;

    public GameObject TilePrefab;
    public Vector2Int BoardSize;
    public Ball[,] CurrentBalls;

    public NumToImageString ScoreUI;
    public NumToImageString RecordUI;
    public GameObject GameOverUI;
    public NumToImageString GameOverScoreUI;
    public int Score = 0;
    public int Record = 0;

    public bool IsPlayingNow = false;

    public GameObject AlertLine;

    public static string RecordKey = "Record";
    public static string IsPlayingKey = "IsPlaying";
    public static string PresetIndexKey = "PresetIndex";
    public static string BallsDataKey = "BallsData";
    public static string LastScoreKey = "LastScore";
    public static string ShowADKey = "ShowAD";
    public static string SoundOnOffKey = "SoundOnOff";
    public static int ShowADTerm = 2;

    [SerializeField]
    private AudioSource m_PopAudioSource;

    [SerializeField]
    private AudioSource m_DumpAudioSource;

    public Image SoundUI;
    public Sprite[] SoundOnOffSprite;
    private bool IsSoundOn = true;

    private void Start()
    {

        CurrentBalls = new Ball[BoardSize.x, BoardSize.y];
        CreateTiles();

        CheckData();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && IsPlayingNow && BoardInput.IsInputableNow())
            SaveCurrentData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (IsPlayingNow && BoardInput.IsInputableNow())
            SaveCurrentData();
    }

    public void PlayPopSound()
    {
        m_PopAudioSource.Play();
    }

    public void PlayDumpSound()
    {
        m_DumpAudioSource.Play();
    }

    public void SoundOnOff()
    {
        if (IsSoundOn)
        {
            m_DumpAudioSource.volume = 0;
            SoundUI.sprite = SoundOnOffSprite[1];
            IsSoundOn = false;
            EncryptedPlayerPrefs.SetInt(SoundOnOffKey, 0);
        }
        else
        {
            m_DumpAudioSource.volume = 0.4f;
            SoundUI.sprite = SoundOnOffSprite[0];
            IsSoundOn = true;
            EncryptedPlayerPrefs.SetInt(SoundOnOffKey, 1);
        }

    }

    public void SoundSet(bool isOn)
    {
        if (isOn)
        {
            m_DumpAudioSource.volume = 0.4f;
            SoundUI.sprite = SoundOnOffSprite[0];
            IsSoundOn = true;
        }
        else
        {
            m_DumpAudioSource.volume = 0;
            SoundUI.sprite = SoundOnOffSprite[1];
            IsSoundOn = false;
        }
    }

    public void GameOver()
    {
        BoardInput.InputDisable();

#if UNITY_ANDROID

        try
        {
            var _adShowValue = EncryptedPlayerPrefs.GetInt(ShowADKey);
            if (_adShowValue <= 0)
            {
                if (OnGameStart.Instance != null && OnGameStart.ShowAD)
                {
                    OnGameStart.Instance.AddClosedCallbackPopupAD((sender, arg) =>
                    {
                        VeiwGameOverUI();
                        LetterBox.Instance.EnablePanel();
                        ClearSaveData();
                    });
                }
                else
                {
                    VeiwGameOverUI();
                    LetterBox.Instance.EnablePanel();
                    ClearSaveData();
                    EncryptedPlayerPrefs.SetInt(ShowADKey, --_adShowValue);
                }
                EncryptedPlayerPrefs.SetInt(ShowADKey, ShowADTerm);
            }
            else
            {
                VeiwGameOverUI();
                LetterBox.Instance.EnablePanel();
                ClearSaveData();
                EncryptedPlayerPrefs.SetInt(ShowADKey, --_adShowValue);
            }
        }
        catch (System.Exception)
        {

            throw;
        }
#endif

    }

    private void VeiwGameOverUI()
    {
        GameOverUI.SetActive(true);
        GameOverScoreUI.SetString(Score, true);
        if (Record == Score)
        {
            EncryptedPlayerPrefs.SetInt(RecordKey, Record);

#if UNITY_ANDROID
            try
            {

                if (OnGameStart.IsLoginGPGS)
                {
                    Social.ReportScore(Record, GPGSIds.leaderboard_best_score,
                        (success) =>
                        {
                            if (success)
                            {
                                    //리더보드 등록 성공
                            }
                            else
                            {
                                    //리더보드 등록 실패
                                    throw new System.Exception("리더보드 등록 에러");
                            }
                        });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
#endif
        }

        foreach (var _ball in CurrentBalls)
        {
            if (!ReferenceEquals(_ball, null))
                _ball.StopAfraidAnim();
        }
    }

    public string BallsToData()
    {
        string _result = "";

        for (int x = 0; x < BoardSize.x; x++)
        {
            for (int y = 0; y < BoardSize.y; y++)
            {
                if (ReferenceEquals(CurrentBalls[x, y], null))
                {
                    _result += '9';
                }
                else
                {
                    for (int i = 0; i < BoardFiller.CurrentBallPreset.Length; i++)
                    {
                        if (BoardFiller.CurrentBallPreset[i] == CurrentBalls[x, y].MatchColor)
                            _result += $"{i}";
                    }
                }
            }
        }

        return _result;
    }

    public void CheckData()
    {
        if (EncryptedPlayerPrefs.GetInt(IsPlayingKey) == 1)
        {
            LoadLastGame();
        }
        else
        {
            StartNewGame();
        }

        if (!EncryptedPlayerPrefs.HasKey(SoundOnOffKey))
            EncryptedPlayerPrefs.SetInt(SoundOnOffKey, 1);

        if (EncryptedPlayerPrefs.GetInt(SoundOnOffKey) == 1)
        {
            SoundSet(true);
        }
        else if (EncryptedPlayerPrefs.GetInt(SoundOnOffKey) == 0)
        {
            SoundSet(false);
        }
    }

    public void LoadLastGame()
    {
        Record = EncryptedPlayerPrefs.GetInt(RecordKey);
        RecordUI.SetString(Record);

        Score = EncryptedPlayerPrefs.GetInt(LastScoreKey);
        ScoreUI.SetString(Score);

        BoardFiller.SetPreset(EncryptedPlayerPrefs.GetInt(PresetIndexKey));
        BoardFiller.FillRowFromData(EncryptedPlayerPrefs.GetString(BallsDataKey));

        IsPlayingNow = true;
        ClearSaveData();
    }

    public void SaveCurrentData()
    {
        EncryptedPlayerPrefs.SetInt(IsPlayingKey, 1);
        EncryptedPlayerPrefs.SetInt(PresetIndexKey, BoardFiller.CurrentBallPresetIndex);
        EncryptedPlayerPrefs.SetInt(LastScoreKey, Score);
        EncryptedPlayerPrefs.SetInt(RecordKey, Record);
        EncryptedPlayerPrefs.SetString(BallsDataKey, BallsToData());
    }

    public void ClearSaveData()
    {
        EncryptedPlayerPrefs.SetInt(IsPlayingKey, 0);
        EncryptedPlayerPrefs.SetInt(PresetIndexKey, 0);
        EncryptedPlayerPrefs.SetInt(LastScoreKey, 0);
        EncryptedPlayerPrefs.SetString(BallsDataKey, "Null");
    }

    public void StartNewGame()
    {
        Record = EncryptedPlayerPrefs.GetInt(RecordKey);
        RecordUI.SetString(Record);
        ScoreUI.SetString(0);

        BoardFiller.SetPreset();
        BoardFiller.FillRow();

        IsPlayingNow = true;
    }

    public void AddScore()
    {
        ScoreUI.SetString(++Score);
        if (Record < Score)
        {
            RecordUI.SetString(Score);
            Record = Score;
        }
    }


    public Ball GetBall(Vector2Int pos)
    {
        return CurrentBalls[pos.x, pos.y];
    }

    private void CreateTiles()
    {
        for (int x = 0; x < BoardSize.x; x++)
        {
            for (int y = 0; y < BoardSize.y; y++)
            {
                var _createdTile = Instantiate(TilePrefab, transform);

                var _tile = _createdTile.GetComponent<Tile>();
                _tile.Init(this, new Vector2Int(x, y));
            }
        }
    }
}
