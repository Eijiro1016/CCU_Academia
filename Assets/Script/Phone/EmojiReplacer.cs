using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class EmojiReplacer
{
    private static Dictionary<string, string> emojiMap;
    private static bool isLoaded = false;

    private static void LoadEmojiMap()
    {
        if (isLoaded) return;

        // 請將這個路徑改成你實際存放 emoji_replacement_map.json 的 Resources 目錄路徑
        TextAsset jsonAsset = Resources.Load<TextAsset>("emoji_replacement_map");
        if (jsonAsset != null)
        {
            emojiMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonAsset.text);
            isLoaded = true;
        }
        else
        {
            Debug.LogError("找不到 emoji_replacement_map.json！請確認 Resources 目錄是否正確放置。");
            emojiMap = new Dictionary<string, string>();
        }
    }

    public static string Replace(string input)
    {
        LoadEmojiMap();
        if (string.IsNullOrEmpty(input)) return input;

        StringBuilder result = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            string match = input[i].ToString();

            // 處理 surrogate pair（高位/低位代理字符）
            if (char.IsHighSurrogate(input[i]) && i + 1 < input.Length && char.IsLowSurrogate(input[i + 1]))
            {
                match += input[i + 1];
                i++;
            }

            if (emojiMap.TryGetValue(match, out string spriteName))
            {
                result.Append($"<sprite name=\"{spriteName}\">");
            }
            else
            {
                result.Append(match);
            }
        }

        return result.ToString();
    }
}
