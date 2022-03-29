using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace JCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);

        }


        /**  Gregorian & Jalali (Hijri_Shamsi,Solar) Date Converter Functions
Author: JDF.SCR.IR =>> Download Full Version :  http://jdf.scr.ir/jdf
License: GNU/LGPL _ Open Source & Free :: Version: 2.80 : [2020=1399]
---------------------------------------------------------------------
355746=361590-5844 & 361590=(30*33*365)+(30*8) & 5844=(16*365)+(16/4)
355666=355746-79-1 & 355668=355746-79+1 &  1595=605+990 &  605=621-16
990=30*33 & 12053=(365*33)+(32/4) & 36524=(365*100)+(100/4)-(100/100)
1461=(365*4)+(4/4) & 146097=(365*400)+(400/4)-(400/100)+(400/400)  */

        public static int[] gregorian_to_jalali(int gy, int gm, int gd)
        {
            int[] g_d_m = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            int gy2 = (gm > 2) ? (gy + 1) : gy;
            int days = 355666 + (365 * gy) + ((int)((gy2 + 3) / 4)) - ((int)((gy2 + 99) / 100)) + ((int)((gy2 + 399) / 400)) + gd + g_d_m[gm - 1];
            int jy = -1595 + (33 * ((int)(days / 12053)));
            days %= 12053;
            jy += 4 * ((int)(days / 1461));
            days %= 1461;
            if (days > 365)
            {
                jy += (int)((days - 1) / 365);
                days = (days - 1) % 365;
            }
            int jm = (days < 186) ? 1 + (int)(days / 31) : 7 + (int)((days - 186) / 30);
            int jd = 1 + ((days < 186) ? (days % 31) : ((days - 186) % 30));
            int[] jalali = { jy, jm, jd };
            return jalali;
        }

        public static int[] jalali_to_gregorian(int jy, int jm, int jd)
        {
            jy += 1595;
            int days = -355668 + (365 * jy) + (((int)(jy / 33)) * 8) + ((int)(((jy % 33) + 3) / 4)) + jd + ((jm < 7) ? (jm - 1) * 31 : ((jm - 7) * 30) + 186);
            int gy = 400 * ((int)(days / 146097));
            days %= 146097;
            if (days > 36524)
            {
                gy += 100 * ((int)(--days / 36524));
                days %= 36524;
                if (days >= 365) days++;
            }
            gy += 4 * ((int)(days / 1461));
            days %= 1461;
            if (days > 365)
            {
                gy += (int)((days - 1) / 365);
                days = (days - 1) % 365;
            }
            int gd = days + 1;
            int[] sal_a = { 0, 31, ((gy % 4 == 0 && gy % 100 != 0) || (gy % 400 == 0)) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int gm;
            for (gm = 0; gm < 13 && gd > sal_a[gm]; gm++) gd -= sal_a[gm];
            int[] gregorian = { gy, gm, gd };
            return gregorian;
        }
        public static string jToday()
        {
            DateTime today = DateTime.Now;
            int year = today.Year;
            int month = today.Month;
            int day = today.Day;
            int[] tarikh_j = gregorian_to_jalali(year, month, day);
            return (tarikh_j[0] + "/" + tarikh_j[1] + "/" + tarikh_j[2]);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.jDate_LBL.Content = jToday();
            Timer newTimer = new Timer();
            newTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            newTimer.Interval = 6000;
            newTimer.AutoReset = true;
            newTimer.Enabled = true;
        }
        public void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                jDate_LBL.Content = jToday();
            });
        }
    }

}
