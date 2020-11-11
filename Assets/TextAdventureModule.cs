using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
using KModkit;

public class TextAdventureModule : MonoBehaviour
{
    //General
    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMSelectable Module;
    public KMSelectable SecretButton;
    public TextMesh InputText;
    public TextMesh MainScreen;
    public TextMesh LetterScreen;
    public TextMesh NumberScreen;
    public GameObject[] Walls;

    private bool TypingActive;
    private bool ModuleSolved;
    private bool EnemyPresent;
    private bool OutsideUnlocked;

    private int S = 1;
    private int R = 0;
    private int HP = 0;
    private int DMG = 0;
    private int SolveText;
    private int CurLoc;
    private int Orientation;
    private int Letter;
    private int Number;
    private int IJoke;
    private int CurrentScreenFontSize;

    //Easter Eggs
    private static readonly string Welcome = "█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█\n█░░╦─╦╔╗╦─╔╗╔╗╔╦╗╔╗░░█\n█░░║║║╠─║─║─║║║║║╠─░░█\n█░░╚╩╝╚╝╚╝╚╝╚╝╩─╩╚╝░░█\n█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█";
    private static readonly string Zefod = "▒▒▒▒▒▒▄▄██████▄\n▒▒▒▒▒▒▒▒▒▒▄▄████████████▄\n▒▒▒▒▒▒▄▄██████████████████\n▒▒▒▄████▀▀▀██▀██▌███▀▀▀████\n▒▒▐▀████▌▀██▌▀▐█▌████▌█████▌\n▒▒█▒▒▀██▀▀▐█▐█▌█▌▀▀██▌██████\n▒▒█▒▒▒▒████████████████████▌\n▒▒▒▌▒▒▒▒█████░░░░░░░██████▀\n▒▒▒▀▄▓▓▓▒███░░░░░░█████▀▀\n▒▒▒▒▀░▓▓▒▐█████████▀▀▒\n▒▒▒▒▒░░▒▒▐█████▀▀▒▒▒▒▒▒\n▒▒░░░░░▀▀▀▀▀▀▒▒▒▒▒▒▒▒▒\n▒▒▒░░░░░░░░▒▒";
    private static readonly string Doge = "░░░░░░░░░▄░░░░░░░░░░░░░░▄░░░░\n░░░░░░░░▌▒█░░░░░░░░░░░▄▀▒▌░░░\n░░░░░░░░▌▒▒█░░░░░░░░▄▀▒▒▒▐░░░\n░░░░░░░▐▄▀▒▒▀▀▀▀▄▄▄▀▒▒▒▒▒▐░░░\n░░░░░▄▄▀▒░▒▒▒▒▒▒▒▒▒█▒▒▄█▒▐░░░\n░░░▄▀▒▒▒░░░▒▒▒░░░▒▒▒▀██▀▒▌░░░\n░░▐▒▒▒▄▄▒▒▒▒░░░▒▒▒▒▒▒▒▀▄▒▒▌░░\n░░▌░░▌█▀▒▒▒▒▒▄▀█▄▒▒▒▒▒▒▒█▒▐░░\n░▐░░░▒▒▒▒▒▒▒▒▌██▀▒▒░░░▒▒▒▀▄▌░\n░▌░▒▄██▄▒▒▒▒▒▒▒▒▒░░░░░░▒▒▒▒▌░\n▀▒▀▐▄█▄█▌▄░▀▒▒░░░░░░░░░░▒▒▒▐░\n▐▒▒▐▀▐▀▒░▄▄▒▄▒▒▒▒▒▒░▒░▒░▒▒▒▒▌\n▐▒▒▒▀▀▄▄▒▒▒▄▒▒▒▒▒▒▒▒░▒░▒░▒▒▐░\n░▌▒▒▒▒▒▒▀▀▀▒▒▒▒▒▒░▒░▒░▒░▒▒▒▌░\n░▐▒▒▒▒▒▒▒▒▒▒▒▒▒▒░▒░▒░▒▒▄▒▒▐░░\n░░▀▄▒▒▒▒▒▒▒▒▒▒▒░▒░▒░▒▄▒▒▒▒▌░░\n░░░░▀▄▒▒▒▒▒▒▒▒▒▒▄▄▄▀▒▒▒▒▄▀░░░\n░░░░░░▀▄▄▄▄▄▄▀▀▀▒▒▒▒▒▄▄▀░░░░░\n░░░░░░░░░▒▒▒▒▒▒▒▒▒▒▀▀░░░░░░░░";
    private static readonly string ButtonMusic = "───────────────█████★彡\n─────────────███▓▓▓███★彡\n───────────███▓▓▓▓▓▓▓██★彡\n───────────███████▓▓▓▓███★彡\n───────────███───████▓▓▓█████►★彡\n───────────███──────███▓▓███★彡\n───────────███────────████★彡\n───────────███─██████★彡\n───────────█████▓▓▓▓███★彡\n───────────██████▓▓▓▓▓██★彡\n───────────███──████▓▓▓████★彡\n───────────███─────███▓▓▓███►★彡\n───────────███───────█████★彡\n───────────███\n───────────███\n───────────███\n───────────███\n───────────███\n─────█████─███\n──████▓▓▓█████\n─██▓▓▓▓▓▓▓▓███\n██▓▓▓▓▓▓▓▓▓▓██\n██▓▓▓▓▓▓▓▓▓██★彡\n─███▓▓▓▓████★彡\n____██████★彡";
    private static readonly string Tank = "░░░░░░███████]▄▄▄▄▄▄▄▄\n▂▄▅█████████▅▄▃▂\nI███████████████████].\n◥⊙▲⊙▲⊙▲⊙▲⊙▲⊙▲⊙◤...";
    private static readonly string SS4 = "... - .-. .. -.- .\n___________________\n-.- .- -... --- --- --";
    private static readonly string Timwi = "--------------------------------------------------\n--------------------0000000000--------------------\n------------------000000000000------------------\n-----------------00000------00000-----------------\n-----------------0000----------0000----------------\n-----------------0000---------0000----------------\n-----------------00000------00000-----------------\n------------------000000000000-------------------\n-----------------0000000000000-----------------\n----------------00000--------00000----------------\n---------000000000------0000000000---------------\n-----00000000000------000000000000-------------\n-----0000------0000------0000---------0000-----\n----0000--------0000-----0000-----------0000----\n----0000--------0000-----0000-----------0000----\n-----0000------00000----00000--------0000-----\n------00000000000--------00000000000------\n--------000000000------------000000000--------\n-----------------------------------------------------------------------------";
    private static readonly string Keyboard = "Using\nyour\n.-----------------------------------------------------------------------------------------------------------------.\n|-[Esc]-[F1][F2][F3][F4][F5][F6][F7][F8][F9][F0][F10][F11][F12]\n|-----------------------------------------------------------------------------------------------------------------|\n|-[`]-[1]-[2]-[3]-[4]-[5]-[6]-[7]-[8]-[9]-[0]-[-]-[=]-[_<_]-[I]-[H]-[U]-[N][/][*][-]|\n|-[|-][Q]-[W]-[E]-[R]-[T]-[Y]-[U]-[I]-[O]-[P]-[{][}]-|-|-[D][E][D]-[7][8][9]|+||\n|-[CAP]-[A]-[S]-[D]-[F]-[G]-[H]-[J]-[K]-[L]-[;]-[']-[#]-|_|-----------[4][5][6]|_||\n|-[^]-[/]-[Z]-[X]-[C]-[V]-[B]-[N]-[M]-[,]-[.]-[/]-[__^__]------[^]-----[1][2][3]|-||\n|-[c]---[a][___________________________][a]---[c]-[<][V][>]-[-0--][.]|_||\n`-----------------------------------------------------------------------------------------------------------------'\nyou can type <color=\"yellow\">commands</color> into the input terminal.";
    //private static readonly string Banana = "________________________________________████____________________________________________\n______________________________________████████__________________________________________\n";
    private static readonly string[] SolveString = { "▒▒▒▒▒▒▒▒▒▄▄▄▄▒▄▄▄▒▒▒\n▒▒▒▒▒▒▄▀▀▓▓▓▀█░░░█▒▒\n▒▒▒▒▄▀▓▓▄██████▄░█▒▒\n▒▒▒▄█▄█▀░░▄░▄░█▀▀▄▒▒\n▒▒▄▀░██▄░░▀░▀░▀▄▓█▒▒\n▒▒▀▄░░▀░▄█▄▄░░▄█▄▀▒▒\n▒▒▒▒▀█▄▄░░▀▀▀█▀▓█▒▒▒\n▒▒▒▄▀▓▓▓▀██▀▀█▄▀▒▒▒▒\n▒▒█▓▓▄▀▀▀▄█▄▓▓▀█▒▒▒▒\n▒▒▀▄█░░░░░█▀▀▄▄▀█▒▒▒\n▒▒▒▄▀▀▄▄▄██▄▄█▀▓▓█▒▒\n▒▒█▀▓█████████▓▓▓█▒▒\n▒▒█▓▓██▀▀▀▒▒▒▀▄▄█▀▒▒\n▒▒▒▀▀▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒", "░░░░▄▀▀▀▀▀█▀▄▄▄▄░░░░\n░░▄▀▒▓▒▓▓▒▓▒▒▓▒▓▀▄░░\n▄▀▒▒▓▒▓▒▒▓▒▓▒▓▓▒▒▓█░\n█▓▒▓▒▓▒▓▓▓░░░░░░▓▓█░\n█▓▓▓▓▓▒▓▒░░░░░░░░▓█░\n▓▓▓▓▓▒░░░░░░░░░░░░█░\n▓▓▓▓░░░░▄▄▄▄░░░▄█▄▀░\n░▀▄▓░░▒▀▓▓▒▒░░█▓▒▒░░\n▀▄░░░░░░░░░░░░▀▄▒▒█░\n░▀░▀░░░░░▒▒▀▄▄▒▀▒▒█░\n░░▀░░░░░░▒▄▄▒▄▄▄▒▒█░\n░░░▀▄▄▒▒░░░░▀▀▒▒▄▀░░\n░░░░░▀█▄▒▒░░░░▒▄▀░░░\n░░░░░░░░▀▀█▄▄▄▄▀░░░", "░░░░░░░░░░█▄▄▄░░░░░░░░\n░░░░░████▀▀▀███▄░░░░░░\n░░░▄▄█▒▒▒▒▒▒▒▒▀█▌░░░░░\n░░▐██▒▄■▀▒▒▒▀■▄██░░░░░\n░░▐██▒▒▄▄▌▒▐▄▄▒▐█▌░░░░\n░░░██▒▒▒▒▒▒▒▒▒▒▒█▌░░░░\n░░░██▒▒▒▒▒▀▀▒▒▒▒█▌░░░░\n░░░░█▒▒▒▒▒▒▒▒▒▒▒▓░░░░░\n░░░░▓▒▒▒▒▄██▄▒▒▒▓░░░░░\n░░░░▓▒▒▒▒████▒▒▒▓░░░░░\n░░░░░▓▒▒▒▒▒▒▒▒▒▓░░░░░░\n░░░░░░▓▐█▌▒▒▐█▌░░░░░░░\n░░░░░░░░▀████▀░░░░░░░░░", "░░░░░░░░▄▄█▀▀▄░░░░░░░\n░░░░░░▄█████▄▄█▄░░░░░\n░░░░░▄▀██████▄▄██░░░░\n░░░░░█░█▀░░▄▄▀█░█░░░░\n░░░░░▄██░░░▀▀░▀░█░░░░\n░░▄█▀░░▀█░▀▀▀▀▄▀▀█▄░░\n░▄███░▄░░▀▀▀▀▀▄░███▄░\n░██████░░░░░░░██████░\n░▀███▀█████████▀███▀░\n░░░░▄█▄░▀▀█▀░░░█▄░░░░\n░▄▄█████▄▀░▀▄█████▄▄░\n█████████░░░█████████" };
    private static readonly string[] Jokes = { "My boss told me to have\na good day.. so I went\nhome.", "I couldn't figure out\nwhy the baseball kept\ngetting larger. Then\nit hit me.", "I ate a clock\nyesterday, it was\nvery time consuming.", "Why couldn't the\nbicycle stand up?\nBecause it was two\ntired!", "As I suspected,\nsomeone has been\nadding soil to my\ngarden. The plot\nthickens.", "And the lord said\nunto John, 'Come forth\nand you will receive\neternal life'. John\ncame fifth and won a\ntoaster.", "I told my girlfriend\nshe drew her eyebrows\ntoo high. She seemed\nsurprised.", "What do you call a\nfake noodle? An\nImpasta.", "Why did the coffee\nfile a police report?\nIt got mugged.", "Want to hear a joke\nabout construction?\nI'm still working on it.", "What do you call a\nfat psychic? A\nfour-chin teller.", "An apple a day keeps\nanyone away if you\nthrow it hard enough." };
    //private static readonly string Banana = "████\n████████\n██    ██\n██    ████\n██      ████\n██        ████\n██          ██\n██████████      ████\n██  ██      ██      ██\n████████    ██      ██\n██  ██      ██      ██\n██████████    ██  ██\n██      ██████  ██\n████████▒▒▒▒██  ██\n██▒▒▒▒▒▒██    ██\n██▒▒████      ██\n██████          ██\n██████      ██            ████  ██████\n██    ████  ████            ██  ████    ██\n██      ██  ██            ████  ██      ██\n██    ████  ██            ██    ████    ██\n██████  ████          ████    ▒▒██████\n▒▒  ██          ████▒▒  ▒▒▒▒\n▒▒▒▒████    ██████  ▒▒▒▒▒▒\n▒▒▒▒▒▒██████████▒▒▒▒▒▒▒▒▒▒\n██████                ▒▒██████\n██      ██████      ██████      ██\n██          ██      ██          ██\n▓▓▓▓████▓▓██      ██▓▓▓▓▓▓████";

