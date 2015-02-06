/*  SysUtils v1.6.0
 *  Ecrit par J.F.Gratton, 1993 - 2011
 *
 * Time.cs : Classe Time
*/
using System;
using JFG.SysUtils.Properties;

namespace JFG.SysUtils
{
    public class Time
    {
        public uint Jours { get; set; }
        public uint Heures { get; set; }
        public uint Minutes { get; set; }
        public uint Secondes { get; set; }

        //!+    Constructeur par défaut:
        //  Initialise Jours, Heures, Minutes et Secondes à zéro
        public Time()
        {
            Jours = Heures = Minutes = Secondes = 0;
        }

        //!+    Constructeur par Classe
        //  Initialise la classe avec une autre classe Time
        public Time(Time t)
        {
            Jours = t.Jours;
            Heures = t.Heures;
            Minutes = t.Minutes;
            Secondes = t.Secondes;
        }

        //!+    Constructeur numérique
        //  Initialise Jours, Heures, Minutes et Secondes avec des valeurs numériques
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

        //!+    Constructeur par chaîne
        //  Initialise Jours, Heures, Minutes et Secondes avec une chaîne de caractères (string)
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

        //!+    EnHeures ()
        //  Retourne la classe Time() en heures, en arrondissant les minutes à l'heure près
        //  Si bRounded == true
        public int EnHeures(bool bRounded = false)
        {
            int n = (int)(Heures + Jours * 24);
            if (Minutes > 30 && bRounded)
                ++n;
            return n;
        }

        //!+    EnMinutes ()
        //  Retourne la classe Time() en minutes, en arrondissant les secondes à la minute près
        //  Si bRounded == true
        public int EnMinutes(bool bRounded = false)
        {
            int n = (int)(Jours * 1440 + Heures * 60 + Minutes);
            if (Secondes > 30 && bRounded)
                ++n;

            return n;
        }

        //!+    EnSecondes ()
        //  Retourne la classe Time() en secondes
        public long EnSecondes()
        {
            return (long)(Jours * 1440 + Heures * 3600 + Minutes * 60 + Secondes);
        }

        //!+    ToString()
        //  Retourne les valeurs de la classe en chaîne de caractères (string)
        public new string ToString()
        {
            return ToString(new Time(this.Jours, this.Heures, this.Minutes, this.Secondes));
        }

        //!+    ToString (Time t)
        //  Retourne la représentation string d'une classe Time passée en paramètres
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