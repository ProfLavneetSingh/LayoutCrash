using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LayoutCrash
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new CrashViewModel();
        }
    }

    public class Task
    {
        public int Hours { get; set; } = 5;
    }
    public class Job : BaseViewModel
    {
        private ObservableCollection<Task> _assignedTasks = new();
        public ObservableCollection<Task> AssignedTasks 
        { 
            get => _assignedTasks; 
            set => SetProperty(ref _assignedTasks, value);
        }
    }
    public class CrashViewModel : BaseViewModel
    {
        public CrashViewModel()
        {
            LoadData();
            ExpectedWidth = ColCount * 200;
        }
        double _expectedWidth;
        public double ExpectedWidth { get => _expectedWidth; set => SetProperty(ref _expectedWidth, value); }

        public int ColCount { get; set; } = 20;
        public string AssignedHours { get; set; } = "10";
        public string HourLimit { get; set; } = "8";

        ObservableCollection<Job> _allAssignedJobs = new();
        public ObservableCollection<Job> Jobs 
        { 
            get => _allAssignedJobs;  
            set => SetProperty(ref _allAssignedJobs, value);
        }

        void LoadData()
        {
            List<Job> jobs = new List<Job>();
            Random random = new Random();
            for (int j = 0; j < ColCount * 20; j++)
            {
                Job job = new Job();
                for (int i = 0; i < 5; i++)
                {
                    Task task = new Task() { Hours = random.Next(8) };
                    job.AssignedTasks.Add(task);
                }
                jobs.Add(job);
            }
            Jobs = new (jobs);
        }
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

}
