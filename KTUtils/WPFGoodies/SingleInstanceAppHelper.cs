using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace KotturTech.WPFGoodies
{
    /// <summary>
    /// Class that encapsulates single instance application behavior.
    /// Implementation relies on win32 mutexes
    /// </summary>
    public class SingleInstanceAppHelper
    {
        private readonly string _mutexName;
        private const uint ERROR_ALREADY_EXISTS = 183;

        #region Win32 API helpers

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        [DllImport("User32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        #endregion

        public Exception LastError { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uniqueAppMutexName">Application mutex identifier, used to identify instances of the checked application</param>
        public SingleInstanceAppHelper(string uniqueAppMutexName)
        {
            _mutexName = uniqueAppMutexName;
        }

        /// <summary>
        /// Checks single instance mutex
        /// </summary>
        /// <returns>true if no other instance of this application (Mutex not created), false otherwise</returns>
        public bool CheckSingleInstance()
        {
            CreateEvent(IntPtr.Zero, false, false, _mutexName);
            var err = GetLastError();
            return err != ERROR_ALREADY_EXISTS;
        }

        /// <summary>
        /// Utility function of the class, allows bringing application main window into front
        /// </summary>
        public bool BringAppToFront()
        {
            var currenProc = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(currenProc.ProcessName);
            var procCount = processes.Count();
            var alreadyRunningProc = processes.FirstOrDefault(p => p.Id != currenProc.Id);

            if (procCount > 0 && alreadyRunningProc != null)
            {
                try
                {
                    SwitchToThisWindow(alreadyRunningProc.MainWindowHandle, true);
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex;
                    return false;
                }
            }
            return false;
        }


    }
}
