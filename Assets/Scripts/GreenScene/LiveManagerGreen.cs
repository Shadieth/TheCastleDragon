using UnityEngine;
using UnityEngine.UI;

public class LiveManagerGreen : MonoBehaviour
{
    public Image[] hearts;
    public Sprite heartFull;
    public Sprite heartEmpty;
    private int lives;

    void Start()
    {
        lives = DataManager.vidas;
        UpdateHearts();
    }

    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--;
            UpdateHearts();
        }
    }

    public int CurrentLives()
    {
        return lives;
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < lives) ? heartFull : heartEmpty;
        }
    }
}
