using System;
using System.Collections.Generic;
using System.Text;

namespace Daoxian
{
    public class Point3d
    {
        //在对象中创建字段和属性
        int _dh;
        public int DH
        {
            get { return _dh; }
            set { _dh = value; }
        }
        double _x;
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        double _y;
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        double _z;
        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }
        //重载构造函数
        public Point3d()
        {
        }

        public Point3d(double x)
        {
            this.X = x;
            this.Y = 0;
            this.Z = 0;
        }

        public Point3d(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
        }
        public Point3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
