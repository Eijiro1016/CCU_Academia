import os
import json
import shutil
import ollama

# 載入語料資料
def load_dialogue_data(filename):
    with open(filename, 'r', encoding='utf-8') as f:
        return json.load(f)

# 使用 Ollama 產生回應
def generate_response(prompt, user_input):
    prompt.append({"role": "user", "content": user_input})
    response = ollama.chat(
        model='llama3.2',
        messages=prompt
    )
    return response["message"]["content"]

# 對話系統類別
class DialogueSystem:
    def __init__(self, dialogue_data):
        self.characters = dialogue_data['characters']

    def get_character_dialogue(self, character_id):
        character = next((char for char in self.characters if char['ID'] == character_id), None)
        return character['dialogue'] if character else []

    def add_dialogue_to_character(self, character_id, new_dialogue):
        character = next((char for char in self.characters if char['ID'] == character_id), None)
        if character:
            character['dialogue'].append(new_dialogue)

    def save_dialogue_data(self, filename='tmp_ConversationLog.json'):
        with open(filename, 'w', encoding='utf-8') as f:
            json.dump({'characters': self.characters}, f, ensure_ascii=False, indent=4)

# 封裝的主流程函式，可供 Flask API 調用
def run_dialogue_session(character_id: int, user_inputs: list[str], reset: bool = False) -> list[str]:
    """
    傳入角色 ID 與一串 user 輸入，回傳 assistant 回應的清單。
    如果 reset=True，則會清除記憶並重新載入初始資料。
    """
    responses = []
    temp_file = f'memory/tmp_char_{character_id}.json'  # 每位角色有自己的記憶檔

    # 初始化資料
    if reset or not os.path.exists(temp_file):
        with open("NPC_info.json", "r", encoding="utf-8") as f:
            all_data = json.load(f)
        character_data = next((c for c in all_data["characters"] if c["ID"] == character_id), None)
        if character_data is None:
            return ["找不到該角色 ID"]
        with open(temp_file, "w", encoding="utf-8") as f:
            json.dump({"characters": [character_data]}, f, ensure_ascii=False, indent=4)

    # 載入資料
    data = load_dialogue_data(temp_file)
    dialogue_system = DialogueSystem(data)

    prompt = dialogue_system.get_character_dialogue(character_id)

    # # 如果是新對話，加入開場白
    # if prompt and len(prompt) == 1:
    #     responses.append(prompt[0]["content"])

    # 每一句話都記錄
    for user_input in user_inputs:
        if user_input.lower() == "exit":
            break

        response = generate_response(prompt, user_input)
        responses.append(response)

        dialogue_system.add_dialogue_to_character(character_id, {"role": "assistant", "content": response})
        dialogue_system.save_dialogue_data(temp_file)

    return responses
