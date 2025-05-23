using UnityEngine;

/// <summary>
/// ? 當玩家進入樓層切換區域時，自動切換當前顯示的樓層
/// 此腳本通常掛在觸發區域（如上下樓的樓梯口）
/// </summary>
public class FloorZone : MonoBehaviour
{
    // ? 所有樓層的物件（每一層都掛有 FloorLayer.cs）
    public FloorLayer[] allFloors;

    // ? 玩家進入此區域後，應顯示的樓層
    public FloorLayer currentFloor;

    /// <summary>
    /// 當玩家進入此觸發區域時，切換樓層顯示狀態
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var floor in allFloors)
            {
                // 顯示當前樓層，其他隱藏
                floor.SetVisible(floor == currentFloor);
            }

            Debug.Log("? 已切換至樓層：" + currentFloor.gameObject.name);
        }
    }
}
