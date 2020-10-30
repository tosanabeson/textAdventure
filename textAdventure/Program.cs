using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
	class Exit
	{
		public enum Directions
		{
			Undefined, North, South, East, West, Up, Down, NorthEast, NorthWest, SouthEast, SouthWest, In, Out
		};

		public static string[] shortDirections = { "Null", "N", "S", "E", "W", "U", "D", "NE", "NW", "SE", "SW", "I", "O" };

		private Location leadsTo;
		private Directions direction;

		public Exit()
		{
			direction = Directions.Undefined;
			leadsTo = null;
		}

		public Exit(Directions _direction, Location newLeadsTo)
		{
			direction = _direction;
			leadsTo = newLeadsTo;
		}

		public override string ToString()
		{
			return direction.ToString();
		}

		public void setDirection(Directions _direction)
		{
			direction = _direction;
		}

		public Directions getDirection()
		{
			return direction;
		}

		public string getShortDirection()
		{
			return shortDirections[(int)direction].ToLower();
		}

		public void setLeadsTo(Location _leadsTo)
		{
			leadsTo = _leadsTo;
		}

		public Location getLeadsTo()
		{
			return leadsTo;
		}
	}
}

namespace TextAdventure
{
    class Game
    {
        Location currentLocation;

        public bool isRunning = true;
        private bool _gameOver = false;

        private List<Item> inventory;

        public Game()
        {
            inventory = new List<Item>();

            Console.WriteLine("Welcome adventurer, prepare yourself for a fantastical journey into the unknown.");
            Console.WriteLine("Press 'h' or type 'help' for help.");

            // build the "map"
            Location l1 = new Location("Entrance to hall", "You stand at the entrance of a long hallway. The hallways gets darker\nand darker, and you cannot see what lies beyond. To the east\nis an old oaken door, unlocked and beckoning.");
            Item rock = new Item("rock", true, "A rather jagged rock, slightly smaller than a fist.");
            l1.addItem(rock);

            Location l2 = new Location("End of hall", "You have reached the end of a long dark hallway. You can\nsee nowhere to go but back.");
            Item window = new Item("window", false, "A single sheet of glass. It seems sealed up.");
            l2.addItem(window);

            Location l3 = new Location("Small study", "This is a small and cluttered study, containing a desk covered with\npapers. Though they no doubt are of some importance,\nyou cannot read their writing");

            l1.addExit(new Exit(Exit.Directions.North, l2));
            l1.addExit(new Exit(Exit.Directions.East, l3));

            l2.addExit(new Exit(Exit.Directions.South, l1));

            l3.addExit(new Exit(Exit.Directions.West, l1));

            currentLocation = l1;
            showLocation();
        }

        public void showLocation()
        {
            Console.WriteLine("\n" + currentLocation.getTitle() + "\n");
            Console.WriteLine(currentLocation.getDescription());

            if (currentLocation.getInventory().Count > 0)
            {
                Console.WriteLine("\nThe room contains the following:\n");

                for (int i = 0; i < currentLocation.getInventory().Count; i++)
                {
                    Console.WriteLine(currentLocation.getInventory()[i].Name);
                }
            }

            Console.WriteLine("\nAvailable Exits: \n");

            foreach (Exit exit in currentLocation.getExits())
            {
                Console.WriteLine(exit.getDirection());
            }

            Console.WriteLine();
        }

