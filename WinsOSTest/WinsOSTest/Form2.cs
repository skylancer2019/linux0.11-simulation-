using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace WinsOSTest
{
    public partial class Form2 : Form
    {
        ArrayList calledPages=new ArrayList();
        int data1;
        ArrayList pro6=new ArrayList();
        ArrayList pro7=new ArrayList();
        int data2;
        public Form2(ArrayList calledPagesn,int data1n,ArrayList pro6n,ArrayList pro7n,int data2n)
        {
            InitializeComponent();
            calledPages = calledPagesn;
            data1 = data1n;
            pro6 = pro6n;
            pro7 = pro7n;
            data2 = data2n;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
         /*   initAll();
            initDoNo(calledPages, data1);
            initUnWp(pro6, data1, 6);
            initUnWp(pro7, data2, 7);*/
        }

        Pen p = new Pen(Color.Red, 2);//画笔
        Pen p1 = new Pen(Color.Black, 2);
        Pen p2 = new Pen(Color.Brown, 5);
        Pen p3 = new Pen(Color.DeepSkyBlue, 5);
        Pen p3w = new Pen(Color.White, 5);
        Font f = new Font("Lucida Console", 20);//字体
        Font Smallf = new Font("Lucida Console", 10);
        Font mediumf = new Font("Lucida Console", 13);
        Font bigf = new Font("Lucida Console", 28);
        SolidBrush bluebush = new SolidBrush(Color.DeepSkyBlue);//蓝色刷子
        SolidBrush blackbush = new SolidBrush(Color.Black);
        SolidBrush oranbush = new SolidBrush(Color.Orange);
        SolidBrush greenbush = new SolidBrush(Color.LimeGreen);
        SolidBrush yellowbrush = new SolidBrush(Color.Yellow);
        SolidBrush graybrush = new SolidBrush(Color.Gray);
        SolidBrush whitebrush = new SolidBrush(Color.White);
        SolidBrush redbrush = new SolidBrush(Color.Red);
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        
        public void initAll()
        {
            Graphics g1=pictureBox1.CreateGraphics();
            Graphics g2 = pictureBox2.CreateGraphics();
            g1.FillRectangle(whitebrush,10, 10, 1000, 1000);
            g2.FillRectangle(whitebrush, 10, 10, 1000, 1000);

            g1.DrawRectangle(p3, new Rectangle(100, 10, 100, 470));//前1000
            //g1.DrawString(". . .", bigf, blackbush, new Point(10, 90));
            g1.DrawRectangle(p3, new Rectangle(220, 10, 100, 470));//后1000
          
            g2.DrawRectangle(p3, new Rectangle(100, 10, 100, 470));//前1000
            //g2.DrawString(". . .", bigf, blackbush, new Point(10, 90));
            g2.DrawRectangle(p3, new Rectangle(220, 10, 100, 470));//后1000
            for (int i=0;i<11;i++)
            {
                g1.DrawLine(p3, new Point(100, (i + 1) * 40 + 10), new Point(200, (i + 1) * 40 + 10));
                g2.DrawLine(p3, new Point(100, (i + 1) * 40 + 10), new Point(200, (i + 1) * 40 + 10));

            }
            g1.DrawString("...", mediumf, blackbush, new Point(103, 11 * 40 + 13));
            g2.DrawString("...", mediumf, blackbush, new Point(103, 11 * 40 + 13));

            g1.DrawLine(p3, new Point(220, 450), new Point(320, 450));
            g2.DrawLine(p3, new Point(220, 450), new Point(320, 450));


            g1.DrawString("...", mediumf, blackbush, new Point(223, 10 * 40 + 13));
            g2.DrawString("...", mediumf, blackbush, new Point(223, 10 * 40 + 13));


            Graphics g3 = pictureBox3.CreateGraphics();
            g3.FillRectangle(yellowbrush, new Rectangle(10, 10, 20, 20));
            g3.DrawString("do_no_page", mediumf, blackbush, new Point(10, 30));

            g3.FillRectangle(oranbush, new Rectangle(300, 10, 20, 20));
            g3.DrawString("un_wp_page", mediumf, blackbush, new Point(300, 30));

            g3.FillRectangle(greenbush, new Rectangle(600, 10, 20, 20));
            g3.DrawString("both", mediumf, blackbush, new Point(600, 30));

        }
        public void initDoNo(ArrayList array,int baseAdr)
        {int row = 0;
            int PageNum = 16384;  Graphics g1 = pictureBox1.CreateGraphics();
            array.Sort();
            
            foreach(var c in array)//表示前1000
            {
                string cc = c.ToString();
                int ccc = 0;int.TryParse(cc, out ccc);
                int page = (ccc-baseAdr) / 4096;
                g1.DrawString(cc, mediumf, redbrush, new Point(0, row * 40 + 10));
                g1.DrawLine(p3, new Point(100, (row + 1) * 40 + 10), new Point(200, (row + 1) * 40 + 10));
                g1.FillRectangle(yellowbrush, 103, row * 40 + 13, 94, 34);
                row++;
            }    

        }
        public void initUnWp(ArrayList array,int baseAdr,int pro)
        {
            Graphics g1;
        //    int col = 100;
            if(pro==6)
            {g1= pictureBox1.CreateGraphics();

            }else
            {
                g1 = pictureBox2.CreateGraphics();
            }
            
            foreach (var c in array)
            {
                string temp = c.ToString();
                int cur = Convert.ToInt32(temp, 16);
                int page = (cur - baseAdr) / 4096;
                int gap = 440;//展示60--100之间的内容
                if (page <= 10)
                {
                    g1.DrawLine(p3, new Point(100, (page + 1) * 40 + 10), new Point(200, (page + 1) * 40 + 10));
                    //对于进程6而言
                    if (page < 5)
                    {
                        g1.FillRectangle(greenbush, 103, page * 40 + 13, 94, 34);
                    }
                    else
                    {
                        g1.FillRectangle(oranbush, 103, page * 40 + 13, 94, 34);
                    }
                    g1.DrawString(temp, mediumf, redbrush, new Point(0, page * 40 + 10));
                }
                else
                {
                    if (page == 16383)
                    {
                        g1.DrawLine(p3, new Point(220, 450), new Point(320, 450)); 
                        g1.FillRectangle(oranbush, 223, 453, 94, 34);
                        g1.DrawString(temp, mediumf, blackbush, new Point(320,453));
                    }else
                    {
                        int interval=(page - 60) * 440 / 40+10;
                        g1.DrawLine(p3, new Point(220, interval), new Point(320,interval));
                        g1.FillRectangle(oranbush, 223, interval+3,94, 5); 
                        g1.DrawString(temp, mediumf, blackbush, new Point(320,interval));
                        g1.DrawLine(p3, new Point(220, interval+11), new Point(320, interval+11));

                    }
                }

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            initAll();
            initDoNo(calledPages, data1);
            initUnWp(pro6, data1, 6);
            initUnWp(pro7, data2, 7);
        }
    }
}
