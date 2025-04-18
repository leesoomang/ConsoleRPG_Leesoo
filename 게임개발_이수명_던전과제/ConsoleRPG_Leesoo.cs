using System;
using System.Collections.Generic;
using System.Diagnostics;

public class item
{
    public string Name;
    public string Stat;
    public string Description;

    public item(string name, string stat, string description)
    {
        Name = name;
        Stat = stat;
        Description = description;
    }

}

public class shopitem
{
    public string Name;
    public string Stat;
    public string Description;
    public int Price;
    public bool IsPurchased;
    public bool IsRepeatable;

    public shopitem(string name, string stat, string description, int price, bool isRepeatable)
    {
        Name = name;
        Stat = stat;
        Description = description;
        Price = price;
        IsPurchased = false; 
        IsRepeatable = isRepeatable;
    }
}


public class GameMain
{
    // 기본 세팅
    static bool isGameRunning = true;
    static bool dungeonEntered = false;

    ////////////// 스텟
    static int Level = 01;
    static string Jop = "";
    static int Attack = 0;
    static int Defense = 0;
    static float Hp = 0f;
    static int Gold = 0;

    static string playerName = ""; // playerName을 전역으로 이동

    // 아이템관련 리스트
    static List<item> wearingItems = new List<item>();
    static List<item> myItems = new List<item>();
    static List<item> itemDB = new List<item>()
    {
        new item("낡은옷", "방어력 +2", "해지고 낡은 평범한 옷"),
        new item("숏소드", "공격력 +8", "짧지만 날카로운 검"),
        new item("소드", "공격력 +5, 방어력 +5", "공수 밸런스 좋은 기본 검"),
        new item("단검", "공격력 +2", "빠르게 찌르기에 적합한 작은 검")
    };


    // shopitemDB
    static List<shopitem> shopItemDB = new List<shopitem>()
{
    new shopitem("수련자 갑옷", "방어력 +5", "수련에 도움을 주는 갑옷입니다.", 1000, false),
    new shopitem("무쇠갑옷", "방어력 +9", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1200, false),
    new shopitem("모험가의 갑옷", "방어력 +15", "전설의 모험가들이 사용했다는 전설의 갑옷입니다.", 3500, false),
    new shopitem("소드", "공격력 +2", "쉽게 볼 수 있는 낡은 검 입니다.", 600, false),
    new shopitem("청동 도끼", "공격력 +5", "어디선가 사용됐던거 같은 도끼입니다.", 1500, false),
    new shopitem("단검", "공격력 +2", "반복 구매 가능한 추가 공격력을 올려주는 단도다.", 1500, true)
};


    /////////////////////////////////////////////////////////////////////////////
    static void Main()
    {
        // 게임 진행
        Console.WriteLine("소환사의 협곡에 오신걸 환영합니다.");
        Console.WriteLine("원하시는 이름을 입력해 주세요.");
        playerName = Console.ReadLine();
        Console.WriteLine();
        Console.WriteLine($"{playerName} 소환사님이 소환되었습니다.");
        Console.WriteLine();
        ChoosingJop();
        Console.WriteLine();
        while (isGameRunning)
        {
            beforeDungeon();
            Console.WriteLine();
        }
    }

    //직업 선택
    static void ChoosingJop()
    {
        Console.WriteLine();
        Console.WriteLine("------------직업선택------------");
        Console.WriteLine("직업을 선택해 주세요.");
        Console.WriteLine("1. 전사");
        Console.WriteLine("2. 도적");
        string Jopnum = Console.ReadLine();
        if (Jopnum == "1")
        {
            Jop = "전사";
            Attack = 10;
            Defense = 10;
            Hp = 100f;
            myItems.Add(itemDB[0]);
            myItems.Add(itemDB[2]);
            myItems.Add(itemDB[3]);
            Console.WriteLine($"{Jop}을 선택하셨습니다.");
            return;
        }
        else if (Jopnum == "2")
        {
            Jop = "도적";
            Attack = 25;
            Defense = 5;
            Hp = 50f;
            myItems.Add(itemDB[0]);
            myItems.Add(itemDB[1]);
            myItems.Add(itemDB[3]);
            Console.WriteLine($"{Jop}을 선택하셨습니다.");
            return;
        }
        else
        {
            Console.WriteLine("미구현 입니다. 다시 선택해 주세요.");
        }
    }

