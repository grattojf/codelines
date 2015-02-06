/*  SysUtils v1.6.0
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *
 * Time.cs : Classe Time
*/
using System;

namespace JFG.SysUtils
{
    /// <summary> Time : Time operations (add, sub, etc) </summary>
    public class Time
    {
        /// <summary> Days </summary>
        public uint Jours { get; set; }
        /// <summary> Hours </summary>
        public uint Heures { get; set; }
        /// <summary> Minutes </summary>
        public uint Minutes { get; set; }
        /// <summary> Seconds </summary>
        public uint Secondes { get; set; }

        /// <summary>
        /// Default constructor : initialises all class properties to zero
        /// </summary>
        public Time()
        {
            Jours = Heures = Minutes = Secondes = 0;
        }

        /// <summary>
        /// Second constructor : initialises the class with values from another Time class
        /// </summary>
        /// <param name="t"> The other <see cref="Time"/> class</param>
        public Time(Time t)
        {
            Jours = t.Jours;
            Heures = t.Heures;
            Minutes = t.Minutes;
            Secondes = t.Secondes;
        }
        
        /// <summary>
        /// Third constructor : initialises the class with the following params
        /// </summary>
        /// <param name="j"> Days</param>
        /// <param name="h"> Hours (maximum = 23)"/></param>
        /// <param name="m"> Minutes (maximum = 59)</param>
        /// <param name="s"> Seconds (default = 0, maximum = 59)</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Time(uint j, uint h, uint m, uint s = 0)
        {
            Heures = h;
            if (h > 23)
                throw new ArgumentOutOfRangeException("Heures", "Les heures ne peuvent dépasser 23");
            Minutes = m;
            if (m > 59)
                throw new ArgumentOutOfRangeException("Minutes", "Les minutes ne peuvent dépasser 59");
            Secondes = s;
            if (m > 59)
                throw new ArgumentOutOfRangeException("Secondes", "Les secondes ne peuvent dépasser 59");
            Jours = j;
        }

        /// <summary>
        /// Fourth constructor : initialises the class with a string representing the needed values
        /// TO BE DEPRECATED
        /// </summary>
        /// <param name="s"> The string to initialise the class</param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Time(string s)
        {
            Jours = Heures = Minutes = Secondes = 0;

            uint newval;
            int len;

            //+ Jours
            int ndx = s.IndexOf('.');
            if (ndx > 0)
            {
                if (UInt32.TryParse(sysutils.Left(ndx, s), out newval))
                    Jours = newval;
            }
            s = sysutils.Right(s.Length - ndx - 1, s);

            //+ Heures
            if ((ndx = s.IndexOf(':')) < 1)
                throw new FormatException("Paramètres invalides");

            if (UInt32.TryParse(sysutils.Left(ndx, s), out newval))
                Heures = newval;
            if (Heures > 23)
                throw new ArgumentOutOfRangeException("Heures", "Heures > 23");

            s = sysutils.Right(s.Length - ndx - 1, s);

            //+ Minutes
            if ((ndx = s.IndexOf(':')) == -1)
                ndx = s.Length;

            if (UInt32.TryParse(sysutils.Left(ndx, s), out newval))
                Minutes = newval;
            if (Minutes > 59)
                throw new ArgumentOutOfRangeException("Minutes", "Minutes > 59");

            if ((ndx = s.IndexOf(':')) < 1)
            {
                Secondes = 0;
                return;
            }

            len = s.Length - ndx;
            if (len > 2)
                len = 2;
            if (ndx < 1)
                ndx = 0;
            s = s.Substring(++ndx, len);
            if (UInt32.TryParse(s, out newval))
                Secondes = newval;
            if (Secondes > 59)
                throw new ArgumentOutOfRangeException("Secondes", "Secondes > 59");
        }

        /// <summary>
        /// Converts the <see cref="Time"/> class into hours
        /// </summary>
        /// <param name="bRounded"> Rounds the minutes to the next hour if true</param>
        /// <returns> The number of hours, converted</returns>
        public int EnHeures(bool bRounded = false)
        {
            int n = (int)(Heures + Jours * 24);
            if (Minutes > 30 && bRounded)
                ++n;
            return n;
        }

        /// <summary>
        /// Converts the <see cref="Time"/> class into minutes
        /// </summary>
        /// <param name="bRounded"> Rounds the minutes to the following hour if true</param>
        /// <returns> The number of minutes, converted</returns>
        public int EnMinutes(bool bRounded = false)
        {
            int n = (int)(Jours * 1440 + Heures * 60 + Minutes);
            if (Secondes > 30 && bRounded)
                ++n;

            return n;
        }

        /// <summary>
        /// Converts the whole <see cref="Time"/> class in seconds
        /// </summary>
        /// <returns> The number of seconds in the class</returns>
        public long EnSecondes()
        {
            return (long)(Jours * 1440 + Heures * 3600 + Minutes * 60 + Secondes);
        }

        /// <summary>
        /// UNDOCUMENTED (useless ?)
        /// </summary>
		/// <returns> The string representation</returns>
        public new string ToString()
        {
            return ToString(new Time(this.Jours, this.Heures, this.Minutes, this.Secondes));
        }

        /// <summary>
        /// Returns a <see cref="Time"/> class in its string representation (d.HH:MM:SS)
        /// </summary>
        /// <param name="t"> The Time class to convert in string</param>
        /// <returns> The string representation</returns>
        public static string ToString(Time t)
        {
            string s = "", s2 = "";

            if (t.Jours > 0)
                s2 = t.Jours.ToString() + ".";

            if (t.Heures == 0)
                s = s2 + "00:";
            if (t.Heures < 10 && t.Heures > 0)
                s = s2 + "0" + t.Heures.ToString() + ":";
            if (t.Heures > 9)
                s = s2 + t.Heures.ToString() + ":";

            if (t.Minutes == 0)
                s += "00:";
            if (t.Minutes < 10 && t.Minutes > 0)
                s += "0" + t.Minutes.ToString() + ":";
            if (t.Minutes > 9)
                s += t.Minutes.ToString() + ":";

            if (t.Secondes == 0)
                s += "00";
            if (t.Secondes < 10 && t.Secondes > 0)
                s += "0" + t.Secondes.ToString();
            if (t.Secondes > 9)
                s += t.Secondes.ToString();

            return s;
        }
    }
}