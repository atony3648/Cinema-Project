using Cinema_Project;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cinema_Project
{
    internal class MainMenu
    {
        private static string StaffFilename = "UsernameAndPasswords.txt";
        private static string CinemaTreeFilename = "CinemaTree.txt";
        private static string FilmFilename = "FilmNames.txt";
        private static string username;
        private static bool IsManager;
        private static string password;
        private static Cinema cinema;

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---- CINEMA MANAGEMENT SYSTEM ----");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            bool IsRunning = true;
            OutputMenu();
            while (IsRunning)
            {
                if (IsManager)
                {
                    ManagerMenu();
                }
                else
                {
                    EmployeeMenu();
                }
            }
        }

        public static void OutputMenu()
        {
            // Type user and password
            Console.Write("Please enter username: ");
            username = Console.ReadLine();
            Console.Write("Please enter password: ");
            password = Console.ReadLine();

            // calls password and username checker inside
            // of Status as CredentialChecker returns boolean
            Console.Clear();
            Status(CredentialChecker(username, password));
        }

        static int CredentialChecker(string username, string password)
        {
            string hashedUser = SimpleHasher.GetFnv1aHash(username).ToString();
            string hashedPass = SimpleHasher.GetFnv1aHash(password).ToString();

            // using makes file close automatically - not sure if needed
            using (StreamReader sr = new StreamReader(StaffFilename))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    string storedPasswordHash = sr.ReadLine();
                    bool storedIsManager = Convert.ToBoolean(sr.ReadLine());

                    if (currentLine == hashedUser && storedPasswordHash == hashedPass)
                    {
                        IsManager = storedIsManager;
                        return IsManager ? 1 : 0; // basically an if else statement on one, returns 1 or 0
                    }
                }
                Console.WriteLine($"{username} is not a staff member! ");
                return -1;
            }
        }
        public static void Status(int isManager)
        {
            cinema = new Cinema(CinemaTreeFilename, IsManager, FilmFilename, CinemaTreeFilename);
            // true means is manager and false employee
            if (isManager == 1)
            {
                ManagerMenu();
                return;
            }
            else if (isManager == 0)
            {
                EmployeeMenu();
                return;
            }
            else
            {
                OutputMenu();
                return;
            }
        }
        public static void ManagerMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--------- MANAGER MENU ---------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Which option would you like to do?");
            Console.WriteLine("1: Make a new booking");
            Console.WriteLine("2: View finance reports");
            Console.WriteLine("3: Change film schedule");
            Console.WriteLine("4: Add a new film");
            Console.WriteLine("5: View Screen");
            Console.WriteLine("6: View all available films");
            Console.WriteLine("7: Set film to a screen");
            Console.WriteLine("8: Query a seat is free");
            Console.WriteLine("9: Logout");
            string input = Console.ReadLine();
            bool valid = false;

            while (valid == false)
            {
                if (input == "1")
                {
                    Console.Clear();
                    cinema.MakeBooking();
                    return;
                }// make booking
                else if (input == "2")
                {
                    Console.Clear();
                    cinema.ViewFinancialReport();
                    return;
                }// view finance reports
                else if (input == "3")
                {
                    Console.Clear();
                    cinema.ChangeFilmSchedule();
                    return;
                }// change film schedule
                else if (input == "4")
                {
                    string FilmInput = " ";
                    Console.Clear();
                    Console.Write("Enter Film name: ");
                    FilmInput = Console.ReadLine();
                    while (string.IsNullOrEmpty(FilmInput))
                    {
                        Console.Write("Enter a film name: ");
                        FilmInput = Console.ReadLine();
                    }
                    cinema.AddFilm(FilmInput);
                    return;
                }// Add a new film
                else if (input == "5")
                {
                    Console.Clear();
                    bool ValScreenNum = false;
                    int ScreenNum = 0;
                    Console.Write("Screen No: ");
                    while (!ValScreenNum)
                    {
                        try
                        {
                            ScreenNum = Convert.ToInt32(Console.ReadLine()) - 1; // -1 to turn it into an index
                            if (ScreenNum > -1 && ScreenNum < 5)
                            {
                                ValScreenNum = true;
                            }
                            else
                            {
                                Console.Write("Enter a number 1-5 inclusive: ");
                            }
                        }
                        catch
                        {
                            Console.Write("Enter a number 1-5 inclusive: ");
                        }
                    }
                    cinema.DisplayScreen(ScreenNum);
                    return;
                }// View screen
                else if (input == "6")
                {
                    Console.Clear();
                    cinema.ViewFilmsAvailable();
                    return;
                }// View available screens
                else if (input == "7")
                {
                    Console.Clear();
                    bool ValResponse = false;
                    int ScreenNum = 0;
                    string FilmName = "";
                    int FilmDuration = 0;
                    Console.Write("Screen No: ");
                    while (!ValResponse)
                    {
                        try
                        {
                            ScreenNum = Convert.ToInt32(Console.ReadLine()) - 1; // -1 to turn it into an index
                            if (ScreenNum > -1 && ScreenNum < 5)
                            {
                                ValResponse = true;
                            }
                            else
                            {
                                Console.Write("Enter a number 1-5 inclusive: ");
                            }
                        }
                        catch
                        {
                            Console.Write("Enter a number 1-5 inclusive: ");
                        }
                    }
                    Console.Write("Film Name: ");
                    FilmName = Console.ReadLine();
                    while (string.IsNullOrEmpty(FilmName))
                    {
                        Console.Write("Film Name: ");
                        FilmName = Console.ReadLine();
                    }
                    Console.Write("Film Duration: ");
                    ValResponse = false;
                    while (!ValResponse)
                    {
                        try
                        {
                            FilmDuration = Convert.ToInt32(Console.ReadLine());
                            ValResponse = true;
                        }
                        catch
                        {
                            Console.Write("Enter a number: ");
                        }
                    }
                    cinema.SetScreenFilm(ScreenNum, FilmName, FilmDuration);
                    return;
                }// set film to a screen
                else if (input == "8")
                {
                    Console.Clear();
                    bool ValResponse = false;
                    int ScreenNum = 0;
                    string SeatType = "";
                    Console.Write("Screen No: ");
                    while (!ValResponse)
                    {
                        try
                        {
                            ScreenNum = Convert.ToInt32(Console.ReadLine()) - 1; // -1 to turn it into an index
                            if (ScreenNum > -1 && ScreenNum < 5)
                            {
                                ValResponse = true;
                            }
                            else
                            {
                                Console.Write("Enter a number 1-5 inclusive: ");
                            }
                        }
                        catch
                        {
                            Console.Write("Enter a number 1-5 inclusive: ");
                        }
                    }
                    Console.Write("VIP or Regular: ");
                    while (SeatType != "vip" && SeatType != "regular")
                    {
                        SeatType = Console.ReadLine().ToLower();
                        if (SeatType != "vip" && SeatType != "regular")
                        {
                            Console.Write("Enter VIP or Regular: ");
                        }
                    }
                    cinema.QuerySeatFree(ScreenNum, SeatType);
                    return;
                }// query a seat is free
                else if (input == "9")
                {
                    Console.Clear();
                    valid = true;
                    cinema.WriteToFile();
                    Console.WriteLine("Successfully logged out");
                    Environment.Exit(0);
                } // logout
                else
                {
                    Console.WriteLine("Invalid input - Please enter 1,2,3 or 4");
                    input = Console.ReadLine(); // Added to prevent infinite loop
                }
            }
        }

        public static void EmployeeMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--------- EMPLOYEE MENU ---------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Which option would you like to do?");
            Console.WriteLine("1: Make a new booking");
            Console.WriteLine("2: View all available films");
            Console.WriteLine("3: View a screen");
            Console.WriteLine("4: Query a seat is free");
            Console.WriteLine("5: Logout");
            string input = Console.ReadLine();
            bool valid = false;

            while (valid == false)
            {
                if (input == "1")
                {
                    Console.Clear();
                    cinema.MakeBooking();
                    return;
                }// make a new booking
                else if (input == "2")
                {
                    Console.Clear();
                    cinema.ViewFilmsAvailable();
                    return;
                }// view all available films
                else if (input == "3")
                {
                    Console.Clear();
                    bool ValScreenNum = false;
                    int ScreenNum = 0;
                    Console.Write("Screen No: ");
                    while (!ValScreenNum)
                    {
                        try
                        {
                            ScreenNum = Convert.ToInt32(Console.ReadLine()) - 1; // -1 to turn it into an index
                            if (ScreenNum > -1 && ScreenNum < 5)
                            {
                                ValScreenNum = true;
                            }
                            else
                            {
                                Console.Write("Enter a number 1-5 inclusive: ");
                            }
                        }
                        catch
                        {
                            Console.Write("Enter a number 1-5 inclusive: ");
                        }
                    }
                    cinema.DisplayScreen(ScreenNum);
                    return;
                }// view a screen
                else if (input == "4")
                {
                    Console.Clear();
                    bool ValResponse = false;
                    int ScreenNum = 0;
                    string SeatType = "";
                    Console.Write("Screen No: ");
                    while (!ValResponse)
                    {
                        try
                        {
                            ScreenNum = Convert.ToInt32(Console.ReadLine()) - 1; // -1 to turn it into an index
                            if (ScreenNum > -1 && ScreenNum < 5)
                            {
                                ValResponse = true;
                            }
                            else
                            {
                                Console.Write("Enter a number 1-5 inclusive: ");
                            }
                        }
                        catch
                        {
                            Console.Write("Enter a number 1-5 inclusive: ");
                        }
                    }
                    Console.Write("VIP or Regular: ");
                    while (SeatType != "vip" && SeatType != "regular")
                    {
                        SeatType = Console.ReadLine().ToLower();
                        if (SeatType != "vip" && SeatType != "regular")
                        {
                            Console.Write("Enter VIP or Regular: ");
                        }
                    }
                    cinema.QuerySeatFree(ScreenNum, SeatType);
                    return;
                }// query a seat is free
                else if (input == "5")
                {
                    Console.Clear();
                    valid = true;
                    cinema.WriteToFile();
                    Console.WriteLine("Successfully logged out");
                    Environment.Exit(0);
                    return;
                }// logout
                else
                {
                    Console.WriteLine("Invalid input - Please enter 1 or 2");
                    input = Console.ReadLine(); // Added to prevent infinite loop
                }
            }
        }
    }
    public static class SimpleHasher
    {
        private const uint FnvOffsetBasis = 2166136261;
        private const uint FnvPrime = 16777619;

        public static uint GetFnv1aHash(string input)
        {
            if (input == null) return 0;

            byte[] bytes = Encoding.UTF8.GetBytes(input);
            uint hash = FnvOffsetBasis;

            foreach (byte b in bytes)
            {
                hash ^= b;
                hash *= FnvPrime;
            }
            return hash;
        }
    }
    class Cinema
    {
        private string StaffFilename;
        private string FilmFilename;
        private string CinemaFilename;
        private Screen[] Screens;
        private List<string> FilmNames;
        private bool isManager;
        public Cinema(string StaffFilenameSet, bool isManagerSet, string FilmFilenameSet, string CinemaFileNameSet)
        {
            StaffFilename = StaffFilenameSet;
            FilmFilename = FilmFilenameSet;
            isManager = isManagerSet;
            CinemaFilename = CinemaFileNameSet;
            FilmNames = new List<string>();
            Screens = new Screen[]
            {
                new Screen(),
                new Screen(),
                new Screen(),
                new Screen(),
                new Screen()
            };
            ReadFile();
            using (StreamReader sr = new StreamReader(FilmFilename))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    FilmNames.Add(currentLine);
                }
            }
            SortFilmNames();
        }
        public double ViewFinancialReport() //manager only
        {
            double total = 0;
            if (isManager)
            {
                for (int i = 0; i < Screens.Length; i++)
                {
                    double profit = Screens[i].CalculateScreenProfit();
                    Console.WriteLine($"Screen {i + 1} Profit: £{profit}");
                    total += profit;
                }
                Console.WriteLine($"Total Profit from all screens is: £{total}");
            }
            else
            {
                Console.WriteLine("You do not have the access level to view the financial report. ");
            }
            return total;
        }
        public void SortFilmNames() // Using Bubble Sort
        {
            int numSwaps;
            for (int pass = 0; pass < FilmNames.Count() - 1; pass++)
            {
                numSwaps = 0;
                for (int i = 0; i < FilmNames.Count() - pass - 1; i++)
                {
                    if (string.Compare(FilmNames[i], FilmNames[i + 1]) > 0)
                    {
                        string temp = FilmNames[i];
                        FilmNames[i] = FilmNames[i + 1];
                        FilmNames[i + 1] = temp;
                        numSwaps++;
                    }
                }
                if (numSwaps == 0) { return; }
            }
        }
        public bool SearchForFilm(string FilmSought) // using Binary Search
        {
            int low = 0;
            int high = FilmNames.Count() - 1;
            int index = -1;
            bool found = false;
            FilmSought = FilmSought.ToLower();

            while (low <= high && !found)
            {
                int mid = (low + high) / 2;
                if (FilmNames[mid].ToLower() == FilmSought)
                {
                    index = mid;
                    return true;
                }
                else if (string.Compare(FilmSought, FilmNames[mid].ToLower()) < 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }
            return false;
        }
        public void AddFilm(string FilmName) //manager only
        {
            if (isManager)
            {
                string FilmNameTemp = FilmName.ToLower();
                if (!SearchForFilm(FilmNameTemp))
                {
                    FilmNames.Add(FilmName);
                    SortFilmNames();
                    using (StreamWriter sw = new StreamWriter(FilmFilename)) // using so file closes automatically
                    {
                        for (int i = 0; i < FilmNames.Count(); i++)
                        {
                            sw.WriteLine(FilmNames[i]);
                        }
                    }
                    Console.WriteLine($"{FilmName} Added");
                    return;
                }
                else
                {
                    Console.WriteLine($"{FilmName} is already in the list! ");
                }
            }
            else
            {
                Console.WriteLine("You do not have the access level to add a film! ");
                return;
            }
        }
        public void ReadFile()
        {
            using (StreamReader sr = new StreamReader(CinemaFilename))
            {
                for (int i = 0; i < Screens.Length; i++)
                {
                    Screens[i].FilmName = sr.ReadLine();
                    Screens[i].FilmDuration = Convert.ToInt32(sr.ReadLine());
                    for (int j = 0; j < Screens[i].seats.Length; j++)
                    {
                        Screens[i].seats[j].IsOccupied = Convert.ToBoolean(sr.ReadLine().ToLower());
                        Screens[i].seats[j].OccupiedBy = sr.ReadLine();
                        Screens[i].seats[j].SeatPrice = Convert.ToDouble(sr.ReadLine());
                    }
                }
            }
        }
        public void WriteToFile()
        {
            using (StreamWriter sw = new StreamWriter(CinemaFilename))
            {
                for (int i = 0; i < Screens.Length; i++)
                {
                    sw.WriteLine(Screens[i].FilmName);
                    sw.WriteLine(Screens[i].FilmDuration);
                    for (int j = 0; j < Screens[i].seats.Length; j++)
                    {
                        sw.WriteLine(Screens[i].seats[j].IsOccupied.ToString());
                        sw.WriteLine(Screens[i].seats[j].OccupiedBy);
                        sw.WriteLine(Screens[i].seats[j].SeatPrice);
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter(FilmFilename, false))
            {
                for (int i = 0; i < FilmNames.Count(); i++)
                {
                    sw.WriteLine(FilmNames[i]);
                }
            }
        }
        public void MakeBooking()
        {
            bool ValScreenNum = false;
            int ScreenNum = 0;
            string SeatType = "";
            string Name;
            string OAPOrStudent;
            double OAPStudentDiscount = 0.75;
            bool IsOAPOrStudent = false;
            DisplayFilmsOnScreens();
            Console.Write("Screen No: ");
            while (!ValScreenNum)
            {
                try
                {
                    ScreenNum = Convert.ToInt32(Console.ReadLine()) - 1; // -1 to turn it into an index
                    if (ScreenNum > -1 && ScreenNum < 5)
                    {
                        ValScreenNum = true;
                        Screens[ScreenNum].Display();
                    }
                    else
                    {
                        Console.Write("Enter a number 1-5 inclusive: ");
                    }
                }
                catch
                {
                    Console.Write("Enter a number 1-5 inclusive: ");
                }
            }
            Console.Write("VIP or Regular: ");
            while (SeatType != "vip" && SeatType != "regular")
            {
                SeatType = Console.ReadLine().ToLower();
                if (SeatType != "vip" && SeatType != "regular")
                {
                    Console.Write("Enter VIP or Regular: ");
                }
            }
            Console.Write("Name for seat: ");
            Name = Console.ReadLine();
            while (string.IsNullOrEmpty(Name))
            {
                Console.Write("Enter a name: ");
                Name = Console.ReadLine();
            }
            Console.Write("OAP or student (enter for neither): ");
            OAPOrStudent = Console.ReadLine().ToLower();
            while (OAPOrStudent != "oap" && OAPOrStudent != "student" && OAPOrStudent != "")
            {
                Console.Write("OAP or student (enter for neither): ");
                OAPOrStudent = Console.ReadLine().ToLower();
            }
            if (OAPOrStudent == "oap" || OAPOrStudent == "student")
            {
                IsOAPOrStudent = true;
            }
            for (int i = 0; i < Screens[ScreenNum].seats.Length; i++)
            {
                if (Screens[ScreenNum].seats[i].SeatType == SeatType && Screens[ScreenNum].seats[i].IsOccupied == false)
                {
                    Screens[ScreenNum].seats[i].IsOccupied = true;
                    Screens[ScreenNum].seats[i].OccupiedBy = Name;
                    if (IsOAPOrStudent)
                    {
                        Screens[ScreenNum].seats[i].SeatPrice = Screens[ScreenNum].seats[i].SeatPrice * OAPStudentDiscount;
                    }
                    Console.WriteLine($"{SeatType} Seat booked in screen {ScreenNum + 1} in seat {i} under the name {Name}. ");
                    return;
                }
            }
            Console.WriteLine("No available seats of that type in the selected screen.");
        }
        public void ViewFilmsAvailable()
        {
            Console.WriteLine("Films Available: ");
            for (int i = 0; i < FilmNames.Count(); i++)
            {
                Console.WriteLine($"{FilmNames[i]}");
            }
        }
        public void ChangeFilmSchedule() //manager only
        {
            int ScreenNum = 0;
            bool ValResponse = false;
            string FilmName = "";
            int FilmDuration = 0;
            if (isManager)
            {
                Console.Write("Screen No:");
                while (!ValResponse)
                {
                    try
                    {
                        ScreenNum = Convert.ToInt32(Console.ReadLine()) - 1; // -1 to turn it into an index
                        if (ScreenNum > -1 && ScreenNum < 5)
                        {
                            ValResponse = true;
                        }
                        else
                        {
                            Console.Write("Enter a number 1-5 inclusive: ");
                        }
                    }
                    catch
                    {
                        Console.Write("Enter a number 1-5 inclusive: ");
                    }
                }
                Console.Write("Film Name: ");
                ValResponse = false;
                while (!ValResponse)
                {
                    try
                    {
                        FilmName = Console.ReadLine().ToLower();
                        if (FilmName.Length != 0) //ensures no nulls are passed through
                        {
                            ValResponse = true;
                            Screens[ScreenNum].FilmName = FilmName;
                        }
                        else
                        {
                            Console.Write("Enter a film name: ");
                        }
                    }
                    catch
                    {
                        Console.Write("Enter a film name: ");
                    }
                }
                Console.Write("Film Duration: ");
                ValResponse = false;
                while (!ValResponse)
                {
                    try
                    {
                        FilmDuration = int.Parse(Console.ReadLine());
                        if (FilmDuration > 0)
                        {
                            Screens[ScreenNum].FilmDuration = FilmDuration;
                            ValResponse = true;
                        }
                        else
                        {
                            Console.Write("Enter a valid duration: ");
                        }
                    }
                    catch
                    {
                        Console.Write("Enter a valid duration: ");
                    }
                }
            }
            else
            {
                Console.WriteLine("You do not have the access level to change the film schedule! ");
                return;
            }
        }
        public void DisplayScreen(int ScreenNum)
        {
            Screens[ScreenNum].Display();
            Console.WriteLine($"Screen {ScreenNum + 1}: {Screens[ScreenNum].FilmName}");
        }
        public void QuerySeatFree(int ScreenNum, string SeatType) // querys if there are seats of SeatType in Screen[ScreenNum}
        {
            for (int i = 0; i < Screens[ScreenNum].seats.Length; i++)
            {
                if (Screens[ScreenNum].seats[i].SeatType == SeatType)
                {
                    if (!Screens[ScreenNum].seats[i].IsOccupied)
                    {
                        Console.WriteLine($"There is a {SeatType} seat in Screen {ScreenNum + 1}");
                        return;
                    }
                }
            }
            Console.WriteLine($"No {SeatType} seats in Screen {ScreenNum + 1}");
        }
        public void SetScreenFilm(int ScreenNum, string FilmName, int FilmDuration)
        {
            if (isManager)
            {
                if (!SearchForFilm(FilmName))
                {
                    AddFilm(FilmName);
                    Console.WriteLine($"{FilmName} was not in the list of names but has been added.");
                }
                Screens[ScreenNum].FilmName = FilmName;
                Screens[ScreenNum].FilmDuration = FilmDuration;
                Console.WriteLine($"Screen {ScreenNum + 1} is now showing {FilmName} lasting {FilmDuration} minutes. ");
                return;
            }
            else
            {
                Console.WriteLine("You do not have the access level to set a film to a screen");
            }
        } //manager only
        public void DisplayFilmsOnScreens()
        {
            Console.WriteLine("Films now showing");
            for (int i = 0; i < Screens.Length; i++)
            {
                Console.WriteLine($"Screen {i + 1}: {Screens[i].FilmName}");
            }
        }
    }
    class Screen
    {
        public string FilmName { get; set; }
        public int FilmDuration { get; set; }

        public Seats[] seats { get; set; } = new Seats[20];

        public Screen()
        {
            for (int i = 0; i < seats.Length; i++)
            {
                if (i < seats.Length - 5)
                {
                    seats[i] = new Seats("Regular");
                }
                else
                {
                    seats[i] = new Seats("VIP");
                }
            }
        }
        public void Display()
        {
            char[,] screenGrid = new char[4, 5];
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    int index = (row * 5) + col;
                    screenGrid[row, col] = seats[index].IsOccupied ? 'X' : '.';
                }
            }
            Console.Write("  ");
            for (int c = 0; c < 5; c++) Console.Write(c + " ");
            Console.WriteLine();

            for (int r = 0; r < 4; r++)
            {
                Console.Write(r + " ");
                for (int c = 0; c < 5; c++)
                {
                    Console.Write(screenGrid[r, c] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("---SCREEN---");
        }
        public int GetNumberSeatsOccupied()
        {
            int i = 0;
            for (int x = 0; x < seats.Length; x++)
            {
                if (seats[x].IsOccupied == true)
                {
                    i++;
                }
            }
            return i;
            // Calculates how many seats are occupied
        }
        public double CalculateScreenProfit()
        {
            double total = 0;
            for (int y = 0; y < seats.Length; y++)
            {
                if (seats[y].IsOccupied == true)
                {
                    total = total + seats[y].SeatPrice;
                }
            }
            return total;
            //Calculates the total profit by adding up the prices according tothe seats that are occupied and their type
        }
    }
    class Seats
    {
        public double SeatPrice { get; set; }
        public string SeatType { get; set; }
        public bool IsOccupied { get; set; }
        public string OccupiedBy { get; set; }
        public Seats(string SeatTypeSet)
        {
            IsOccupied = false;
            SeatType = SeatTypeSet.ToLower();
            if (SeatType.ToLower() == "regular")
            {
                SeatPrice = 9.50;
            }
            else if (SeatType.ToLower() == "vip")
            {
                SeatPrice = 17.00;
            }
        }
    }
}