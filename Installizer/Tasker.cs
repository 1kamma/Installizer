using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Installizer {
    public class TaskerBase {
        public Dictionary<string, List<List<object>>> Triggers { get; set; }
        public Dictionary<string, bool> Principal { get; set; }
        public List<string[]> Actions { get; set; }
        public string TaskName { get; private set; }
        public string TaskDescription { get; set; }
        public string TaskPath { get; private set; }
        public Dictionary<string, bool> Settings { get; set; }

        public TaskerBase(string taskName, string taskPath) {
            TaskName = taskName;
            TaskPath = taskPath;
        }

        public static string[] GetEventNameLog(string subscription) {
            System.Text.RegularExpressions.Regex regex = new(@"(?<=\"")[A-z/\-]+(?=\"")|(?<=EventID=)\d+");
            System.Text.RegularExpressions.Regex regex1 = new(@"(<=\:\s)[^[,$]]+");
            return regex.IsMatch(subscription) ? regex.Matches(subscription).Select(match => { return match.Groups[1].Value; }).ToArray() : regex1.Matches(subscription).Select(match => { return match.Groups[1].Value; }).ToArray();
        }
    }
    /// <summary>
    /// This Tasker Class and object is a connection to the TaskScheduler API.
    /// All the data will be saved first as local object, and then will be convert to the task scheduler api
    /// </summary>
    public class Tasker {
        #region Properties
        /// <summary>
        /// Collection of all the triggers that will start the task. public property
        /// </summary>
        public TriggerCollection Triggers { get; private set; }
        /// <summary>
        /// Principal setting of how and with what permission and level the task will run. public property
        /// </summary>
        public TaskPrincipal Principal { get; private set; }
        /// <summary>
        /// Principal setting of how and with what permission and level the task will run. private property
        /// </summary>
        private TaskPrincipal _principal { set => Principal = value; }
        public ActionCollection Actions { get; private set; }
        public string TaskName { get; private set; }
        public string TaskDescription { get; set; }
        public string TaskPath { get; private set; }

        public TaskSettings Settings { get; private set; }
        private TaskDefinition _taskDefinition { get; set; }
        public string Author { get; set; }
        private TaskService ScTask { get; set; }
        private TaskerBase taskerBase { get; set; }
        public enum Repitition {
            None,
            Daily,
            Weekly,
            Monthly
        }
        public enum StartAt {
            None,
            Logon,
            Event,
            Startup
        }
        #endregion

        #region Constructors
        [JsonConstructor]
        public Tasker(string filePath) {
            //foreach(var trig in jsonConstructor.)
            var fileRead = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(System.IO.File.ReadAllText(filePath));

        }
        public Tasker(string Name, string Path) {
            taskerBase = new(Name, Path);
            TaskName = Name;
            TaskPath = Path;
            ScTask = new();
            Author = $"{Environment.MachineName}\\{Environment.UserName}";
            _taskDefinition = ScTask.NewTask();
            Triggers = _taskDefinition.Triggers;
            Actions = _taskDefinition.Actions;
            Settings = _taskDefinition.Settings;
            Principal = _taskDefinition.Principal;
        }
        public Tasker(string Name, string Path, string description = "") {
            taskerBase = new(Name, Path);
            TaskName = Name;
            TaskPath = Path;
            TaskDescription = description;
            ScTask = new();
            Author = $"{Environment.MachineName}\\{Environment.UserName}";
            _taskDefinition = ScTask.NewTask();
            Triggers = _taskDefinition.Triggers;
            Actions = _taskDefinition.Actions;
            Settings = _taskDefinition.Settings;
            Principal = _taskDefinition.Principal;
        }
        public Tasker(string Name, string Path, TaskDefinition taskDefinition) {
            taskerBase = new(Name, Path);
            TaskName = Name;
            TaskPath = Path;
            ScTask = new();
            _taskDefinition = taskDefinition;
            Triggers = _taskDefinition.Triggers;
            Author = _taskDefinition.RegistrationInfo.Author;
            Actions = _taskDefinition.Actions;
            TaskDescription = _taskDefinition.RegistrationInfo.Description;
            Settings = _taskDefinition.Settings;
            Principal = _taskDefinition.Principal;
        }
        #endregion

        #region Definitions

        #region Triggers
        public void SetTriggers(string hour, Repitition reps, string days = "") {

            switch (reps) {
                case Repitition.Daily: {
                        DailyTrigger dailyTrigger = new();
                        dailyTrigger.StartBoundary = DateTime.Today + TimeSpan.FromHours(int.Parse(hour.Split(":")[0])) + TimeSpan.FromMinutes(int.Parse(hour.Split(":")[1]));
                        dailyTrigger.Enabled = true;
                        Triggers.Add(dailyTrigger);
                        //_taskDefinition.Triggers.Add(Triggers);
                        break;
                    }
                case Repitition.Weekly: {
                        WeeklyTrigger weeklyTrigger = new();
                        weeklyTrigger.Enabled = true;
                        weeklyTrigger.DaysOfWeek = DaysOftheWeek(days);
                        weeklyTrigger.StartBoundary = DateTime.Today + TimeSpan.FromHours(int.Parse(hour.Split(":")[0])) + TimeSpan.FromMinutes(int.Parse(hour.Split(":")[1]));
                        Triggers.Add(weeklyTrigger);
                        //_taskDefinition.Triggers.Add(weeklyTrigger);
                        break;
                    }
                case Repitition.Monthly: {
                        MonthlyTrigger monthlyTrigger = new();
                        monthlyTrigger.Enabled = true;
                        monthlyTrigger.DaysOfMonth = days.Length > 0 ? DaysOfTheMonth(days) : DaysOfTheMonth("1");
                        monthlyTrigger.MonthsOfYear = MonthsOfTheYear.January | MonthsOfTheYear.February | MonthsOfTheYear.March | MonthsOfTheYear.April | MonthsOfTheYear.May | MonthsOfTheYear.June | MonthsOfTheYear.July | MonthsOfTheYear.August | MonthsOfTheYear.September | MonthsOfTheYear.October | MonthsOfTheYear.November | MonthsOfTheYear.December;
                        Triggers.Add(monthlyTrigger);
                        //_taskDefinition.Triggers.Add(monthlyTrigger);
                        break;
                    }
            }

        }
        public void SetTriggers(string[] hours, Repitition reps, string days = "") {
            foreach (string hour in hours) {
                SetTriggers(hour, reps, days);
            }
        }
        public void SetTriggers(string hour, Repitition reps, int[] days) {

            switch (reps) {
                case Repitition.Daily: {
                        DailyTrigger dailyTrigger = new();
                        dailyTrigger.StartBoundary = DateTime.Today + TimeSpan.FromHours(int.Parse(hour.Split(":")[0])) + TimeSpan.FromMinutes(int.Parse(hour.Split(":")[1]));
                        dailyTrigger.Enabled = true;
                        Triggers.Add(dailyTrigger);
                        //_taskDefinition.Triggers;
                        //_taskDefinition.Triggers.Add(dailyTrigger);
                        break;
                    }
                case Repitition.Weekly: {
                        WeeklyTrigger weeklyTrigger = new();
                        weeklyTrigger.Enabled = true;
                        weeklyTrigger.DaysOfWeek = DaysOftheWeek(days);
                        weeklyTrigger.StartBoundary = DateTime.Today + TimeSpan.FromHours(int.Parse(hour.Split(":")[0])) + TimeSpan.FromMinutes(int.Parse(hour.Split(":")[1]));
                        Triggers.Add(weeklyTrigger);
                        //_taskDefinition.Triggers.Add(weeklyTrigger);
                        break;
                    }
                case Repitition.Monthly: {
                        MonthlyTrigger monthlyTrigger = new();
                        monthlyTrigger.Enabled = true;
                        monthlyTrigger.DaysOfMonth = days.Length > 0 ? DaysOfTheMonth(days) : DaysOfTheMonth("1");
                        monthlyTrigger.MonthsOfYear = MonthsOfTheYear.January | MonthsOfTheYear.February | MonthsOfTheYear.March | MonthsOfTheYear.April | MonthsOfTheYear.May | MonthsOfTheYear.June | MonthsOfTheYear.July | MonthsOfTheYear.August | MonthsOfTheYear.September | MonthsOfTheYear.October | MonthsOfTheYear.November | MonthsOfTheYear.December;
                        Triggers.Add(monthlyTrigger);
                        //_taskDefinition.Triggers.Add(monthlyTrigger);
                        break;
                    }
            }
        }
        public void SetTriggers(string[] hours, Repitition reps, int[] days) {
            foreach (var hour in hours) {
                SetTriggers(hour, reps, days);
            }
        }
        public void SetTriggers(StartAt start, string[]? events = null) {
            switch (start) {
                case StartAt.None:
                    break;
                case StartAt.Event: {
                        if (events != null && events.Length > 1 && events.Length <= 3) {
                            EventTrigger trigger = new();
                            trigger.Enabled = true;
                            if (events[2] != null)
                                trigger.SetBasic(events[0], events[1], int.Parse(events[2]));
                            else
                                trigger.SetBasic(events[0], events[1], null);
                            Triggers.Add(trigger);
                            _taskDefinition.Triggers.Add(trigger);
                        }
                        break;
                    }
                case StartAt.Startup: {
                        BootTrigger trigger = new BootTrigger();
                        Triggers.Add(trigger);
                        _taskDefinition.Triggers.Add(trigger);
                        break;
                    }
                default: {
                        LogonTrigger trigger = new();
                        Triggers.Add(trigger);
                        _taskDefinition.Triggers.Add(trigger);
                        break;
                    }
            }

        }
        #endregion

        #region Actions
        public void SetActions(string path, string argumentList = "", string workingDirectory = "") {
            ExecAction action = new ExecAction();
            action.Path = path;
            action.Arguments = argumentList;
            action.WorkingDirectory = workingDirectory;
            Actions.Add(action);

            _taskDefinition.Actions.Add(action);
        }
        public void SetActions(string[] path, string[]? argumentList = null, string workingDirectory = "") {
            string[] arguments = new string[path.Length];
            if (argumentList.Length < path.Length) {
                arguments = new string[path.Length];
                for (int i = 0; i < argumentList.Length; i++) {
                    arguments[i] = argumentList[i];
                }
                for (int i = argumentList.Length; i < arguments.Length; i++) {
                    arguments[i] = "";
                }
            } else if (argumentList.Length > path.Length) {
                arguments = new string[path.Length];
                for (int i = argumentList.Length; i < arguments.Length; i++) {
                    arguments[i] = argumentList[i];
                }
            }
            var pathArgs = path.Length == argumentList.Length ? path.Zip(argumentList, (Path, Argu) => new { ActPath = Path, ActArg = Argu }) : path.Zip(arguments, (Path, Argu) => new { ActPath = Path, ActArg = Argu });
            foreach (var ar in pathArgs) {
                SetActions(ar.ActPath, ar.ActArg, workingDirectory);
            }
        }
        #endregion

        #region Pricipal
        public void SetPrincipal(bool Highest = false, bool User = true) {
            Principal.Id = $"{Environment.MachineName}\\{Environment.UserName}";
            Principal.LogonType = User ? TaskLogonType.Password : TaskLogonType.S4U;
            Principal.RunLevel = Highest ? TaskRunLevel.Highest : TaskRunLevel.LUA;
            _taskDefinition.Principal.RunLevel = Highest ? TaskRunLevel.Highest : TaskRunLevel.LUA;
            _taskDefinition.Principal.LogonType = User ? TaskLogonType.Password : TaskLogonType.S4U;
        }
        #endregion

        #region Settings

        public void SetSettings(bool batteries = true, bool hidden = false, bool startWhenAvailable = true, bool enable = true) {
            (Settings.StopIfGoingOnBatteries, Settings.DisallowStartIfOnBatteries) = (!batteries, !batteries);
            Settings.Hidden = hidden;
            Settings.StartWhenAvailable = startWhenAvailable;
            Settings.AllowDemandStart = true;
            Settings.Enabled = enable;
            (_taskDefinition.Settings.StopIfGoingOnBatteries, _taskDefinition.Settings.DisallowStartIfOnBatteries) = (!batteries, !batteries);
            _taskDefinition.Settings.Hidden = hidden;
            _taskDefinition.Settings.StartWhenAvailable = startWhenAvailable;
            _taskDefinition.Settings.AllowDemandStart = true;
            _taskDefinition.Settings.Enabled = enable;

        }

        #endregion

        #endregion

        #region SetupTask
        public void SetTask(string description = "") {
            TaskDescription = description;
            TaskDefinition task = ScTask.NewTask();
            task.Triggers.AddRange(Triggers);
            (task.Principal.RunLevel, task.Principal.LogonType, task.Principal.UserId) = (Principal.RunLevel, Principal.LogonType, Principal.UserId);
            task.Actions.AddRange(Actions);
            (task.Settings.DisallowStartIfOnBatteries, task.Settings.StopIfGoingOnBatteries, task.Settings.StartWhenAvailable, task.Settings.Enabled, task.Settings.Hidden, task.Settings.AllowDemandStart) = (Settings.DisallowStartIfOnBatteries, Settings.StopIfGoingOnBatteries, Settings.StartWhenAvailable, Settings.Enabled, Settings.Hidden, Settings.AllowDemandStart);
            task.RegistrationInfo.Description = TaskDescription;
            task.RegistrationInfo.Author ??= $"{Environment.MachineName}\\{Environment.UserName}";
            ScTask.RootFolder.RegisterTaskDefinition($"\\{TaskPath}\\{TaskName}", task);
        }
        public void ExportTask(string Path) {

            var settings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            settings.Error += (o, args) => {
                if (args.ErrorContext.Error.InnerException is NotImplementedException)
                    args.ErrorContext.Handled = true;
            };

            string data = JsonConvert.SerializeObject(this, Formatting.Indented, settings);
            System.IO.File.WriteAllText(Path, data);
        }
        private void SetTaskerBase() {
            foreach (var trig in Triggers) {
                if (trig.GetType().Name.Equals("EventTrigger")) {
                    //taskerBase.Triggers["EventTrigger"].Add(new Dictionary<string, object> { ("event", new string[] { } })

                }
            }
        }
        public static void ImportTask(string Path) {
            string data = System.IO.File.ReadAllText(Path);
            Tasker tasker = JsonConvert.DeserializeObject<Tasker>(data);
            tasker.SetTask();
        }
        public static Tasker ExportTask(string taskName, string taskPath = "") {
            TaskService service = new();
            var tsk = service.FindTask(taskName, true);
            Tasker tasker = new(taskName, taskPath);
            tasker.Actions = tsk.Definition.Actions;
            tasker.Settings = tsk.Definition.Settings;
            tasker.Triggers = tsk.Definition.Triggers;
            tasker.Principal = tsk.Definition.Principal;
            tasker.TaskDescription = tsk.Definition.RegistrationInfo.Description;
            tasker.Author = tsk.Definition.RegistrationInfo.Author;
            return tasker;
        }
        #endregion

        #region Helpers
        private DaysOfTheWeek DaysOftheWeek(string days) {

            DaysOfTheWeek? day = null;
            foreach (int dayOfWeek in new int[] { 0, 1, 2, 3, 4, 5, 6 }) {

                if (day == null && days.Contains($"{dayOfWeek}")) {
                    day = (DaysOfTheWeek)dayOfWeek;
                } else if (days.Contains($"{dayOfWeek}")) {
                    day |= (DaysOfTheWeek)dayOfWeek;
                }
            }
            return (DaysOfTheWeek)day;

        }
        private DaysOfTheWeek DaysOftheWeek(int[] days) {
            DaysOfTheWeek? day = null;
            foreach (int dayOfWeek in days) {
                if (dayOfWeek < 7) {
                    if (day == null) {
                        day = (DaysOfTheWeek)dayOfWeek;
                    } else {
                        day |= (DaysOfTheWeek)dayOfWeek;
                    }
                }
            }
            return (DaysOfTheWeek)day;
        }
        private int[] DaysOfTheMonth(string days) {
            return days.Split(",").Select(x => int.Parse(x)).Where(y => y > 0 && y < 32).ToArray();
            //List<int> daysOfTheMonth = new List<int>();
            //foreach (string day in days.Split(","))
            //{
            //    if (int.Parse(day) > 0 && int.Parse(day) < 32)
            //    {
            //        daysOfTheMonth.Add(int.Parse(day));
            //    }
            //}
            //return daysOfTheMonth.ToArray();
        }
        private int[] DaysOfTheMonth(int[] days) {
            List<int> daysOfTheMonth = new(days);
            return daysOfTheMonth.Where(x => x > 0 && x < 32).ToArray();
        }
        public void Mu(string days) {
            Console.WriteLine(DaysOfTheMonth(days));
        }
        #endregion
        private Repitition GetRepitition(Newtonsoft.Json.Linq.JObject Trig) {
            switch (int.Parse(Trig["TriggerType"].ToString())) {
                case 2:
                    return Repitition.Daily;
                case 3:
                    return Repitition.Weekly;
                case 4:
                    return Repitition.Monthly;
                default:
                    return Repitition.None;
            }
        }
    }
}
