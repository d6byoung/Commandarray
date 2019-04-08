using FinchAPI;
using System;

namespace CommandArray
{
    // *************************************************************
    // Title: Command Array
    // Description: program to tell the Finch to do things
    // Application Type: Console
    // Author: Young, David
    // Dated Created: 3 April 2019
    // Last Modified: 7 April 2019
    // *************************************************************

    public enum FinchCommand
    {
        DONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        DELAY,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF
    }

    class Program
    {
        static void Main(string[] args)
        {
            Finch myFinch = new Finch();

            DisplayOpeningScreen();
            DisplayInitializeFinch(myFinch);
            DisplayMainMenu(myFinch);
            DisplayClosingScreen(myFinch);
        }
        
        static void DisplayMainMenu(Finch myFinch)
        {
            string menuChoice;
            bool exiting = false;

            int delayDuration = 100;
            int numberOfCommands = 6;
            int motorSpeed = 155;
            int LEDBrightness = 155;
            FinchCommand[] commands = null;

            while (!exiting)
            {
                //
                // display menu
                //
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Main Menu");
                Console.WriteLine();

                Console.WriteLine("\t1) Enter Command Parameters");
                Console.WriteLine("\t2) Enter Finch Robot Commands");
                Console.WriteLine("\t3) Display Commands");
                Console.WriteLine("\t4) Execute Commands");
                Console.WriteLine("\tE) Exit");
                Console.WriteLine();
                Console.Write("Enter Choice:");
                menuChoice = Console.ReadLine();

                //
                // process menu
                //
                switch (menuChoice)
                {
                    case "1":
                        numberOfCommands = DisplayGetNumberOfCommands();
                        delayDuration = DisplayGetDelayDuration();
                        motorSpeed = DisplayGetMotorSpeed();
                        LEDBrightness = DisplayGetLEDBrightness();
                        break;
                    case "2":
                        commands = DisplayGetCommands(numberOfCommands);
                        break;
                    case "3":
                        if (commands == null)
                        {
                            Console.WriteLine("No Commands Entered!");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            DisplayCommands(commands);
                        }
                        break;
                    case "4":
                        if (commands == null)
                        {
                            Console.WriteLine("No Commands Entered!");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            ExecuteCommands(commands, myFinch, motorSpeed, delayDuration, LEDBrightness);
                        }
                        break;
                    case "e":
                    case "E":
                        exiting = true;
                        break;
                    default:
                        break;
                }
            }
        }

        static void ExecuteCommands(FinchCommand[] commands, Finch myFinch, int motorSpeed, int delayDuration, int LEDBrightness)
        {
            DisplayHeader("Execute Commands");

            Console.Write("The commands you entered are:");
            for (int i = 0; i < commands.Length; i++)
            {
                Console.Write($"{commands[i]} - ");
            }

            Console.WriteLine("Press any key to execute.");
            Console.ReadKey();

            DisplayHeader("Execute Commands");

            Console.WriteLine("Executing...");

            for (int i = 0; i < commands.Length; i++)
            {
                Console.WriteLine($"{commands[i]}... ");

                switch (commands[i])
                {
                    case FinchCommand.DONE:
                        break;
                    case FinchCommand.MOVEFORWARD:
                        myFinch.setMotors(motorSpeed, motorSpeed);
                        break;
                    case FinchCommand.MOVEBACKWARD:
                        myFinch.setMotors(-motorSpeed, -motorSpeed);
                        break;
                    case FinchCommand.STOPMOTORS:
                        myFinch.setMotors(0, 0);
                        break;
                    case FinchCommand.DELAY:
                        myFinch.wait(delayDuration);
                        break;
                    case FinchCommand.TURNRIGHT:
                        myFinch.setMotors(motorSpeed, -motorSpeed);
                        break;
                    case FinchCommand.TURNLEFT:
                        myFinch.setMotors(-motorSpeed, motorSpeed);
                        break;
                    case FinchCommand.LEDON:
                        myFinch.setLED(LEDBrightness, LEDBrightness, LEDBrightness);
                        break;
                    case FinchCommand.LEDOFF:
                        myFinch.setLED(0, 0, 0);
                        break;
                    default:
                        break;
                }
            }

            DisplayContinuePrompt();
        }

