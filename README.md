# Cinema-Project
A simple cinema project that can book seats and uses a hashing algorithm to encrypt passwords (see all methods below)
Add the text files to the bin -> debug folder created by visual studio

MainMenu Class
Runs subroutine OutputMenu() which displays Cinema management system and asks for a username and password. These credentials are passed through a hashing algorithm which returns an integer value but are also saved to a txt file. These variables are then used to determine whether the user is an employee or a manager and outputs a menu with options that they can choose to do with  managers having more options available than employees.

static void Main()
Runs subroutine OutputMenu and then returns the result from the hashing algorithm. Using a while IsRunning loop, the program uses the result from the hashing algorithm to determine whether the user is an employee or manager and directs them to the employee menu or manager menu.

public static void OutputMenu()
Displays —CINEMA MANAGEMENT SYSTEM— and then asks for two variables, username and password which are stored as a string. Then runs CredentialChecker() then calls Status() with the parameter the output of CredentialChecker()

public static int CredentialChecker(string username, string password)
Takes parameters and hashes them by calling SimpleHasher.GetFnv1aHash() on both and comparing them by using a StreamReader stream. If a username and password matches it reads the next line and if that line is “True” then it returns 1, if it is “False” then it returns 0. If there is no match then it returns -1.

public static void Status(int isManager)
Works out whether the current operator is an employee or manager using a bool isManager with it being 1 meaning their a manager or 0 making them an employee, if isManager is -1 then it calls OutputMenu()

public static void ManagerMenu()
Outputs the options which a manager can choose to do; Make new booking, view finance reports, change film schedule, add new film, view a screen, view all available films, set film to a screen, query if a seat is free or log out. Whatever option is chosen will direct them to a subroutine in Cinema Class.

public static void EmployeeMenu()
Outputs their options of Make new booking, view all available films, view a screen, view a seat is free or logout and if they choose to make new booking or view all available films  it directs them to the corresponding subroutines in Cinema Class



Cinema Class
This has the screens array which have seats inside of them. This is where the user can manage the cinema. This accounts for if the user is a manager or not by restricting access levels to certain methods such as public void ChangeFilmSchedule(). It has attributes StaffFilename, FilmFilename, CinemaFilename, Screens, FilmNames and isManager. The main class calls methods in this class which are saved when the file is closed.

public Cinema(string StaffFilenameSet, bool isManagerSet, string FilmFilenameSet, string CinemaFileNameSet)
Creates 5 screen classes and sets if the user is a manager and stores the Cinema file name, staffFilename and the film filename. It then populates the array Screens with five screen classes. It then reads the file so it can construct the tree and populate the cinema. Then it adds all the films to the filmNameList by looping through each line.

public void MakeBooking()
The user is prompted what screen to make the booking, if they want a VIP or regular and a name for the booking. Try-catch inside a while loop is used to make the program more robust. It also calls DisplayFilmsOnScreens() which tells the user what films are showing on what screens so they can tell the buyer which movie they are buying the ticket to. It also asks if they are an OAP or a student and applies a 25% discount on the price.

public void ChangeFilmSchedule() 
If the user is a manager then it prompts them which screen and the film name and it edits the attributes directly. Try-catch inside a while loop is used to make the program more robust.

public void AddFilm(string FilmName)
If the user is a manager then this calls SearchForFilm() and sees if FilmName is in the file or not, if it is not then a using block is used and opens the Film file and writes Film name to that then closes the file. If it is in the file then it displays an appropriate message.

public double ViewFinancialReports()
Uses a for loop to iterate through each screen and calls CalculateScreenProfit() to calculate the total from all screens. It also outputs the profit of each screen to the console window. It also outputs the total profit from all screens. 

public void SortFilmNames()
Uses bubble sort to sort the films by name. This is done whenever a new film is added to the films list so it is always sorted before it is stored to the file. It uses a flag numSwaps which keeps track of the number of swaps. If it is 0 by the end of the first pass then that means the list is sorted so it returns. string.Compare is used so that when we compare the two film names it returns a number - 0 if they match, -1 if the left is smaller than the right and 1 if the left is larger than the right.

public void ReadFile()
Uses streamwriter to read the tree from the file by using two for loops to iterate through each seat in each screen to read whether they are occupied or not and who by. Notice this does not take a parameter as this is designed to only read the cinema file and reconstruct the cinema. It reads the film name and the duration which meets the design brief that the cinema must be reconstructed like it was before. A using block is used because just in case there is an exception thrown (like if the file does not exist) it will still close the file stream.

