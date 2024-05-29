using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

                                //* Program Name:ABET Part 3:Console
                                /* Date: 04/04/2022
                                /* Programmer:Sagar Ghorsai
                                /* Class:CSCI 234
                                /* Program Description: Tasked with finishing the rest of the console program for the project. 
                                The end result must have the following function: New Record, to add new records to the database. Open Record
                                : To open the records in the database. Display All records: To Display all records with Status D. Undelete: Setting the
                                status back to N. Purge: To delete any records with D in status. Also The following in the option menu. Modify: To change the
                                values in the records. Delete:Setting the status to D(logical delete) and Back to menu.
                                /* Input:Records,choices
                                /* Output:Data from records
                                /* Givens:None
                                /* Testing Data: 
                                Players Maintenance                 Monday, April 4, 2022


                                Enter value for  ID: 7
                                Enter value for FirstName: Shazzi
                                Enter value for LastName: Mitchell
                                Enter value for Email: Shazzi9809@gmail.com
                                Enter value for GameType: 4

                                1.        ID: 7
                                2. FirstName: Shazzi
                                3.  LastName: Mitchell
                                4.     Email: Shazzi9809@gmail.com
                                5.  GameType: 4

                                1 = Modify record    2 = Delete record   3 = Return to main menu

                                Enter you choice:




                                */




class Program
{
    static Database database;
    static string[] Menuitems = { "New Record", "Open Record",
        "Display All Records","Undelete","Purge", "Exit" };
    static string[] Record;
    static string[][] Players;
    static string[][] Games;
    static int LongestFieldLabel;
    static string PlayerID;

