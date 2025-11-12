using UnityEngine;
using UnityEngine.UI; // Required for Image/Button components
using System.Collections;

public class Card : MonoBehaviour
{
    // --- Public References ---
    public int cardId; // The unique cardID matching value (e.g., 0, 1, 2)
    public Sprite cardFace; // The image when the card is flipped
    public Sprite cardBack; // The image when the card is covered

    // --- Private References ---
    private Image cardImage;
    private Button button;

    void Awake()
    {
        // Get references to the components on this GameObject
        cardImage = GetComponent<Image>();
        button = GetComponent<Button>();

        // Ensure the card starts face down
        cardImage.sprite = cardBack;
    }

    // Called to initialize the card
    public void Initialize(int id, Sprite faceSprite)
    {
        cardId = id;
        cardFace = faceSprite;

        // Set the click handler
        button.onClick.AddListener(OnCardClicked);
    }

    // Called when the user clicks the card
    public void OnCardClicked()
    {
        // Only allow clicking if the card is face down 
        if (cardImage.sprite == cardBack )
        {
            Debug.Log("Do something to reveal card");
        }
    }

    
    public void FlipToFace()
    {
        cardImage.sprite = cardFace;
    }

    // Flips the card face down
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
