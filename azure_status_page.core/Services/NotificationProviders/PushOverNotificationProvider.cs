using System;
using PushoverClient;

namespace azure_status_page.core.NotificationProviders
{
	public class PushOverNotificationProvider : INotificationProvider
	{
		private string Token { get; set; }
		private string User { get; set; }

		public PushOverNotificationProvider(string token, string user)
		{
			Token = token;
			User = user;
		}

		public void notify(MeterCheckResult checkResult)
		{
			var pushClient = new Pushover(Token);

			var subject = String.Format("Meter Check {0} on {1} failed", checkResult.MeterName, checkResult.MeterInstanceId);

			pushClient.Push(subject, checkResult.MeterCheckAlarmMessage, User);
		}
	}
}
