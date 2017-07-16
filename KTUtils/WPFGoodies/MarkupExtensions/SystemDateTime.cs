using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace KotturTech.WPFGoodies.MarkupExtensions
{

    /// <summary>
    /// Markup extension that provides the current system date and time.
    /// Updates the target value every second
    /// </summary>
    public class SystemDateTime : UpdatableMarkupExtension
    {
        private static readonly Timer Timer;

        private static event EventHandler TimerTick;

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


        static SystemDateTime()
        {
            Timer = new Timer(x=> { TimerTick?.Invoke(null,EventArgs.Empty); Timer.Change(1000 - DateTime.Now.Millisecond, 1000); }, null, 1000 - DateTime.Now.Millisecond, 1000);
        }

        public SystemDateTime()
        {
            TimerTick += SystemDateTime_TimerTick;
        }

        private void SystemDateTime_TimerTick(object sender, EventArgs e)
        {
            UpdateValue(ProvideValueInternal(null));
        }

        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
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
                if (TargetObject is DependencyObject)
                {
                    var dp = TargetObject as DependencyObject;
                    string strFormat = string.Empty;
                    if (dp.CheckAccess())
                        strFormat = GetStringFormat(dp);
                    else
                    {
                        dp.Dispatcher.Invoke(()=>strFormat = GetStringFormat(dp));
                    }

                    if (!string.IsNullOrEmpty(strFormat))
                        return DateTime.Now.ToString(strFormat, CultureInfo.InstalledUICulture);
                }
                return DateTime.Now.ToString(CultureInfo.InstalledUICulture);
            }
           
            return DateTime.Now;
            
        }
    }
}
