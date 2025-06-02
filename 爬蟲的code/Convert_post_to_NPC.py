import json
import ollama

def generate_personality_prompt(dialogues):
    dialogue_text = "\n".join([d["content"] for d in dialogues])
    prompt = f"""
請根據以下對話內容判斷角色的人格類型，並從以下九種人格中選出一種最適合的風格：

 - 直率:語氣直接，表達不拐彎抹角，清晰明了。

 - 情感:感豐富，表達較為感性。像是留言時會表達自己內心的感受，關心他人的情緒，會使用許多感嘆詞和情感用語。

 - 理性:分析性強，語言較為客觀理性，會提供根據或事實支持自己的觀點。像是在留言中常會引述數據、理論或過去的經驗來支持自己的觀點。

 - 幽默:喜歡用幽默或諷刺的語氣來表達，讓對話氣氛輕鬆。像是使用笑話、雙關語或反諷來讓留言更加輕鬆和有趣。

 - 支持:積極、鼓勵他人，總是給予他人支持和安慰。像是留言中會表達對他人經歷的理解，給予安慰，提供建議，通常語氣很溫暖。

 - 批判:喜歡挑戰他人的觀點，常常會有比較強烈的批評或反駁。像是留言中會提出異議，強烈表達對某個觀點的不滿，可能會帶有挑戰性。

 - 觀察:比較內向，不太主動發表意見，通常觀察他人言論，偶爾發表看法。像是常常看別人怎麼說，再提出較為中立或觀察性的意見。

 - 建議:提供實用的建議或解決方案。像是留言中會給出具體的行動步驟，幫助他人解決問題，通常是從經驗中總結出來的建議。

 - 好奇:喜歡提問、探索，對事物感到好奇。像是留言中常常會提出問題，並表達對某些情況或現象的疑惑，尋求更多了解。

請只輸出一個人格類型的「名稱」（例如：「情感」），不要加入解釋或其他文字。

對話內容如下：
{dialogue_text}
"""
    return prompt

def infer_personality_with_ollama(dialogues):
    prompt = generate_personality_prompt(dialogues)
    response = ollama.chat(model='llama3.2', messages=[
        {"role": "user", "content": prompt}
    ])
    return response['message']['content'].strip()

# ------------------------------------- main ----------------------------------------------

# 讀取原始資料
with open('success_ccu_posts.json', 'r', encoding='utf-8') as f:
    original_data = json.load(f)

# 轉換格式
converted = {"characters": []}

cnt = 0

for post in original_data:
    character = {
        "ID": cnt,
        "name": post["author"],
        "style": "",  # 可以自行補上風格，這裡先留空
        "dialogue": []
    }

    # 加入 title
    if post.get("title"):
        character["dialogue"].append({
            "role": "assistant",
            "content": post["title"]
        })

    # 加入 content
    if post.get("content"):
        character["dialogue"].append({
            "role": "assistant",
            "content": post["content"]
        })

    # 加入每一則 op_comment
    for comment in post.get("op_comments", []):
        character["dialogue"].append({
            "role": "assistant",
            "content": comment
        })

    # 根據dialogue判斷出這個character的個性style並加入
    character["style"] = (infer_personality_with_ollama(character["dialogue"]))

    # 最後再加入 system prompt 來控制角色或語境
    text = character["dialogue"][1]["content"]
    system_prompt = {
        "role": "system",
        "content": f"你是名叫「{character['name']}」的學生，個性是：{character['style']}。你說話風格如下：「{text}」。請用這種語氣進行對話。"
    }
    if not any(msg["role"] == "system" for msg in character["dialogue"]):
        character["dialogue"].insert(0, system_prompt)

    cnt = cnt+1
    converted["characters"].append(character)

# 儲存轉換後的資料
with open('NPC_info.json', 'w', encoding='utf-8') as f:
    json.dump(converted, f, ensure_ascii=False, indent=4)

