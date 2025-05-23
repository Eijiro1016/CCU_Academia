using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] bodyFrames;
    public Sprite[] hairFrames;
    public Sprite[] clothesFrames;

    public SpriteRenderer bodyRenderer;
    public SpriteRenderer hairRenderer;
    public SpriteRenderer clothesRenderer;

    public float frameRate = 0.1f;
    private int currentFrame;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % bodyFrames.Length;

            bodyRenderer.sprite = bodyFrames[currentFrame];
            hairRenderer.sprite = hairFrames[currentFrame];
            clothesRenderer.sprite = clothesFrames[currentFrame];
        }
    }
}
