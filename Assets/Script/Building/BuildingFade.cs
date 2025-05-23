using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

/// <summary>
/// 當玩家進入建築物 Tile 區域時，使建築變透明；離開時恢復原狀。
/// </summary>
public class BuildingFade : MonoBehaviour
{
    private Tilemap tilemap;               // 此建築物對應的 Tilemap
    public float fadeAlpha = 0f;           // 淡出後的透明度
    public float fadeDuration = 0.5f;      // 淡出/淡入的動畫時間（秒）

    private float originalAlpha;           // 原始不透明度
    private Coroutine fadeCoroutine;       // 用來避免重複執行多個協程

    public Transform playerTransform;      // 🔺 玩家 Transform（需手動從 Inspector 指派）

    private void Awake()
    {
        // 取得 Tilemap 元件
        tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found on this GameObject!");
        }
        else
        {
            // 儲存 Tilemap 原始透明度
            originalAlpha = tilemap.color.a;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return; // 若玩家尚未指定，略過處理

        // 取得玩家目前位置所對應的格子座標
        Vector3 playerWorldPos = playerTransform.position;
        Vector3Int playerCellPos = tilemap.WorldToCell(playerWorldPos);

        // 檢查該格子是否為目前這個 tilemap 上的 tile（即玩家是否站在建築上）
        TileBase currentTile = tilemap.GetTile(playerCellPos);

        if (currentTile != null)
        {
            // 玩家站在建築內：淡出建築
            if (fadeCoroutine == null)
            {
                fadeCoroutine = StartCoroutine(FadeToAlpha(fadeAlpha));
            }
        }
        else
        {
            // 玩家離開建築範圍：淡入恢復透明度
            if (fadeCoroutine == null && tilemap.color.a < originalAlpha)
            {
                fadeCoroutine = StartCoroutine(FadeToAlpha(originalAlpha));
            }
        }
    }

    /// <summary>
    /// 使用協程漸變透明度至指定 alpha
    /// </summary>
    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        Color color = tilemap.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            color.a = newAlpha;
            tilemap.color = color;

            yield return null;
        }

        // 結束後強制設為目標透明度（避免不精確）
        color.a = targetAlpha;
        tilemap.color = color;

        // 協程執行完畢，重置旗標
        fadeCoroutine = null;
    }
}
