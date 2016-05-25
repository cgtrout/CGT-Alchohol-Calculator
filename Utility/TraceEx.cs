/*
 * Created by SharpDevelop.
 * User: Admin Security
 * Date: 7/24/15
 * Time: 12:06 AM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Diagnostics;

namespace BloodAlcoholCalculator.Utility
{
     /// <summary>
     /// Description of TraceEx.
     /// </summary>
     public static class TraceEx
     {
          #region Methods

          public static void PrintLog(string message)
          {
               var now = DateTime.Now;
               Trace.WriteLine(now + " " + message);
          }

          #endregion Methods
     }
}