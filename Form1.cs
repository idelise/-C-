using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;//Excel表格
using System.Drawing.Drawing2D;//绘图
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Daoxian
{

    public partial class 导线 : Form
    {
        //初始化数据
        List<string> dianhao;//点号
        List<double> X;//X坐标
        List<double> Y;//Y坐标
        List<double> right;//盘左观测
        List<double> left;//盘右观测
        List<double> guancejiao;//计算后观测角
        List<double> guancejiao_1;//平差后观测角
        List<double> juli;//初始录入的距离
        List<double> juli1;//平差后的距离
        List<double> fangweijiao;//方位角
        List<double> jiaogaizheng;//角度改正数
        List<double> jiaogaihou;//改正后角值
        List<double> deltaX;//X坐标增量
        List<double> deltaY;//Y坐标增量
        List<double> Xgaizheng;//X坐标改正值
        List<double> Ygaizheng;//Y坐标改正值
        List<double> Xgaihou;//改正后X坐标增量
        List<double> Ygaihou;//改正后Y坐标增量
        List<string> wenben1;//输入时储存的文本
        double jiaoduBHC;//角度闭合差
        double XBHC;//X坐标增量闭合差
        double YBHC;//Y坐标增量闭合差
        double BeginAng;
        double EndAng;
        Bitmap image;
        //创建数组
        public void chushihua()
        {
            dianhao = new List<string>();
            deltaX = new List<double>();
            deltaY = new List<double>();
            guancejiao = new List<double>();
            guancejiao_1 = new List<double>();
            juli = new List<double>();
            fangweijiao = new List<double>();
            jiaogaizheng = new List<double>();
            jiaogaihou = new List<double>();
            deltaX = new List<double>();
            deltaY = new List<double>();
            Xgaizheng = new List<double>();
            Ygaizheng = new List<double>();
            Xgaihou = new List<double>();
            Ygaihou = new List<double>();
            wenben1 = new List<string>();
            juli1 = new List<double>();
            X = new List<double>();
            Y = new List<double>();

        }
        public 导线()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            chushihua();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox18.Text = "";
            初始数据.Text = "";
            //初始化当前用来装数据的list
            right = new List<double>();
            left = new List<double>();
            //声明一个数组存储当前所需的度分秒
            double[][] dufenmiao = new double[8][];
            string[] hang = new string[8];
            try
            {
                //把所有行录入
                hang[0] = 左1.Text.ToString();
                hang[1] = 左2.Text.ToString();
                hang[2] = 左3.Text.ToString();
                hang[3] = 左4.Text.ToString();
                hang[4] = 右1.Text.ToString();
                hang[5] = 右2.Text.ToString();
                hang[6] = 右3.Text.ToString();
                hang[7] = 右4.Text.ToString();
                //把数据转化成以度为单位
                for (int j = 0; j < 8; j++)
                {
                    dufenmiao[j] = new double[3];
                    string[] hangfen = new string[3];
                    hangfen = hang[j].Split(' ');
                    //把分好的字符串添加到储存度分秒的数组中
                    for (int i = 0; i < 3; i++)
                    {
                        dufenmiao[j][i] = Convert.ToDouble(hangfen[i]);
                    }
                }
                left.Add(Program.dms2jiaodu(dufenmiao[0][0], dufenmiao[0][1], dufenmiao[0][2]));//将盘左角度制存入list
                left.Add(Program.dms2jiaodu(dufenmiao[1][0], dufenmiao[1][1], dufenmiao[1][2]));
                left.Add(Program.dms2jiaodu(dufenmiao[2][0], dufenmiao[2][1], dufenmiao[2][2]));
                left.Add(Program.dms2jiaodu(dufenmiao[3][0], dufenmiao[3][1], dufenmiao[3][2]));
                right.Add(Program.dms2jiaodu(dufenmiao[4][0], dufenmiao[4][1], dufenmiao[4][2]));//将盘右角度制存入list
                right.Add(Program.dms2jiaodu(dufenmiao[5][0], dufenmiao[5][1], dufenmiao[5][2]));
                right.Add(Program.dms2jiaodu(dufenmiao[6][0], dufenmiao[6][1], dufenmiao[6][2]));
                right.Add(Program.dms2jiaodu(dufenmiao[7][0], dufenmiao[7][1], dufenmiao[7][2]));
                double guancejiaoji; double dajiao = 0, xiaojiao = 0;//累加的角
                //判断是否需要把后面的角度加360
                for (int i = 0; i < left.Count / 2; i++)
                {
                    if (left[2 * i] > left[2 * i + 1])
                    {
                        left[2 * i + 1] += 360;
                    }
                    if (right[2 * i] > right[2 * i + 1])
                    {
                        right[2 * i + 1] += 360;
                    }
                }
                dajiao = left[1] + left[3] + right[1] + right[3];
                xiaojiao = left[0] + left[2] + right[0] + right[2];
                guancejiaoji = Math.Round((dajiao - xiaojiao) / 4, 4);//指定四位小数
                guancejiao.Add(guancejiaoji);//未分配误差的观测角
                dianhao.Add(richTextBox1.Text);//把当前点号录入list                
                double juli_1 = 0, juli_2 = 0;
                //把距离存入数组
                juli_1 = Convert.ToDouble(观测1.Text);
                juli_2 = Convert.ToDouble(观测2.Text);
                juli.Add(juli_1);
                juli.Add(juli_2);
            }
            catch
            {
                MessageBox.Show("请输入正确的数据");
            }
            //初始坐标的设置,使用点对象来存取坐标
            Point3d begin1 = new Point3d();
            Point3d begin2 = new Point3d();
            Point3d end1 = new Point3d();
            Point3d end2 = new Point3d();
            begin1.X = Convert.ToDouble(Kown1X.Text);
            begin1.Y = Convert.ToDouble(Kown1Y.Text);
            begin2.X = Convert.ToDouble(Kown2X.Text);
            begin2.Y = Convert.ToDouble(Kown2Y.Text);
            end1.X = Convert.ToDouble(Kown3X.Text);
            end1.Y = Convert.ToDouble(Kown3Y.Text);
            end2.X = Convert.ToDouble(Kown4X.Text);
            end2.Y = Convert.ToDouble(Kown4Y.Text);
            //计算初始方位角和最后方位角
            BeginAng = Program.fangwei(begin1.X, begin1.Y, begin2.X, begin2.Y);
            EndAng = Program.fangwei(end1.X, end1.Y, end2.X, end2.Y);
            //把点的坐标存入
            X.Add(begin1.X);
            X.Add(begin2.X);
            X.Add(end1.X);
            X.Add(end2.X);
            Y.Add(begin1.Y);
            Y.Add(begin2.Y);
            Y.Add(end1.Y);
            Y.Add(end2.Y);
            //写入当前数据进入显示文本框并清空输入文本框
            string dangqian = "测站" + " " + richTextBox1.Text + "\t" + "盘左" + "\t" + "盘右" + "\n";
            for (int i = 0; i < 4; i++)
            {
                dangqian += "\t" + dufenmiao[i][0] + "°" + dufenmiao[i][1] + "′" + dufenmiao[i][2] + "″" + "\t" + dufenmiao[i + 4][0] + "°" + dufenmiao[i + 4][1] + "′" + dufenmiao[i + 4][2] + "″" + "\n";//
            }
            //把当前已知的观测角和距离输出
            初始数据.Text = "";
            for (int i = 0; i < dianhao.Count - 1; i++)
            {
                初始数据.Text += dianhao[i] + " " + Program.jiaodu2dms(guancejiao[i]) + "  " + juli[2 * i] + "  " + juli[2 * i + 1] + "\n";
            }
            wenben1.Add(dangqian);//存入事先定义好的文本list
            //显示文本框状态更新
            for (int i = 0; i < wenben1.Count; i++)
            {
                richTextBox18.Text += wenben1[i];
            }
            //清空文本框
            左1.Text = "";
            左2.Text = "";
            左3.Text = "";
            左4.Text = "";
            右1.Text = "";
            右2.Text = "";
            右3.Text = "";
            右4.Text = "";
            richTextBox2.Text = "";
            richTextBox6.Text = "";
            richTextBox7.Text = "";
            richTextBox8.Text = "";
            richTextBox16.Text = "";
            richTextBox17.Text = "";
            观测1.Text = "";
            观测2.Text = "";
            richTextBox1.Text = "";
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //存储观测角和距离数据
            //判断个数n
            int n = dianhao.Count;//输入点号的个数即为总的个数
            string[] lines = new string[1 + 4 + n];//根据n设定行数 1个计数 4个已知点 n个测站
            lines[0] = n.ToString();//存入点号计数器
            //存已知点坐标
            lines[1] = "已知点1" + "  " + Kown1X.Text + "  " + Kown1Y.Text;
            lines[2] = "已知点2" + "  " + Kown2X.Text + "  " + Kown2Y.Text;
            lines[3] = "已知点3" + "  " + Kown3X.Text + "  " + Kown3Y.Text;
            lines[4] = "已知点4" + "  " + Kown4X.Text + "  " + Kown4Y.Text;
            int i = 5;
            bool p = true;
            while (p)//将要导出的数据依次存入string[]
            {
                lines[i] = dianhao[i - 5] + "  " + Program.jiaodu2dms(guancejiao[i - 5]) + "  " + juli[2 * (i - 5)] + "  " + juli[2 * (i - 5) + 1];//最后要加一个距离来输出
                i++;
                if (i - 5 == n)
                {
                    p = false;
                }
            }
            //接下来进行输出
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;//是否记住上次打开的路径
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false);
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
                file.Close();
                MessageBox.Show("保存成功");
            }

        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chushihua();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = " 请选择您要导入的模板文件：";
            openFileDialog.Filter = "TextDocument(*.txt)|*.txt";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")//判断文件名是否为空
            {
                StreamReader streamReader = new StreamReader(openFileDialog.FileName, Encoding.Default);
                this.基本数据.Text = streamReader.ReadToEnd();
            }
            //导入成功后把数据读入程序
            //先读取第一行看需要几行
            StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.Default);
            int num = Convert.ToInt32(sr.ReadLine());
            //再建立需要的字符串
            string[] luru = new string[num + 5];
            for (int i = 0; i < num + 4; i++)
            {
                luru[i] = sr.ReadLine();
            }
            //读入并存入初始坐标
            double[] zuobiao = new double[8];//临时存放坐标数据
            string[] split = new string[3];//转数据用的
            for (int i = 0; i < 4; i++)
            {
                split = luru[i].Split("  ");
                zuobiao[2 * i] = Convert.ToDouble(split[1]);
                zuobiao[2 * i + 1] = Convert.ToDouble(split[2]);
            }
            Point3d begin1 = new Point3d();
            Point3d begin2 = new Point3d();
            Point3d end1 = new Point3d();
            Point3d end2 = new Point3d();
            begin1.X = zuobiao[0];
            begin1.Y = zuobiao[1];
            begin2.X = zuobiao[2];
            begin2.Y = zuobiao[3];
            end1.X = zuobiao[4];
            end1.Y = zuobiao[5];
            end2.X = zuobiao[6];
            end2.Y = zuobiao[7];
            X.Add(begin1.X);
            X.Add(begin2.X);
            X.Add(end1.X);
            X.Add(end2.X);
            Y.Add(begin1.Y);
            Y.Add(begin2.Y);
            Y.Add(end1.Y);
            Y.Add(end2.Y);
            //计算初始方位角和最后方位角
            BeginAng = Program.fangwei(begin1.X, begin1.Y, begin2.X, begin2.Y);
            EndAng = Program.fangwei(end1.X, end1.Y, end2.X, end2.Y);
            //读取观测角数据和距离数据
            string[] Guancejiao = new string[num];//存观测角的string
            string[] guancejiao1 = new string[3];//存储观测角度分秒的double
            for (int i = 0; i < num; i++)
            {
                split = luru[4 + i].Split("  ");
                dianhao.Add(split[0]);
                Guancejiao[i] = split[1];
                juli.Add(Convert.ToDouble(split[2]));
                juli.Add(Convert.ToDouble(split[3]));
                for (int j = 0; j < 2; j++)
                {
                    char[] separating = { '°', '′', '″' };
                    guancejiao1 = Guancejiao[i].Split(separating);
                }
                guancejiao.Add(Convert.ToDouble(guancejiao1[0]) + Convert.ToDouble(guancejiao1[1]) / 60 + Convert.ToDouble(guancejiao1[2]) / 3600);

            }

        }

        private void 导出的数据_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void 基本数据_TextChanged(object sender, EventArgs e)
        {

        }

        private void 存储输入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //存储观测角和距离数据
            //判断个数n
            int n = dianhao.Count;//输入点号的个数即为总的个数
            string[] lines = new string[1 + 4 + n];//根据n设定行数 1个计数 4个已知点 n个测站
            lines[0] = n.ToString();//存入点号计数器
            //存已知点坐标
            lines[1] = "已知点1" + "  " + Kown1X.Text + "  " + Kown1Y.Text;
            lines[2] = "已知点2" + "  " + Kown2X.Text + "  " + Kown2Y.Text;
            lines[3] = "已知点3" + "  " + Kown3X.Text + "  " + Kown3Y.Text;
            lines[4] = "已知点4" + "  " + Kown4X.Text + "  " + Kown4Y.Text;
            int i = 5;
            bool p = true;
            if (dianhao.Count == 0)
            {
                MessageBox.Show("请输入正确的数据");
            }
            while (p)//将要导出的数据依次存入string[]
            {
                lines[i] = dianhao[i - 5] + "  " + Program.jiaodu2dms(guancejiao[i - 5]) + "  " + juli[2 * (i - 5)] + "  " + juli[2 * (i - 5) + 1];//最后要加一个距离来输出
                i++;
                if (i - 5 == n)
                {
                    p = false;
                }
            }
            //接下来进行输出
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;//是否记住上次打开的路径
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false);
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
                file.Close();
                MessageBox.Show("保存成功");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //打开窗体二以设置参数
            Form2 f = new Form2();
            f.ShowDialog();

            for (int i = 0; i < juli.Count / 2 - 1; i++)
            {
                juli1.Add(Math.Round((juli[2 * i + 1] + juli[2 * i + 2]) / 2, 3));
            }
            dataGridView1.Rows.Add(dianhao.Count*3+1);
            //已知数据导出
            dataGridView1.Rows[0].Cells[0].Value = "定向点1";
            dataGridView1.Rows[guancejiao.Count * 2 + 2].Cells[0].Value = "定向点2";
            dataGridView1.Rows[guancejiao.Count * 2 + 3].Cells[1].Value = "角度和:"+Program.jiaodu2dms(guancejiao.Sum());
            for (int i = 0; i < dianhao.Count; i++)//点号
            {
                dataGridView1.Rows[2 * i + 2].Cells[0].Value = dianhao[i];
            }
            for (int i = 0; i < guancejiao.Count; i++)//观测角
            {
                dataGridView1.Rows[2 * i + 2].Cells[1].Value = Program.jiaodu2dms(guancejiao[i]);
            }
            for (int i = 0; i < juli.Count / 2 - 1; i++)//距离
            {
                dataGridView1.Rows[2 * i + 3].Cells[5].Value = juli1[i];
            }
            dataGridView1.Rows[2 * (juli.Count / 2 - 1) + 4].Cells[4].Value = "Sum:";
            dataGridView1.Rows[2 * (juli.Count / 2 - 1) + 4].Cells[5].Value =juli1.Sum();
            //首先依次计算方位角
            List<double> fangwei1 = new List<double>();
            fangweijiao.Add(BeginAng);
            fangwei1.Add(BeginAng);
            double n = 0;//计算大于360或者小于0的值的累积和
            for (int i = 0; i < guancejiao.Count; i++)
            {
                double a = fangwei1[i] +Program .K * guancejiao[i] -Program.K * 180;
                if (a > 360)
                {
                    a = a - 360;
                    n = n - 360;
                }
                else if (a < 0)
                {
                    a = a + 360;
                    n = n + 360;
                }
                fangwei1.Add(a);
            }

            jiaoduBHC = fangweijiao[0] + Program.K* guancejiao.Sum() - EndAng - Program.K  * 180*(guancejiao.Count) + n;//观测值减去真实值
            if (jiaoduBHC * 3600 > Program .P  * Math.Sqrt(guancejiao.Count))//限差设置为40倍的根号n
            {
                MessageBox.Show("角度闭合差超限！！！");
            }
            //下面对角度改正值做计算
            //因为可能出现余数的情况，所以先对其进行取整
            jiaoduBHC = Math.Round(jiaoduBHC * 3600);//将角度闭合差转换成秒并取整
            for (int i = 0; i < guancejiao.Count; i++)
            {
                jiaogaizheng.Add(0);//创建指定个数的改正数
            }
            double bhc = -(jiaoduBHC % guancejiao.Count);
            for (int i = 0; i < guancejiao.Count; i++)
            {
                jiaogaizheng[i] = (-jiaoduBHC - bhc) / guancejiao.Count;
            }
            if (jiaoduBHC % guancejiao.Count != 0)
            {
                for (int i = 0; i < bhc; i++)//把多一的部分一个一个赋值给改正数
                {
                    jiaogaizheng[i] += 1;
                }
            }
            //方位角计算
            for (int i = 0; i < guancejiao.Count; i++)
            {
                jiaogaihou.Add(Program.K*guancejiao[i] + jiaogaizheng[i] / 3600);
                double a = fangweijiao[i] + Program.K* jiaogaihou[i] - Program.K*180;
                if (a > 360)
                {
                    a = a - 360;
                }
                else if (a < 0)
                {
                    a = a + 360;
                }
                fangweijiao.Insert(i + 1, a);
            }

            for (int i = 0; i < guancejiao.Count; i++)
            {
                dataGridView1.Rows[2 * i + 2].Cells[2].Value = jiaogaizheng[i] + "″";//改正数
                dataGridView1.Rows[2 * i + 2].Cells[3].Value = Program.jiaodu2dms(jiaogaihou[i]);//改正后角值
            }
            dataGridView1.Rows[guancejiao.Count * 2 + 1].Cells[2].Value ="fβ：" + jiaoduBHC + "″";//角度闭合差显示相反数
            dataGridView1.Rows[guancejiao.Count * 2 + 3].Cells[3].Value = "角度和:" + Program.jiaodu2dms(jiaogaihou.Sum());
            //把方位角写入表格
            for (int i = 0; i < fangweijiao.Count; i++)
            {
                dataGridView1.Rows[2 * i + 1].Cells[4].Value = Program.jiaodu2dms(fangweijiao[i]);//方位角
            }


            //接下来计算坐标增量
            for (int i = 0; i < juli1.Count; i++)
            {
                deltaX.Add(juli1[i] * Math.Cos(fangweijiao[i + 1] * Math.PI / 180));
                deltaY.Add(juli1[i] * Math.Sin(fangweijiao[i + 1] * Math.PI / 180));
            }
            XBHC = deltaX.Sum() - (X[2] - X[1]);//观测值减去真实值,由于前面依次存的定向点——已知点——已知点——定向点
            YBHC = deltaY.Sum() - (Y[2] - Y[1]);
            double aa = Math.Sqrt((XBHC * XBHC + YBHC * YBHC)) / juli1.Sum();
            if (aa > Program.Q)//限差设置为1/4000
            {
                MessageBox.Show("导线全长闭合差超限！");
            }
            for (int i = 0; i < juli1.Count; i++)
            {
                Xgaizheng.Add(XBHC * juli1[i] / juli1.Sum());
                Ygaizheng.Add(YBHC * juli1[i] / juli1.Sum());
                Xgaihou.Add(deltaX[i] - Xgaizheng[i]);
                Ygaihou.Add(deltaY[i] - Ygaizheng[i]);
            }

            for (int i = 0; i < deltaX.Count; i++)
            {
                dataGridView1.Rows[2*i + 3].Cells[6].Value =string.Format ("{0:0.000}",Math.Round(deltaX[i], 4));//坐标增量
                dataGridView1.Rows[2*i + 3].Cells[7].Value = string.Format ("{0:0.000}",Math.Round(deltaY[i], 4));
                dataGridView1.Rows[2*i + 3].Cells[8].Value =string .Format ("{0:0.000}" ,Math.Round(Xgaizheng[i], 4) * 100);//坐标增量改正数
                dataGridView1.Rows[2*i + 3].Cells[9].Value =string.Format ("{0:0.000}", Math.Round(Ygaizheng[i], 4) * 100);
                dataGridView1.Rows[2*i + 3].Cells[10].Value =string.Format ("{0:0.000}", Math.Round(Xgaihou[i], 4));//改后坐标增量
                dataGridView1.Rows[2*i + 3].Cells[11].Value =string.Format ("{0:0.000}", Math.Round(Ygaihou[i], 4));
            }
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[6].Value =string.Format ("{0:0.000}", Math.Round(deltaX.Sum(), 4));//坐标增量之和
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[7].Value =string.Format ("{0:0.000}", Math.Round(deltaY.Sum(), 4));
            dataGridView1.Rows[2*deltaX.Count + 4].Cells[8].Value =string .Format ("{0:0.000}", Math.Round(XBHC, 4) * 100);//坐标增量闭合差
            dataGridView1.Rows[2*deltaX.Count + 4].Cells[9].Value = string.Format ("{0:0.000}",Math.Round(YBHC, 4) * 100);

            //计算坐标
            for (int i = 0; i < Xgaihou.Count - 1; i++)//坐标计算c点多出一个，所以-1，不用判断，计算机不会算错
            {
                X.Insert(i + 2, Xgaihou[i] + X[i+1]);
                Y.Insert(i + 2, Ygaihou[i] + Y[i+1]);
            }
            for (int i = 0; i < X.Count-2; i++)
            {
                dataGridView1.Rows[2 * i + 2].Cells[12].Value = string.Format("{0:0.000}", Math.Round(X[i + 1], 4)); ;
                dataGridView1.Rows[2*i+2].Cells[13].Value = string.Format ("{0:0.000}",Math.Round(Y[i+1], 4));
            }
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[10].Value ="fx=" + -Math.Round(Xgaihou.Sum(), 4);//改正后坐标增量
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[11].Value ="fy=" + -Math.Round(Ygaihou.Sum(), 4);

            Pen p = new Pen(Color.Black, 2.5f);
            Pen p1 = new Pen(Color.Red, 3);
            Pen p2 = new Pen(Color.Blue, 2);
            image = new Bitmap((int)(Y.Max() - Y.Min()) + 300, (int)(X.Max() - X.Min()) + 400);//显示图形范围
            Graphics g = Graphics.FromImage(image);
            g.RotateTransform(-90);//旋转为测量坐标系
            g.TranslateTransform(-(int)(X.Max() + 200), -(int)Y.Min() + 200);//划定原点位置
            PointF[] pf = new PointF[X.Count];
            //线形绘制
            for (int i = 0; i < X.Count; i++)
            {
                pf[i].X = (float)X[i];
                pf[i].Y = (float)Y[i];
            }
            g.DrawLines(p, pf);


            //注记双线
            float[] single = { 0, 0.25f, 0.75f, 1 };
            p1.CompoundArray = single;
            g.DrawLine(p1, pf[0], pf[1]);
            g.DrawLine(p1, pf[pf.Length - 2], pf[pf.Length - 1]);

            //绘制三角
            Program.sanjiao(g, pf[0]);
            Program.sanjiao(g, pf[1]);
            Program.sanjiao(g, pf[pf.Length - 2]);
            Program.sanjiao(g, pf[pf.Length - 1]);

            //绘制字体
            for (int i = 0; i < dianhao.Count; i++)
            {
                Program.ziti(g, pf[i + 1], dianhao[i].ToString());
            }
            GraphicsState gstate = g.Save();
            g.ResetTransform();
            g.Restore(gstate);

            //绘制箭头
            AdjustableArrowCap cap = new AdjustableArrowCap(10, 20);
            p.CustomEndCap = cap;
            g.DrawLine(p, pf[0].X - 150, pf[1].Y - 50, pf[0].X + 150, pf[1].Y - 50);//x轴
            g.DrawLine(p, pf[0].X - 150, pf[1].Y - 50, pf[0].X - 150, pf[1].Y + 250);//y轴
            Program.ziti(g, new PointF(pf[0].X + 150 , pf[0].Y - 50 - 10), "X(m)");
            Program.ziti(g, new PointF(pf[0].X - 150 - 25, pf[0].Y + 250), "Y(m)");
            Program.ziti(g, new PointF(pf[0].X - 150 - 10, pf[0].Y - 50 ), "O");
            pictureBox1.Image = (Image)image;

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void 平差ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //打开窗体二以设置参数
            Form2 f = new Form2();
            f.ShowDialog();

            for (int i = 0; i < juli.Count / 2 - 1; i++)
            {
                juli1.Add(Math.Round((juli[2 * i + 1] + juli[2 * i + 2]) / 2, 3));
            }
            dataGridView1.Rows.Add(dianhao.Count * 3 + 1);
            //已知数据导出
            dataGridView1.Rows[0].Cells[0].Value = "定向点1";
            dataGridView1.Rows[guancejiao.Count * 2 + 2].Cells[0].Value = "定向点2";
            dataGridView1.Rows[guancejiao.Count * 2 + 3].Cells[1].Value = "角度和:" + Program.jiaodu2dms(guancejiao.Sum());
            for (int i = 0; i < dianhao.Count; i++)//点号
            {
                dataGridView1.Rows[2 * i + 2].Cells[0].Value = dianhao[i];
            }
            for (int i = 0; i < guancejiao.Count; i++)//观测角
            {
                dataGridView1.Rows[2 * i + 2].Cells[1].Value = Program.jiaodu2dms(guancejiao[i]);
            }
            for (int i = 0; i < juli.Count / 2 - 1; i++)//距离
            {
                dataGridView1.Rows[2 * i + 3].Cells[5].Value = juli1[i];
            }
            dataGridView1.Rows[2 * (juli.Count / 2 - 1) + 4].Cells[4].Value = "Sum:";
            dataGridView1.Rows[2 * (juli.Count / 2 - 1) + 4].Cells[5].Value = juli1.Sum();
            //首先依次计算方位角
            List<double> fangwei1 = new List<double>();
            fangweijiao.Add(BeginAng);
            fangwei1.Add(BeginAng);
            double n = 0;//计算大于360或者小于0的值的累积和
            for (int i = 0; i < guancejiao.Count; i++)
            {
                double a = fangwei1[i] + Program.K * guancejiao[i] - Program.K * 180;
                if (a > 360)
                {
                    a = a - 360;
                    n = n - 360;
                }
                else if (a < 0)
                {
                    a = a + 360;
                    n = n + 360;
                }
                fangwei1.Add(a);
            }

            jiaoduBHC = fangweijiao[0] + Program.K * guancejiao.Sum() - EndAng - Program.K * 180 * (guancejiao.Count) + n;//观测值减去真实值
            if (jiaoduBHC * 3600 > Program.P * Math.Sqrt(guancejiao.Count))//限差设置为40倍的根号n
            {
                MessageBox.Show("角度闭合差超限！！！");
            }
            //下面对角度改正值做计算
            //因为可能出现余数的情况，所以先对其进行取整
            jiaoduBHC = Math.Round(jiaoduBHC * 3600);//将角度闭合差转换成度并取整
            for (int i = 0; i < guancejiao.Count; i++)
            {
                jiaogaizheng.Add(0);//创建指定个数的改正数
            }
            double bhc = -(jiaoduBHC % guancejiao.Count);
            for (int i = 0; i < guancejiao.Count; i++)
            {
                jiaogaizheng[i] = (-jiaoduBHC - bhc) / guancejiao.Count;
            }
            if (jiaoduBHC % guancejiao.Count != 0)
            {
                for (int i = 0; i < bhc; i++)//把多一的部分一个一个赋值给改正数
                {
                    jiaogaizheng[i] += 1;
                }
            }
            //方位角计算
            for (int i = 0; i < guancejiao.Count; i++)
            {
                jiaogaihou.Add(Program.K * guancejiao[i] + jiaogaizheng[i] / 3600);
                double a = fangweijiao[i] + Program.K * jiaogaihou[i] - Program.K * 180;
                if (a > 360)
                {
                    a = a - 360;
                }
                else if (a < 0)
                {
                    a = a + 360;
                }
                fangweijiao.Insert(i + 1, a);
            }

            for (int i = 0; i < guancejiao.Count; i++)
            {
                dataGridView1.Rows[2 * i + 2].Cells[2].Value = jiaogaizheng[i] + "″";//改正数
                dataGridView1.Rows[2 * i + 2].Cells[3].Value = Program.jiaodu2dms(jiaogaihou[i]);//改正后角值
            }
            dataGridView1.Rows[guancejiao.Count * 2 + 1].Cells[2].Value = "fβ：" + jiaoduBHC + "″";//角度闭合差显示相反数
            dataGridView1.Rows[guancejiao.Count * 2 + 3].Cells[3].Value = "角度和:" + Program.jiaodu2dms(jiaogaihou.Sum());
            //把方位角写入表格
            for (int i = 0; i < fangweijiao.Count; i++)
            {
                dataGridView1.Rows[2 * i + 1].Cells[4].Value = Program.jiaodu2dms(fangweijiao[i]);//方位角
            }


            //接下来计算坐标增量
            for (int i = 0; i < juli1.Count; i++)
            {
                deltaX.Add(juli1[i] * Math.Cos(fangweijiao[i + 1] * Math.PI / 180));
                deltaY.Add(juli1[i] * Math.Sin(fangweijiao[i + 1] * Math.PI / 180));
            }
            XBHC = deltaX.Sum() - (X[2] - X[1]);//观测值减去真实值,由于前面依次存的定向点——已知点——已知点——定向点
            YBHC = deltaY.Sum() - (Y[2] - Y[1]);
            double aa = Math.Sqrt((XBHC * XBHC + YBHC * YBHC)) / juli1.Sum();
            if (aa > Program.Q)//限差设置为1/4000
            {
                MessageBox.Show("导线全长闭合差超限！");
            }
            for (int i = 0; i < juli1.Count; i++)
            {
                Xgaizheng.Add(XBHC * juli1[i] / juli1.Sum());
                Ygaizheng.Add(YBHC * juli1[i] / juli1.Sum());
                Xgaihou.Add(deltaX[i] - Xgaizheng[i]);
                Ygaihou.Add(deltaY[i] - Ygaizheng[i]);
            }

            for (int i = 0; i < deltaX.Count; i++)
            {
                dataGridView1.Rows[2 * i + 3].Cells[6].Value = string.Format("{0:0.000}", Math.Round(deltaX[i], 4));//坐标增量
                dataGridView1.Rows[2 * i + 3].Cells[7].Value = string.Format("{0:0.000}", Math.Round(deltaY[i], 4));
                dataGridView1.Rows[2 * i + 3].Cells[8].Value = string.Format("{0:0.000}", Math.Round(Xgaizheng[i], 4) * 100);//坐标增量改正数
                dataGridView1.Rows[2 * i + 3].Cells[9].Value = string.Format("{0:0.000}", Math.Round(Ygaizheng[i], 4) * 100);
                dataGridView1.Rows[2 * i + 3].Cells[10].Value = string.Format("{0:0.000}", Math.Round(Xgaihou[i], 4));//改后坐标增量
                dataGridView1.Rows[2 * i + 3].Cells[11].Value = string.Format("{0:0.000}", Math.Round(Ygaihou[i], 4));
            }
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[6].Value = string.Format("{0:0.000}", Math.Round(deltaX.Sum(), 4));//坐标增量之和
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[7].Value = string.Format("{0:0.000}", Math.Round(deltaY.Sum(), 4));
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[8].Value = string.Format("{0:0.000}", Math.Round(XBHC, 4) * 100);//坐标增量闭合差
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[9].Value = string.Format("{0:0.000}", Math.Round(YBHC, 4) * 100);

            //计算坐标
            for (int i = 0; i < Xgaihou.Count - 1; i++)//坐标计算c点多出一个，所以-1，不用判断，计算机不会算错
            {
                X.Insert(i + 2, Xgaihou[i] + X[i + 1]);
                Y.Insert(i + 2, Ygaihou[i] + Y[i + 1]);
            }
            for (int i = 0; i < X.Count - 2; i++)
            {
                dataGridView1.Rows[2 * i + 2].Cells[12].Value = string.Format("{0:0.000}", Math.Round(X[i + 1], 4)); ;
                dataGridView1.Rows[2 * i + 2].Cells[13].Value = string.Format("{0:0.000}", Math.Round(Y[i + 1], 4));
            }
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[10].Value = "fx=" + -Math.Round(Xgaihou.Sum(), 4);//改正后坐标增量
            dataGridView1.Rows[2 * deltaX.Count + 4].Cells[11].Value = "fy=" + -Math.Round(Ygaihou.Sum(), 4);

            Pen p = new Pen(Color.Black, 2.5f);
            Pen p1 = new Pen(Color.Red, 3);
            Pen p2 = new Pen(Color.Blue, 2);
            image = new Bitmap((int)(Y.Max() - Y.Min()) + 300, (int)(X.Max() - X.Min()) + 400);//显示图形范围
            Graphics g = Graphics.FromImage(image);
            g.RotateTransform(-90);//旋转为测量坐标系
            g.TranslateTransform(-(int)(X.Max() + 200), -(int)Y.Min() + 200);//划定原点位置
            PointF[] pf = new PointF[X.Count];
            //线形绘制
            for (int i = 0; i < X.Count; i++)
            {
                pf[i].X = (float)X[i];
                pf[i].Y = (float)Y[i];
            }
            g.DrawLines(p, pf);


            //注记双线
            float[] single = { 0, 0.25f, 0.75f, 1 };
            p1.CompoundArray = single;
            g.DrawLine(p1, pf[0], pf[1]);
            g.DrawLine(p1, pf[pf.Length - 2], pf[pf.Length - 1]);

            //绘制三角
            Program.sanjiao(g, pf[0]);
            Program.sanjiao(g, pf[1]);
            Program.sanjiao(g, pf[pf.Length - 2]);
            Program.sanjiao(g, pf[pf.Length - 1]);

            //绘制字体
            for (int i = 0; i < dianhao.Count; i++)
            {
                Program.ziti(g, pf[i + 1], dianhao[i].ToString());
            }
            GraphicsState gstate = g.Save();
            g.ResetTransform();
            g.Restore(gstate);

            //绘制箭头
            AdjustableArrowCap cap = new AdjustableArrowCap(10, 20);
            p.CustomEndCap = cap;
            g.DrawLine(p, pf[0].X - 150, pf[1].Y - 50, pf[0].X + 150, pf[1].Y - 50);//x轴
            g.DrawLine(p, pf[0].X - 150, pf[1].Y - 50, pf[0].X - 150, pf[1].Y + 250);//y轴
            Program.ziti(g, new PointF(pf[0].X + 150, pf[0].Y - 50 - 10), "X(m)");
            Program.ziti(g, new PointF(pf[0].X - 150 - 25, pf[0].Y + 250), "Y(m)");
            Program.ziti(g, new PointF(pf[0].X - 150 - 10, pf[0].Y - 50), "O");
            pictureBox1.Image = (Image)image;
        }

        private void 生成外业观测手薄ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "附和导线平差计算结果保存";
            saveFileDialog1.Filter = "文本文件(*.txt)|*.txt|Excel文件(*.xlsx)|*.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // txt文件
                if (saveFileDialog1.FilterIndex == 1)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                    List<string> arrstr = new List<string>();
                    string str = null;
                    sw.WriteLine("附和导线近似平差计算结果：\n");
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)//表头
                    {
                        arrstr.Add(string.Format("{0,10}", dataGridView1.Columns[i].HeaderText));//10表示格式化10个字符，原字符右对齐，不足则补空格,但是对文字好像不是特别好使
                    }
                    str = string.Join("\t", arrstr);
                    sw.WriteLine(str);
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)//数据
                    {
                        str = null;
                        arrstr.Clear();
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value == null)
                            {
                                arrstr.Add(string.Format("{0,10}", "\t"));
                            }
                            else
                            {
                                arrstr.Add(string.Format("{0,10}", dataGridView1.Rows[i].Cells[j].Value));
                            }
                        }
                        str = string.Join("\t", arrstr);
                        sw.WriteLine(str);
                    }
                    sw.Close();
                }

                // excel文件
                else
                {
                    string fileName = "";   //文件路径，文件名


                    saveFileDialog1.DefaultExt = "xls";//获取或设置默认文件扩展名

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        fileName = saveFileDialog1.FileName;
                    }
                    else
                    {
                        return;
                    }

                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    if (xlApp == null)
                    {
                        MessageBox.Show("无法创建Excel对象，您的电脑可能未安装Excel");
                        return;
                    }

                    Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 

                    //写入标题
                    for (int i = 0; i < dataGridView1.ColumnCount; i++)
                    {
                        worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                    }
                    //写入数值
                    for (int r = 0; r < dataGridView1.Rows.Count; r++)
                    {
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            worksheet.Cells[r + 2, i + 1] = dataGridView1.Rows[r].Cells[i].Value;
                        }
                        System.Windows.Forms.Application.DoEvents();
                    }
                    worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
                    MessageBox.Show(fileName + "的结果保存成功", "提示", MessageBoxButtons.OK);
                    if (fileName != "")
                    {
                        try
                        {
                            workbook.Saved = true;
                            workbook.SaveCopyAs(fileName);
                            //fileSaved = true;
                        }
                        catch (Exception ex)
                        {
                            //fileSaved = false;
                            MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                        }
                    }
                    xlApp.Quit();
                    GC.Collect();
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
