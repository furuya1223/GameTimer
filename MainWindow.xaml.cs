using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameTimer
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer dispatcherTimer;
		int stamina;		// 
		int now_stamina;	// 
		int max;			// 
		int target;			// 
		int rest;			// 
		DateTime target_dt;	// 
		DateTime max_dt;	// 
		TimeSpan minute;	// 1分
		TimeSpan second;	// 1秒

		public MainWindow()
		{
			InitializeComponent();
			
			dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 1);	// タイマー刻みの間隔。1秒ごと。
			dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);	// タイマー間隔が経過すると発生するイベント
			minute = new TimeSpan(0, 1, 0);
			second = new TimeSpan(0, 0, 1);

		}

		// タイマー間隔が経過すると（毎秒）発生するイベント
		void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			// 5分ごとにスタミナ回復
			if ((target_dt - DateTime.Now).Seconds == 0 && (target_dt - DateTime.Now).Minutes % 5 == 0)
			{
				now_stamina++;
			}
			stamina_label.Text = now_stamina.ToString();
			now_time.Text = DateTime.Now.ToLongTimeString();
			target_time.Text = (target_dt - DateTime.Now).ToString();
			max_time.Text = (max_dt - DateTime.Now).ToString();
			target_time2.Text = target_dt.ToLongTimeString();
			max_time2.Text = max_dt.ToLongTimeString();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			stamina = int.Parse(now_box.Text);
			now_stamina = stamina;
			max = int.Parse(max_box.Text);			// スタミナの限界値
			target = int.Parse(target_box.Text);	// 目標のスタミナ値（イベントに合わせて、50など）
			rest = int.Parse(rest_time.Text);		// 現時点でスタミナ増加までの残り時間

			TimeSpan target_diff;
			TimeSpan max_diff;

			int target_minute = (target - stamina) * 5 - (5 - rest);
			int max_minute = (max - stamina) * 5 - (5 - rest);

			target_diff = new TimeSpan(target_minute / 60, ((target - stamina) * 5 - (5 - rest)) % 60, int.Parse(rest_time_sec.Text));
			max_diff = new TimeSpan(max_minute / 60, max_minute % 60, int.Parse(rest_time_sec.Text));

			target_dt = DateTime.Now.Add(target_diff);
			max_dt = DateTime.Now.Add(max_diff);

			dispatcherTimer.Start();	// タイマーを開始
		}
	}
}
