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
            Application.Run(new ����());
        }
        public static double K;//ȫ�ֱ���K����������ҽ�
        public static double P;//����Ƕȱպϲ��ϵ��
        public static double Q;//������ȫ���պϲ�ϵ��
         //�ȷ���ת�Ƕ�
        public static double dms2jiaodu(double du,double fen ,double miao)
        {
            du += fen / 60 + miao / 3600;
            return du;
        }
        //�Ƕ�ת����
        public static double dmstohudu(double dms)//�Ƕ������ﲻ����ָ���
        {
            double d, m, s;
            d = Math.Floor(dms);
            m = Math.Floor((dms - d) * 100);
            s = ((dms - d) * 100 - m) * 100;
            return (d + m / 60 + s / 3600) * Math.PI / 180;
        }
       
        //����ת�Ƕ�
        public static double hudutodms(double hudu)//�Ƕ������ﲻ����ָ���
        {
            double d, m, s;
            double du = hudu * 180 / Math.PI;
            d = Math.Floor(du);
            m = Math.Floor((du - d) * 60);
            s = ((du - d) * 60 - m) * 60;
            return Math.Round(d + m / 100 + s / 10000, 4);//��������
        }
   
        //���Ȼ�
        public static double hudutos(double hudu)
        {
            double d, m, s;
            double du = hudu * 180 / Math.PI;
            d = Math.Floor(du);
            m = Math.Floor((du - d) * 60);
            s = Math.Round(((du - d) * 60 - m) * 60, 1);
            return d * 3600 + m * 60 + s;//������0.1��
        }
    
        //���㷽λ��
        public static double fangwei(double x1, double y1, double x2, double y2)//��λ�Ƿ��ؽǶ�ֵ
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
        //�Ƕ�ת�ȷ��� 
        public static string jiaodu2dms(double ang)
        {
            string output= "";
            double d, m, s;
            d = Math.Floor(ang);
            m = Math.Floor((ang - d) * 60);
            s = Math.Round(((ang - d) * 60 - m) * 60);//��Ҫ���浽����
            output = d + "��" + m + "��" + s + "��";
            return output;
        }
        // ��������
        public static void sanjiao(Graphics g, PointF pf)
        {
            //����������ε�ԭ��
            Bitmap bt1 = new Bitmap(20, 20);//����
            PointF[] pfs2 = { new PointF(20, 10), new PointF(1, 0), new PointF(1, 20) };//���ǵ�������
            Graphics g1 = Graphics.FromImage(bt1);
            g1.FillPolygon(Brushes.White, pfs2);//���
            g1.DrawPolygon(new Pen(Color.Black, 1.5f), pfs2);//����
            g.DrawImage((Image)bt1, pf.X-10, pf.Y-10);//ͼ�λ��Ƶ�λ��
        }
        //����ע��
        public static void ziti(Graphics g, PointF pf, string dianhao)
        {
            Bitmap bt2 = new Bitmap(40, 100);
            Graphics g2 = Graphics.FromImage(bt2);
            g2.RotateTransform(90);
            g2.TranslateTransform(0, -30);//����ԭ��λ��
            g2.DrawString(dianhao, new Font("����",10), Brushes.Black, new Point(5, 5));
            g.DrawImage((Image)bt2, pf.X - 25, pf.Y );
        }
        
    }
}