    //던전 이전 활동
    static void beforeDungeon()
    {
        while (!dungeonEntered)
        {
            Console.WriteLine();
            Console.WriteLine("------------활동------------");
            Console.WriteLine("던전 입장 전 활동을 선택해 주세요.");
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("원하는 활동의 번호를 입력해 주세요.");
            string Behavior = Console.ReadLine();
            int BehaviorNum = int.Parse(Behavior);

            chooseBehavior(BehaviorNum);
        }
    }

    // 나가기
    static void back(string backcheck)
    {
        Console.WriteLine();
        dungeonEntered = false;
        if (backcheck == "0")
        {
            beforeDungeon();
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            back("0");
        }
    }

    // 행동실행
    static void chooseBehavior(int choiceenum)
    {
        switch (choiceenum)
        {
            case 1:
                statue(Jop);
                break;
            case 2:
                inventory();
                break;
            case 3:
                shop();
                break;
            case 4:
                dungeon();
                break;
            default:
                Console.WriteLine("잘못된 번호입니다.");
                break;
        }
    }

    // 상태보기
    static void statue(string x)
    {
        Console.WriteLine();
        Console.WriteLine("------------상태보기------------");

        int addAttack = 0;
        int addDefense = 0;

        foreach (item itm in myItems)
        {
            if (itm.Name.StartsWith("[E]"))
            {
                string[] parts = itm.Stat.Split(',');
                foreach (string part in parts)
                {
                    if (part.Contains("공격력"))
                    {
                        string atkStr = part.Replace("공격력 +", "").Trim();
                        addAttack += int.Parse(atkStr);
                        Attack += addAttack;
                        

                    }
                    else if (part.Contains("방어력"))
                    {
                        string defStr = part.Replace("방어력 +", "").Trim();
                        addDefense += int.Parse(defStr);
                        Defense += addDefense;
                    }
                }
            }
        }

        Console.WriteLine($"Lv. {Level:00}");
        Console.WriteLine($"{playerName} ( {x} )");
        Console.WriteLine($"공격력 : {Attack} (+{addAttack})");
        Console.WriteLine($"방어력 : {Defense} (+{addDefense})");
        Console.WriteLine($"체력 : {Hp}");
        Console.WriteLine($"Gold : {Gold} G");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        string Choise = Console.ReadLine();
        back(Choise);
    }



    // 인벤토리
    static void inventory()
    {
        Console.WriteLine();
        Console.WriteLine("------------인벤토리------------");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        foreach (item i in myItems)
        {
            Console.WriteLine($"{i.Name} | {i.Stat} | {i.Description}");
        }
        Console.WriteLine();
        Console.WriteLine("1. 장착관리");
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        string Choise = Console.ReadLine();

        if (Choise == "1")
        {
            Wearing();
        }
        else if (Choise == "0")
        {
            back(Choise);
        }
        else
        {
            Console.WriteLine("잘못 입력하셨습니다.");
        }
        Console.WriteLine();
        dungeonEntered = false;
    }

