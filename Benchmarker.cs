﻿/*
 * Created by SharpDevelop.
 * User: Admin Security
 * Date: 06/13/2015
 * Time: 00:15
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BloodAlcoholCalculator
{
     /// <summary>
     /// Description of Benchmarker.
     /// </summary>
     public static class Benchmarker
     {
          #region Constructors

          static Benchmarker()
          {
               BenchMarks = new ConcurrentDictionary<string, BenchMark>();
          }

          #endregion Constructors

          #region Properties

          private static ConcurrentDictionary<string, BenchMark> BenchMarks
          {
               get;
               set;
          }

          #endregion Properties

          #region Methods

          #region Methods

          /// <summary>
          /// Get output string
          /// </summary>
          /// <returns></returns>
          public static string GetString()
          {
               StringWriter sw = new StringWriter();
               var query = from x in BenchMarks.Values
                           orderby x.TotalTime descending
                           select x;
               foreach (var bm in query) {
                    sw.WriteLine(bm.ToString());
               }
               sw.WriteLine($"{Process.GetCurrentProcess().PrivateMemorySize64 * 0.000001} mb being used.");

               return sw.ToString();
          }

          /// <summary>
          /// Start
          ///   - start a benchmark
          /// </summary>
          /// <param name="name">name string of benchmark to start</param>
          [Conditional("BENCHMARK")]
          public static void Start(string name)
          {
               if (BenchMarks.ContainsKey(name) == false) {
                    BenchMarks[name] = new BenchMark(name);
               }
               BenchMarks[name].Start();
          }

          /// <summary>
          /// Stop a benchmark
          /// </summary>
          /// <param name="name">name string of benchmark to stop</param>
          [Conditional("BENCHMARK")]
          public static void Stop(string name)
          {
               BenchMarks[name].Stop();
          }

          #endregion Methods

          #endregion Methods
     }

     public class BenchMark
     {
          #region Fields

          private static int CollectionMax = 1000;
          private Stopwatch sw = new Stopwatch();

          //use a queue so we can cap how big benchmark times get
          private Queue<TimeSpan> times = new Queue<TimeSpan>(CollectionMax);

          #endregion Fields

          #region Constructors

          public BenchMark(string name)
          {
               Name = name;
          }

          #endregion Constructors

          #region Properties

          public TimeSpan AverageTime
          {
               get {
                    if (times.Count == 0) {
                         return new TimeSpan(0);
                    }
                    long totalTime = 0;
                    foreach (var t in times.ToArray()) {
                         totalTime += t.Ticks;
                    }
                    return new TimeSpan(totalTime / times.Count);
               }
          }

          public String Name { get; set; }

          public TimeSpan TotalTime
          {
               get {
                    TimeSpan[] array = null;
                    lock (times) {
                         array = times.ToArray();
                    }
                    return new TimeSpan(array.Sum(x => x.Ticks));
               }
          }

          #endregion Properties

          public void Start()
          {
               sw.Reset();
               sw.Start();
          }

          public void Stop()
          {
               sw.Stop();

               lock (times) {
                    CapCollectionSize();
                    times.Enqueue(sw.Elapsed);
               }
          }

          public override string ToString() => $"Avg={AverageTime.TotalSeconds}sec Ttl={TotalTime.TotalSeconds}sec {Name}:";

          private void CapCollectionSize()
          {
               lock (times) {
                    if (times.Count > CollectionMax) {
                         times.Dequeue();
                    }
               }
          }
     }
}