# 🏡 我的鳳梨學院 - 2D Pixel RPG 專案

## 🎯 專案簡介
這是一款以 2D 像素風格製作的校園 RPG 遊戲，玩家能在校園中自由探索、與 NPC 聊天互動，甚至體驗來自 Dcard 真實留言生成的 AI 角色對話。遊戲以模組化方式設計，具備地圖遮擋、建築透明、怪物追擊與攻擊、樓層顯示控制等完整功能。

---

## 🎮 遊戲玩法
- 使用方向鍵或 WASD 控制角色上下左右單方向移動
- 按下 `F` 與 NPC 互動，展開對話（支援 AI 回應）
- 按下 `Tab` 呼叫手機，輸入訊息與 AI 聊天
- 玩家接近建築會使建築淡出透明
- 樓梯區域自動切換樓層顯示
- 敵人靠近會追擊並攻擊，玩家會扣血與擊退

---

## 🛠️ 開發技術與工具
- Unity 2021 / 2022（2D 專案模板）
- C# 腳本編寫
- Tilemap + Sorting Layer 管理遮擋
- Rigidbody2D + Trigger 判斷碰撞
- ScriptableObject 對話資料管理
- Coroutine 實作打字與透明效果
- 自製 AI 聊天：串接本地 FastAPI / Ollama / GPT API

---

## 📂 腳本結構（Script 資料夾說明）

```plaintext
Script/
├── AI/                        -> AI 聊天模組（Ollama / GPT）
│   ├── GPTDialog.cs           -> 串接 OpenAI GPT API
│   └── OllamaDialog.cs        -> 串接本地 FastAPI 模型
│
├── Building/                 -> 建築相關模組
│   ├── BuildingFade.cs        -> 玩家進入時建築淡出透明
│   ├── FloorLayer.cs          -> 控制單一樓層顯示 / 隱藏
│   └── FloorZone.cs           -> 玩家進入區域時切換樓層
│
├── Dialog/                   -> 對話系統模組
│   ├── Dialog.cs              -> 對話資料容器（ScriptableObject）
│   ├── DialogBuilder.cs       -> 動態建立對話內容
│   └── DialogManager.cs       -> 控制對話框顯示與打字動畫
│
├── Enemy/                    -> 敵人 AI 模組
│   ├── Enemy.cs               -> 敵人追蹤與移動控制
│   ├── AttackZone.cs          -> 攻擊區域、冷卻與扣血邏輯
│   └── DetectionZone.cs       -> 偵測玩家進入並開始追蹤
│
├── NPC/                      -> NPC 互動模組
│   └── NPCcontrol.cs          -> 接收玩家互動並呼叫 AI 對話
│
├── Player/                   -> 玩家控制模組
│   ├── Player_control.cs      -> 移動、動畫、互動邏輯
│   └── Player_health.cs       -> 扣血與擊退處理
│
├── gameControl.cs            -> 遊戲狀態控制（自由/對話/戰鬥）
├── Phone.cs                  -> 手機 UI 開啟、輸入與 AI 回覆
├── interactable.cs           -> 可互動物件介面（供 NPC 使用）
```


---

## 🧪 開發紀錄（Changelog）

### ✅ 2025-04-29
- 實作基本玩家移動控制
- 實作建築淡出與 Tilemap 遮擋
- 建立對話框 UI 與逐字顯示功能

### ✅ 2025-05-04
- 新增 NPC 互動對話系統（整合 Dcard 設定）
- 支援成就收集（櫻桃）

### ✅ 2025-05-12
- 敵人 AI 追擊與攻擊邏輯
- 玩家被攻擊會扣血與向後推退
- 手機 UI 串接本地 Ollama API 對話

---

## ▶️ 安裝與執行方式

1. **下載專案** 或 `git clone` 本倉庫
2. 使用 Unity Hub 加入專案
3. 開啟 `Assets/Scenes/MainScene.unity`（或其他主場景）
4. 點擊 `▶️ Play` 開始遊戲

---

## 💡 功能規劃（未來可擴充）

- [ ] 加入血條 UI（搭配 Player_health）
- [ ] 對話選項與分支選擇
- [ ] 玩家輸入自由提問 NPC（手機與對話皆支援）
- [ ] 任務系統與地圖傳送點
- [ ] AI 角色記憶與情感系統

---

## 🙏 特別感謝

- [OpenGameArt.org](https://opengameart.org/) 提供免費角色素材
- 參考 YouTube 教學：《Brackeys》、《Pixeland》、《Blackthornprod》
- Unity 官方文檔：Tilemap、2D Physics、Coroutine 教學

---

🎓 本專案為課程《程式設計（二）》期末專題，由學生自製，學習 Unity 遊戲開發、AI 對話整合、遊戲系統