    //The Castle
    private readonly string[] UpstairsRoomNames = {
        "Lord’s Chambers", "Lady’s Chambers", "Workshop", "Armory",
        "Storage Room", "Observatory", "Minstrel’s Gallery", "Guest Room",
        "Laboratory", "Study", "Wardrobe", "Buttery",
        "Archives", "Old Archives", "Laundry", "Library"
    };
    private readonly string[] DownstairsRoomNames = {
        "Guillotine Room", "Great Hall", "Kitchen", "Pantry",
        "Winery", "Refinery", "Prison", "Bathroom",
        "Bakery", "Forge", "Throne Room", "Ballroom",
        "Dungeon", "Oratory", "Undercroft", "Porter’s Lodge"
    };
    private string[] StartingLetterString = { "A", "B", "C", "D", "E", "F", "G", "X" };
    private string[] StartingNumberString = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
    private string CurrentInput;
    private string CurrentLetter;
    private string CurrentNumber;
    private string StoreStartingNumber;
    private string StoreStartingLetter;
    private string CurrentScreen;

    private Coroutine textActive;

    private int?[][] RoomConnections;

    const int size = 4;

    /* NOTES:
     * Laundry starting location unicorn?
     * Make the workshop a room for useable items?
     *
    */

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;

    private void Awake()
    {
        moduleId = moduleIdCounter++;
        {
            Module.OnFocus += delegate () { TypingActive = true; };
            Module.OnInteract += delegate () { TypingActive = true; return true; };
            Module.OnCancel += delegate () { TypingActive = false; return true; };
            Module.OnDefocus += delegate () { TypingActive = false; };
            KMSelectable PressedButton = SecretButton;
            SecretButton.OnInteract += delegate () { ButtonPress(PressedButton); return false; };
        }
    }
    //Begin
    void Start()
    {
        InputText.text = "";
        CurrentInput = "";
        MainScreen.text = "";
        GenerateCastle();
        if (R == 0)
        {
            GenerateNumber();
            GenerateModifier();
        }
        else
        {
            NumberScreen.text = StoreStartingNumber;
            LetterScreen.text = StoreStartingLetter;
        }
        StatCalculations();
        UpdateWalls();
    }