        // TODO: Implement the input handling algorithm.
        public void doAction(string command)
        {
            //Help command is NEW
            if (command == "help" || command == "h")
            {
                Console.WriteLine("Welcome to this Text Adventure!");
                Console.WriteLine("'l' / 'look':        Shows you the room, its exits, and any items it contains.");
                Console.WriteLine("'Look at X':         Gives you information about a specific item in your \n                     inventory, where X is the items name.");
                Console.WriteLine("'pick up X':         Attempts to pick up an item, where X is the items name.");
                Console.WriteLine("'use X':             Attempts to use an item, where X is the items name.");
                Console.WriteLine("'i' / 'inventory':   Allows you to see the items in your inventory.");
                Console.WriteLine("'q' / 'quit':        Quits the game.");
                Console.WriteLine();
                Console.WriteLine("Directions can be input as either the full word, or the abbriviation, \ne.g. 'North or N'");
                return;
            }

            //If statement to access the player inventory
            //This can't be changed a great deal
            if (command == "inventory" || command == "i")
            {
                showInventory();
                Console.WriteLine();
                return;
            }

            //If statement for player to pick up objects
            //This works fine. Change how the function works later though.
            if (command.Length >= 7 && command.Substring(0, 7) == "pick up")
            {
                if (command == "pick up")
                {
                    Console.WriteLine("\nPlease specify what you would like to pick up.\n");
                    return;
                }
                if (currentLocation.getInventory().Exists(x => x.Name == command.Substring(8)) && currentLocation.getInventory().Exists(x => x.Useable == true))
                {
                    inventory.Add(currentLocation.takeItem(command.Substring(8)));
                    Console.WriteLine("\nYou pick up the " + command.Substring(8) + ".\n");
                    return;
                }
                if (currentLocation.getInventory().Exists(x => x.Name == command.Substring(8)) && currentLocation.getInventory().Exists(x => x.Useable == false))
                {
                    Console.WriteLine("\nYou cannot pick up the " + command.Substring(8) + ".\n");
                    return;
                }
                else
                {
                    Console.WriteLine("\n" + command.Substring(8) + " does not exist.\n");
                    return;
                }
            }

            if (command == "look" || command == "l")
            {
                showLocation();
                if (currentLocation.getInventory().Count == 0)
                {
                    Console.WriteLine("There are no items of interest in the room.\n");
                }
                return;
            }

            if (command.Length >= 7 && command.Substring(0, 7) == "look at")
            {
                if (command == "look at")
                {
                    Console.WriteLine("\nPlease specify what you wish to look at.\n");
                    return;
                }
                if (currentLocation.getInventory().Exists(x => x.Name == command.Substring(8)) || inventory.Exists(x => x.Name == command.ToLower().Substring(8)))
                {
                    if (command.Substring(8).ToLower() == "rock")
                    {
                        Console.WriteLine("\n" + currentLocation.getInventory().Find(x => x.Name.Contains("rock")).Description + "\n");
                        return;
                    }

                    if (command.Substring(8).ToLower() == "window")
                    {
                        Console.WriteLine("\n" + currentLocation.getInventory().Find(x => x.Name.Contains("window")).Description + "\n");
                        return;
                    }

                    if (command.Substring(8).ToLower() == "smashed window")
                    {
                        Console.WriteLine("\n" + currentLocation.getInventory().Find(x => x.Name.Contains("smashed window")).Description + "\n");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("\nThat item does not exist in this location or your inventory.\n");
                    return;
                }
            }

            if (command.Length >= 3 && command.Substring(0, 3) == "use")
            {
                if (command == "use")
                {
                    Console.WriteLine("\nPlease specify what you wish to use.\n");
                    return;
                }
                if (inventory.Exists(x => x.Name == command.ToLower().Substring(4)))
                {
                    Console.WriteLine("\nUse " + command.Substring(4) + " with?\n");
                    string secondItem = Console.ReadLine();
                    if (currentLocation.getInventory().Exists(x => x.Name == secondItem))
                    {
                        if (secondItem == "window" && command.Substring(4) == "rock")
                        {
                            Item smashedWindow = new Item("smashed window", false, "A window frame with broken pieces of glass inside.");
                            currentLocation.addItem(smashedWindow);
                            foreach (Item item in currentLocation.getInventory())
                            {
                                if (item.Name.ToLower() == "window")
                                {
                                    currentLocation.removeItem(item);
                                    break;
                                }

                                if (item.Name.ToLower() == "rock")
                                {
                                    currentLocation.removeItem(item);
                                    break;
                                }

                            }
                            Console.WriteLine("\nYou smash in the window.\n");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot do the thing.");
                        return;
                    }
                }
                if (currentLocation.getInventory().Exists(x => x.Name == command.ToLower().Substring(4)))
                {
                    if (command.ToLower().Substring(4) == "window")
                    {
                        Console.WriteLine("\nThe window's locked tight, with no way of opening it.\n");
                        return;
                    }
                    if (command.ToLower().Substring(4) == "smashed window")
                    {
                        Console.WriteLine("\nYou clamber out the window. Victory is yours!");
                        _gameOver = true;
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("\nThere is nothing to use.\n");
                    return;
                }
            }

            if (moveRoom(command))
                return;

            Console.WriteLine("\nInvalid command, are you confused?\n");
        }

        private bool moveRoom(string command)
        {
            foreach (Exit exit in currentLocation.getExits())
            {
                if (command == exit.ToString().ToLower() || command == exit.getShortDirection().ToLower())
                {
                    currentLocation = exit.getLeadsTo();
                    Console.WriteLine("\nYou move " + exit.ToString() + " to the:\n");
                    showLocation();
                    return true;
                }
            }
            return false;
        }


        private void showInventory()
        {
            if (inventory.Count > 0)
            {
                Console.WriteLine("\nA quick look in your bag reveals the following:\n");

                foreach (Item item in inventory)
                {
                    Console.WriteLine(item.Name);
                }
            }
            else
            {
                Console.WriteLine("\nYour bag is empty.\n");
            }
        }

        public void Update()
        {
            string currentCommand = Console.ReadLine().ToLower();

            // instantly check for a quit
            if (currentCommand == "quit" || currentCommand == "q")
            {
                isRunning = false;
                return;
            }


            if (!_gameOver)
            {
                // otherwise, process commands.
                doAction(currentCommand);
            }
            else
            {
                Console.WriteLine("\nNope. Time to quit.\n");
            }
        }
    }
}


namespace TextAdventure
{
    class Item
    {

        public string name;
        private bool useable;
        private string description;

        public Item(string _name, bool canUse, string _description)
        {
            name = _name;
            useable = canUse;
            description = _description;
        }

        public string Name
        {
            get { return name; }
        }

        public bool Useable
        {
            get { return useable; }
        }

        public string Description
        {
            get { return description; }
        }
    }
}

namespace TextAdventure
{
    class Location
    {
        private string roomTitle;
        private string roomDescription;
        private List<Exit> exits;
        private List<Item> inventory;

        public Location()
        {
            // Blank out the title and description at start
            roomTitle = "";
            roomDescription = "";
            exits = new List<Exit>();
            inventory = new List<Item>();
        }

        public Location(string title)
        {
            roomTitle = title;
            roomDescription = "";
            exits = new List<Exit>();
            inventory = new List<Item>();
        }

        public Location(string title, string description)
        {
            roomTitle = title;
            roomDescription = description;
            exits = new List<Exit>();
            inventory = new List<Item>();
        }

        public override string ToString()
        {
            return roomTitle;
        }

        public void addExit(Exit exit)
        {
            exits.Add(exit);
        }

        public void removeExit(Exit exit)
        {
            if (exits.Contains(exit))
            {
                exits.Remove(exit);
            }
        }

        public List<Exit> getExits()
        {
            return new List<Exit>(exits);
        }

        public List<Item> getInventory()
        {
            return new List<Item>(inventory);
        }

        public void addItem(Item itemToAdd)
        {
            inventory.Add(itemToAdd);
        }

        public void removeItem(Item itemToRemove)
        {
            if (inventory.Contains(itemToRemove))
            {
                inventory.Remove(itemToRemove);
            }
        }

        public Item takeItem(string name)
        {
            foreach (Item _item in inventory)
            {
                if (_item.Name.ToLower() == name)
                {
                    Item temp = _item;
                    inventory.Remove(temp);
                    return temp;
                }
            }

            return null;
        }

        public string getTitle()
        {
            return roomTitle;
        }

        public void setTitle(string title)
        {
            roomTitle = title;
        }

        public string getDescription()
        {
            return roomDescription;
        }

        public void setDescription(string description)
        {
            roomDescription = description;
        }
    }
}
namespace TextAdventure
{
    class Program
    {


        static void Main(string[] args)
        {

            Game _Game = new Game();

            //start our game loop - we keep running this function until the player quits.
            while (_Game.isRunning)
            {
                _Game.Update();
            }

        }
    }
}