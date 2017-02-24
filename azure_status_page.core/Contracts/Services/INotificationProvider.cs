using System;
namespace azure_status_page.core
{
	public interface INotificationProvider
	{
		void notify(MeterCheckResult checkResult);
	}
}