    //장착관리
    static void Wearing()
    {
        Console.WriteLine();
        Console.WriteLine("---------인벤토리(장착관리)---------");
        Console.WriteLine();
        Console.WriteLine("보유 아이템 목록");
        for (int i = 0; i < myItems.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {myItems[i].Name} | {myItems[i].Stat} | {myItems[i].Description}");
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하는 행동을 입력해주세요.");
        string actnum = Console.ReadLine();
        int act = int.Parse(actnum);
        if (act == 0)
        {
            back(actnum);
        }
        else if (act > 0 && act <= myItems.Count)
        {
            WearingItem(act - 1);
            Wearing();
        }
        else
        {
            Console.WriteLine("잘못된 입력 입니다.");
            Wearing();
        }
    }

    //장비 입기
    static void WearingItem(int itemNum)
    {
        if (itemNum < 0 || itemNum >= myItems.Count)
        {
            Console.WriteLine("잘못된 번호입니다. 다시 선택해 주세요.");
            Wearing();
            return;
        }

        string name = myItems[itemNum].Name;
        bool isEquipped = name.StartsWith("[E]");
        string cleanName = name.Replace("[E]", "");
        item oldItem = myItems[itemNum];

        item newItem;

        if (isEquipped)
            newItem = new item(cleanName, oldItem.Stat, oldItem.Description);
        else
            newItem = new item("[E]" + cleanName, oldItem.Stat, oldItem.Description);

        myItems[itemNum] = newItem;
        Wearing();
    }

    // 상점
    static void shop()
    {
        Console.WriteLine();
        Console.WriteLine("------------상점------------");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{Gold} G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < shopItemDB.Count; i++)
        {
            bool alreadyOwned = myItems.Exists(itm => itm.Name.Replace("[E]", "") == shopItemDB[i].Name);
            string priceInfo;
            if (alreadyOwned && shopItemDB[i].IsRepeatable == false)
            {
                priceInfo = "구매완료";
            }
            else
            {
                priceInfo = shopItemDB[i].Price + " G";
            }
            Console.WriteLine($"[-] {shopItemDB[i].Name} | {shopItemDB[i].Stat} | {shopItemDB[i].Description} | {priceInfo}");
        }


        Console.WriteLine("1. 아이템 구매");
        Console.WriteLine("0. 나가기");
        Console.WriteLine("");
        Console.WriteLine("원하는 행동을 입력하세요.");
        string Choise = Console.ReadLine();

        if (Choise == "1")
        {
            Purchasing();
        }
        else if (Choise == "0")
        {
            back(Choise);
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    // 상점 아이템 구매
    static void Purchasing()
    {
        Console.WriteLine();
        Console.WriteLine("------------아이템 구매------------");
        Console.WriteLine($"[보유 골드] {Gold} G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < shopItemDB.Count; i++)
        {
            bool alreadyOwned1 = myItems.Exists(itm => itm.Name.Replace("[E]", "") == shopItemDB[i].Name);
            string priceInfo;

            if (alreadyOwned1 && shopItemDB[i].IsRepeatable == false)
            {
                priceInfo = "구매완료";
            }
            else
            {
                priceInfo = shopItemDB[i].Price + " G";
            }

            Console.WriteLine($"[{i + 1}] {shopItemDB[i].Name} | {shopItemDB[i].Stat} | {shopItemDB[i].Description} | {priceInfo}");
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.Write("원하는 행동을 입력하세요. ");
        string input = Console.ReadLine();

        if (input == "0")
        {
            shop();
            return;
        }

        if (!int.TryParse(input, out int choiceNum) || choiceNum < 1 || choiceNum > shopItemDB.Count)
        {
            Console.WriteLine("잘못된 입력입니다.");
            Purchasing();
            return;
        }

        int index = choiceNum - 1;
        shopitem selectedItem = shopItemDB[index];


        bool alreadyOwned = myItems.Exists(itm => itm.Name.Replace("[E]", "") == selectedItem.Name);

        if (alreadyOwned && !selectedItem.IsRepeatable)
        {
            Console.WriteLine("이미 구매한 아이템입니다. 아무 키나 누르세요...");
            Console.ReadKey(true);
        }
        else if (Gold < selectedItem.Price)
        {
            Console.WriteLine("골드가 부족합니다! 아무 키나 누르세요...");
            Console.ReadKey(true);
        }
        else
        {
            Gold -= selectedItem.Price;
            myItems.Add(new item(selectedItem.Name, selectedItem.Stat, selectedItem.Description));
            Console.WriteLine($"{selectedItem.Name} 아이템을 구매하였습니다!");
        }


        Console.WriteLine();
        Purchasing();
    }

    //던전
    static void dungeon()
    {
        Console.WriteLine();
        Console.WriteLine("------------던전------------");
        Console.WriteLine("던전에 입장합니다.");
        Console.WriteLine("던전에서의 활동을 선택해 주세요.");
        Console.WriteLine("1. 전투");
        Console.WriteLine("2. 탐험");
        Console.WriteLine("3. 퇴장");
        string dungeonBehavior = Console.ReadLine();
        int dungeonBehaviorNum = int.Parse(dungeonBehavior);
        dungeonEntered = true;
    }
}