public void WriteToFile(string filename) (Cinema Class)
Uses streamwriter to write the tree to the file by using two for loops to iterate through each seat in each screen to write whether they are occupied or not and who by. It also clears the film file then writes the film list in it. It writes the film name and the duration which meets the design brief that the cinema must be reconstructed like it was before. A using block is used because just in case there is an exception thrown (like if the file does not exist) it will still close the file stream.

public bool SearchForFilm(string FilmSought)
This uses a binary search to search if a film is in the film list. This is called by AddFilm() because it shouldn’t add a film that is already in the list. It returns a boolean value which is used and compared so we can identify if FilmSought is in the list. string.Compare() is used and if it is less then 0, meaning FilmSought is lower then FilmNames[mid].

public void ViewFilmsAvailable()
This just prints out all the films in the films list by iterating through the whole list.

public void DisplayScreen(int ScreenNum)
This displays the screen in index screenNum in the array. Try catch is used so that it will always receive a value between 0 and 4. It also prints the film showing on that screen.

public void QuerySeatFree(int ScreenNum, string SeatType)
This just checks if there is a seattype of SeatType in Screen ScreenNum. if there is it will print there is a seat with this type in this screen.

public void SetScreenFilm(int ScreenNum, string FilmName, int FilmDuration)
If the user is a manager then it will change the screen ScreenNum and change it to show FilmName lasting for FilmDuration minutes. If the film name is not in the list then it will amend that and display that the film was not in the list but has now been added.

public void DisplayFilmsOnScreens()
Iterates through all the screens and shows what movie each screen is showing.

Screen Class
This is where everything about each screen can be accessed : Each seat is given a type where 5 seats are set as VIP and the rest as Regular. It shows how many seats are occupied and also according to this and the seat’s type the total profit made by the screen is calculated. It has attributes FilmName and FilmDuration


public Screen()
Creates 20 seats objects as an array where they are all empty and unoccupied and creates a film object and this has the seats array.
Using a count - controlled loop, the first 5 seats are set to vip and the rest are set to regular. With i as the variable for the index. 

public int GetNumberSeatsOccupied()
Returns number of seats that are occupied
With x as the variable for the index and i as the variable for the counter. Using a for loop, if the seat is occupied the counter goes up by 1 and finally it is returned outside the loop.

public void Display()
Initialises a character 2D array ScreenGrid and loops through each seat and if the seat is occupied, it stores a X, if it is not then it stores a dot to indicate it is free. It then prints 0-4 and prints each seat in the grid.

public double CalculateScreenProfit()
Calculates the profit by adding the price of each ticket to the total (will need to find the number of VIP seats and regular seats occupied).
With total as the variable(double) for the total profit and y as the variable for the index. Using a for loop. If the seat is occupied the price of that seat will be added to the total and finally outside the for loop total is returned.

public Seats(string SeatTypeSet) (Seats Class)
Sets seats occupied to false
They are then given a price according to their type. It has attributes SeatPrice, SeatType, IsOccupied and OccupiedBy.
The variable IsOccupied is set to false. An  if statement is used where the variable SeatType is used to calculate the price.  If SeatType is = regular then the variable SeatPrice is set to 9.50 otherwise is SeatType is = vip then the variable SeatPrice is set to 17.00


Public static class SimpleHasher (class) 
This class holds constants and methods for hashing, two constants that are initialized at the top. private const uint FnvOffsetBasis - starting value so that every hash does not result in 0, private const uint FnvPrime - prime number used to scramble bits so that small changes = very different output numbers.

public static uint GetFnv1aHash(string input) (method)
When username and password are entered in OutputMenu() this method is called in CredentialChecker() using username and password to hash both username and password and stored in a new variable.

This is the hashing method, due to being static a new instance cannot be created you just need to call the method to use it, its visibility is public to be used outside of the SimpleHasher class. In our program it is called by using SimpleHasher.GetFnv1aHash(password); string password is our string input and in our program we need to hash any passwords entered in OutputMenu(). The data type uint is used to prevent negative numbers

if (input == null) return 0; is a safety check to see if anything has been entered

[how it works]
Computers do not hash letters so the string needs to be converted to numbers, in this method byte[] bytes = Encoding.UTF8.GetBytes(input); turns the string into an array of bytes. These bytes are then moved around by using XOR hash ^= b; and multiplying hash *= FnvPrime; this ensures we get a completely unique number hashed every time, these two operations are managed by a foreach loop the condition is (byte b in bytes)
