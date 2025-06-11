# 🏡 我的鳳梨學院 - 2D Pixel RPG 專案

## 🎯 專案簡介

《我的鳳梨學院》是一款以國立中正大學為背景、融合 Dcard 熱門貼文與本地語言模型（LLaMA 3.2B）的 2D 校園 RPG 像素遊戲。  
玩家可以在熟悉的校園中自由探索，與 AI NPC 對話、躲避電動車敵人、完成任務主線，體驗結合真實社群資料與生成式 AI 的沉浸式冒險。

### 🔥 遊戲特色
- 🌟 真實 Dcard 熱門貼文轉化 NPC，具人格風格  
- 🤖 串接本地 Ollama 模型，支援 AI 即時回應  
- 🗺️ 中正校園像素地圖、樓層切換、建築透明  
- 🎮 任務主線、敵人追擊、手機 UI 對話面板  

---

## 🎮 遊戲玩法

- 使用方向鍵 / WASD 控制角色移動
- 按 `Shift`跑步加速
- 按 `F` 與 NPC 對話（AI 風格回應）  
- 按 `Tab` 呼叫手機輸入訊息與 NPC 聊天  
- 按 `M` 查看小地圖、`Esc` 開啟設定畫面  
- 樓梯區域會自動切換樓層顯示  
- 敵人靠近會追擊攻擊，玩家會扣血與擊退  

---

## 🧱 系統架構與整合

```plaintext
[Dcard 爬蟲 (Python)]
        ↓
[NPC 個性分類 + Prompt 建立 (Ollama LLaMA3.2B)]
        ↓
[Flask API → 本地 Web Server]
        ↓
[Unity C# 腳本 → 呼叫 API 串接對話系統]

```

## 🛠️ 開發技術與工具
- Unity：2D 專案模板

- C#：物件導向腳本編寫

- Python：Dcard 爬蟲 + 對話分類處理

- Flask API：串接 Ollama 模型

- Ollama：本地部署 LLaMA 3.2B

- GitHub + Git LFS：版本控制與大型檔案管理
---
## 📂 腳本結構
```plaintext
Script/
├── AI/                  → 串接 GPT / Ollama API 的對話功能
├── Building/            → 建築淡出與樓層控制
├── Dialog/              → ScriptableObject 對話資料管理系統
├── Enemy/               → 敵人偵測、攻擊與追蹤行為
├── NPC/                 → NPC 對話控制（串接 AI 回應）
├── Player/              → 玩家移動、血量、動畫
├── Phone.cs             → 手機 UI 與聊天面板功能
├── gameControl.cs       → 控制整體遊戲狀態
├── interactable.cs      → 可互動物件的共用介面

```
---
## 🧪 開發歷程（精選）
✅ 第1階段：遊戲雛形建立
- 玩家基本移動、建築遮擋、對話框與打字動畫

✅ 第2階段：功能拓展
- 串接 Ollama + 手機 UI 實現 AI 對話

- 加入地圖面板與 UI 系統

✅ 第3階段：內容增強
- 主畫面、角色換裝、配樂

- 支援角色外觀自訂（髮型、衣褲、膚色）

✅ 第4階段：核心機制建置
- 敵人追擊與攻擊邏輯、玩家死亡與退場動畫

- 主線任務、成功 / 失敗場景切換
  
---
## ▶️ 執行方式
點選以下連結即可直接於瀏覽器遊玩（無需安裝）：
👉 🎮 Unity Play - [我的鳳梨學院](https://play.unity.com/en/games/0b839150-5aed-4c84-affa-60d44727fdbf/ccuacademia)
---
## 🔧 功能規劃（未來擴充）
- 對話選項 / 分支 / 好感度系統

- 任務系統 + 成就獎勵機制

- 地圖傳送點與快速移動

- 多人連線遊玩（伺服器同步）

- 一鍵刷新 Dcard → 即時生成新 NPC
---
## 🙏 特別感謝
- [OpenGameArt.org](https://opengameart.org/)

- 素材來源：

  - 建築：[ModernExteriors](https://limezu.itch.io/modernexteriors)

  - UI：[Fairytale UI Pack](https://toffeecraft.itch.io/fairytale-ui-pack)

  - 人物：[Cozy People](https://shubibubi.itch.io/cozy-people)

  - 手機面板：[Pixelized Phone](https://ashizian.itch.io/pixelized-phone)

- YouTube 教學參考：

  - [Pixeland 系列](https://www.youtube.com/playlist?list=PL_Pb2I110MfGAsoqtDs8-6kEU55wU8CnE)

  - [角色換裝教學](https://www.youtube.com/watch?v=PNWK5o9l54w)

  - [敵人與血量教學](https://www.youtube.com/watch?v=VOdYtqV_meo)

---
🎓 本專案為《程式設計（二）》期末專題，由中正大學資工與資管學生協作完成。
