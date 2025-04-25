using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{

    public GameObject GoFish;
    public GameObject Fishing;
    public HandManager playerHand;
    public AIManager aiHand;
    public DeckManager deck;
    public TurnManager turn;
    public ResetEnvirement reset;
    public GameObject end;
    public StatManager stats;
    public TextMeshProUGUI PlayerScore;
    public TextMeshProUGUI AIScore;


    public bool Final = false;

    public bool Playerdone = false;
    public bool Aidone = false;
    private bool isReset = false;
    private int i;

    // Start is called before the first frame update
    void Start()
    {
        GoFish.SetActive(true);
        Fishing.SetActive(false);
    }

    public void ChangeScene()
    {
        GoFish.SetActive(!GoFish.activeSelf);
        Fishing.SetActive(!Fishing.activeSelf);

        if (GoFish.activeSelf)
        {
            if(!turn.isPlayerTurn)
            {
                aiHand.AddCard();
            }
            if(turn.isPlayerTurn)
            {
                playerHand.AddCard();
            }

            if (playerHand.hand.Count == 0 && deck.deck.Count != 0)
            {
                playerHand.DrawHand();
            }

            if (aiHand.hand.Count == 0 && deck.deck.Count != 0)
            {
                aiHand.DrawHand();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        CheckGameEnd();
    }

    private void CheckGameEnd()
    {
        if (playerHand.hand.Count == 0 && aiHand.hand.Count == 0 && deck.deck.Count == 0)
        {
            EnterFinalFishingMode();
            Final = true;
        }
    }

    private void EnterFinalFishingMode()
    {
        GoFish.SetActive(false);
        Fishing.SetActive(true);

        if (!Playerdone && !Aidone && i <= 0)
        {
            Debug.Log("FinalFishing");
            i += 1;
        }

        if (Playerdone && !Aidone && !isReset)
        {
            turn.isPlayerTurn = false;
            reset.ResetGame();
            isReset = true;
        }

        if (!Playerdone && Aidone && !isReset)
        {
            turn.isPlayerTurn = true;
            reset.ResetGame();
            isReset = true;
        }

        if (Playerdone && Aidone)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Fishing.SetActive(false);
        end.SetActive(true);
        PlayerScore.text = $"Player Score:\n{stats.playerStats.score}";
        AIScore.text = $"AI Score:\n{stats.aiStats.score}";
    }
}