    void Update()
    {
        if (TypingActive == true)
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b')
                {
                    if (InputText.text.Length != 0)
                    {
                        InputText.text = InputText.text.Substring(0, InputText.text.Length - 1);
                    }
                }
                else if ((c == '\n') || (c == '\r'))
                {
                    CurrentInput = InputText.text;
                    StringInterpreter();
                }
                else if (InputText.text.Length >= 15)
                {
                    return;
                }
                else
                {
                    InputText.text += c;
                }
            }
        }
        else if (TypingActive != true)
        {
            CurrentInput = "";
            InputText.text = "";
            return;
        }
    }

    protected void GenerateCastle()
    {
        foreach (GameObject Wall in Walls)
        {
            Wall.SetActive(false);
        }
        // Decide on the names of the rooms
        DownstairsRoomNames.Shuffle();
        UpstairsRoomNames.Shuffle();

        // Decide which pair of rooms is the staircase
        var staircaseLocation = UnityEngine.Random.Range(0, size * size);

        // Decide which room we start in
        var startLocation = Enumerable.Range(0, 2 * size * size).Where(room => room % (size * size) != staircaseLocation).PickRandom();
        CurLoc = startLocation;

        // Decide which room on the non-starting floor is the secret room
        var secretRoom = Enumerable.Range(0, 2 * size * size).Where(room => room / (size * size) != startLocation / (size * size) && room % (size * size) != staircaseLocation).PickRandom();

        // Decide on the starting orientation
        Orientation = UnityEngine.Random.Range(0, 4);

        // Generate the mazes (one for each floor)
        RoomConnections = new int?[2 * size * size][];
        var bottomFloorMaze = generateMaze(startLocation / (size * size) == 0 ? staircaseLocation : secretRoom % (size * size));
        var topFloorMaze = generateMaze(startLocation / (size * size) == 0 ? secretRoom % (size * size) : staircaseLocation);

        for (var r = 0; r < (size * size); r++)
            RoomConnections[r] = bottomFloorMaze[r];
        for (var r = 0; r < (size * size); r++)
            RoomConnections[r + size * size] = topFloorMaze[r];

        // Generate a way out of the castle anywhere on the bottom floor
        var exitLocation = UnityEngine.Random.Range(0, 4 * size);
        if (exitLocation < size)
            RoomConnections[exitLocation][0] = -1;
        else if (exitLocation < 2 * size)
            RoomConnections[(exitLocation - size) * size + size - 1][1] = -1;
        else if (exitLocation < 3 * size)
            RoomConnections[(exitLocation - 2 * size) + size * size - size][2] = -1;
        else
            RoomConnections[(exitLocation - 3 * size) * size][3] = -1;

        // Connect the staircase location on both floors with each other
        RoomConnections[staircaseLocation][4] = staircaseLocation + (size * size);
        RoomConnections[staircaseLocation + (size * size)][4] = staircaseLocation;

        string GenerateStartText = String.Format("{0}\n<<>><<>><<>><<>><<>><<>><<>><<>>\nYou find yourself in the\n<color=\"red\">{1}</color>.\nLooking for a way to get\n<color=\"cyan\">outside</color>.", Welcome, currentRoomName());
        textActive = StartCoroutine(DisplayText(GenerateStartText, speed: 0.025f, fontSize: 15));
        Debug.LogFormat("[Text Adventure #{0}] You started in the {1}!", moduleId, currentRoomName());
    }

    static int?[][] generateMaze(params int[] avoidRooms)
    {
        int?[][] maze = new int?[size * size][];
        for (var r = 0; r < (size * size); r++)
            maze[r] = new int?[5];

        List<int> todo = Enumerable.Range(0, size * size).Except(avoidRooms).ToList();
        List<int> processed = new List<int>();
        List<int> done = new List<int>();

        // Decide on a random position to start generating the maze
        var randomStartRoom = todo.PickRandom();
        todo.Remove(randomStartRoom);
        processed.Add(randomStartRoom);

        var dx = new[] { 0, 1, 0, -1 };
        var dy = new[] { -1, 0, 1, 0 };

        while (todo.Count > 0)
        {
            var randomRoom = processed.PickRandom();
            var x = randomRoom % size;
            var y = randomRoom / size;
            var availableDoors = Enumerable.Range(0, 4).Where(door => x + dx[door] >= 0 && x + dx[door] < size && y + dy[door] >= 0 && y + dy[door] < size && todo.Contains(x + dx[door] + size * (y + dy[door]))).ToArray();
            if (availableDoors.Length == 0)
            {
                processed.Remove(randomRoom);
                done.Add(randomRoom);
            }
            else
            {
                var door = availableDoors.PickRandom();
                var newRoom = x + dx[door] + size * (y + dy[door]);
                maze[randomRoom][door] = newRoom;
                maze[newRoom][(door + 2) % 4] = randomRoom;
                todo.Remove(newRoom);
                processed.Add(newRoom);
            }
        }
        return maze;
    }

    protected void GenerateModifier()
    {
        if (ModuleSolved != true)
        {
            Letter = UnityEngine.Random.Range(0, 8);
            LetterScreen.text = StartingLetterString[Letter];
            StoreStartingLetter = LetterScreen.text;
            CurrentLetter = LetterScreen.text;

            switch (CurrentLetter)
            {
                case "A":
                {

                    break;
                }
                case "B":
                {

                    break;
                }
                case "C":
                {

                    break;
                }
                case "D":
                {

                    break;
                }
                case "E":
                {
                    break;
                }
                case "F":
                {
                    break;
                }
                case "G":
                {
                    break;
                }
                case "X":
                {
                    break;
                }
                default:
                    textActive = StartCoroutine(DisplayText("Failed to return letter! THIS IS A BUG! REPORT IT!"));
                    Debug.LogFormat("[Text Adventure #{0}] Failed to return letter! THIS IS A BUG! REPORT IT!", moduleId);
                    HandleSolve();
                    return;
            }
        }
    }

    protected void GenerateNumber()
    {
        if (ModuleSolved != true)
        {

            Number = 0;
            Number = UnityEngine.Random.Range(0, 10);
            NumberScreen.text = StartingNumberString[Number];
            StoreStartingNumber = NumberScreen.text;
            CurrentNumber = NumberScreen.text;
            if (Number > 5)
            {
                //Stuff
            }
        }
    }

    protected void StatCalculations()
    {
        if (ModuleSolved != true)
        {
            switch (CurrentNumber)
            {
                case "1":
                {
                    HP = 2;
                    DMG = 2;
                    break;
                }
                case "2":
                {
                    HP = 4;
                    DMG = 4;
                    break;
                }
                case "3":
                {
                    HP = 6;
                    DMG = 6;
                    break;
                }
                case "4":
                {
                    HP = 8;
                    DMG = 8;
                    break;
                }
                case "5":
                {
                    HP = 10;
                    DMG = 10;
                    break;
                }
                case "6":
                {
                    HP = 12;
                    DMG = 12;
                    break;
                }
                case "7":
                {
                    HP = 14;
                    DMG = 14;
                    break;
                }
                case "8":
                {
                    HP = 16;
                    DMG = 16;
                    break;
                }
                case "9":
                {
                    HP = 18;
                    DMG = 18;
                    break;
                }
                default:
                    if (Number >= 10)
                    {
                        HP = 20;
                        DMG = 20;
                        break;
                    }
                    break;
            }
        }
    }

    private string currentRoomName()
    {
        return (CurLoc / (size * size) == 0 ? DownstairsRoomNames : UpstairsRoomNames)[CurLoc % (size * size)];
    }

    protected void MoveDirection(int dir, string givenDirection)
    {
        // RoomConnections contains a list of doors for our current location:
        // null means there’s a wall (no door)
        // -1 means the door leads to the outside
        // any other number is the index of the room it leads to
        if (RoomConnections[CurLoc][dir] == -1 && OutsideUnlocked)
        {
            foreach (GameObject Wall in Walls)
            {
                Wall.SetActive(false);
            }
            HandleSolve();
        }
        else if (RoomConnections[CurLoc][dir] != null)
        {
            switch (dir)
            {
                //This should hopefully move the proper direction, the connections itself should have already checked
                //if it's possible to turn in the given direction or not
                case 0:
                    CurLoc -= size;
                    break;
                case 1:
                    CurLoc++;
                    break;
                case 2:
                    CurLoc += size;
                    break;
                case 3:
                    CurLoc--;
                    break;
            }
            Orientation = dir;
            Debug.LogFormat("[Text Adventure #{0}] You went {1}!", moduleId, givenDirection);
            string roomName = currentRoomName();
            Debug.LogFormat("[Text Adventure #{0}] Moved to {1}", moduleId, roomName);

            var floorNames = new[] { "1st", "2nd" };
            var directionNames = new[] { "north", "east", "south", "west" };

            Debug.LogFormat("<Text Adventure #{0}> The current room is located in {1}{2} on the {3} floor of the castle. Currently facing {4}", moduleId, (char) ('A' + CurLoc % size), (CurLoc % (size * size)) / size + 1, floorNames[CurLoc / (size * size)], directionNames[Orientation]);
            string GenerateText = String.Format("You moved <color=\"yellow\">{1}</color> and\nyou are now in the\n<color=\"red\">{0}</color>!", roomName, givenDirection);
            textActive = StartCoroutine(DisplayText(GenerateText, speed: 0.025f));
            UpdateWalls();
        }
        else
        {
            Debug.LogFormat("[Text Adventure #{0}] You cannot move {1}.", moduleId, givenDirection);
            textActive = StartCoroutine(DisplayText(string.Format("You cannot move\n<color=\"yellow\">{0}</color>.", givenDirection), speed: 0.025f, restoreTextAfterDelay: 1f));
        }
    }

    private void UpdateWalls()
    {
        for (var d = 0; d < 4; d++)
            Walls[(d + 4 - Orientation) % 4].SetActive(RoomConnections[CurLoc][d] == null);
    }

    protected void StringInterpreter()
    {
        int sendRotation = Orientation;
        if (ModuleSolved != true)
        {
            switch (CurrentInput.ToLower())
            {
                case "left":
                case "l":
                case "ml":
                case "move left":
                case "moveleft":
                case "movel":
                case "mleft":
                    MoveDirection((Orientation + 3) % 4, "left");
                    break;
                case "right":
                case "r":
                case "mr":
                case "move right":
                case "moveright":
                case "mover":
                case "mright":
                    MoveDirection((Orientation + 1) % 4, "right");
                    break;
                case "back":
                case "b":
                case "mb":
                case "move back":
                case "moveback":
                case "moveb":
                case "mback":
                    MoveDirection((Orientation + 2) % 4, "back");
                    break;
                case "forward":
                case "f":
                case "mf":
                case "move forward":
                case "moveforward":
                case "movef":
                case "mforward":
                    MoveDirection(Orientation, "forward");
                    break;
                //Commands for staircase usage (bridge between both floors)
                case "up":
                case "down":
                {
                    if (RoomConnections[CurLoc][4] != null)
                    {
                        CurLoc = (CurLoc + size * size) % (size * size);
                        Debug.LogFormat("[Text Adventure #{0}] You went {1}stairs!", moduleId, CurrentInput.ToLower());
                        Debug.LogFormat("[Text Adventure #{0}] Moved to {1}", moduleId, currentRoomName());
                        string GenerateText = String.Format("You went <color=\"yellow\">{0}</color> the\nstairs and you are\nnow in the <color=\"red\">{1}</color>!", CurrentInput.ToLowerInvariant(), currentRoomName());
                        textActive = StartCoroutine(DisplayText(GenerateText, fontSize: 20, speed: 0.025f));
                    }
                    else
                    {
                        Debug.LogFormat("[Text Adventure #{0}] There are no stairs in this room.", moduleId);
                        int Rndm = UnityEngine.Random.Range(0, 2);
                        string[] randomString = { "You tried with all your\nmight to locate a set of\nwooden planks to\ncontinue your quest\nonward, <color=\"red\">But! alas</color> - there\nwere none available, and\nso you must continue\nyour quest in search\nof the path to the\nother floor.", "No." };
                        textActive = StartCoroutine(DisplayText(randomString[Rndm], fontSize: 20, speed: 0.025f, restoreTextAfterDelay: 2f));
                    }
                    break;
                }
                case "flee":
                case "fl":
                {
                    if (EnemyPresent == true)
                    {
                        textActive = StartCoroutine(DisplayText("Attempting to flee combat!..."));
                        Debug.LogFormat("[Text Adventure #{0}] You attempted to flee!", moduleId);
                        break;
                    }
                    else
                    {
                        Debug.LogFormat("[Text Adventure #{0}] The enemy caught you as you attempted to flee and were unable to escape! Fight or perish! Good luck!", moduleId);
                    }
                    break;
                }
                case "attack":
                case "att":
                case "a":
                {
                    if (EnemyPresent == true)
                    {
                        Debug.LogFormat("[Text Adventure #{0}] You attempted an attack!", moduleId);
                        //ATTACK SOMETHING
                    }
                    break;
                }
                case "stats":
                case "player":
                case "p":
                {
                    break;
                }
                case "search room":
                case "searchroom":
                case "search":
                case "s":
                {
                    break;
                }
                case "inspect":
                case "i":
                {
                    break;
                }
                case "inspectstats":
                case "is":
                {
                    break;
                }
                //Testing Purposes
                case "solve":
                {
                    HandleSolve();
                    break;
                }
                //Testing Purposes
                case "strike":
                {
                    HandleStrike();
                    break;
                }
                case "helpme":
                case "help me":
                {
                    textActive = StartCoroutine(DisplayText("You are alone in this endeavor... To succeed \nor to perish within the castle is up to those \nwho are <color=\"blue\">worthy</color> to seek thy name.", fontSize: 13, speed: 0.025f, restoreTextAfterDelay: 1f));
                    break;
                }
                case "reset":
                case "restart":
                {
                    Debug.LogFormat("[Text Adventure #{0}] Module restart called! Restarting the module...", moduleId);
                    Audio.PlaySoundAtTransform("Reset", transform);
                    R++;
                    Start();
                    break;
                }
                //Easter Eggs
                case "banana":
                {
                        //textActive = StartCoroutine(DisplayText(Banana, fontSize: 10, speed: 0.025f, restoreTextAfterDelay: 1f));
                        break;
                }
                case "zefod42":
                case "zefod":
                case "salt":
                {
                    Audio.PlaySoundAtTransform("Zefod", transform);
                    textActive = StartCoroutine(DisplayText(Zefod, fontSize: 13, speed: 0.025f, restoreTextAfterDelay: 1f));
                    Debug.LogFormat("[Text Adventure #{0}] One Salty Easter Egg!", moduleId);
                    break;
                }
                case "timwi":
                {
                    textActive = StartCoroutine(DisplayText(Timwi, fontSize: 10, speed: 0.00025f, restoreTextAfterDelay: 1f));
                    Debug.LogFormat("[Text Adventure #{0}] Timwi!", moduleId);
                    break;
                }
                case "tank":
                case "tonk":
                {
                    textActive = StartCoroutine(DisplayText(Tank, fontSize: 17, speed: 0.00025f, restoreTextAfterDelay: 1f));
                    Debug.LogFormat("[Text Adventure #{0}] Tonk!", moduleId);
                    break;
                }
                case "doge":
                {
                    Audio.PlaySoundAtTransform("Doge", transform);
                    textActive = StartCoroutine(DisplayText(Doge, fontSize: 10, speed: 0.025f, restoreTextAfterDelay: 1f));
                    Debug.LogFormat("[Text Adventure #{0}] Doge has been found!", moduleId);
                    break;
                }
                case "tell me a joke":
                case "tellmeajoke":
                case "joke":
                case "jokes":
                {
                    IJoke = UnityEngine.Random.Range(0, Jokes.Length);
                    textActive = StartCoroutine(DisplayText(Jokes[IJoke], speed: 0.025f, restoreTextAfterDelay: 3f));
                    break;
                }
                case "help":
                {
                    textActive = StartCoroutine(DisplayText(Keyboard, fontSize: 9, speed: 0.025f, restoreTextAfterDelay: 2f));
                    break;
                }
                default:
                    textActive = StartCoroutine(DisplayText("<color=\"red\">I don't understand\nwhat you mean.</color>", speed: 0.025f, fontSize: 30, restoreTextAfterDelay: 1f));
                    break;
            }
        }
        else if (S == 1)
        {
            textActive = StartCoroutine(DisplayText("You have already solved this module! How about solve the module next to me instead? →", speed: 0.025f, fontSize: 30));
            S++;
        }
        else if (S == 2)
        {
            textActive = StartCoroutine(DisplayText("I'm flattered you want to continue talking to me but I've said you've already finished this module.", speed: 0.025f, fontSize: 30));
            S++;
        }
        else if (S == 3)
        {
            textActive = StartCoroutine(DisplayText("That's it! I've had enough of this. I'm not talking to you anymore. (>ლ)", speed: 0.025f, fontSize: 30));
            S++;
        }
        else if (S == 4)
        {
            textActive = StartCoroutine(DisplayText(SS4, fontSize: 6));
            Audio.PlaySoundAtTransform("SS4", transform);
            S++;
        }
        else
        {
            return;
        }
    }

    IEnumerator DisplayText(string text, int fontSize = 24, float? speed = null, float? restoreTextAfterDelay = null)
    {
        if (textActive != null)
        {
            StopCoroutine(textActive);
            textActive = null;
        }

        if (restoreTextAfterDelay == null)
        {
            CurrentScreen = text;
            CurrentScreenFontSize = fontSize;
        }

        CurrentInput = "";
        InputText.text = "";
        MainScreen.text = "";
        MainScreen.fontSize = fontSize;

        string colorTag = null;
        string colorEndTag = null;
        int colorTagStartIndex = -1; // Position where the start tag will need to be inserted into MainScreen.text
        int colorTagEndIndex = -1;   // Position within ‘text’ where the end tag was found

        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(speed.Value);

            // Check for the presence of a color tag
            var m = Regex.Match(text.Substring(i), @"^(<(\w+)[^>]*>).*?(</\2>)", RegexOptions.Singleline);
            if (m.Success)
            {
                colorTag = m.Groups[1].Value;
                colorEndTag = m.Groups[3].Value;
                colorTagStartIndex = MainScreen.text.Length;
                colorTagEndIndex = i + m.Groups[3].Index;
                i += m.Groups[1].Length;
            }

            if (i == colorTagEndIndex)
            {
                var oldText = MainScreen.text;
                var newText = oldText.Substring(0, colorTagStartIndex) + colorTag + oldText.Substring(colorTagStartIndex) + colorEndTag;
                for (var j = 0; j < 3; j++)
                {
                    MainScreen.text = oldText;
                    yield return new WaitForSeconds(0.1f);
                    MainScreen.text = newText;
                    yield return new WaitForSeconds(0.1f);
                }
                i += colorEndTag.Length;
            }
            if (i < text.Length)
                MainScreen.text += text[i];
        }

        if (restoreTextAfterDelay != null)
        {
            yield return new WaitForSeconds(restoreTextAfterDelay.Value);
            textActive = StartCoroutine(DisplayText(CurrentScreen, speed: 0.025f, fontSize: CurrentScreenFontSize));
        }
        else
        {

        }
    }

    protected void ButtonPress(KMSelectable SecretButton)
    {
        if (ModuleSolved == true)
        {
            MainScreen.transform.localPosition = MainScreen.transform.localPosition + Vector3.left * 0.05f;
        }
        SecretButton.AddInteractionPunch(.3f);
        Audio.PlaySoundAtTransform("SecretButton", transform);
        textActive = StartCoroutine(DisplayText(ButtonMusic, fontSize: 8, speed: .06f));
    }

    protected void HandleStrike()
    {
        GetComponent<KMBombModule>().HandleStrike();
        Audio.PlaySoundAtTransform("Strike", transform);
        Start();
    }
    protected void HandleSolve()
    {
        ModuleSolved = true;
        MainScreen.transform.localPosition = MainScreen.transform.localPosition + Vector3.right * 0.05f;
        MainScreen.text = "";
        NumberScreen.fontSize = 59;
        Audio.PlaySoundAtTransform("Solve", transform);
        SolveText = UnityEngine.Random.Range(0, 5);
        textActive = StartCoroutine(DisplayText(SolveString[SolveText], fontSize: 14));
        LetterScreen.text = "G";
        NumberScreen.text = "G";
        NumberScreen.transform.localPosition = NumberScreen.transform.localPosition + Vector3.right * 0.04f;
        GetComponent<KMBombModule>().HandlePass();
    }


#pragma warning disable 414
    private static readonly string TwitchHelpMessage = @"!{0} press <1234> [Presses the specified buttons in reading order]";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*secretbutton\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            SecretButton.OnInteract();
            yield break;
        }
    }
}