    static void heading()           //Heading 
    {
        Console.Clear();
        Console.Write("Players Maintenance                 ");
        Console.WriteLine(DateTime.Now.ToLongDateString());
        Console.WriteLine();
    }
    static void DisplayMenu()       //Menu display
    {
        int i;

        for (i = 0; i < Menuitems.Length; i++)
            Console.WriteLine($"{i + 1}. {Menuitems[i]}\n");
    }
    static void GetSupportData()  //support table
    {
        try
        {
            Games = database.GetGames();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return to main menu: ");
            Console.ReadLine();
        }
    }
    static void DisplayRecord()  //Display the records
    {
        int i;

        for (i = 0; i < Database.FieldHeads.Length; i++)
        {
            Console.Write("{0}. {1," + LongestFieldLabel + "}: ", i + 1, Database.FieldHeads[i]);
            Console.Write(Record[i]);
            if (i == 5)
            {
                Console.Write("   (");
                Console.Write((Record[i], Games, 1));
                Console.Write(")");
            }
            Console.WriteLine();
        }
    }
    static void OpenRecord()        //opens record of the selected player
    {
        heading();

        Console.WriteLine();

        Console.Write("Enter a Players #: ");
        PlayerID = Console.ReadLine();

        try
        {
            Record = database.GetRecord(PlayerID);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return to main menu: ");
            Console.ReadLine();
        }

        Console.WriteLine();
        if (Record[0] == "no record found")
        {
            Console.WriteLine("Record for Player " + PlayerID + " not found");
            Console.WriteLine("Press any key to return to main menu: ");
            Console.ReadLine();
        }
        else
        {
            DisplayRecord();
            RecordOptions();
        }
    }
    static void SaveRecord()
    {
        try
        {
            database.SaveRecord(Record);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return to main menu: ");
            Console.ReadLine();
        }
    }
    static void DeleteRecord() //logicaldelete
    {
        try
        {
            database.LogicalDeleteRecord(PlayerID);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return to main menu: ");
            Console.ReadLine();
        }
    }
    static void NewRecord()
    {
        int i;
        string value;
        bool OK;
        int number;

        heading();

        Record = new string[Database.FieldHeads.Length + 3];

        Console.WriteLine();

        for (i = 0; i < Database.FieldHeads.Length; i++)
        {
            Console.Write($"Enter value for {Database.FieldHeads[i]}: ");
            value = Console.ReadLine();
            if (i == 0)
            {
                value = value.PadLeft(1);
                if (database.RecordExists(value))
                {
                    Console.WriteLine("Player #" + value + " already exists");
                    Console.WriteLine("Press any key to return to main menu: ");
                    Console.ReadLine();
                    return;

                }
            }

            if (i == Database.FieldHeads.Length)
            {
                OK = false;
                do
                {
                    OK = int.TryParse(value, out number);
                    if (!OK)
                    {
                        Console.Write($"Enter value for {Database.FieldHeads[i]} (Must be an integer): ");
                        value = Console.ReadLine();
                    }
                }
                while (!OK);
                value = $"{number:d2}";
            }
            Record[i] = value;
        }

        //Was kinda tricky
        Record[5] = "N";
        Record[6] = DateTime.Now.ToString("MM/dd/yyyy");
        PlayerID = Record[0];

        Console.WriteLine();


        SaveRecord();
        DisplayRecord();
        RecordOptions();
    }
    static void RecordOptions()
    {
        int choice;
        string value;
        bool OK;
        int number;

        Console.WriteLine();
        Console.WriteLine("1 = Modify record    2 = Delete record   3 = Return to main menu");
        Console.WriteLine();
        Console.Write("Enter you choice: ");
        value = Console.ReadLine();
        OK = false;
        do
        {
            OK = int.TryParse(value, out number);
            if (!OK)
            {
                Console.Write("Enter you choice (Must be an integer): ");
                value = Console.ReadLine();
            }
        }
        while (!OK);
        choice = number;

        if (choice == 1)
        {
            Console.WriteLine();
            Console.Write("Which field do you want to modify? ");
            value = Console.ReadLine();
            OK = false;
            do
            {
                OK = int.TryParse(value, out number);
                if (!OK)
                {
                    Console.Write("Which field do you want to modify? (Must be an integer)");
                    value = Console.ReadLine();
                }
            }
            while (!OK);
            choice = number;
            if (choice >= 1 && choice <= Database.FieldHeads.Length)
            {
                Console.Write($"Enter new value for field {choice}: ");
                value = Console.ReadLine();
                if (choice == 6)
                {
                    OK = false;
                    do
                    {
                        OK = int.TryParse(value, out number);
                        if (!OK)
                        {
                            Console.Write($"Enter new value for field {choice} (Must be an integer): ");
                            value = Console.ReadLine();
                        }
                    }
                    while (!OK);
                    value = $"{number:d2}";
                }
                Record[choice - 1] = value;
                SaveRecord();
                DisplayRecord();
            }
        }
        else if (choice == 2)
        {
            DeleteRecord();
        }

    }
    private static void UndeleteRecord()
    {
        try
        {
            heading();
            Console.WriteLine();
            Console.Write("Enter a Player # to undelete: ");
            PlayerID = Console.ReadLine();
            database.UnDeleteRecord(PlayerID);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return to main menu: ");
            Console.ReadLine();
        }
    }
    static void DisplayAllRecords()
    {
        int i, k;

        heading();

        Console.WriteLine();

        for (i = 0; i < Database.FieldHeads.Length; i++)
            Console.Write("{0,-" + database.FieldLens[i] + "} ", Database.FieldHeads[i]);
        Console.WriteLine();

        for (i = 0; i < Database.FieldHeads.Length; i++)
            Console.Write($"{new string('=', database.FieldLens[i])} ");
        Console.WriteLine();

        try
        {
            Players = database.GetAllPlayers();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return to main menu: ");
            Console.ReadLine();
        }

        for (i = 0; i < Players.Length; i++)
        {

            for (k = 0; k < Players[i].Length; k++)
                Console.Write("{0,-" + database.FieldLens[k] + "} ", Players[i][k]);
            Console.WriteLine();
        }
        Console.WriteLine();

        Console.WriteLine("Press any key to return to main menu: ");
        Console.ReadLine();
    }
    private static void Purge()
    {

        try
        {
            heading();
            Console.WriteLine();
            Console.Write("Are you sure you want to permanently purge all deleted records? (Y/N) ");
            if (Console.ReadLine().ToUpper()[0] != 'Y')
                return;
            database.Purge();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return to main menu: ");
            Console.ReadLine();
        }


    }


    private static void Main(string[] args)
    {
        int choice;
        bool OK;
        int number;
        string value;
        try
        {
            database = new Database();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
        LongestFieldLabel = Database.GetLongestFieldLabel();
        GetSupportData();

        do
        {
            heading();
            DisplayMenu();

            Console.Write("Enter your choice: ");
            value = Console.ReadLine();
            OK = false;
            do
            {
                OK = int.TryParse(value, out number);
                if (!OK)
                {
                    Console.WriteLine("Enter your choice (Must be an integer): ");
                    value = Console.ReadLine();
                }
            }
            while (!OK);
            choice = number;

            switch (choice)
            {
                case 1:
                    NewRecord();
                    break;
                case 2:
                    OpenRecord();
                    break;
                case 3:
                    DisplayAllRecords();
                    break;
                case 4:
                    UndeleteRecord();
                    break;
                case 5:
                    Purge();
                    break;
            }
        }
        while (choice != Menuitems.Length);

    }
}

