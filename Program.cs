using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Daoxian
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new 导线());
        }
        public static double K;//全局变量K，代表左角右角
        public static double P;//代表角度闭合差的系数
        public static double Q;//代表导线全长闭合差系数
         //度分秒转角度
        public static double dms2jiaodu(double du,double fen ,double miao)
        {
            du += fen / 60 + miao / 3600;
            return du;
        }
        //角度转弧度
        public static double dmstohudu(double dms)//角度在这里不会出现负数
        {
            double d, m, s;
            d = Math.Floor(dms);
            m = Math.Floor((dms - d) * 100);
            s = ((dms - d) * 100 - m) * 100;
            return (d + m / 60 + s / 3600) * Math.PI / 180;
        }
       
        //弧度转角度
        public static double hudutodms(double hudu)//角度在这里不会出现负数
        {
            double d, m, s;
            double du = hudu * 180 / Math.PI;
            d = Math.Floor(du);
            m = Math.Floor((du - d) * 60);
            s = ((du - d) * 60 - m) * 60;
            return Math.Round(d + m / 100 + s / 10000, 4);//保留到秒
        }
   
        //弧度化
        public static double hudutos(double hudu)
        {
            double d, m, s;
            double du = hudu * 180 / Math.PI;
            d = Math.Floor(du);
            m = Math.Floor((du - d) * 60);
            s = Math.Round(((du - d) * 60 - m) * 60, 1);
            return d * 3600 + m * 60 + s;//保留到0.1秒
        }
    
        //计算方位角
        public static double fangwei(double x1, double y1, double x2, double y2)//方位角返回角度值
        {
            double ang = 0;
            double detax = x2 - x1;
            double detay = y2 - y1;
            double ang1 = Math.Abs(Math.Atan(detay / detax) * (180 / Math.PI));
            if (detax > 0 && detay > 0)
            {
                ang = ang1;
            }
            else if (detax < 0 && detay > 0)
            {
                ang = 180 - ang1;
            }
            else if (detax < 0 && detay < 0)
            {
                ang = 180 + ang1;
            }
            else if (detax > 0 && detay < 0)
            {
                ang = 360 - ang1;
            }
            return ang;
        }
        //角度转度分秒 
        public static string jiaodu2dms(double ang)
        {
            string output= "";
            double d, m, s;
            d = Math.Floor(ang);
            m = Math.Floor((ang - d) * 60);
            s = Math.Round(((ang - d) * 60 - m) * 60);//秒要保存到整数
            output = d + "°" + m + "′" + s + "″";
            return output;
        }
        // 绘制三角
        public static void sanjiao(Graphics g, PointF pf)
        {
            //绘制填充多边形的原理
            Bitmap bt1 = new Bitmap(20, 20);//画板
            PointF[] pfs2 = { new PointF(20, 10), new PointF(1, 0), new PointF(1, 20) };//三角的三个点
            Graphics g1 = Graphics.FromImage(bt1);
            g1.FillPolygon(Brushes.White, pfs2);//填充
            g1.DrawPolygon(new Pen(Color.Black, 1.5f), pfs2);//绘制
            g.DrawImage((Image)bt1, pf.X-10, pf.Y-10);//图形绘制的位置
        }
        //绘制注记
        public static void ziti(Graphics g, PointF pf, string dianhao)
        {
            Bitmap bt2 = new Bitmap(40, 100);
            Graphics g2 = Graphics.FromImage(bt2);
            g2.RotateTransform(90);
            g2.TranslateTransform(0, -30);//划定原点位置
            g2.DrawString(dianhao, new Font("黑体",10), Brushes.Black, new Point(5, 5));
            g.DrawImage((Image)bt2, pf.X - 25, pf.Y );
        }
        
    }
}
