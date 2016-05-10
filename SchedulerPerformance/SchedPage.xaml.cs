using System;
using System.Collections.Generic;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace SchedulerPerformance
{
	public partial class SchedPage : ContentPage
	{
		private SfSchedule Scheduler = new SfSchedule();
		private int CurrentMonth;

		public SchedPage ()
		{
			InitializeComponent ();
			Scheduler = new SfSchedule();
			Scheduler.ScheduleView = ScheduleView.MonthView;
			Scheduler.VisibleDatesChangedEvent += (object sender, VisibleDatesChangedEventArgs args) => {
				if(CurrentMonth != args.visibleDates[10].Month){
					Task.Run(() => GetAppointments(args.visibleDates[10].Year, args.visibleDates[10].Month));
				}
			};
			this.Content = Scheduler;


		}

		protected override void OnAppearing(){
			base.OnAppearing ();
			Task.Run (() => GetAppointments (DateTime.Now.Year, DateTime.Now.Month));
		}

		/// <summary>
		/// Get appointments from the "server" and feed the datasource
		/// </summary>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		public  void GetAppointments(int year, int month){
			CurrentMonth = month;
			List<Appointment> appointments = new List<Appointment> ();
			ScheduleAppointmentCollection Collection = new ScheduleAppointmentCollection ();

			appointments =  GenerateAppointments(year, month);

			foreach (Appointment a in appointments) {
				ScheduleAppointment scheduleAppointment = new ScheduleAppointment();
				scheduleAppointment.StartTime = a.date;
				scheduleAppointment.EndTime = a.date;
				scheduleAppointment.StartTime = scheduleAppointment.StartTime.AddHours (0);
				scheduleAppointment.EndTime = scheduleAppointment.EndTime.AddHours (23);
				Collection.Add (scheduleAppointment);
			}

			Device.BeginInvokeOnMainThread(()=>{
				Scheduler.DataSource=Collection;
			});
		}

		/// <summary>
		/// Simulates the server
		/// </summary>
		/// <returns>The appointments.</returns>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		public  List<Appointment> GenerateAppointments(int year, int month){
			List<Appointment> appointments = new List<Appointment> ();
			for (var i = 1; i < 29; i++) {
				Appointment a = new Appointment ();
				a.date = new DateTime (year, month, i);
				appointments.Add (a);
			}
			return appointments;
		}
	}

	public class Appointment{
		public DateTime date{get;set;}
	}
}

