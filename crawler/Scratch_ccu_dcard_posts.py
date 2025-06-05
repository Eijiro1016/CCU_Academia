import os
# import csv # 匯出抓到的資料
import time
import json # 匯出抓到的資料
import pickle
from time import sleep
from random import randint
from bs4 import BeautifulSoup
import undetected_chromedriver as uc # 若是沒有用這個有很大的機率會被block!

from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

# --- Cookie 儲存與載入功能 ---
def save_cookies(driver, path):
    with open(path, "wb") as file:
        pickle.dump(driver.get_cookies(), file)

def load_cookies(driver, path):
    with open(path, "rb") as file:
        cookies = pickle.load(file)
    for cookie in cookies:
        if "sameSite" in cookie:
            del cookie["sameSite"]
        try:
            driver.add_cookie(cookie)
        except Exception as e:
            print(f"⚠️ 加 cookie 時出錯：{e}")


# --- 主流程 ---
cookie_path = "cookies.pkl"

options = uc.ChromeOptions()
# options.add_argument("--headless")  # 若要背景執行
driver = uc.Chrome(options=options)

driver.get("https://www.dcard.tw/f/ccu")
time.sleep(randint(3, 6))

if os.path.exists(cookie_path):
    print("🔐 嘗試使用已儲存的 cookies 登入中...")
    load_cookies(driver, cookie_path)
    driver.refresh()
    time.sleep(randint(4, 6))
else:
    print("📝 請手動登入 Dcard 帳號...")
    input("✅ 登入後按下 Enter 儲存 cookie...")
    save_cookies(driver, cookie_path)
    print("✅ Cookie 已儲存完成！")

# === 設定滾動頁面的相關參數 ===
SCROLL_PAUSE_TIME = randint(3, 12)
SCROLL_TIMES = 2
last_height = driver.execute_script("return document.body.scrollHeight")

visited_urls = set() # 將收集到的url放到set裡，避免重複讀取
post_data = [] # 創建一個等等放抓下來資料的空間

for i in range(SCROLL_TIMES):  # 捲動 2 次

    # BeautifulSoup  抓取分析
    html = driver.page_source
    soup = BeautifulSoup(html, "html.parser")

    # ✅ 抓取特定板的文章列表（較穩定的 CSS selector）
    articles = soup.select('a[href^="/f/ccu/p/"]')

    for article in articles:
        try:
            full_url = f"https://www.dcard.tw{article['href']}"
            if full_url in visited_urls:
                continue
            visited_urls.add(full_url)

            # 前往文章頁面解析
            driver.get(full_url)
            sleep(4)

            # --------------------------------------------------------------------------------------------------
            op_comments = []

            # 重複滾動直到無法再滾
            while True:
                buttons = driver.find_elements(By.XPATH, "//button[contains(text(), '查看其他')]")
                # print(f"找到 {len(buttons)} 個展開按鈕")
                while (buttons) :
                    btn = buttons[0]
                    driver.execute_script("arguments[0].scrollIntoView({block: 'center'});", btn)
                    time.sleep(2)
                    btn.click()
                    time.sleep(4)  # 等留言載入

                    # 立刻抓目前載入進 DOM 的留言
                    current_soup = BeautifulSoup(driver.page_source, "html.parser")
                    comment_blocks = current_soup.select('div[data-key^="comment-"]')

                    for comment in comment_blocks:
                        try:
                            if "原 PO" in comment.text:
                                comment_text_tag = comment.select_one('div[class*="d_xa_34 d_xj_2v"]')
                                if comment_text_tag:
                                    text = comment_text_tag.text.strip()
                                    if text not in op_comments:
                                        op_comments.append(text)
                        except Exception as e:
                            continue
                    buttons = driver.find_elements(By.XPATH, "//button[contains(text(), '查看其他')]")

                # 再次滾動頁面載入更多留言
                previous_height = driver.execute_script("return document.body.scrollHeight")
                driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
                time.sleep(3)
                new_height = driver.execute_script("return document.body.scrollHeight")
                if new_height == previous_height:
                    print("📭 沒有更多留言可以載入")
                    break

            # --------------------------------------------------------------------------------------------------
            sleep(5)
            post_soup = BeautifulSoup(driver.page_source, "html.parser")

            title_tag = post_soup.select_one('h1')
            title = title_tag.text.strip() if title_tag else "No Title"

            content_tag = post_soup.select_one('div[class*= "d_xa_34 d_xj_2v c1ehvwc9"]')
            content = content_tag.text.strip() if content_tag else "No Content"

            author_tag = post_soup.select_one('div[class*= "d_xa_2b d_tx_2c d_lc_1u d_7v_5 a6buno9"]')
            author = author_tag.text.strip() if author_tag else "Unknown"

            time_tag = post_soup.select_one('time')
            post_time = time_tag["datetime"] if time_tag else "N/A"

            print(f'📌 作者：{author}｜{post_time}')
            print(f'🔗 {full_url}')
            print(f"標題：{title}")
            print(f"內文：{content}\n")
            print(f"原PO留言：{op_comments}")
            print("-" * 60)

            post_data.append({
                'author': author,
                'time': post_time,
                'title': title,
                'content': content,
                'url': full_url,
                'op_comments': op_comments
            })

        except Exception as e:
            print("❌ Failed to parse post:", e)

    driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
    time.sleep(SCROLL_PAUSE_TIME)
    new_height = driver.execute_script("return document.body.scrollHeight")
    if new_height == last_height:
        break
    last_height = new_height

# === 匯出 JSON 資料 ===
with open('ccu_posts.json', 'w', encoding='utf-8') as f:
    json.dump(post_data, f, ensure_ascii=False, indent=2)
print("✅ 已輸出為 ccu_posts.json")

# === 關閉瀏覽器 ===
driver.quit()