        static void DisplayCommands(FinchCommand[] commands)
        {
            DisplayHeader("Display Commands");

            Console.Write("The commands you entered are:");
            for (int i = 0; i < commands.Length; i++)
            {
                Console.Write($"{commands[i]} - ");
            }

            DisplayContinuePrompt();
        }

        static FinchCommand[] DisplayGetCommands(int numberOfCommands)
        {
            FinchCommand[] commands = new FinchCommand[numberOfCommands];

            DisplayHeader("Enter Finch Commands");

            Console.WriteLine("DONE, MOVEFORWARD, MOVEBACKWARD, STOPMOTORS, DELAY, TURNRIGHT, TURNLEFT, LEDON, LEDOFF");

            for (int i = 0; i < numberOfCommands; i++)
            {
                Console.Write($"Command number {i + 1}:");
                Enum.TryParse(Console.ReadLine().ToUpper(), out commands[i]);
            }

            Console.WriteLine();
            Console.Write("The commands you entered are:");
            for (int i = 0; i < numberOfCommands; i++)
            {
                Console.Write($"{commands[i]} - ");
            }

            DisplayContinuePrompt();

            return commands;
        }

        static int DisplayGetLEDBrightness()
        {
            int brightness;
            string userResponse;

            DisplayHeader("LED Brightness");

            Console.Write("Enter LED Brightness:");
            userResponse = Console.ReadLine();

            brightness = int.Parse(userResponse);

            return brightness;
        }

        static int DisplayGetMotorSpeed()
        {
            int speed;
            string userResponse;

            DisplayHeader("Motor Speed");

            Console.Write("Enter Motor Speed (1-255):");
            userResponse = Console.ReadLine();

            speed = int.Parse(userResponse);

            return speed;
        }

        static int DisplayGetDelayDuration()
        {
            int delayDuration;
            string userResponse;

            DisplayHeader("Length of Delay");

            Console.WriteLine("Enter Length of Delay (milliseconds)");
            userResponse = Console.ReadLine();

            delayDuration = int.Parse(userResponse);

            return delayDuration;
        }
        
        static int DisplayGetNumberOfCommands()
        {
            int numberOfCommands;
            string userResponse;

            DisplayHeader("Number of Commands");

            Console.Write("Enter the number of commands:");
            userResponse = Console.ReadLine();

            numberOfCommands = int.Parse(userResponse);

            return numberOfCommands;
        }
        
        static void DisplayInitializeFinch(Finch myFinch)
        {
            DisplayHeader("Initialize the Finch");

            Console.WriteLine("Please plug your Finch Robot into the computer.");
            Console.WriteLine();
            DisplayContinuePrompt();

            while (!myFinch.connect())
            {
                Console.WriteLine("Please confirm the Finch Robot is connect");
                DisplayContinuePrompt();
            }

            FinchConnectedAlert(myFinch);
            Console.WriteLine("Your Finch Robot is now connected");

            DisplayContinuePrompt();
        }
        
        static void FinchConnectedAlert(Finch myFinch)
        {
            myFinch.setLED(0, 255, 0);

            for (int frequency = 17000; frequency > 100; frequency -= 100)
            {
                myFinch.noteOn(frequency);
                myFinch.wait(10);
            }

            myFinch.noteOff();
        }
        
        static void DisplayOpeningScreen()
        {
            Console.WriteLine();
            Console.WriteLine("\tProgram Your Finch");
            Console.WriteLine();

            DisplayContinuePrompt();
        }
        
        static void DisplayClosingScreen(Finch myFinch)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\tThank You!");
            Console.WriteLine();

            myFinch.disConnect();

            DisplayContinuePrompt();
        }
        
        static void DisplayHeader(string header)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + header);
            Console.WriteLine();
        }
        
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
        
    }
}
