using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace KotturTech.WPFGoodies.MarkupExtensions
{
    public class SystemDateTime : UpdatableMarkupExtension
    {
        private readonly Timer _timer;
        public string CurrentDateTimeString => string.IsNullOrWhiteSpace(StringFormat)? DateTime.Now.ToString(CultureInfo.InvariantCulture) : DateTime.Now.ToString(StringFormat);

        public string StringFormat { get; set; }

      

        public SystemDateTime()
        {
            _timer = new Timer(OnTimerTick, null, 1000 - DateTime.Now.Millisecond, 1000);
        }

        private void OnTimerTick(object state)
        {
            UpdateValue(ProvideValueInternal(null));
            //Resync clock on the second
            _timer.Change(1000 - DateTime.Now.Millisecond, 1000);
        }

        protected override object ProvideValueInternal(IServiceProvider serviceProvider)
        {
            Type propertyType;
            if (TargetProperty is DependencyProperty)
            {
                DependencyProperty prop = TargetProperty as DependencyProperty;
                propertyType = prop.PropertyType;
            }
            else // _targetProperty is PropertyInfo
            {
                PropertyInfo pInfo = TargetProperty as PropertyInfo;
                propertyType = pInfo?.PropertyType;
            }

            if (propertyType == typeof(string))
            {
                return CurrentDateTimeString;
            }
           
            return DateTime.Now;
            
        }
    }
}
