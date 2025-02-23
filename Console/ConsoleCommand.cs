﻿/*
 * Created by SharpDevelop.
 * User: Admin Security
 * Date: 10/10/15
 * Time: 8:28 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace BloodAlcoholCalculator.Console
{
     /// <summary>
     /// Represents one console command
     /// </summary>
     public class ConsoleCommand
     {
          #region Constructors

          public ConsoleCommand(string name, string description, Action method)
          {
               Name = name;
               Description = description;
               Method = method;
          }

          public ConsoleCommand(string name, string description, Action<object> method)
          {
               Name = name;
               Description = description;
               MethodWithParam = method;
          }

          #endregion Constructors

          #region Properties

          /// <summary>
          /// Description of this command
          /// </summary>
          public string Description { get; set; }

          /// <summary>
          /// Method to run when this command is executed
          /// </summary>
          public Action Method { get; set; } = null;

          public Action<object> MethodWithParam { get; set; } = null;

          /// <summary>
          /// Name of this command
          /// </summary>
          public string Name { get; set; }

          #endregion Properties
     }
}