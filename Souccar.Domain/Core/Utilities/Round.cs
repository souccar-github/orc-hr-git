using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Core.Utilities
{
    public class RoundUtility
    {
        public static double Round(double value, RoundDirection direction, RoundSite site, int digits)
        {
            double result = 0;
            if (site == RoundSite.AfterComma)
            {
                value = value * Math.Pow(10, digits);
                if (direction == RoundDirection.Up)
                {
                    value = Math.Ceiling(value);
                }
                else if (direction == RoundDirection.Down)
                {
                    value = Math.Floor(value);
                }
                else if (direction == RoundDirection.Normal)
                {
                    value = Math.Round(value);
                }
                else if (direction == RoundDirection.None)
                {
                    value = (int)value;
                }
                result = value * Math.Pow(10, -1 * digits);
            }
            else if (site == RoundSite.BeforeComma)
            {
                value = Round(value, direction, RoundSite.AfterComma, 0);
                value = value * Math.Pow(10, -1 * digits);
                if (direction == RoundDirection.Up)
                {
                    value = Math.Ceiling(value);
                }
                else if (direction == RoundDirection.Down)
                {
                    value = Math.Floor(value);
                }
                else if (direction == RoundDirection.Normal)
                {
                    value = Math.Round(value);
                }
                result = value * Math.Pow(10, digits);
            }
            return Double.Parse(result.ToString());
        }

        public static double PreDefinedRoundValue(PreDefinedRound round, double value)
        {
            double result = 0;
            switch (round)
            {
                case PreDefinedRound.ToOne:
                    {
                        result = RoundUtility.Round(value, RoundDirection.Normal, RoundSite.AfterComma, 0);
                        break;
                    }
                case PreDefinedRound.ToTen:
                    {
                        result = RoundUtility.Round(value, RoundDirection.Normal, RoundSite.BeforeComma, 1);
                        break;
                    }
                case PreDefinedRound.DownToOne:
                    {
                        result = RoundUtility.Round(value, RoundDirection.Down, RoundSite.AfterComma, 0);
                        break;
                    }
                case PreDefinedRound.DownToTen:
                    {
                        result = RoundUtility.Round(value, RoundDirection.Down, RoundSite.BeforeComma, 1);
                        break;
                    }
                case PreDefinedRound.UpToOne:
                    {
                        result = RoundUtility.Round(value, RoundDirection.Up, RoundSite.AfterComma, 0);
                        break;
                    }
                case PreDefinedRound.UpToTen:
                    {
                        result = RoundUtility.Round(value, RoundDirection.Up, RoundSite.BeforeComma, 1);
                        break;
                    }
                case PreDefinedRound.WithoutRound:
                    {
                        result = Math.Round(value, 2);
                        break;
                    }
            }
            return result;
        }

    }

    public enum RoundDirection
    {
        Up,
        Down,
        Normal,
        None
    }

    public enum RoundSite
    {
        BeforeComma,
        AfterComma
    }

    public enum PreDefinedRound
    {
        WithoutRound = 0, // بدون تقريب
        ToOne = 6, // لأقرب ليرة اذا كان الفواصل اصغر من نص تقرب للاسفل والعكس تقرب للاعلى
        UpToOne = 1, //  دائما تقرب لأقرب ليرة للأعلى
        DownToOne = 2, // أقرب ليرة للاسفل
        ToTen = 3, // لأقرب عشر ليرات اذا كان الاحاد اصغر من خمسة تقرب للاسفل والعكس تقرب للاعلى
        UpToTen = 4, //  دائما تقرب لأقرب عشر ليرات للأعلى
        DownToTen = 5 // أقرب عشر ليرات للاسفل
    }
}
