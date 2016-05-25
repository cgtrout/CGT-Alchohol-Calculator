/*
 * Created by SharpDevelop.
 * User: Admin Security
 * Date: 10/10/15
 * Time: 8:28 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using BloodAlcoholCalculator.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodAlcoholCalculator.Console
{
     /// <summary>
     /// Console System - allows user to type in commands to be run
     /// </summary>
     public class ConsoleSystem : ViewModelBase
     {
          #region Fields

          private static ConsoleSystem _instance;

          private StringBuilder _history = new StringBuilder();

          #endregion Fields

          #region Constructors

          public ConsoleSystem()
          {
               Commands = new SortedDictionary<string, ConsoleCommand>(StringComparer.InvariantCultureIgnoreCase);
               History = new StringBuilder();

               History.AppendLine($"BloodAlcoholCalculator Version: {Version.Default.VersionString}");
               History.AppendLine();
               History.AppendLine("Type help to see a list of commands.\n");

               AddCommand(new ConsoleCommand("Help", "This command", () => {
                    foreach (var command in Commands) {
                         History.AppendLine(String.Format("{0, -20} | {1, -50}", command.Key, command.Value.Description));
                         //History.AppendLine();
                    }
               }));
          }

          #endregion Constructors

          #region Properties

          /// <summary>
          /// Get global static instance for the console system
          /// </summary>
          public static ConsoleSystem ConsoleSystemInstance
          {
               get {
                    if (_instance == null) {
                         _instance = new ConsoleSystem();
                    }
                    return _instance;
               }
          }

          /// <summary>
          /// Used to store history of commands entered
          /// </summary>
          public StringBuilder History
          {
               get { return _history; }
               set {
                    _history = value;
                    OnPropertyChanged("History");
               }
          }

          /// <summary>
          /// Dictionary of commands
          /// </summary>
          public SortedDictionary<string, ConsoleCommand> Commands { get; set; }

          #endregion Properties

          #region Methods

          /// <summary>
          /// Add a new command to the system
          /// </summary>
          /// <param name="command">Command to add</param>
          public void AddCommand(ConsoleCommand command)
          {
               if (Commands.ContainsKey(command.Name) == false) {
                    Commands.Add(command.Name, command);
               }
          }

          /// <summary>
          /// Executes a command by name
          /// </summary>
          /// <param name="name">name of command to run</param>
          public void ExecuteCommand(string name)
          {
               if (String.IsNullOrEmpty(name)) {
                    throw new ConsoleSystemException("Nothing entered.");
               }

               //parse string
               var splitString = name.Split(' ');

               //first is command
               var command = GetCommand(splitString[0]);
               if (command == null) {
                    throw new ConsoleSystemException("Command does not exist.");
               }

               if (command.Method != null) {
                    command.Method();
               } else if (command.MethodWithParam != null) {
                    if (splitString.Length > 1) {
                         command.MethodWithParam(splitString[1]);
                    }
               }

               WriteLine($"Executed command: {command.Name}");
               History.AppendLine();
          }

          /// <summary>
          /// Get a command by name
          /// </summary>
          /// <param name="name">string of name to obtain</param>
          /// <returns>Console command specified</returns>
          public ConsoleCommand GetCommand(string name)
          {
               if (!Commands.ContainsKey(name)) {
                    return null;
               }
               var command = Commands[name];
               return command;
          }

          /// <summary>
          /// Write a line to history
          /// </summary>
          /// <param name="message"></param>
          /// <param name="args"></param>
          public void WriteLine(string message, Object[] args)
          {
               History.AppendFormat(message, args);
               OnPropertyChanged("History");
          }

          public void WriteLine(string message)
          {
               History.AppendLine(message);
               OnPropertyChanged("History");
          }

          #endregion Methods
     }

     /// <summary>
     /// Exception used by ConsoleSystem
     /// </summary>
     public class ConsoleSystemException : Exception
     {
          #region Constructors

          public ConsoleSystemException(string message) : base(message)
          {
          }

          #endregion Constructors
     }
}