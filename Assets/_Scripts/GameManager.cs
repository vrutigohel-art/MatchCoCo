using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Header("Card Setup")]
    public GameObject cardPrefab;
    public Transform gridParent; // The parent where cards will be instantiated (e.g., a GridLayoutGroup)
    public Sprite[] cardFaces;   // Array of all available face sprites (e.g., 8 unique sprites)
    public int gridRows = 4;
    public int gridCols = 4;
    public TextMeshProUGUI scoretext;

    [Header("Game Settings")]
    public float flipBackDelay = 1.0f; // Time to wait before flipping mismatched cards back

    // --- Game State Variables ---
    private List<int> gameCardIds = new List<int>();
    private List<Card> revealedCards = new List<Card>();
    private int matchesFound = 0;
    public int totalPairs;
    public GameObject StartPanel;
    GridLayoutGroup G;
    public bool CanReveal => revealedCards.Count < 2; // Public accessor for the Card script

    void Start()
    {
        G = gridParent.GetComponent<GridLayoutGroup>();
        //totalPairs = (gridRows * gridCols) / 2;
    }
    // A public function that accepts an integer argument
    public void PerformAction(int actionID)
    {
        Debug.Log("Button clicked! Action ID: " + actionID);

        // Use the argument to determine what action to take
        switch (actionID)
        {
            case 1:
                Debug.Log("Executing Action 1: Open Inventory");
                gridRows = gridCols =  2;
                break;
            case 2:
                Debug.Log("Executing Action 2: Open Map");
                gridRows = 3;
                gridCols = 4;
                break;
            case 3:
                Debug.Log("Executing Action 3: Open Settings");
                gridRows = 4;
                gridCols = 5;
                break;
            default:
                Debug.LogWarning("Unknown Action ID!");
                break;
        }
        G.constraintCount = gridRows;
        totalPairs = (gridRows * gridCols) / 2;
        StartPanel.SetActive(false);
        SetupGame();
    }
    void SetupGame()
    {
        // 1. Prepare the Card IDs (The Matching Logic)
        for (int i = 0; i < totalPairs; i++)
        {
            // Add two copies of the card ID (index into the cardFaces array)
            gameCardIds.Add(i);
            gameCardIds.Add(i);
        }

        // 2. Shuffle the IDs
        Shuffle(gameCardIds);

        // 3. Instantiate and Initialize Cards
        for (int i = 0; i < gameCardIds.Count; i++)
        {
            GameObject newCardObject = Instantiate(cardPrefab, gridParent);
            Card newCard = newCardObject.GetComponent<Card>();

            int id = gameCardIds[i];
            Sprite face = cardFaces[id];

            newCard.Initialize(id, face, this);
        }
    }

    // Called by a Card when it is clicked and flipped face up
    public void CardRevealed(Card card)
    {
        if (!CanReveal || revealedCards.Contains(card)) return;

        card.FlipToFace();
        revealedCards.Add(card);

        // Check for a match if two cards are revealed
        if (revealedCards.Count == 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    // --- Core Matching Logic ---
    IEnumerator CheckMatch()
    {
        // Wait for a moment so the player can see the second card
        yield return new WaitForSeconds(flipBackDelay);

        Card card1 = revealedCards[0];
        Card card2 = revealedCards[1];

        if (card1.cardId == card2.cardId)
        {
            // --- Match Found! ---
            matchesFound++;
            PlayerPrefs.SetInt("GameScore", matchesFound);
            scoretext.text = matchesFound.ToString();
            card1.Matched();
            card2.Matched();
            Debug.Log("Match Found!");

            if (matchesFound >= totalPairs)
            {
                Debug.Log("Game Over! You Win!");
                // Optionally show a Game Over screen
            }
        }
        else
        {
            // --- No Match ---
            StartCoroutine(card1.FlipToBack(0f)); // Flip back immediately
            StartCoroutine(card2.FlipToBack(0f));
            Debug.Log("No Match. Try again.");
        }

        // Clear the revealed cards list for the next turn
        revealedCards.Clear();
    }

    // Simple Fisher-Yates shuffle algorithm
    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void RestartGame()
    {
        // Reload the current scene to restart
        int t = PlayerPrefs.GetInt("GameScore", 0);
        Debug.Log("Current score ::" + t);
        PlayerPrefs.SetInt("GameScore", 0);
        scoretext.text = "0";
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}