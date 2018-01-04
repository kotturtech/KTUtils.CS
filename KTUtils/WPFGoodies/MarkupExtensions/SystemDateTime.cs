using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KotturTech.WPFGoodies.MarkupExtensions
{

    /// <summary>
    /// Markup extension that provides the current system date and time.
    /// Updates the target value every second
    /// </summary>
    public class SystemDateTime : UpdatableMarkupExtension
    {
        #region Private Fields

        private static readonly Timer Timer;

        private int _busyFlag;

        private static event EventHandler TimerTick;
        

        #endregion
       
        #region StringFormatProperty

        public static string GetStringFormat(DependencyObject obj)
        {
            return (string)obj.GetValue(StringFormatProperty);
        }

        public static void SetStringFormat(DependencyObject obj, string value)
        {
            obj.SetValue(StringFormatProperty, value);
        }

        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.RegisterAttached("StringFormat", typeof(string), typeof(SystemDateTime), new UIPropertyMetadata(string.Empty));



        #endregion

        #region MyRegion

        static SystemDateTime()
        {
            //Starting the timer, while invoking the callback each next second: 1000 ms - current time milliseconds gives amount of millisecond to the next full second
            Timer = new Timer(x=> { TimerTick?.Invoke(null,EventArgs.Empty); Timer.Change(1000 - DateTime.Now.Millisecond, 1000); }, null, 1000 - DateTime.Now.Millisecond, 1000);
        }

        public SystemDateTime()
        {
            TimerTick += SystemDateTime_TimerTick;
        }

        #endregion
       
        private async void SystemDateTime_TimerTick(object sender, EventArgs e)
        {
            //Using busy flag: If some updates are so busy that next time interval came and the previous updates still go on,
            //just skip the new update to avoid threads piling up, and let the single active thread quietly finish its job
            Interlocked.Increment(ref _busyFlag);

            if (_busyFlag > 1)
            {
                Interlocked.Decrement(ref _busyFlag);
                return;
            }

            //Invoking asynchronously on UI thread, to avoid deadlocks
            var updateClockDispatcherOperation = Application.Current?.Dispatcher?.InvokeAsync(
                () =>
                {
                    foreach (var targetObject in TargetObjects)
                        UpdateValue(ProvideValueInternal(null, targetObject));
                }
            );
            if (updateClockDispatcherOperation != null)
                try
                {
                    await updateClockDispatcherOperation;
                }
                catch (TaskCanceledException)
                {
                    //ignore in this case
                }
                

            Interlocked.Decrement(ref _busyFlag);
        }

        protected override object ProvideValueInternal(IServiceProvider serviceProvider, object targetObject)
        {
            Type propertyType;
            if (TargetProperty is DependencyProperty)
            {
                DependencyProperty prop = TargetProperty as DependencyProperty;
                propertyType = prop.PropertyType;
            }
            else 
            {
                PropertyInfo pInfo = TargetProperty as PropertyInfo;
                propertyType = pInfo?.PropertyType;
            }

            if (propertyType == typeof(string))
            {
                if (targetObject is DependencyObject)
                {
                    var dp = targetObject as DependencyObject;

                    var strFormat = GetStringFormat(dp);
                   
                    if (!string.IsNullOrEmpty(strFormat))
                        return DateTime.Now.ToString(strFormat, CultureInfo.InstalledUICulture);
                }
                return DateTime.Now.ToString(CultureInfo.InstalledUICulture);
            }
           
            return DateTime.Now;
            
        }

    }
}
