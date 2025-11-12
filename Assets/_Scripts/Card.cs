using UnityEngine;
using UnityEngine.UI; // Required for Image/Button components
using System.Collections;

public class Card : MonoBehaviour
{
    // --- Public Properties ---
    public int cardId; // The unique ID representing the card's matching value (e.g., 0, 1, 2)
    public Sprite cardFace; // The image shown when the card is flipped
    public Sprite cardBack; // The image shown when the card is covered

    // --- Private References ---
    private Image cardImage;
    private GameManager gameManager;
    private Button button;

    void Awake()
    {
        // Get references to the components on this GameObject
        cardImage = GetComponent<Image>();
        button = GetComponent<Button>();

        // Ensure the card starts face down
        cardImage.sprite = cardBack;
    }

    // Called by the GameManager to initialize the card
    public void Initialize(int id, Sprite faceSprite, GameManager manager)
    {
        cardId = id;
        cardFace = faceSprite;
        gameManager = manager;

        // Set the click handler
        button.onClick.AddListener(OnCardClicked);
    }

    // Called when the user clicks the card
    public void OnCardClicked()
    {
        // Only allow clicking if the card is face down and the game manager allows a new reveal
        if (cardImage.sprite == cardBack && gameManager.CanReveal)
        {
            gameManager.CardRevealed(this);
        }
    }

    // Flips the card face up (called by GameManager)
    public void FlipToFace()
    {
        cardImage.sprite = cardFace;
    }

    // Flips the card face down after a delay (called by GameManager)
    public IEnumerator FlipToBack(float delay)
    {
        yield return new WaitForSeconds(delay);
        cardImage.sprite = cardBack;
    }

    // Hides the card after a match
    public void Matched()
    {
        // Disable the button to prevent interaction
        button.interactable = false;
        // Optionally, destroy the card or make it invisible
        Destroy(gameObject, 0.5f);
    }
}