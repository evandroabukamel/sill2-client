using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace sill2_client
{
	public partial class sill2_client_form : Form
	{
		string version = " - v. 2.17.10.23";
		public static int lastErrorCode = 0;

		// Timers
		public static System.Timers.Timer timerLogoff; // Set up the timer for 3 minutes;
		public static System.Timers.Timer timerLock; // Set up the timer for 5 minutes;
		System.Threading.Timer detectStatTimer; // Set up the timer for 5 seconds;
		public static bool timerLockEnabled; // Control the enabled status of the timerLock
		static WindowsSession session;

		string domainName, userLogin, computerName;

		public sill2_client_form()
		{
			InitializeComponent();

			// Initialize the WindowsSession instance. 
			session = new WindowsSession();

			timerLogoff = new System.Timers.Timer(3 * 60 * 1000); // Set up the timer for 3 minutes;
			timerLock = new System.Timers.Timer(5 * 60 * 1000); // Set up the timer for 5 minutes;
			detectStatTimer = new System.Threading.Timer(new TimerCallback(DetectSessionState),
				null, Timeout.Infinite, 5000); // Set up the timer for 5 seconds;

			timerLogoff.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
			timerLock.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
			timerLockEnabled = false;

			// Detect when the application exits
			Application.ApplicationExit += new EventHandler(OnProcessExit);
		}

		private void sill2_client_form_Load(object sender, EventArgs e)
		{
			Hide();
			ShowInTaskbar = false;

			// Register the StateChanged event. 
			session.StateChanged += new EventHandler<SessionSwitchEventArgs>(session_StateChanged);

			// Initializes the notifyIcon
			trayIcon = new NotifyIcon();
			trayIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath.ToString());
			trayIcon.Visible = true;
			trayIcon.Click += TrayIcon_Click;

			// The the texts on the form labels
			PutLabelsText();

			/*this.Show();
			this.Activate();
			this.WindowState = FormWindowState.Normal;
			this.Focus();
			this.BringToFront();*/

			// Initializes the processing
			Task.Run(() => Verification.Start(this));
		}

		// Set the values of the form labels
		public void PutLabelsText()
		{
			/* Get Domain name, User login and Computer name */
			try
			{
				domainName = Environment.UserDomainName;
				userLogin = Environment.UserName;
				computerName = Environment.MachineName;

				lblComputador.Text = computerName;
				lblUsuario.Text = domainName + " \\ " + userLogin;
				SetTrayIconText(computerName + " \\ " + userLogin);

				Verification.domainName = domainName;
				Verification.userLogin = userLogin;
				Verification.computerName = computerName;
			}
			catch (Exception ex)
			{
				if (lastErrorCode != 7)
				{
					SetTrayIconError("sill2-client: 7 - Erro para obter os nomes do usuário, do domínio e do computador.\n" + ex.Message);
					lastErrorCode = 7;
					Environment.Exit(7);
					// saveException(ex.Message + "\n" + ex.StackTrace);
				}
			}
		}

		// Get logoff timer Enabled
		public bool TimerLogoffEnabled
		{
			get { return timerLogoff.Enabled; } 
		}

		// Enable logoff timer
		public void timerLogoffEnable()
		{
			//timerLogoff.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
			timerLogoff.Start();
		}

		// Disable logoff timer
		public void timerLogoffDisable()
		{
			timerLogoff.Stop();
			//timerLogoff.Elapsed -= new ElapsedEventHandler(OnTimerElapsed);
		}

		/// <summary> 
		/// Handle the StateChanged event of WindowsSession. 
		/// </summary> 
		void session_StateChanged(object sender, SessionSwitchEventArgs e)
		{
			/*StreamWriter sw = new StreamWriter("timerLock.txt", true);
			sw.WriteLine("State changed: {0}    Detect Time: {1} ", e.Reason, DateTime.Now);
			sw.Close();*/

			// Enable the lock timer when user locks its session
			if (timerLockEnabled && e.Reason == SessionSwitchReason.SessionLock)
			{
				timerLock.Start();
			}
			else if (e.Reason == SessionSwitchReason.SessionUnlock)
			{
				timerLockDisable();
			}
		}

		void DetectSessionState(object obj)
		{
			// Check whether the current session is locked. 
			bool isCurrentLocked = session.IsLocked();

			var state = isCurrentLocked ? SessionSwitchReason.SessionLock
				: SessionSwitchReason.SessionUnlock;

			/*StreamWriter sw = new StreamWriter("timerLock.txt", true);
			sw.WriteLine("Current State: {0}    Time: {1} ", state, DateTime.Now);
			sw.Close();*/
		}

		// Get logoff timer Enabled
		public bool TimerLockEnabled
		{
			get { return timerLockEnabled; }
		}

		// Enable logoff timer
		public void timerLockEnable()
		{
			//timerLock.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
			timerLockEnabled = true;
			//SystemEvents.SessionSwitch += sseh;

			/*StreamWriter sw = new StreamWriter("timerLock.txt", true);
			sw.WriteLine("timerLock Enable");
			sw.Close();*/
		}

		// Disable logoff timer
		public void timerLockDisable()
		{
			//timerLock.Elapsed -= new ElapsedEventHandler(OnTimerElapsed);
			//SystemEvents.SessionSwitch -= new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
			timerLockEnabled = false;
			timerLock.Stop();

			/*StreamWriter sw = new StreamWriter("timerLock.txt", true);
			sw.WriteLine("timerLock Disable");
			sw.Close();*/
		}

		/**
		 * Time elapsed for the user log oof on the other computers or lock the session.
		 * Make log off on this computer. 
		 */
		static void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			/*StreamWriter sw = new StreamWriter("timerLock.txt", true);
			sw.WriteLine("LOGOFF");
			sw.Close();*/

			// Logoff the user
			Process.Start("shutdown", "/l /f");
			//MessageBox.Show("\nsill2: Tem que fazer logoff!");
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			this.Visible = false;
		}

		/** Handle Closing of the Form and show the tray icon */
		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = true;
			//this.Hide();
		}

		/** Close and dispose the connection before exit */
		static void OnProcessExit(object sender, EventArgs e)
		{
			timerLogoff.Elapsed -= new ElapsedEventHandler(OnTimerElapsed);
			timerLock.Elapsed -= new ElapsedEventHandler(OnTimerElapsed);
			session.Dispose();
			Verification.mysqlDispose();
		}

		private void TrayIcon_Click(object sender, EventArgs e)
		{
			SetTrayIconText(computerName + " \\ " + userLogin);
		}

		/** Set the info text of the tray icon */
		public void SetTrayIconText(string text)
		{
			trayIcon.ShowBalloonTip(5, "SILL2 Client" + version, text, ToolTipIcon.Info);
		}

		/*  Set the text error of the tray icon */
		public void SetTrayIconError(string text)
		{
			trayIcon.ShowBalloonTip(5, "SILL2 Client" + version, text, ToolTipIcon.Error);
		}

		/*  Set the text warning of the tray icon */
		public void SetTrayIconWarning(string text)
		{
			trayIcon.ShowBalloonTip(5, "SILL2 Client" + version, text, ToolTipIcon.Warning);
		}
	}
}
