import os
import csv # 匯出抓到的資料
import time
import json # 匯出抓到的資料
import pickle
from time import sleep
from bs4 import BeautifulSoup
import undetected_chromedriver as uc # 若是沒有用這個有很大的機率會被block!

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

driver.get("https://www.dcard.tw/")
time.sleep(3)

if os.path.exists(cookie_path):
    print("🔐 嘗試使用已儲存的 cookies 登入中...")
    load_cookies(driver, cookie_path)
    driver.refresh()
    time.sleep(3)
else:
    print("📝 請手動登入 Dcard 帳號...")
    input("✅ 登入後按下 Enter 儲存 cookie...")
    save_cookies(driver, cookie_path)
    print("✅ Cookie 已儲存完成！")

# === 設定滾動頁面的相關參數 ===
SCROLL_PAUSE_TIME = 5
SCROLL_TIMES = 5
last_height = driver.execute_script("return document.body.scrollHeight")

post_data = [] # 創建一個等等放抓下來資料的空間

for i in range(SCROLL_TIMES):  # 捲動 5 次

    # BeautifulSoup  抓取分析
    html = driver.page_source
    soup = BeautifulSoup(html, "html.parser")

    # 解析貼文
    articles = soup.select('div[class^="d_a5_22 d_eg_6y7mag d_s7_2o d_2l_f d_9w_25 d_mk_f7aqx6 d_mh_140cd6v d_mg_140cd6v w1n8s3eg"]')

    for article in articles:
        try:
            # 抓發文者
            name_tag = article.select_one('.d_7v_6.d_ju_1s.d_xa_2b.d_tx_2c.d_lc_1u')
            author_name = name_tag.text.strip() if name_tag else 'Unknown'

            # 抓板
            board_name = article.select_one('span:contains("板")')
            board_name = board_name.text.strip() if board_name else '未知板塊'

            # 抓連結
            post_url = article.select_one('a[href^="/f/"][href*="/p/"]')
            post_url = post_url['href'] if post_url else '#'
            full_url = f'https://www.dcard.tw{post_url}'

            # 抓發文時間
            time_tag = article.select_one('time')
            post_time = time_tag['datetime'] if time_tag else 'N/A'

            # 抓文章標題
            title_tag = article.find('h2')
            title = title_tag.text.strip() if title_tag else 'No Title'

            # 抓內文摘要
            content_tag = article.find('p')
            content = content_tag.text.strip() if content_tag else 'No Content'

            # 初始化互動數
            like_count = comment_count = share_count = bookmark_count = 0

            # 抓按讚、留言、分享、收藏數量
            counts = article.find_all(class_='l8yr2he')

            if counts and len(counts) >= 5: # 因為第0個是追蹤按鈕，所以從第一個開始
                like_count = int(counts[1].text.strip())
                comment_count = int(counts[2].text.strip())
                share_count = int(counts[3].text.strip())
                bookmark_count = int(counts[4].text.strip())

            print(f'📌 {board_name}｜{author_name}｜{post_time}')
            print(f'🔗 {full_url}')
            print(f"標題：{title}")
            print(f"內文：{content}\n")
            print(f'❤️ 愛心：{like_count}　💬 留言：{comment_count}　🔗 分享：{share_count}　🔖 收藏：{bookmark_count}')
            print('-' * 60)

            post_data.append({
                'board': board_name,
                'author': author_name,
                'time': post_time,
                'title': title,
                'content': content,
                'url': full_url,
                'like': like_count,
                'comment': comment_count,
                'share': share_count,
                'bookmark': bookmark_count
            })
            """ # 前往該文章頁面
            driver.get(full_url)
            sleep(2)
            post_soup = BeautifulSoup(driver.page_source, 'html.parser')

            # 抓標題與內文
            title_tag = post_soup.select_one('h1')  # 通常標題是 h1
            # 此class下包含內文與留言
            content_tag = post_soup.select_one('div[class^="d_xa_34 d_xj_2v c1ehvwc9"]')  # 內文所在區塊

            title = title_tag.text.strip() if title_tag else 'No Title'
            content = content_tag.text.strip() if content_tag else 'No Content'

            print(f'📝 標題：{title}')
            print(f'📄 內文：{content}\n') """
        except Exception as e:
            print('❌ Failed to parse an article block:', e)

    driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
    time.sleep(SCROLL_PAUSE_TIME)  # 等待新內容載入
    new_height = driver.execute_script("return document.body.scrollHeight")
    if new_height == last_height:
        break  # 如果沒新增內容就停止
    last_height = new_height

# ===  匯出資料  ===

# json資料
with open('dcard_posts.json', 'w', encoding='utf-8') as f:
    json.dump(post_data, f, ensure_ascii=False, indent=2)
print("✅ 已輸出為 dcard_posts.json")

# csv資料
with open('dcard_posts.csv', mode='w', encoding='utf-8-sig', newline='') as file:
    writer = csv.DictWriter(file, fieldnames=post_data[0].keys())
    writer.writeheader()
    writer.writerows(post_data)
print("✅ 已輸出為 dcard_posts.csv")

# ===  關閉瀏覽器並釋放資源  ===
try:
    driver.quit()
except Exception as e:
    print(f"🛑 關閉瀏覽器時出錯: {e}")
finally:
    del driver
