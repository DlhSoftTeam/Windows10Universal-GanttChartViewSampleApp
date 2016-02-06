using DlhSoft.ProjectData.GanttChart.WinRT.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GanttChartViewSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static readonly DateTime date = DateTime.Today;
        private static readonly int year = date.Year, month = date.Month;

        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeGanttChartView();
        }

        private void InitializeGanttChartView()
        {
            // Prepare data items.
            var items = new ObservableCollection<GanttChartItem>
            {
                new GanttChartItem { Content = "Task 1", IsExpanded = false },
                new GanttChartItem { Content = "Task 1.1", Indentation = 1, Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 4, 16, 0, 0) },
                new GanttChartItem { Content = "Task 1.2", Indentation = 1, Start = new DateTime(year, month, 3, 8, 0, 0), Finish = new DateTime(year, month, 5, 12, 0, 0) },
                new GanttChartItem { Content = "Task 2", IsExpanded = true },
                new GanttChartItem { Content = "Task 2.1", Indentation = 1, Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 8, 16, 0, 0), CompletedFinish = new DateTime(year, month, 5, 16, 0, 0), AssignmentsContent = "Resource 1, Resource 2 [50%]" },
                new GanttChartItem { Content = "Task 2.2", Indentation = 1 },
                new GanttChartItem { Content = "Task 2.2.1", Indentation = 2, Start = new DateTime(year, month, 11, 8, 0, 0), Finish = new DateTime(year, month, 12, 16, 0, 0), CompletedFinish = new DateTime(year, month, 12, 16, 0, 0), AssignmentsContent = "Resource 2" },
                new GanttChartItem { Content = "Task 2.2.2", Indentation = 2, Start = new DateTime(year, month, 12, 12, 0, 0), Finish = new DateTime(year, month, 14, 16, 0, 0) },
                new GanttChartItem { Content = "Task 3", Indentation = 1, Start = new DateTime(year, month, 15, 16, 0, 0), IsMilestone = true }
            };
            items[3].Predecessors = new ObservableCollection<PredecessorItem> { new PredecessorItem { Item = items[0], DependencyType = DependencyType.StartStart } };
            items[7].Predecessors = new ObservableCollection<PredecessorItem> { new PredecessorItem { Item = items[6], Lag = TimeSpan.FromHours(2) } };
            items[8].Predecessors = new ObservableCollection<PredecessorItem> { new PredecessorItem { Item = items[4] }, new PredecessorItem { Item = items[5] } };
            for (int i = 4; i <= 32; i++)
                items.Add(new GanttChartItem { Content = "Task " + i, Start = new DateTime(year, month, 2, 8, 0, 0), Finish = new DateTime(year, month, 4, 16, 0, 0) });
            GanttChartView.Items = items;

            // Optionally, hide data grid or set grid and chart widths, and/or set read only settings.
            // GanttChartView.IsGridVisible = false;
            // GanttChartView.GridWidthPercent = 30;
            // GanttChartView.ChartWidthPercent = 70;
            // GanttChartView.IsGridReadOnly = true;
            // GanttChartView.IsChartReadOnly = true;

            // Set the scrollable timeline to present, and the displayed and current time values to automatically scroll to a specific chart coordinate, and display a vertical bar highlighter at the specified point.
            GanttChartView.TimelineStart = new DateTime(year, month, 1).AddDays(-7);
            GanttChartView.TimelineFinish = new DateTime(year + 1, month, 1);
            GanttChartView.DisplayedTime = new DateTime(year, month, 1);
            GanttChartView.CurrentTime = new DateTime(year, month, 2, 12, 0, 0);

            // Optionally, configure the chart scales and their display settings.
            // GanttChartView.HeaderHeight = 21 * 3;
            // GanttChartView.Scales = new List<Scale>
            // {
            //     new Scale { ScaleType = ScaleType.Months, HeaderTextFormat = ScaleHeaderTextFormat.Month, IsSeparatorVisible = true },
            //     new Scale { ScaleType = ScaleType.Weeks, HeaderTextFormat = ScaleHeaderTextFormat.Date, IsSeparatorVisible = true },
            //     new Scale { ScaleType = ScaleType.Days, HeaderTextFormat = ScaleHeaderTextFormat.Day }
            // };
            // GanttChartView.CurrentTimeLineColor = Colors.Red;
            // GanttChartView.UpdateScaleInterval = TimeSpan.FromHours(1);

            // Optionally, zoom in or out to display more time details or higher level project overviews.
            // GanttChartView.HourWidth = 5;

            // Optionally, configure the working time schedule of the week and week days.
            // GanttChartView.VisibleWeekStart = (int)DayOfWeek.Monday;
            // GanttChartView.VisibleWeekFinish = (int)DayOfWeek.Friday;
            // GanttChartView.WorkingWeekStart = (int)DayOfWeek.Monday;
            // GanttChartView.WorkingWeekFinish = (int)DayOfWeek.Thursday;
            // GanttChartView.VisibleDayStart = TimeSpan.Parse("10:00:00"); // 10 AM
            // GanttChartView.VisibleDayFinish = TimeSpan.Parse("20:00:00");  // 8 PM
            // GanttChartView.SpecialNonworkingDays = new List<DateTimeOffset>() { new DateTime(year, month, 24), new DateTime(year, month, 25) };

            // Optionally, set custom appearance settings.
            // GanttChartView.BorderColor = Colors.Gray;
            // GanttChartView.StandardBarStroke = Colors.Green;
            // GanttChartView.StandardBarFill = Colors.LightGreen;
            // GanttChartView.StandardCompletedBarFill = Colors.DarkGreen;
            // GanttChartView.StandardCompletedBarStroke = Colors.DarkGreen;
            // GanttChartView.DependencyLineStroke = Colors.Green;

            // Optionally, display alternative row background.
            // GanttChartView.AlternativeRowBackgroundColor = Color.FromArgb(0xff, 0xf5, 0xf5, 0xf5);

            // Optionally, configure selection appearance.
            // GanttChartView.SelectedItemBackgroundColor = Colors.LightCyan;

            // Optionally, initialize item selection.
            // GanttChartView.SelectedItem = GanttChartView.Items[6];

            // Optionally, set baseline properties.
            // GanttChartView.Items[6].BaselineStart = new DateTime(year, month, 10, 8, 0, 0);
            // GanttChartView.Items[6].BaselineFinish = new DateTime(year, month, 11, 16, 0, 0);
            // GanttChartView.Items[7].BaselineStart = new DateTime(year, month, 8, 8, 0, 0);
            // GanttChartView.Items[7].BaselineFinish = new DateTime(year, month, 11, 16, 0, 0);
            // GanttChartView.Items[8].BaselineStart = new DateTime(year, month, 12, 8, 0, 0);

            // Optionally, configure columns.
            // GanttChartView.Columns[(int)ColumnType.Content].Header = "Work items";
            // GanttChartView.Columns[(int)ColumnType.Content].Width = 240;
            // GanttChartView.Columns[(int)ColumnType.Start].Header = "Beginning";
            // GanttChartView.Columns[(int)ColumnType.Finish].Header = "End";
            // GanttChartView.Columns[(int)ColumnType.Milestone].Header = "Is milestone";
            // GanttChartView.Columns[(int)ColumnType.Assignments].Header = "Workers";
            // GanttChartView.Columns[(int)ColumnType.Completed].Header = "Is completed";
            // GanttChartView.Columns[(int)ColumnType.RowHeader].IsVisible = false;

            // Optionally, set custom item properties, and append read only custom columns bound to their values.
            // GanttChartView.Items[7].SetCustomValue("Description", "Custom description");
            // GanttChartView.Columns.Add(new Column { Header = "Description", Width = 200, PropertyName = "Description" });

            // Optionally, define assignable resources.
            GanttChartView.AssignableResources = new List<string> { "Resource 1", "Resource 2", "Resource 3" };
            GanttChartView.AutoAppendAssignableResources = true;

            // Optionally, set up date and time values to be formatted as simple dates without time information.
            // GanttChartView.UseSimpleDateFormatting = true;

            // Optionally, set up auto-scheduling behavior for dependent tasks based on predecessor information, supplementary disallowing circular dependencies.
            GanttChartView.AreTaskDependencyConstraintsEnabled = true;

            // Optionally, receive individual notifications for the item property changes that have occured on the client side by handling the ItemPropertyChanged event.
            // GanttChartView.ItemPropertyChanged += delegate(object sender, GanttChartItemPropertyChangedEventArgs change)
            // {
            //     if (change.IsDirect && change.IsFinal && change.PropertyName != "IsSelected" && change.PropertyName != "IsExpanded")
            //     {
            //         MessageDialog dialog = new MessageDialog(string.Format("{1} property has changed for {0}.", change.Item.Content, change.PropertyName));
            //         dialog.ShowAsync();
            //     }
            // };
        }
    }
}
