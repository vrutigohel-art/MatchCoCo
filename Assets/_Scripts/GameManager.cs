using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Card Setup")]
    public GameObject cardPrefab;
    public Transform gridParent; // The parent where cards will be instantiated (e.g., a GridLayoutGroup)
    public Sprite[] cardFaces;   // Array of all available face sprites (e.g., 8 unique sprites)
    public int gridRows = 4;
    public int gridCols = 4;

    [Header("Game Settings")]
    public float flipBackDelay = 1.0f; // Time to wait before flipping mismatched cards back

    // --- Game State Variables ---
    private List<int> gameCardIds = new List<int>();
    private List<Card> revealedCards = new List<Card>();
    private int matchesFound = 0;
    private int totalPairs;

    public bool CanReveal => revealedCards.Count < 2; // Public accessor for the Card script

    void Start()
    {
        totalPairs = (gridRows * gridCols) / 2;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}