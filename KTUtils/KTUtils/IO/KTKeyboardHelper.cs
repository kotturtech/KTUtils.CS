using System.Windows.Input;
namespace KotturTech.IO
{
    /// <summary>
    /// Keyboard input helper class
    /// </summary>
    public static class KTKeyboardHelper
    {
        /// <summary>
        /// Indicates whether the specified key is alphanumeric
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns>True if key is alphanumeric</returns>
        public static bool IsAlphaNumericKey(Key k)
        {
            if(k == Key.A || k == Key.B || k == Key.C || k == Key.D
                || k == Key.E || k == Key.F || k == Key.G || k == Key.H
                || k == Key.I || k == Key.J || k == Key.K || k == Key.L
                || k == Key.M || k == Key.N || k == Key.O || k == Key.P
                || k == Key.Q || k == Key.R || k == Key.S || k == Key.T
                || k == Key.U || k == Key.V || k == Key.W || k == Key.X
                || k == Key.Y || k == Key.Z)
                return true;
            if(k == Key.D0 || k == Key.D1 || k == Key.D2 || k == Key.D3 || k == Key.D4 || k == Key.D5
                || k == Key.D6 || k == Key.D7 || k == Key.D8 || k == Key.D9)
                return true;
            if(k == Key.NumPad0 || k == Key.NumPad1 || k == Key.NumPad2 || k == Key.NumPad3 || k == Key.NumPad4 || k == Key.NumPad5
               || k == Key.NumPad6 || k == Key.NumPad7 || k == Key.NumPad8 || k == Key.NumPad9)
                return true;

            return false;

        }
    }
}
