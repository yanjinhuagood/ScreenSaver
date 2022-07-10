using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ScreenSaver
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty stringCollectionProperty =
            DependencyProperty.Register("stringCollection", typeof(ObservableCollection<string>), typeof(MainWindow),
                new PropertyMetadata(null));

        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register("Hour", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register("Minute", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty SecondProperty =
            DependencyProperty.Register("Second", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(MainWindow), new PropertyMetadata());

        private readonly DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += delegate
            {
                WindowState = WindowState.Maximized;
                Mouse.OverrideCursor = Cursors.None;
                var date = DateTime.Now;
                Hour = date.ToString("HH");
                Minute = date.ToString("mm");
                Date =
                    $"{date.Month} / {date.Day}   {CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek)}";
                stringCollection = new ObservableCollection<string>();
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                var directoryInfo = new DirectoryInfo(path);
                foreach (var item in directoryInfo.GetFiles())
                {
                    if (Path.GetExtension(item.Name) != ".jpg") continue;
                    stringCollection.Add(item.FullName);
                }

                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += delegate
                {
                    date = DateTime.Now;
                    Hour = date.ToString("HH");
                    Minute = date.ToString("mm");
                    Date =
                        $"{date.Month} / {date.Day}   {CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek)}";
                };
                timer.Start();
            };
            MouseDown += delegate { Application.Current.Shutdown(); };
            KeyDown += delegate { Application.Current.Shutdown(); };
        }

        public ObservableCollection<string> stringCollection
        {
            get => (ObservableCollection<string>)GetValue(stringCollectionProperty);
            set => SetValue(stringCollectionProperty, value);
        }


        public string Hour
        {
            get => (string)GetValue(HourProperty);
            set => SetValue(HourProperty, value);
        }

        public string Minute
        {
            get => (string)GetValue(MinuteProperty);
            set => SetValue(MinuteProperty, value);
        }

        public string Second
        {
            get => (string)GetValue(SecondProperty);
            set => SetValue(SecondProperty, value);
        }


        public string Date
        {
            get => (string)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

       
    }
}