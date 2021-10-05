using System;
using System.Collections.Generic;
using System.Threading;

namespace FinalBattle
{
    public class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Initialize();
            game.RunGame();
        }

        public static int GetInt(int lowerLimit = int.MinValue, int upperLimit = int.MaxValue, string prompt = "Give me a number: ")
        {
            bool legitNumber = false;
            int userNumber;
            while (!legitNumber)
            {
                Console.Write(prompt);
                string userInput = Console.ReadLine();
                try
                {
                    userNumber = Convert.ToInt32(userInput);
                    if (userNumber >= lowerLimit && userNumber <= upperLimit)
                    {
                        return userNumber;
                    }
                    else
                    {
                        Console.WriteLine("That number is out of range!");
                        continue;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine($"'{userInput}' is not a legitimate number.");
                    continue;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("That number is too large for a 32 bit int");
                    continue;
                }
            }
            return 0;
        }

    }


    public class Game
    {
        //Properties:
        //List of battles
        //Two players
        //bool GameOver
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }

        public List<Battle> BattleList { get; private set; }

        public Party GameWinner { get; private set; }

        //Methods:
        //Initialize game
        //Run Game
        //Determine if over, announce results

        public void RunGame()
        {
            for (int i = 0; i < BattleList.Count; i++)
            {
                Party battleWinner = BattleList[i].RunBattle();
                if (battleWinner.Type == PartyType.Monsters)
                {
                    GameWinner = battleWinner;
                    break;
                }
                else if (battleWinner.Type == PartyType.Heroes && i == BattleList.Count - 1)
                {
                    GameWinner = battleWinner;
                    break;
                }
            }

            AnnounceWinner();
        }

        public void AnnounceWinner()
        {
            if (GameWinner.Type == PartyType.Monsters)
                Console.WriteLine("The Monster have won!");
            else
                Console.WriteLine("The Heroes won! The Uncoded One has been defeated!");
        }

        public void Initialize()
        {

            // First initialize the players
            Console.WriteLine("What kind of player should control the Heroes?");
            Console.WriteLine("1 - Human player");
            Console.WriteLine("2 - Computer");
            int heroChoice = Program.GetInt(1, 2, "Enter your choice: ");

            switch(heroChoice)
            {
                case 1:
                    Player1 = new Player(PlayerType.Human, PartyType.Heroes);
                    break;
                case 2:
                    Player1 = new Player(PlayerType.Computer, PartyType.Heroes);
                    break;
            }

            Console.WriteLine("What kind of player should control the Monsters?");
            Console.WriteLine("1 - Human player");
            Console.WriteLine("2 - Computer");
            int monsterChoice = Program.GetInt(1, 2, "Enter your choice: ");

            switch (monsterChoice)
            {
                case 1:
                    Player2 = new Player(PlayerType.Human, PartyType.Monsters);
                    break;
                case 2:
                    Player2 = new Player(PlayerType.Computer, PartyType.Monsters);
                    break;
            }

            //Then initialize characters and put them in lists for each battle
            //True Programmer
            Character trueProgrammer;
            if (Player1.Type == PlayerType.Human)
            {
                Console.Write("Enter a name for the leader of the Hero Party, the True Programmer: ");
                string trueProgrammerName = Console.ReadLine();
                trueProgrammer = new Character(trueProgrammerName);
            }
            else
            {
                trueProgrammer = new Character("True Programmer"); 
            }

            List<Character> heroCharacters = new List<Character>() { trueProgrammer };

            //Monster Party characters for the first battle
            Character firstBattleSkeleton = new Character(CharacterType.Skeleton);
            List<Character> firstBattleMonsters = new List<Character>() { firstBattleSkeleton };

            //Monster Party characters for the second battle
            Character secondBattleSkeleton1 = new Character(CharacterType.Skeleton);
            Character secondBattleSkeleton2 = new Character(CharacterType.Skeleton);
            List<Character> secondBattleMonsters = new List<Character>() { secondBattleSkeleton1, secondBattleSkeleton2 };

            //Monster Party characters for the third battle
            Character uncodedOne = new Character(CharacterType.UncodedOne);
            List<Character> thirdBattleMonsters = new List<Character>() { uncodedOne };

            // Create health potions to add to the parties for the various battles
            HealthPotion firstMonsterPotion = new HealthPotion();
            HealthPotion secondMonsterPotion = new HealthPotion();
            HealthPotion thirdMonsterPotion = new HealthPotion();

            List<Item> firstMonsterItems = new List<Item>() { firstMonsterPotion };
            List<Item> secondMonsterItems = new List<Item>() { secondMonsterPotion };
            List<Item> thirdMonsterItems = new List<Item>() { thirdMonsterPotion };

            HealthPotion firstHeroPotion = new HealthPotion();
            HealthPotion secondHeroPotion = new HealthPotion();
            HealthPotion thirdHeroPotion = new HealthPotion();

            List<Item> heroItems = new List<Item> { firstHeroPotion, secondHeroPotion, thirdHeroPotion };

            // Create gear to add to the parties for various battles
            Dagger firstBattleDagger = new Dagger();
            Dagger secondBattleDagger1 = new Dagger();
            Dagger secondBattleDagger2 = new Dagger();
            Sword trueProgrammerSword = new Sword();

            List<Gear> firstMonsterPartyGear = new List<Gear>() { firstBattleDagger };
            List<Gear> secondMonsterPartyGear = new List<Gear>() { secondBattleDagger1, secondBattleDagger2 };
            List<Gear> heroGear = new List<Gear>() { trueProgrammerSword };




            //Then initialize parties
            Party heroParty = new Party(PartyType.Heroes, "Heroes", heroCharacters, Player1, heroItems, heroGear);
            Party firstMonsterParty = new Party(PartyType.Monsters, "Monsters", firstBattleMonsters, Player2, firstMonsterItems, firstMonsterPartyGear);
            Party secondMonsterParty = new Party(PartyType.Monsters, "Monsters", secondBattleMonsters, Player2, secondMonsterItems, secondMonsterPartyGear);
            Party thirdMonsterParty = new Party(PartyType.Monsters, "Monsters", thirdBattleMonsters, Player2, thirdMonsterItems, new List<Gear>());

            //Then initialize Battles
            Battle battle1 = new Battle(firstMonsterParty, heroParty);
            Battle battle2 = new Battle(secondMonsterParty, heroParty);
            Battle battle3 = new Battle(thirdMonsterParty, heroParty);

            List<Battle> battleList = new List<Battle>() { battle1, battle2, battle3 };
            BattleList = battleList;
        }
    }

    public class Player
    {
        public PlayerType Type { get; set; }

        public PartyType Allegiance { get; set; }

        public Player(PlayerType playerType, PartyType allegiance)
        {
            Type = playerType;
            Allegiance = allegiance;
        }

    }

    public class Party
    {
        public PartyType Type { get; set; }

        public string Name { get; set; }

        public List<Character> Characters { get; set; }

        public Player Player { get; set; }

        public List<Item> ItemInventory { get; set; }

        public List<Gear> GearInventory { get; set; }

        public Party(PartyType type, string name, List<Character> characterList, Player player, List<Item> itemList, List<Gear> gearList)
        {
            Type = type;
            Name = name;
            Characters = characterList;
            Player = player;
            ItemInventory = itemList;
            GearInventory = gearList;
        }

        public void RemoveDead()
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i].CurrentHP < 1)
                {
                    Characters[i].AnnounceDeath();
                    Characters.Remove(Characters[i]);
                    i--;
                }
            }
        }

        // This function will update the list of actions available to each character in the party, based on the inventory.
        // For example, if the party has a health potion in their inventory, the function will add the 'use health potion' action to 
        // all characters in the party. However, the function first checks whether such an action is not already in the character's 
        // available action list, so that we don't keep adding the action over and over again. 
        // The function also removes the 'use health potion' action from characters if there is no health potion in the inventory. 
        public void InventoryActionUpdate()
        {
            if (HealthPotionInInventory())
            {
                foreach (Character character in Characters)
                {
                    if (!character.UseHealthPotionAvailable())
                        character.AvailableActions.Add(new UseHealtPotion());
                }
            }
            else
            {
                foreach (Character character in Characters)
                {
                    if (character.UseHealthPotionAvailable())
                    {
                        // Theoretically, it should never happen that the character has more than one 'Use health potion' action in their 
                        // action list, so the loop could exit after finding the first 'Use health potion' action, but just to be safe, 
                        // the loop will be left to keep going.
                        for (int i = 0; i < character.AvailableActions.Count; i++)
                        {
                            if (character.AvailableActions[i] is UseHealtPotion)
                            {
                                character.AvailableActions.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
            }

            Sword inventorySword;

            if (SwordInInventory(out inventorySword))
            {
                foreach (Character character in Characters)
                {
                    if (!character.EquipGearAvailable(typeof(Sword)))
                        character.AvailableActions.Add(new EquipGear(inventorySword));
                }
            }
            else
            {
                foreach (Character character in Characters)
                {
                    if (character.EquipGearAvailable(typeof(Sword)))
                    {
                        for (int i = 0; i < character.AvailableActions.Count; i++)
                        {
                            if (character.AvailableActions[i] is EquipGear)
                            {
                                EquipGear equippingAction = (EquipGear)character.AvailableActions[i];
                                if (equippingAction.GearToEquip.GetType() == typeof(Sword))
                                {
                                    character.AvailableActions.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                }
            }

            Dagger inventoryDagger;

            if (DaggerInInventory(out inventoryDagger))
            {
                foreach (Character character in Characters)
                {
                    if (!character.EquipGearAvailable(typeof(Dagger)))
                        character.AvailableActions.Add(new EquipGear(inventoryDagger));
                }
            }
            else
            {
                foreach (Character character in Characters)
                {
                    if (character.EquipGearAvailable(typeof(Dagger)))
                    {
                        for (int i = 0; i < character.AvailableActions.Count; i++)
                        {
                            if (character.AvailableActions[i] is EquipGear)
                            {
                                EquipGear equippingAction = (EquipGear)character.AvailableActions[i];
                                if (equippingAction.GearToEquip.GetType() == typeof(Dagger))
                                {
                                    character.AvailableActions.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Searches party inventory for health potion, returns true if there is one, returns false otherwise. 
        public bool HealthPotionInInventory()
        {
            foreach (Item item in ItemInventory)
            {
                if (item is HealthPotion)
                    return true;
            }

            return false;
        }

        public bool SwordInInventory(out Sword sword)
        {
            foreach (Gear gear in GearInventory)
            {
                if (gear is Sword)
                {
                    sword = (Sword)gear;
                    return true;
                }       
            }
            sword = null;
            return false;
        }

        public bool DaggerInInventory(out Dagger dagger)
        {
            foreach (Gear gear in GearInventory)
            {
                if (gear is Dagger)
                {
                    dagger = (Dagger)gear;
                    return true;
                }  
            }
            dagger = null;
            return false;
        }
    }

    public class Character
    {
        public CharacterType Type { get; private set; }

        public string Name { get; private set; }

        public int MaxHP { get; set; }

        public int CurrentHP { get; set; }

        public List<Action> AvailableActions { get; set; }

        public Gear EquippedGear { get; set; }

        // Completely custom constructor, probably unnecessary 
        public Character(CharacterType type, string name, int maxHP, List<Action> actionList)
        {
            Type = type;
            Name = name;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            AvailableActions = actionList;
        }

        // Constructor to be used for Skeleton characters and the Uncoded One. Only needs character type.
        public Character(CharacterType type)
        {
            Type = type;
            if (type == CharacterType.Skeleton)
            {
                Name = "SKELETON";
                MaxHP = 5;
                CurrentHP = 5;
                AvailableActions = new List<Action> { new DoNothing(), new BoneCrunchAttack() };
            }
            else if (type == CharacterType.UncodedOne)
            {
                Name = "The Uncoded One";
                MaxHP = 15;
                CurrentHP = 15;
                AvailableActions = new List<Action> { new DoNothing(), new UnravelingAttack() };
            }
        }

        // Constructor to be used for the True Programmer, as the player chooses the name for the True Programmer character
        public Character(string name)
        {
            Type = CharacterType.TrueProgrammer;
            Name = name;
            MaxHP = 25;
            CurrentHP = 25;
            AvailableActions = new List<Action> { new DoNothing(), new PunchAttack() };
        }
        
        // returns true if the character has a 'Use Healh Potion' action in their list of actions, returns false otherwise.
        // This function is used so that we don't redundantly (and incorrectly) keep adding new 'use health potion' actions
        // to characters which already have them.
        public bool UseHealthPotionAvailable()
        {
            foreach (Action action in AvailableActions)
            {
                if (action is UseHealtPotion) return true;
            }

            return false;
        }


        // Returns true if the Character has an 'equip gear' action in their list of actions, relating to the type of gear given by the argument. 
        public bool EquipGearAvailable(Type gearType)
        {
            for (int i = 0; i < AvailableActions.Count; i++)
            {
                if (AvailableActions[i] is EquipGear)
                {
                    EquipGear equippingAction = (EquipGear)AvailableActions[i];
                    if (equippingAction.GearToEquip.GetType() == gearType)
                        return true;
                }
            }

            return false;
        }

        //Returns true if the Character has a gear-enabled attack available in their list of actions, returns false otherwise
        public bool GearAttackAvailable(Gear gear)
        {
            if (gear is Sword)
            {
                foreach (Action action in AvailableActions)
                {
                    if (action is SlashAttack)
                        return true;
                }
            }
            else if (gear is Dagger)
            {
                foreach (Action action in AvailableActions)
                {
                    if (action is StabAttack)
                        return true;
                }
            }

            return false;
        }

        // Updates the character's available actions based on whether they have gear equipped
        public void UpdateActionsEquipment()
        {
            if (GearEquipped())
            {
                if (!GearAttackAvailable(EquippedGear))
                    AvailableActions.Add(EquippedGear.Attack);
            }
            else
            {
                if(GearAttackAvailable(EquippedGear))
                {
                    for (int i = 0; i < AvailableActions.Count; i++)
                    {
                        if (AvailableActions[i].GetType() == EquippedGear.Attack.GetType())
                        {
                            AvailableActions.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        // return true if the character has gear equipped, return false otherwise
        public bool GearEquipped()
        {
            if (EquippedGear == null)
                return false;
            else
                return true;
        }


        public void AnnounceDeath()
        {
            Console.WriteLine($"{Name} has been defeated!");
        }
    }

    public class Battle
    {
        public Party MonsterParty { get; set; }

        public Party HeroParty { get; set; }

        public bool BattleOver { get; private set; }

        public Party BattleWinner { get; set; }

        public Battle(Party monsterParty, Party heroParty)
        {
            MonsterParty = monsterParty;
            HeroParty = heroParty;
            BattleOver = false;
        }

        public Party RunBattle()
        {
            while (!BattleOver)
            {
                RunRound();
            }

            return BattleWinner;
        }

        public void RunRound()
        {
            if (MonsterParty.Player.Type == PlayerType.Computer)
            {
                foreach (Character character in MonsterParty.Characters)
                {
                    AnnounceStatus(character);
                    MonsterParty.InventoryActionUpdate();
                    character.UpdateActionsEquipment();
                    AnnounceTurn(character.Name);
                    Thread.Sleep(1000);
                    ComputerAction(character, HeroParty, MonsterParty);
                    HeroParty.RemoveDead();
                    if (IsOver()) return;
                    MonsterParty.InventoryActionUpdate();
                    character.UpdateActionsEquipment();
                }
            }
            else if (MonsterParty.Player.Type == PlayerType.Human)
            {
                foreach (Character character in MonsterParty.Characters)
                {
                    AnnounceStatus(character);
                    MonsterParty.InventoryActionUpdate();
                    character.UpdateActionsEquipment();
                    AnnounceTurn(character.Name);
                    PerformPlayerAction(character, GetPlayerAction(character), HeroParty, MonsterParty);
                    HeroParty.RemoveDead();
                    if (IsOver()) return;
                    MonsterParty.InventoryActionUpdate();
                    character.UpdateActionsEquipment();
                }
            }
            
            if (HeroParty.Player.Type == PlayerType.Computer)
            {
                foreach (Character character in HeroParty.Characters)
                {
                    AnnounceStatus(character);
                    HeroParty.InventoryActionUpdate();
                    character.UpdateActionsEquipment();
                    AnnounceTurn(character.Name);
                    Thread.Sleep(1000);
                    ComputerAction(character, MonsterParty, HeroParty);
                    MonsterParty.RemoveDead();
                    if (IsOver()) return;
                    HeroParty.InventoryActionUpdate();
                    character.UpdateActionsEquipment();
                }
            }
            else if (HeroParty.Player.Type == PlayerType.Human)
            {
                foreach (Character character in HeroParty.Characters)
                {
                    AnnounceStatus(character);
                    HeroParty.InventoryActionUpdate();
                    character.UpdateActionsEquipment();
                    AnnounceTurn(character.Name);
                    PerformPlayerAction(character, GetPlayerAction(character), MonsterParty, HeroParty);
                    MonsterParty.RemoveDead();
                    if (IsOver()) return;
                    HeroParty.InventoryActionUpdate();
                    character.UpdateActionsEquipment();
                }
            }
            
        }

        public void AnnounceTurn(string characterName)
        {
            Console.WriteLine($"It is {characterName}'s turn...");
            Console.WriteLine();
        }

        public void AnnounceStatus(Character character)
        {
            Console.WriteLine($"================================================ BATTLE ===============================================");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(HeroParty.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            foreach (Character hero in HeroParty.Characters)
            {
                if (hero == character)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(hero.Name);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"                 ( {hero.CurrentHP} / {hero.MaxHP} )");
                }
                else
                {
                    Console.WriteLine($"{hero.Name}                 ( {hero.CurrentHP} / {hero.MaxHP} )");
                }
                if (hero.EquippedGear != null)
                    Console.WriteLine($" - with {hero.EquippedGear.Name} equipped");
            }
            Console.WriteLine("Items: ");
            foreach (Item item in HeroParty.ItemInventory)
            {
                Console.WriteLine(item.Name);
            }
            foreach (Gear gear in HeroParty.GearInventory)
            {
                Console.WriteLine(gear.Name);
            }
            if (HeroParty.ItemInventory.Count < 1 && HeroParty.GearInventory.Count < 1)
                Console.WriteLine("(Empty)");


            Console.WriteLine("------------------------------------------------- VS --------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"                                                                                           {MonsterParty.Name}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            foreach (Character monster in MonsterParty.Characters)
            {
                if (monster == character)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(monster.Name);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"                 ( {monster.CurrentHP} / {monster.MaxHP} )");
                }
                else
                {
                    Console.WriteLine($"{monster.Name}                 ( {monster.CurrentHP} / {monster.MaxHP} )");
                }
                if (monster.EquippedGear != null)
                    Console.WriteLine($" - with {monster.EquippedGear.Name} equipped");
            }
            Console.WriteLine("                                                                                           Items: ");
            foreach (Item item in MonsterParty.ItemInventory)
            {
                Console.WriteLine($"                                                                                           {item.Name}");
            }
            foreach (Gear gear in MonsterParty.GearInventory)
            {
                Console.WriteLine($"                                                                                           {gear.Name}");
            }
            if (MonsterParty.ItemInventory.Count < 1 && MonsterParty.GearInventory.Count < 1)
            {
                Console.WriteLine($"                                                                                           (Empty)");
            }
        }

        public bool IsOver()
        {
            if (MonsterParty.Characters.Count < 1)
            {
                BattleWinner = HeroParty;
                BattleOver = true;
                AnnounceResult();
                return true;
            }
            else if (HeroParty.Characters.Count < 1)
            {
                BattleWinner = MonsterParty;
                BattleOver = true;
                AnnounceResult();
                return true;
            }

            return false;
        }

        public void AnnounceResult()
        {
            Console.WriteLine($"{BattleWinner.Name} won the battle!");
        }

        public Action GetPlayerAction(Character character)
        {   
            for (int i = 0; i < character.AvailableActions.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {character.AvailableActions[i].Name}");
            }
            
            int userChoice = Program.GetInt(1, character.AvailableActions.Count, "What do you want to do? ");

            return character.AvailableActions[userChoice - 1];
        }

        public void PerformPlayerAction(Character character, Action action, Party enemyParty, Party characterParty)
        {
            if (action is Attack)
            {
                Character enemy = ChooseEnemy(enemyParty);
                action.Perform(character, enemy);
            }
            else if (action is DoNothing)
            {
                action.Perform(character);
            }
            else if (action is EquipGear)
            {
                action.Perform(character, playingCharacterParty: characterParty);
            }
            Console.WriteLine();
        }

        public Character ChooseEnemy(Party enemyParty)
        {
            List<Character> characterList = enemyParty.Characters;

            Console.WriteLine("You can attack the following enemies:");

            for (int i = 0; i < characterList.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {characterList[i].Name} {characterList[i].CurrentHP}/{characterList[i].MaxHP}");
            }

            int userChoice = Program.GetInt(1, characterList.Count, "Who do wou want to attack? ");
            return characterList[userChoice - 1];
        }

        // Makes the computer player perform an action for the character it is currently controlling.
        // The logic currently is that it selects the first attack it finds in the character's available actions.
        // Update: added logic for using health potions. If a health potion is available, and the character's health
        // is below 50%, the computer should use a potion 25% of the time. If the computer does not use a potion,
        // then it continues with the original logic of selecting the first available attack.
        public void ComputerAction(Character character, Party enemyParty, Party characterParty)
        {
            if (character.UseHealthPotionAvailable())
            {
                float remainingHPfraction = (float)character.CurrentHP / (float)character.MaxHP;

                if (remainingHPfraction < 0.5)
                {
                    Random random = new Random();
                    if (random.Next(4) == 1)
                    {
                        foreach (Action action in character.AvailableActions)
                        {
                            if (action is UseHealtPotion)
                            {
                                action.Perform(character, playingCharacterParty: characterParty);
                                return;
                            }
                        }
                }
                }
            }
            
            int actionIndex = 0;
            
            for (int i = 0; i < character.AvailableActions.Count; i++)
            {
                if (character.AvailableActions[i] is Attack)
                {
                    actionIndex = i;
                    break;
                }
            }

            Character enemy = GetEnemy(enemyParty);

            character.AvailableActions[actionIndex].Perform(character, enemy);
            Console.WriteLine();
        }

        // Function which randomly selects enemy for the computer player to attack
        public Character GetEnemy(Party enemyParty)
        {
            List<Character> characterList = enemyParty.Characters;
            
            Random random = new Random();
            return characterList[random.Next(characterList.Count)];
        }
    }

    public class Action
    {
        // The message associated with the action
        public string Message { get; protected set; }

        public string Name { get; protected set; }

        public virtual void DisplayMessage(Character playingCharacter, Character attackedCharacter = null)
        {
            Console.WriteLine($"{playingCharacter.Name} {Message}");
        }

        public virtual void Perform(Character playingCharacter, Character attackedChatacter = null, Party playingCharacterParty = null)
        {
            DisplayMessage(playingCharacter);
        }

        public void AnnounceHP(Character attackedCharacter)
        {
            Console.WriteLine($"{attackedCharacter.Name} is now at {attackedCharacter.CurrentHP}/{attackedCharacter.MaxHP} HP.");
        }
    }

    public class DoNothing : Action
    {
        public DoNothing()
        {
            Message = "did NOTHING.";
            Name = "Do nothing";
        }
    }

    public class Attack : Action
    {
        public int Damage { get; protected set; }

        // Decreases HP of attacked character. Returns true if the attack kills the character (resulting HP < 1), reutrns false otherwise.
        public virtual void PerformAttack(Character attackingCharacter, Character attackedCharacter)
        {
            attackedCharacter.CurrentHP -= Damage;

            if (attackedCharacter.CurrentHP < 1) attackedCharacter.CurrentHP = 0;
        }

        public void AnnounceEffect(Character attackedCharacter)
        {
            Console.WriteLine($"{Name} dealt {Damage} damage to {attackedCharacter.Name}.");
        }

        public override void DisplayMessage(Character playingCharacter, Character attackedCharacter = null)
        {
            Console.WriteLine($"{playingCharacter.Name} {Message} on {attackedCharacter.Name}");
        }

        public override void Perform(Character playingCharacter, Character attackedChatacter = null, Party playingCharacterParty = null)
        {
            DisplayMessage(playingCharacter, attackedChatacter);
            PerformAttack(playingCharacter, attackedChatacter);
            AnnounceEffect(attackedChatacter);
            AnnounceHP(attackedChatacter);
        }
    }

    public class PunchAttack : Attack
    {
        public PunchAttack()
        {
            Message = "used PUNCH";
            Name = "Standard Attack (PUNCH)";
            Damage = 1;
        }
    }

    public class BoneCrunchAttack : Attack
    {
        public BoneCrunchAttack()
        {
            Message = "used BONE CRUNCH";
            Name = "Standard Attack (BONE CRUNCH)";
            Random random = new Random();
            Damage = random.Next(2);
        }

        public override void PerformAttack(Character attackingCharacter, Character attackedCharacter)
        {
            Random random = new Random();
            Damage = random.Next(2);
            base.PerformAttack(attackingCharacter, attackedCharacter);
        }
    }

    public class UnravelingAttack : Attack
    {
        public UnravelingAttack()
        {
            Message = "used UNRAVELING";
            Name = "Standard Attack (UNRAVELING)";
            Random random = new Random();
            Damage = random.Next(3);
        }

        public override void PerformAttack(Character attackingCharacter, Character attackedCharacter)
        {
            Random random = new Random();
            Damage = random.Next(3);
            base.PerformAttack(attackingCharacter, attackedCharacter);
        }
    }

    public class Item
    {
        public string Name { get; set; }
    }

    public class HealthPotion : Item
    {
        public HealthPotion()
        {
            Name = "Health potion";
        }
    }

    public class UseHealtPotion : Action
    {
        public UseHealtPotion()
        {
            Message = "used health potion.";
            Name = "Use health potion (+10 HP)";
        }

        public override void Perform(Character playingCharacter, Character attackedChatacter = null, Party playingCharacterParty = null)
        {
            base.Perform(playingCharacter);
            AnnounceEffect(playingCharacter, DrinkPotion(playingCharacter));
            RemovePotion(playingCharacterParty);
            AnnounceHP(playingCharacter);

        }

        // Adds the appropriate HP to the character drinking the potion (either 10 points, or restoring to max, if adding 10 points would exceed maximal HP).
        // The function then returns the number of HP that was restored, so that the function announcing the effect of the potion can use it. 
        public int DrinkPotion(Character playingCharacter)
        {
            int missingHP = playingCharacter.MaxHP - playingCharacter.CurrentHP;

            if (missingHP < 10)
            {
                playingCharacter.CurrentHP += missingHP;
            }
            else
            {
                playingCharacter.CurrentHP += 10;
            }

            return missingHP;
        }

        public void RemovePotion(Party party)
        {
            for (int i = 0; i < party.ItemInventory.Count; i++)
            {
                if (party.ItemInventory[i] is HealthPotion)
                {
                    party.ItemInventory.RemoveAt(i);
                    break;
                }
            }
        }

        public void AnnounceEffect(Character playingCharacter, int restoredHP)
        {
            Console.WriteLine($"The health potion added {restoredHP} HP to {playingCharacter.Name}'s health.");
        }
    }

    public class Gear
    {
        public string Name { get; set; }

        public Attack Attack { get; set; }

    }

    public class Sword : Gear
    {
        public Sword()
        {
            Name = "Sword";
            Attack = new SlashAttack();
        }

        public Sword(string name)
        {
            Name = name;
            Attack = new SlashAttack();
        }
    }

    public class Dagger : Gear
    {
        public Dagger()
        {
            Name = "Dagger";
            Attack = new StabAttack();
        }

        public Dagger(string name)
        {
            Name = name;
            Attack = new StabAttack();
        }
    }

    public class EquipGear : Action
    {
        public Gear GearToEquip { get; set; }

        public EquipGear(Gear gear)
        {
            GearToEquip = gear;
            Message = $"equipped {GearToEquip.Name}.";
            Name = $"Equip {GearToEquip.Name}";
        }

        public override void Perform(Character playingCharacter, Character attackedChatacter = null, Party playingCharacterParty = null)
        {
            base.Perform(playingCharacter);
            Equip(playingCharacter, playingCharacterParty);
            AnnounceEffect(playingCharacter);

        }

        public void AnnounceEffect(Character playingCharacter)
        {
            Console.WriteLine($"{playingCharacter.Name} equipped {GearToEquip.Name}, which added {GearToEquip.Attack.Name}, with {GearToEquip.Attack.Damage} damage.");
        }

        // Removes Gear from Party inventory and adds it to Character
        public void Equip(Character playingCharacter, Party charactersParty)
        {
            // If the character already has some gear equipped, remove it and add it to party inventory
            if (playingCharacter.EquippedGear != null)
            {
                charactersParty.GearInventory.Add(playingCharacter.EquippedGear);
                playingCharacter.EquippedGear = null;
            }
            
            // Loop through Party gear inventory until we find the right type of gear. Then assign the gear to the Character and remove from inventory.
            for (int i = 0; i < charactersParty.GearInventory.Count; i++)
            {
                if (charactersParty.GearInventory[i].GetType() == GearToEquip.GetType())
                {
                    playingCharacter.EquippedGear = charactersParty.GearInventory[i];
                    charactersParty.GearInventory.RemoveAt(i);
                    break;
                }    
            }
        }
    }

    public class SlashAttack : Attack
    {
        public SlashAttack()
        {
            Message = "used SLASH";
            Name = "Slash Attack";
            Damage = 2;
        }
    }

    public class StabAttack : Attack
    {
        public StabAttack()
        {
            Message = "used STAB";
            Name = "Stab Attack";
            Damage = 1;
        }
    }

    public enum PlayerType { Human, Computer}

    public enum PartyType { Heroes, Monsters}

    public enum CharacterType { Skeleton, TrueProgrammer, UncodedOne }
}
