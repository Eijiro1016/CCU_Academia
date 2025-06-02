import os
import atexit
from flask import Flask, request, jsonify, render_template_string
from NPC_function import run_dialogue_session, load_dialogue_data

app = Flask(__name__)

HTML_PAGE = """
<!doctype html>
<html>
  <head>
    <title>NPC 對話系統</title>
  </head>
  <body>
    <h1>與 NPC 對話</h1>
    <form method="post" action="/">
      <label for="character">角色 ID（數字）：</label>
      <input type="text" id="character" name="character"><br><br>
      <label for="message">你說：</label>
      <input type="text" id="message" name="message"><br><br>
      <input type="submit" value="送出">
    </form>
    {% if response %}
      <h2>NPC 回應：</h2>
      {% for r in response %}
        <p>{{ r }}</p>
      {% endfor %}
    {% endif %}
  </body>
</html>
"""

# ===== 清除 memory 資料夾中所有 tmp 開頭的檔案 =====
def cleanup_memory_folder():
    memory_dir = "memory"
    if os.path.exists(memory_dir):
        for filename in os.listdir(memory_dir):
            if filename.startswith("tmp_char_") and filename.endswith(".json"):
                filepath = os.path.join(memory_dir, filename)
                try:
                    os.remove(filepath)
                    print(f"已刪除：{filepath}")
                except Exception as e:
                    print(f"無法刪除 {filepath}：{e}")

# =========== 網頁版回應 ===========
@app.route("/", methods=["GET", "POST"])
def index():
    response = None
    if request.method == "POST":
        character_id = int(request.form["character"])
        message = request.form["message"]
        response = run_dialogue_session(character_id, [message])
    return render_template_string(HTML_PAGE, response=response)

# =========== Unity回應：根據輸入生成回應 ===========
@app.route("/api/chat", methods=["POST"])
def chat_with_npc():
    data = request.json
    character_id = int(data["character"])
    user_inputs = data["message"]
    reset = data.get("reset", False)
    responses = run_dialogue_session(character_id, [user_inputs], reset)
    print(f"收到：{user_inputs}")  
    return jsonify({"responses": responses})

# =========== 取得之前的對話紀錄 ===========
@app.route("/api/history/<int:character_id>", methods=["GET"])
def get_dialogue_history(character_id):
    print(f"[Flask] 收到歷史查詢請求 ID = {character_id}")
    filename = f"memory/tmp_char_{character_id}.json"
    if not os.path.exists(filename):
        return jsonify({"dialogue": []})

    data = load_dialogue_data(filename)
    dialogue = next((c["dialogue"] for c in data["characters"] if c["ID"] == character_id), [])
    print(f"[HISTORY] 成功讀取：{filename}, 對話數：{len(dialogue)}")
    return jsonify({"dialogue": dialogue})

@atexit.register
def cleanup_on_exit():
    print("Flask 關閉中，正在清除暫存記憶檔案...")
    cleanup_memory_folder()

if __name__ == "__main__":
    if not os.path.exists("memory"):
        os.makedirs("memory")

    app.run(host="0.0.0.0", port=5000) # 如果要跨機使用，要用這段
    # 並且請求的網址變成 http://172.20.10.3:5000/
