import os # 為了 在程式結束時將tmp檔案自動刪除
import json # 為了 匯入參考資料的資料型態
import shutil # 為了 需要複製一份參考資料
import ollama # 為了 語言模型產生回應

# 從 JSON 檔案讀取資料
def load_dialogue_data(filename='tmp_ConversationLog.json'):
    with open(filename, 'r', encoding='utf-8') as f:
        return json.load(f)

# 使用 Ollama API 生成回應
def generate_response(prompt, user_input):
    # 加入使用者當前的 prompt
    prompt.append({"role": "user", "content": user_input})

    response = ollama.chat(
        model = 'llama3.2', # 使用llama3.2作為我們回應的模型
        messages = prompt
    )
    return response["message"]["content"]

class DialogueSystem:
    def __init__(self, dialogue_data):
        # 讀取角色的語料
        self.characters = dialogue_data['characters']

    def get_character_dialogue(self, character_id):
        # 根據角色名獲取語料
        character = next((char for char in self.characters if char['ID'] == character_id), None)
        if character:
            return character['dialogue']
        return []

    def save_dialogue_data(self, filename='tmp_ConversationLog.json'):
        with open(filename, 'w', encoding='utf-8') as f:
            # json.dump(data, f, ensure_ascii=False, indent=4)
            json.dump({'characters': self.characters}, f, ensure_ascii=False, indent=4)

    def add_dialogue_to_character(self, character_id, new_dialogue):
        character = next((char for char in self.characters if char['ID'] == character_id), None)
        if character:
            character['dialogue'].append(new_dialogue)

if __name__ == "__main__":

    try:
        # 複製一份記錄使用者對話的檔案
        shutil.copy('NPC_info.json', 'tmp_ConversationLog.json')

        # 讀取資料
        data = load_dialogue_data('tmp_ConversationLog.json')
        dialogue_system = DialogueSystem(data)

        # 假設我們選擇跟 NPC ? 號 對話
        character_id = int("0")

        first_time = True  # 用來判斷是否第一次對話

        while True:
            # 抓出角色的語料
            prompt = dialogue_system.get_character_dialogue(character_id)

            if first_time: # NPC會先講出他的第一句話（預設是Dcard貼文的標題！）
                print(prompt[1]["content"])
                first_time = False
            
            user_input = input(">> ")
            
            if user_input.lower() == "exit":
                break

            # 生成回應
            response = generate_response(prompt,user_input)
            print("\n" + response)

            # 加入歷史並儲存
            dialogue_system.add_dialogue_to_character(character_id, {"role": "assistant", "content": response})

            # 儲存語料庫
            dialogue_system.save_dialogue_data()

    finally: # 使用try,finally的設計是為了避免沒輸入exit就用crtl+c強制終止程式，也可以刪除tmp檔案
        if os.path.exists('tmp_ConversationLog.json'):
            os.remove('tmp_ConversationLog.json')