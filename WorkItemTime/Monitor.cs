using System;
using System.Data;
using System.Linq;
using Microsoft.Win32;

namespace WorkItemTime
{
    public class Monitor : IDisposable
	{
        readonly DataSet _uberSet;
        readonly DataTable _activityTable;
        public Monitor(DataSet uberSet)
        {
            this._uberSet = uberSet;
            this._activityTable = this._uberSet.Tables[Data.ActivityTableName];
        }

        public void Start()
        {
            SystemEvents.SessionSwitch -= SystemEventsOnSessionSwitch;
            SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;

			SystemEvents.SessionEnded -= SystemEventsOnSessionEnded;
	        SystemEvents.SessionEnded += SystemEventsOnSessionEnded;
        }

		private void SystemEventsOnSessionEnded(object sender, SessionEndedEventArgs sessionEndedEventArgs)
		{
			var description = Enum.GetName(typeof(SessionSwitchReason), sessionEndedEventArgs.Reason);
			this.Log(description, Data.ActivityKindStop);
		}

		public void Log(string description, string activityKind)
        {
            var row = this._activityTable.NewRow();
            row.SetField(Data.ActivityDateTime, DateTime.Now);
            row.SetField(Data.ActivityDescription, description);
			row.SetField(Data.ActivityKind, activityKind);
            row.SetField(Data.ActivityStatus, Data.ActivityStatusNotSentToTfs);
			row.SetField(Data.ActivityStatusDateTime, DateTime.Now);
            this._activityTable.Rows.Add(row);
   
        }

		public void ToggleCurrent()
		{
			var latestActivity = this._uberSet.Tables[Data.ActivityTableName]
				.AsEnumerable()
				.OrderByDescending(row => row.Field<DateTime>(Data.ActivityDateTime)).FirstOrDefault();
			if (latestActivity != null)
			{
				if(latestActivity.Field<string>(Data.ActivityKind) == Data.ActivityKindStart)
				{
					this.Log("manual stop", Data.ActivityKindStop);
				}
				else
				{
					this.Log("manual start", Data.ActivityKindStart);
				}
			}
		}

		private void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs sessionSwitchEventArgs)
		{
			var description = Enum.GetName(typeof(SessionSwitchReason), sessionSwitchEventArgs.Reason);
			if (sessionSwitchEventArgs.Reason == SessionSwitchReason.SessionLock ||
			    sessionSwitchEventArgs.Reason == SessionSwitchReason.SessionLogoff)
			{
				this.Log(description, Data.ActivityKindStop);
			}
			else if (sessionSwitchEventArgs.Reason == SessionSwitchReason.SessionUnlock||
			         sessionSwitchEventArgs.Reason == SessionSwitchReason.SessionLogon)
			{
				this.Log(description, Data.ActivityKindStart);
			}
		}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SystemEvents.SessionSwitch -= SystemEventsOnSessionSwitch; 
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Monitor() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

