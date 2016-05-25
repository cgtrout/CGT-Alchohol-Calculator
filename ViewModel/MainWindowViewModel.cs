using BloodAlcoholCalculator.Console;
using BloodAlcoholCalculator.Utility;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

//using BloodAlcoholCalculator.

namespace BloodAlcoholCalculator.ViewModel
{
     /// <summary>
     /// The ViewModel for the application's main window.
     /// </summary>
     public class MainWindowViewModel : WorkspaceViewModel
     {
          //panel on left will show a list of reports
          private ObservableCollection<WorkspaceViewModel> _workspaces;

          private ObservableCollection<WorkspaceViewModel> _anchorables;

          public static MainWindowViewModel MainWindowInstance
          {
               get {
                    if (_mainWindowInstance == null) {
                         _mainWindowInstance = new MainWindowViewModel();
                    }

                    return _mainWindowInstance;
               }
          }

          private static MainWindowViewModel _mainWindowInstance;

          public MainWindowViewModel()
          {
               base.DisplayName = "BA Calculator";

               SetupConsoleCommands();
               benchmarkTimer.Tick += BenchmarkTimer_Tick;
               benchmarkTimer.Interval = new TimeSpan(0, 20, 0);
               benchmarkTimer.Start();
          }

          //timer that will print benchmarker stats every 20 minutes to see how the timing changes over the day
          private DispatcherTimer benchmarkTimer = new DispatcherTimer();

          private void BenchmarkTimer_Tick(object sender, EventArgs e)
          {
               TraceEx.PrintLog($"Printing Benchmark Results:");
               Trace.Write(Benchmarker.GetString());
          }

          ConsoleSystem console => ConsoleSystem.ConsoleSystemInstance;

          private void SetupConsoleCommands()
          {
               console.AddCommand(new ConsoleCommand("GC", "Force Garbage Collect", GarbageCollect));
               console.AddCommand(new ConsoleCommand("mem", "show memory statics", MemoryStats));
#if BENCHMARK
               console.AddCommand(new ConsoleCommand("PrintBenchMarks", "Get benchmark results", printBenchmarks));
#endif
          }

          private void MemoryStats()
          {
               console.WriteLine($"{Process.GetCurrentProcess().PrivateMemorySize64 * 0.000001} mb being used.");
          }

          private void GarbageCollect()
          {
               GC.Collect();
               console.WriteLine("Garbage collect forced.");
          }

          private void printBenchmarks()
          {
               var console = ConsoleSystem.ConsoleSystemInstance;
               console.WriteLine(Benchmarker.GetString());
          }

          public string StatusText
          {
               get {
                    return _statusText;
               }
               private set {
                    _statusText = value;
                    OnPropertyChanged("StatusText");
               }
          }

          private string _statusText;

          public async void PrintStatusText(string val, int showTime = 10)
          {
               StatusText = val;
               var delay = Task.Delay(showTime * 1000);
               await delay;
               StatusText = String.Empty;
          }

          public ICommand OpenConsoleCommand
          {
               get {
                    if (_openConsoleCommand == null) {
                         _openConsoleCommand = new RelayCommand(x => OpenConsole());
                    }
                    return _openConsoleCommand;
               }
          }

          private ICommand _openConsoleCommand;

          private void OpenConsole()
          {
               this.ShowView(new ConsoleViewModel());
          }

          public ICommand OpenViewModelCommand
          {
               get {
                    if (_openViewModelCommand == null) {
                         _openViewModelCommand = new RelayCommand(OpenViewModel);
                    }
                    return _openViewModelCommand;
               }
          }

          private void OpenViewModel(object obj)
          {
               var param = obj as string;

               TraceEx.PrintLog($"Executing shortcut: {param}");
               switch (param) {
                    case "EditUsers":
                         ShowView(new EditUsersViewModel());
                         break;
                    default:
                         throw new ArgumentException("Invalid ViewModel Type");
               }
          }

          private ICommand _openViewModelCommand;

