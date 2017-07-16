using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace KotturTech.WPFGoodies
{
    /// <summary>
    /// Base class for markup extensions that are capable of updating the target properties
    /// </summary>
    public abstract class UpdatableMarkupExtension : MarkupExtension
    {
        protected object TargetObject { get; private set; }

        protected object TargetProperty { get; private set; }

        public sealed override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (target != null)
            {
                TargetObject = target.TargetObject;
                TargetProperty = target.TargetProperty;
            }

            return ProvideValueInternal(serviceProvider);
        }

        protected void UpdateValue(object value)
        {
            if (TargetObject != null)
            {
                if (TargetProperty is DependencyProperty)
                {
                    DependencyObject obj = TargetObject as DependencyObject;
                    DependencyProperty prop = TargetProperty as DependencyProperty;

                    Action updateAction = () => obj.SetValue(prop, value);

                    // Check whether the target object can be accessed from the
                    // current thread, and use Dispatcher.Invoke if it can't

                    if (obj.CheckAccess())
                        updateAction();
                    else
                        obj.Dispatcher.Invoke(updateAction);
                }
                else // _targetProperty is PropertyInfo
                {
                    PropertyInfo prop = TargetProperty as PropertyInfo;
                    prop.SetValue(TargetObject, value, null);
                }
            }
        }

        /// <summary>
        /// Implement this method for providing the value
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        protected abstract object ProvideValueInternal(IServiceProvider serviceProvider);
    }
}
