using FoxSDC_Common.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSDC_Common
{
    public enum ReportingStatusPictureEnum : int
    {
        None = 0,
        Exclamation = 1,
        Good = 2,
        Bad = 3,
        Help = 4,
        Info = 5,
        Key = 6,
        Pencil = 7,
        Point = 8,
        Questionmark = 9,
        Stop = 10,
        NoKey = 11
    }

    public class ReportingStatusPicture
    {
        public static Image GetPicture(int status)
        {
            return (GetPicture((ReportingStatusPictureEnum)status));
        }

        public static string GetPictureDescription(ReportingStatusPictureEnum status)
        {
            switch (status)
            {
                case ReportingStatusPictureEnum.Bad:
                    return ("Bad");
                case ReportingStatusPictureEnum.Exclamation:
                    return ("Exclamation");
                case ReportingStatusPictureEnum.Good:
                    return ("Good");
                case ReportingStatusPictureEnum.Help:
                    return ("Help");
                case ReportingStatusPictureEnum.Info:
                    return ("Info");
                case ReportingStatusPictureEnum.Key:
                    return ("Key");
                case ReportingStatusPictureEnum.Pencil:
                    return ("Pencil");
                case ReportingStatusPictureEnum.Point:
                    return ("Point");
                case ReportingStatusPictureEnum.Questionmark:
                    return ("Questionmark");
                case ReportingStatusPictureEnum.Stop:
                    return ("Stop");
                case ReportingStatusPictureEnum.NoKey:
                    return ("NoKey");
                default:
                    return ("");
            }
        }

        public static Image GetPicture(ReportingStatusPictureEnum status)
        {
            switch (status)
            {
                case ReportingStatusPictureEnum.Bad:
                    return (Resources.Bad.ToBitmap());
                case ReportingStatusPictureEnum.Exclamation:
                    return (Resources.Exclamation.ToBitmap());
                case ReportingStatusPictureEnum.Good:
                    return (Resources.Good.ToBitmap());
                case ReportingStatusPictureEnum.Help:
                    return (Resources.Help.ToBitmap());
                case ReportingStatusPictureEnum.Info:
                    return (Resources.Info.ToBitmap());
                case ReportingStatusPictureEnum.Key:
                    return (Resources.Key.ToBitmap());
                case ReportingStatusPictureEnum.Pencil:
                    return (Resources.Pencil.ToBitmap());
                case ReportingStatusPictureEnum.Point:
                    return (Resources.Point.ToBitmap());
                case ReportingStatusPictureEnum.Questionmark:
                    return (Resources.Questionmark.ToBitmap());
                case ReportingStatusPictureEnum.Stop:
                    return (Resources.Stop.ToBitmap());
                case ReportingStatusPictureEnum.NoKey:
                    return (Resources.NoKey.ToBitmap());
                default:
                    return (new Bitmap(32, 32));
            }
        }
    }
}