          private void CloseAllTabs()
          {
               var wsArray = _workspaces.ToArray();
               foreach (var workspace in wsArray) {
                    workspace.OnRequestClose();
               }
               PrintStatusText("All tabs closed");
          }

          /// <summary>
          /// Returns the collection of available workspaces to display.
          /// A 'workspace' is a ViewModel that can request to be closed.
          /// </summary>
          public ObservableCollection<WorkspaceViewModel> Workspaces
          {
               get {
                    if (_workspaces == null) {
                         _workspaces = new ObservableCollection<WorkspaceViewModel>();
                         _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                    }
                    return _workspaces;
               }
          }

          /// <summary>
          /// Show view in main window
          /// </summary>
          /// <param name="viewModel"></param>
          /// <param name="anchorable"></param>
          /// <returns>true if new window created</returns>
          public bool ShowView(WorkspaceViewModel viewModel, bool anchorable = false)
          {
               //check to ensure it is not running
               if (viewModel.OnlyOneCanRun) {
                    var otherRunning = GetOtherRunning(viewModel);
                    if (otherRunning != null) {
                         bool active = this.SetActiveWorkspace(otherRunning);
                         if (active == false) {
                              MessageBox.Show($"Can only run one '{viewModel.DisplayName}' at a time.");
                         }
                         return false;
                    }
               }

               if (anchorable == false) {
                    this.Workspaces.Add(viewModel);
                    this.SetActiveWorkspace(viewModel);
                    return true;
               } else {
                    this.Anchorables.Add(viewModel);
                    return true;
               }
          }

          /// <summary>
          /// Checks if other types of same kind are already running
          /// </summary>
          /// <param name="viewModel"></param>
          private WorkspaceViewModel GetOtherRunning(WorkspaceViewModel viewModel)
          {
               Type type = viewModel.GetType();

               //check anchorables
               foreach (var v in Anchorables) {
                    if (v.GetType() == type) {
                         return v;
                    }
               }

               //check workspaces
               foreach (var v in Workspaces) {
                    if (v.GetType() == type) {
                         return v;
                    }
               }

               return null;
          }

          public ObservableCollection<WorkspaceViewModel> Anchorables
          {
               get {
                    if (_anchorables == null) {
                         _anchorables = new ObservableCollection<WorkspaceViewModel>();
                         _anchorables.CollectionChanged += this.OnWorkspacesChanged;
                    }
                    return _anchorables;
               }
          }

          //public ObservableCollection<DependencyObject> Anchorables {
          //     get;
          //     set;
          //} = new ObservableCollection<DependencyObject>();

          private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
          {
               if (e.NewItems != null && e.NewItems.Count != 0)
                    foreach (WorkspaceViewModel workspace in e.NewItems)
                         workspace.RequestClose += this.OnWorkspaceRequestClose;

               if (e.OldItems != null && e.OldItems.Count != 0)
                    foreach (WorkspaceViewModel workspace in e.OldItems)
                         workspace.RequestClose -= this.OnWorkspaceRequestClose;
          }

          public WorkspaceViewModel ActiveWorkspace
          {
               get {
                    return _activeWorkspace;
               }
               set {
                    _activeWorkspace = value;
                    OnPropertyChanged("ActiveWorkspace");
               }
          }

          private WorkspaceViewModel _activeWorkspace;

          private bool SetActiveWorkspace(WorkspaceViewModel workspace)
          {
               if (this.Workspaces.Contains(workspace) == false) {
                    return false;
               }
               ActiveWorkspace = workspace;

               ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
               if (collectionView != null)
                    collectionView.MoveCurrentTo(workspace);

               return true;
          }

          private void OnWorkspaceRequestClose(object sender, EventArgs e)
          {
               WorkspaceViewModel workspace = sender as WorkspaceViewModel;
               workspace.Dispose();

               this.Workspaces.Remove(workspace);
               this.Anchorables.Remove(workspace);
          }
     }
}