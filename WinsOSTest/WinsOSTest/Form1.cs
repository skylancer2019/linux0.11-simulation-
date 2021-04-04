using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;
namespace WinsOSTest
{
    public partial class Form1 : Form
    {

        StreamReader sr;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sr = new StreamReader(@"..\..\..\data\input.txt", Encoding.Default);
            regi.Add("eax", null);
            regi.Add("ebx", null);
            regi.Add("ecx", null);
            regi.Add("edx", null);
            regi.Add("cs", null);
            regi.Add("ds", null);
            regi.Add("es", null);
            regi.Add("fs", null);
            regi.Add("gs", null);
            regi.Add("ss", null);
            regi.Add("esp", null);
            regi.Add("ebp", null);
            regi.Add("esi", null);
            regi.Add("edi", null);
            pictureBox3.Visible = true;

            timer1.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (e.ClipRectangle.Top < 112 && e.ClipRectangle.Left < 82)
            {
                Rectangle rectangel = new Rectangle(new Point(0, 0), RectangleSize);
                Rectangle Ellipse = new Rectangle(new Point(0, 50), EllipseSize);
                var dc = e.Graphics;


                dc.DrawRectangle(redPen, rectangel);
                dc.DrawEllipse(redPen, Ellipse);
            }

        }

        //建立后台线程
        private void TimeThread()
        {
            Thread thread = new Thread(execve);
            thread.IsBackground = true;
            thread.Start();
        }
        //后台线程调用的定时器
        private void execve()
        {

        }

        //绘图函数
        private void Maindraw()
        {

        }*/


        private void facktimer()
        {

        }
        private void timer2_Tick(object sender, EventArgs e)
        {




        }

        //绘图时，左上角为原点0-----
        //                    |
        //                    |

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

       
        string imode;
        string iuid;
        string igid; 
        
        string curpid;
        string father;
        string childpid;

        String[] pids = { null, null, null, null };//假设总共只展示四个进程
        int[] states = { -1, -1, -1, -1 };//0 运行1 gray 2 un  3die 4cur run
        String[] stuctPosi = { null, null, null, null };//task struct 分配到的地址
        int pidcounter = 1;//当前出现了几个进程展示
        
        bool stackInMap = false;//初始时画布上没有栈
        int flag = 1;//标识之前timer尚未运行
        bool getfreepage = false;
        bool needclear = false;
        bool LDTbuild = false;
        bool contiCopyPage = false;
        bool switchDraw = false;
        bool rebuildpic1 = false;
        bool rebuildpic2 = false;
        bool needToMainWindow = false;
        bool inSysExec = false;
        bool copyStringWindow = false;//判断是不是由copy_string转到新的界面
        bool changeLDT = false;
        bool retFromClear = false;
        bool store_system_call = false;
        string pagefaultAddress;
        
        string store1="";//读取缓冲区
        string store2 = "";

        string paraPointer;
        string newe;//data base
        string old;
        string[] signals = { " ", "SIGHUP", "SIGINT", "SIGQUIT", "SIGILL", "SIGTRAP", "SIGABRT", "SIGIOT", "SIGUNUSED", "SIGFPE", "SIGKILL", "SIGUSR1", "SIGSEGV", "SIGUSR2", "SIGPIPE", "SIGALRM", "SIGTERM", "SIGSTKFLT", "SIGCHLD", "SIGCONT", "SIGSTOP", "SIGTSTP", "SIGTTIN", "SIGTTOU" };
        int ff = 1;
        //存储寄存器中的值
        Dictionary<string, string> regi = new Dictionary<string, string>();
        Stack<string> stack = new Stack<string>();
        Stack<string> drawStack = new Stack<string>();
        struct Node
        {
            public string id;
            public string nr;
            public string data_base;
            public bool isFather1;
        }        Node process1;//程序中创建的第一个进程
        Node process2;//创建的第二个进程

        string to_page_table;

        //存储栈中的值
        private void init()
        {
            Graphics g1 = pictureBox1.CreateGraphics();
            //进程1是一定在的，不需要更新
            int a = pictureBox1.Size.Height;//282 x 401
            int b = pictureBox1.Size.Width;
            g1.FillEllipse(bluebush, new Rectangle(10, 10, 30, 30));
            g1.DrawString("uninterrputible", Smallf, blackbush, new PointF(30, 35));

            g1.FillEllipse(greenbush, new Rectangle(10, 60, 30, 30));
            g1.DrawString("running", Smallf, blackbush, new PointF(30, 85));

            g1.FillEllipse(oranbush, new Rectangle(10, 110, 30, 30));
            g1.DrawString("zombie", Smallf, blackbush, new PointF(30, 135));

            g1.FillEllipse(yellowbrush, new Rectangle(10, 160, 30, 30));
            g1.DrawString("current", Smallf, blackbush, new Point(30, 185));

            //上面为基础图像
            g1.FillEllipse(greenbush, new Rectangle(200, 10, 50, 50));
            g1.DrawString("1", f, blackbush, new PointF(215, 25));
            pids[0] = "1"; states[0] = 0;
        }

        ArrayList pro6 = new ArrayList();
        ArrayList pro7 = new ArrayList();
        private void timer1_Tick(object sender, EventArgs e)
        {//希望每个时钟输出一个断点

            Graphics g1 = pictureBox1.CreateGraphics();
            Graphics g2 = pictureBox2.CreateGraphics();
            Graphics g3 = pictureBox4.CreateGraphics();
            Graphics g4 = pictureBox3.CreateGraphics();
            if (rebuildpic1)
            {
                init();
                Rebuild();
                rebuildpic1 = false;
            }
            if (rebuildpic2)
            {
                DrawStack();
                Stack<string> tempStack = new Stack<string>();
                int cc = drawStack.Count;
                for (int i = 0; i < cc; i++)
                {

                    tempStack.Push(drawStack.Peek()); drawStack.Pop();
                }

                for (int i = 0; i < cc; i++)
                {
                    pushStack(tempStack.Peek()); tempStack.Pop();
                }
                rebuildpic2 = false;
                Thread.Sleep(1000);
            }
            if(retFromClear)
            {
                g2.FillRectangle(whitebrush, 180, 30, 1000, 1000);
                retFromClear = false;
            }
            if (changeLDT)
            {
                change_ldt();
                changeLDT = false;
            }
            if (copyStringWindow)
            {
                Copy_String();
                copyStringWindow = false;
            }
            String line;
            if (store1.Equals(""))
            {
                line = sr.ReadLine();//断点的头
                if(line==null)
                {
                    timer1.Stop();
                    makeNewFrom();
                    return;
                }
                while (line.Equals(""))
                {
                    line = sr.ReadLine();
                    if(line==null)
                    {
                        timer1.Stop();
                        return;
                    }
                }
            }else
            {
                line = store1;
                store1 = store2;store2 = "";
            }

            String name = "";
            if (store1.Equals(""))
            {
                name = sr.ReadLine();//断点的作用域
            }else
            {
                name = store1;
                store1 = store2;store2 = "";
            }
            String range = name;
            String[] temp = name.Split(' ');
            if (!switchDraw)
            {
                richTextBox1.AppendText(line.ToString() + "\n");
                textBox1.Clear();
                
                textBox1.AppendText(temp[temp.Length - 1].ToString());
            }

            name = sr.ReadLine();//当前name为断点作用域之后的行
            if (flag == 1)
            {
                init();
          //   Thread.Sleep(10000);

            }
            
            if (switchDraw)//copy_page_table切换绘图页面时，需要改变range
            {
                CopyPage(name); switchDraw = false;
                range = "copy_page_table";
                name = sr.ReadLine();
            }
            //进程1是一定在的，不需要更新            
            if (name.Equals("___pid"))
            {//再读出准确的pid值
                name = sr.ReadLine();
                temp = name.Split('=');
           
                temp = temp[1].Split(' ');
                curpid = temp[1];
                int id = 0;
                int.TryParse(curpid, out id);//得到当前数值pid
                if (flag == 1)//
                {
                    g1.FillEllipse(yellowbrush, new Rectangle(200, 10 + 70 * pidcounter, 50, 50));
                    g1.DrawString(curpid, f, blackbush, new PointF(215, 25 + 70 * pidcounter));
                    drawArrow(g1, new Point(225, 60 + 70 * (pidcounter - 1)), new Point(225, 10 + 70 * pidcounter));
                    pids[pidcounter] = curpid;
                    states[pidcounter] = 4;
                    pidcounter++;
                    flag = 0;
                }
                name = sr.ReadLine();

            }
            int oper = 0;//要执行的操作
            bool faultA = false;
            bool ErrC = false;
            //进入while之前，name为pid之后，或者作用域之后的第一条语句
            while (true)//进入***部分
            {
                if (name == null)
                {
                    Console.WriteLine("null accur,end?");
                    timer1.Stop();
                    break;
                }
                //对特殊情况还没考虑---即write之后其他write
                if(range.Contains("sys_write"))//读入的第一个的name是""
                {
                    g2.DrawString("call sys_write", f, blackbush, new Point(200, 100));Thread.Sleep(500);
                    string linee=sr.ReadLine();
                    string storr=sr.ReadLine();
                    if(!storr.Contains("sys_write"))
                    {
                        g2.DrawString("Count fault", f, redbrush, new Point(200, 130));
                        Thread.Sleep(500);
                        store1 = linee;
                        store2 = storr;
                        needclear=true;
                        break;
                    }
                    sr.ReadLine();sr.ReadLine();
                    name = sr.ReadLine();
                    
                    if(name.Contains("fault"))
                    {
                        
                        drawArrow(g2, new Point(200, 120), new Point(200, 150));
                        g2.DrawString("fd fault", f, blackbush, new Point(200, 150));
                        needclear = true;
                        break;

                    }else
                    {
                        if(name.Contains("file"))
                        {
                            drawArrow(g2, new Point(200, 120), new Point(200, 150));
                            g2.DrawString("call file write", f, redbrush, new Point(200, 150));
                            sr.ReadLine();sr.ReadLine();sr.ReadLine();sr.ReadLine();sr.ReadLine();
                            name = sr.ReadLine();
                            do_hd_request(name, g2, 180, 200);
                            if (hd_out_aft)
                            {
                                hd_out_aft = false;
                            }
                            else
                            {
                                needclear = true;
                            }
                            //Thread.Sleep(500);
                            break;
                        }else
                        {
                            if(name.Contains("char"))
                            {
                                drawArrow(g2, new Point(200, 120), new Point(200, 150));
                                g2.DrawString("call char write", f, redbrush, new Point(200, 150));
                                sr.ReadLine();
                                linee = sr.ReadLine();
                                string ran = sr.ReadLine();
                                if(ran.Contains("con_write"))
                                {
                                    Thread.Sleep(500);
                                    drawArrow(g2, new Point(200, 170), new Point(200, 200));
                                    g2.DrawString("call con_write", f, redbrush, new Point(200, 200));
                                    sr.ReadLine();sr.ReadLine();
                                    needclear = true;
                                    Thread.Sleep(500);
                                    break;
                                }else
                                {
                                    store1 = linee;
                                    store2 = ran;
                                    needclear = true;
                                    break;
                                }
                            }else
                            {
                                if(name.Contains("blcok"))
                                {
                                    drawArrow(g2, new Point(200, 120), new Point(200, 150));
                                    g2.DrawString("call block write", f, redbrush, new Point(200, 150));
                                }else
                                {
                                    if(name.Contains("pipe"))
                                    {
                                        drawArrow(g2, new Point(200, 120), new Point(200, 150));
                                        g2.DrawString("call pipe write", f, redbrush, new Point(200, 150));
                                    }
                                }
                                //hread.Sleep(500);
                                needclear = true;
                                break;
                            }
                        }
                    }

                }
                if (range.Contains("sys_waitpid"))
                {
                    g2.DrawString(curpid + " sys_waitpid", f, blackbush, new Point(200, 100));
                    string linee = sr.ReadLine();
                    string stroee = sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                    string cur = sr.ReadLine();
                    while (true)
                    {
                        cur = sr.ReadLine();
                        if (cur.Contains("***address"))
                        {
                            break;
                        }

                    }
                    cur = sr.ReadLine();//address
                    Thread.Sleep(500);
                    drawArrow(g2, new Point(220, 130), new Point(220, 160));
                    g2.DrawString("write_verify: " + cur, f, blackbush, new Point(200, 160));
                    needclear = true;
                    Thread.Sleep(500);
                }
                if (name.Equals(""))
                {

                    if (range.Contains("copy_page_table"))//结束后的操作
                    {
                        ff = 1;
                        contiCopyPage = false;
                        g4.FillRectangle(whitebrush, 10, 10, 800, 800);
                        //返回之前的页面
                        //接着copy_page_table之后是gdt表的更改
                        /*
                        */
                        ////
                    }
                    if (range.Contains("copy_process") && needToMainWindow)
                    {
                        pictureBox2.Visible = true;
                        pictureBox1.Visible = true;
                        pictureBox3.BringToFront();
                        pictureBox3.Visible = false;
                        splitContainer1.Visible = true;
                        switchDraw = false;
                        rebuildpic1 = true;
                        rebuildpic2 = true;

                        needToMainWindow = false;
                    }
                    if (range.Contains("do_wp_page"))
                    {
                        g4.DrawString("Page Fault---Write Protection", f, blackbush, new Point(300, 10));
                        g4.DrawString("linear address: " + pagefaultAddress, mediumf, redbrush, new Point(180, 40));
                        if(curpid=="6")
                        {
                            pro6.Add(pagefaultAddress);
                        }
                        if(curpid=="7")
                        {
                            pro7.Add(pagefaultAddress);
                        }

                        if (name.Contains("code"))//缺少的页在code space
                        {
                            drawArrow(g4, new Point(180, 60), new Point(180, 100));
                            g4.DrawString("IN CODE SPACE", f, blackbush, new Point(180, 103));
                            drawArrow(g4, new Point(180, 125), new Point(180, 165));
                            g4.DrawString("do_exit", mediumf, redbrush, new Point(180, 170));
                            //遇到在code space时候的情况-----------------处理完后对picturebox的处理？未完成
                        }
                        else
                        {
                            un_wp_page();
                            pictureBox3.Visible = false;
                            splitContainer1.Visible = true;
                            g4.FillRectangle(whitebrush, 0, 0, 1200, 1000);
                            rebuildpic1 = true;
                            rebuildpic2 = true;

                        }
                    }

                    break;
                }
                if (name == "-1")
                {
                    Console.WriteLine("File End");
                    timer1.Stop();

                }
                if (needclear)
                {
                    g2.FillRectangle(whitebrush, new Rectangle(180,10, 820, 1000));
                    needclear = false;
                }
                if (range.Contains("system_call") && line.Contains("82") && name.Contains("push ss"))
                {
                    clearAll();
                }
                if (range.Contains("release"))
                {
                    sr.ReadLine(); 
                    name = sr.ReadLine();
                    g2.DrawString(curpid + " release:" + name, f, blackbush, new Point(200, 200));
                    int cf = 0; int cr = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (pids[i] != null)
                        {
                            if (pids[i].Equals(curpid))
                            {
                                cf = i;
                            }
                            if (pids[i].Equals(name))
                            {
                                cr = i;
                                pids[i] = null;
                                states[i] = 3;//die
                            }
                        }
                    }
                    //在g1的释放？？
                    g1.FillRectangle(whitebrush, new Rectangle(200, 10 + 70 * cr, 60, 60));//消除圆本身
                    g1.FillRectangle(whitebrush, 215, 60 + 70 * (cr - 1), 30, 20);//消除可能的直接箭头
                    g1.FillRectangle(whitebrush, 170, 10 + 70 * cf + 25, 30, 10 + 70 * (cr - cf));//消除外部箭头
                    g1.FillRectangle(whitebrush, 180, 25+70 * cr, 20, 20);
                    g1.FillRectangle(whitebrush, 180, 25+70 * cf, 20, 20);
                    // g1.FillEllipse(graybrush, new Rectangle(200, 10 + 70 * pidcounter, 50, 50));
                    // pids[pidcounter] = childpid; states[pidcounter] = 1;

                    // g1.DrawString(childpid, f, blackbush, new PointF(215, 25 + 70 * pidcounter));
                    // drawArrow(g1, new Point(225, 60 + 70 * (pidcounter - 1)), new Point(225, 10 + 70 * pidcounter));

                    needclear = true;
                }
                if(range.Contains("do_signal"))
                {
                    if(name.Contains("SIGCHILD"))
                    {
                        g2.DrawString("do_signal", f, blackbush, new Point(200, 100));
                        Thread.Sleep(500);
                        drawArrow(g2, new Point(220, 130), new Point(220, 200));
                        Thread.Sleep(500);
                        g2.DrawString("get signal:", mediumf, blackbush, new Point(200, 200));
                        g2.DrawString("SIGCHILD", f, redbrush, new Point(200, 220));
                        Thread.Sleep(1000);
                        needclear = true;
                    }else
                    {
                        if(name.Contains("sig old"))//必然断点104与断点107起作用
                        {
                            string oldEIP = sr.ReadLine();sr.ReadLine();
                            string oldESP = sr.ReadLine();
                            g2.DrawString("do_signal", f, blackbush, new Point(200, 100));
                            Thread.Sleep(500);
                            drawArrow(g2, new Point(220, 130), new Point(220, 160));
                            Thread.Sleep(500);
                            g2.DrawString("Old eip:" + oldEIP, f, blackbush, new Point(200, 160));
                            g2.DrawString("change to handler", mediumf, redbrush, new Point(200, 180));
                            Thread.Sleep(500);
                            drawArrow(g2, new Point(220, 200), new Point(220, 230));
                            Thread.Sleep(500);
                            g2.DrawString("Old ESP:" + oldESP, f, blackbush, new Point(200, 230));
                            g2.DrawString("SUB", mediumf, redbrush, new Point(200, 250));

                            needclear = true;

                        }
                    }
                }
                if(range.Contains("sys_close"))
                {
                
                    name = sr.ReadLine();
                    g2.DrawString("sys_close", f, blackbush, new Point(200, 100));
                    g2.DrawString("fd number= "+name, f, redbrush, new Point(200, 130));
                    needclear = true;
                }
                if (range.Contains("do_exit"))
                {
                    /*
                     * 此处只考虑它有一个孩子的情况
                     */
                    if(name.Contains("child loop"))
                    {
                        name = sr.ReadLine();sr.ReadLine();
                        string childState = sr.ReadLine();
                      //  drawArrow(g1, new Point(225, 60 + 70 * (pidcounter - 1)), new Point(225, 10 + 70 * pidcounter));
                        int rows=0;int rowl = 0;int c = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (pids[i] != null)
                            {
                                if (pids[i].Equals(name))
                                {
                                    rows = 60 + 70 * (i - 1);
                                    rowl = 10 + 70 * i;
                                    if(i==2)
                                    {
                                        process1.isFather1 = true;
                                    }else
                                    {
                                        process2.isFather1 = true;
                                    }
                                    c = i;
                                    break;
                                }
                            }
                        }
                        g1.FillRectangle(whitebrush, 215, rows, 20, rowl-rows);
                        Thread.Sleep(500);

                        //g1.FillEllipse(graybrush, new Rectangle(200, 10 + 70 * pidcounter, 50, 50));
                        g1.DrawLine(p, new Point(200,35),new Point(180,35));
                        g1.DrawLine(p, new Point(180, 35), new Point(180, rowl+25));
                        drawArrow(g1, new Point(180, rowl+25), new Point(200,rowl+25));
                        if(childState.Equals("3"))//Zombie
                        {
                            Thread.Sleep(500);
                            states[c] = 3;
                            g2.DrawString("process" + childState, f, blackbush, new Point(200, 200));
                            drawArrow(g2, new Point(220, 200), new Point(220, 230));
                            g2.DrawString("send SIGCHILD", mediumf, redbrush, new Point(220, 210));
                            g2.DrawString("process1", f, blackbush, new Point(200, 230));
                        }
                        //Thread.Sleep(500);
                        needclear = true;

                    }else
                    {
                        if(name.Contains("state"))
                        {
                            name=sr.ReadLine();
                            int c = 0;
                            for(int i=0;i<4;i++)
                            {
                                if(pids[i]!=null)
                                {
                                    if(pids[i].Equals(curpid))
                                    {
                                        c = i;
                                        break;
                                    }
                                }
                            }
                            int id;int.TryParse(name, out id);
                            states[c] = id;
                            g1.FillEllipse(oranbush, new Rectangle(200, 10 + 70 * c, 50, 50));
                            g1.DrawString(curpid, f, blackbush, new PointF(215, 25 + 70 *c));
                            

                        }
                    }
                }
                if(range.Contains("sys_brk"))
                {
                    if(name.Contains("end_data"))
                    {
                        string end_data = sr.ReadLine();sr.ReadLine();
                        string curBrk = sr.ReadLine();
                        g2.DrawString("increase data segment", f,blackbush,200, 20);
                        g2.DrawRectangle(p3, 200, 100, 180, 400);
                        g2.DrawString("linear address", f, blackbush, new Point(200, 70));
                        g2.DrawLine(p3, new Point(200, 140), new Point(380, 140));
                        g2.DrawString(curBrk, mediumf, redbrush, new Point(380, 140));
                        g2.DrawString("data seg end", mediumf, blackbush, new Point(380, 120));
                        g2.FillRectangle(yellowbrush, 203, 103, 174, 34);
                        Thread.Sleep(1000);
                        sr.ReadLine();string newLine=sr.ReadLine();name = sr.ReadLine();
                        if(name.Contains("sys_brk"))
                        {
                            g2.DrawLine(p3, new Point(200, 180), new Point(380, 180));
                            g2.FillRectangle(yellowbrush, 203, 103, 174, 74);
                            g2.DrawString("new end", mediumf, blackbush, new Point(380, 160));
                            g2.DrawString(end_data, mediumf, redbrush, new Point(380, 180));
                            Thread.Sleep(1000);
                            g2.FillRectangle(whitebrush, 180, 80, 600, 800);
                            needclear = true;
                        }
                        else
                        {
                            g2.DrawString("new end out of range", mediumf, redbrush, new Point(200, 500));
                            store1 = newLine;
                            store2 = name;
                            Thread.Sleep(500);
                            g2.FillRectangle(whitebrush, 180, 80, 600, 800);
                            needclear = true;
                            break;
                        }
                        
                    }
                }
                if(range.Contains("do_no_page"))
                {

                    do_no_page();
                    break;
                }else
                {
                    if (!range.Contains("page_fault"))
                    {
                        do_no = 0;
                    }
                }
                if (range.Contains("sys_ioctl"))
                {
                    if (name.Contains("fd"))
                    {
                        name = sr.ReadLine();
                        g2.DrawString("IO Control", f, blackbush, new Point(300, 40));
                        Thread.Sleep(500);
                        g2.DrawString("fd:" + name, f, redbrush, new Point(300, 90));
                        Thread.Sleep(500);
                        g2.DrawString("check", f, blackbush, new Point(300, 120));
                        Thread.Sleep(500);
                        name = sr.ReadLine();
                        if (name.Contains("mode"))
                        {
                            drawArrow(g2, new Point(320, 140), new Point(320, 170));
                            g2.DrawString("mode and fd right", f, blackbush, new Point(300, 170));
                            Thread.Sleep(500);
                            sr.ReadLine();
                            string dev = sr.ReadLine();
                            g2.DrawString("dev number:" + dev, f, redbrush, new Point(300, 200));
                            name = sr.ReadLine();
                            Thread.Sleep(500);
                            if (name.Contains("dev out"))
                            {
                                g2.DrawString("dev number out of range", f, blackbush, new Point(300, 230));
                                needclear = true;
                            }
                            else
                            {
                                if (name.Contains("no fun"))
                                {
                                    g2.DrawString("not find ioctl function", mediumf, blackbush, new Point(300, 230));
                                    needclear = true;
                                }else
                                {
                                    g2.DrawString("find ioctl function", mediumf, blackbush, new Point(300, 230));
                                    needclear = true;
                                }
                            }
                        }
                        else
                        {//没有进入modeb部分

                            g2.DrawString("ioctl error", f, redbrush, new Point(300, 230));
                            Thread.Sleep(500);
                            needclear = true;
                        }
                        break;
                    }
        
                
                }
                if(range.Contains("sys_open"))
                {
                    string fd="";
                    if (name.Contains("filename"))
                    {
                        sr.ReadLine();sr.ReadLine();
                        fd = sr.ReadLine();
                        g2.DrawString("Open File", f, blackbush, new Point(200, 150));
                        Thread.Sleep(500);
                        g2.DrawString("find empty fd:" + fd,mediumf, redbrush, new Point(200, 180));
                    }else
                    {
                        if(name.Contains("f"))
                        {//此时是fd的文件结构指针指向搜索到的文件结构
                            g2.DrawString("find empty file struct", mediumf, blackbush,new Point(200, 200));

                            Thread.Sleep(500);
                            g2.DrawString("task_struct", mediumf, blackbush, new Point(200,280));
                            g2.DrawRectangle(p3, 200, 300, 100, 100);
                            string addr="";
                            for(int i=0;i<4;i++)
                            {
                                if(!pids[i].Equals(null))
                                {
                                    if(curpid.Equals(pids[i]))
                                    {
                                        addr = stuctPosi[i];
                                        break;
                                    }
                                }
                            }
                            g2.DrawString(addr, mediumf, redbrush, new Point(200, 400));
                            Thread.Sleep(500);
                            drawArrow(g2, new Point(250, 400), new Point(250, 430));
                            g2.DrawString("filp[" + fd + "]=f", mediumf, blackbush, new Point(250, 434));
                            Thread.Sleep(500);
                            // g2.FillRectangle(whitebrush, 200, 140, 500, 800);
                            needclear = true;
                        }
                    }

                }
                if(range.Contains("create_table"))
                {
                    create_tables();
                    Thread.Sleep(800);
                }
                if (range.Contains("hd_out"))
                {
                    name = sr.ReadLine();
                    g2.DrawString("drive:", mediumf, blackbush, new Point(200, 300));
                    g2.DrawString(name, mediumf, redbrush, new Point(280, 300));
                    Thread.Sleep(500);
                    sr.ReadLine();
                    name = sr.ReadLine();
                    g2.DrawString("head:", mediumf, blackbush, new Point(200, 320));
                    g2.DrawString(name, mediumf, redbrush, new Point(280, 320));
                    Thread.Sleep(500);

                    //g2.FillRectangle(whitebrush, 200, 180, 500, 900);//清除绘制图形
                    needclear = true;
                }
                if (range.Contains("do_hd_request"))
                {
                    if (name.Contains("call stack"))
                    {
                        do_hd_request(name,g2,180,200);
                        break;
                    }
                }
                if (range.Contains("ret_from_sys"))
                {
                    ret_from_sys_call();
                    Thread.Sleep(1000);
                    retFromClear = true;
                    break;
                }
                if (range.Contains("copy_strings"))
                {
                    pictureBox3.Visible = true;
                    splitContainer1.Visible = false;
                    copyStringWindow = true;
                    break;
                }
                if (range.Contains("change_ldt"))
                {
                    pictureBox3.Visible = true;
                    splitContainer1.Visible = false;
                    changeLDT = true;
                    break;
                }
                if (range.Contains("do_execve"))
                {
                    if (name.Contains("euid"))
                    {
                        if (name.Contains("cur"))
                        {
                            g2.DrawString("do_execve", f, blackbush, new Point(300, 10));
                            g2.DrawRectangle(p3, 200, 100, 180, 180);
                            g2.DrawString("task_struct", f, blackbush, new Point(200, 70));
                            Thread.Sleep(500);
                            drawArrow(g2, new Point(390, 200), new Point(420, 200));
                            string euid = sr.ReadLine(); sr.ReadLine();
                            string egid = sr.ReadLine(); sr.ReadLine(); imode = sr.ReadLine();
                            sr.ReadLine();
                            iuid = sr.ReadLine(); sr.ReadLine(); igid = sr.ReadLine();

                            g2.DrawString("euid:", mediumf, blackbush, new Point(422, 200));
                            g2.DrawString(euid, mediumf, redbrush, new Point(480, 200));

                            g2.DrawString("egid:", mediumf, blackbush, new Point(422, 220));
                            g2.DrawString(euid, mediumf, redbrush, new Point(480, 220));
                            g2.DrawString("change euid and egid according to inode", mediumf, blackbush, new Point(200, 300));
                            Thread.Sleep(500);
                        } else
                        {
                            drawArrow(g2, new Point(170, 330), new Point(200, 330));
                            string euid = sr.ReadLine(); sr.ReadLine();
                            string egid = sr.ReadLine();
                            g2.DrawString("changed euid:", mediumf, blackbush, new Point(200, 320));
                            g2.DrawString(euid, mediumf, redbrush, new Point(340, 320));

                            g2.DrawString("changed egid:", mediumf, blackbush, new Point(200, 340));
                            g2.DrawString(euid, mediumf, redbrush, new Point(340, 340));

                            g2.DrawString("inode euid:", mediumf, blackbush, new Point(380, 320));
                            g2.DrawString(iuid, mediumf, redbrush, new Point(500, 320));

                            g2.DrawString("inode egid:", mediumf, blackbush, new Point(380, 340));
                            g2.DrawString(igid, mediumf, redbrush, new Point(500, 340));
                            Thread.Sleep(500);
                            drawArrow(g2, new Point(230, 360), new Point(230, 390));
                            int imode2; int.TryParse(imode, out imode2);
                            int test = 0; int test2 = 0;
                            if (euid.Equals(iuid))
                            {
                                g2.DrawString("changed euid=inode euid", mediumf, blackbush, new Point(200, 400));
                                Thread.Sleep(500);
                                g2.DrawString("imode:" + Convert.ToString(imode2, 2), mediumf, blackbush, new Point(200, 420));
                                Thread.Sleep(300);
                                drawArrow(g2, new Point(180, 450), new Point(220, 450));
                                g2.DrawString(">>6", mediumf, redbrush, new Point(220, 440));

                                int imode3 = imode2 >>= 6;
                                Thread.Sleep(500);
                                string transmode = Convert.ToString(imode3, 2);
                                drawArrow(g2, new Point(180, 470), new Point(220, 470));
                                g2.DrawString("imode=" + transmode, mediumf, redbrush, new Point(220, 460));
                                test = imode3 & 1;
                                test2 = imode2 & 0111;

                                Thread.Sleep(500);

                            }
                            else
                            {
                                if (igid.Equals("\000"))
                                {
                                    igid = "0";
                                }
                                if (egid.Equals(igid))
                                {
                                    g2.DrawString("changed euid!=inode euid", mediumf, blackbush, new Point(200, 400));
                                    Thread.Sleep(500);

                                    drawArrow(g2, new Point(180, 430), new Point(220, 430));
                                    g2.DrawString("changed egid=inode egid", mediumf, blackbush, new Point(220, 420));
                                    Thread.Sleep(500);

                                    g2.DrawString("imode:" + Convert.ToString(imode2, 2), mediumf, blackbush, new Point(200, 440));
                                    Thread.Sleep(300);
                                    drawArrow(g2, new Point(180, 470), new Point(220, 470));
                                    g2.DrawString(">>3", mediumf, redbrush, new Point(220, 460));

                                    int imode3 = imode2 >>= 3;
                                    Thread.Sleep(500);
                                    test = imode3 & 1;
                                    test2 = imode2 & 0111;

                                }
                            }
                            drawArrow(g2, new Point(180, 530), new Point(220, 530));
                            if (test > 0)//没有做超级用户的判断
                            {
                                g2.DrawString("can execve", f, redbrush, new Point(220, 520));
                                inSysExec = false;

                            }
                            else
                            {
                                g2.DrawString("can not execve:return", f, redbrush, new Point(220, 520));
                                inSysExec = false;
                            }
                            Thread.Sleep(500);
                            g2.FillRectangle(whitebrush, 160, 40, 800, 1000);

                        }
                    }
                    if (name.Contains("parameter pointer"))
                    {
                        name = sr.ReadLine();
                        paraPointer = name;
                        //g4.DrawString("pointer", mediumf, redbrush, new Point(colp, 450));
                        g4.DrawString(name, mediumf, blackbush, new Point(colp, 450));
                        drawArrow(g4, new Point(colp, 470), new Point(colp, 500));
                        Thread.Sleep(500);
                        pictureBox3.Visible = false;
                        splitContainer1.Visible = true;

                    }
                    if(name.Contains("entry"))
                    {
                        g4.DrawString("eip[0]-->new entry", f,redbrush, new Point(100, 50));
                        g4.DrawString("eip[3]", f, redbrush, new Point(colp, 100));
                        drawArrow(g4, new Point(colp, 130), new Point(colp, 160));
                        Thread.Sleep(500);
                        pictureBox3.Visible = false;
                        splitContainer1.Visible = true;
                        rebuildpic1 = true;
                        rebuildpic2 = true;
                    }
                }
                if (range.Contains("free_page_table"))
                {
                    g2.FillRectangle(whitebrush, 150, 40, 800, 800);
                    name = sr.ReadLine(); sr.ReadLine();
                    string size = sr.ReadLine();
                    sr.ReadLine(); sr.ReadLine();
                    int from; int.TryParse(name, out from);
                    int fromindex = from >> 20 & (0xffc);
                    g2.DrawString("free_page_table", f, blackbush, new Point(200, 50));
                    g2.DrawRectangle(p3, 200, 150, 180, 400);
                    g2.DrawString("Directory", f, blackbush, new Point(200, 120));
                    int frow = 150 + fromindex * 400 / 1024;
                    int size1; int.TryParse(size, out size1);

                    g2.DrawLine(p3, new Point(200, frow), new Point(380, frow));
                    g2.DrawString(fromindex.ToString(), mediumf, redbrush, new Point(383, frow - 5));
                    int rowinterval = size1 * 400 / 1024;
                    if (rowinterval <= 0)
                    {
                        rowinterval = 1;
                    }
                    rowinterval = 10 * rowinterval;
                    g2.DrawLine(p3, new Point(200, frow + rowinterval), new Point(380, frow + rowinterval));
                    g2.FillRectangle(yellowbrush, 200, frow, 180, rowinterval);
                    g2.DrawString("size=" + size, mediumf, redbrush, new Point(50, frow + rowinterval / 2));

                    Thread.Sleep(500);
                    g2.DrawString("free those pages", mediumf, blackbush, new Point(420, frow+10));
                    Thread.Sleep(1000);
                    needclear = true;
                }
                if (range.Contains("sys_signal"))
                {
                    if (name.Contains("signum"))
                    {
                        name = sr.ReadLine();
                        int signum; int.TryParse(name, out signum);
                        if (signum < 1 || signum > 32)
                        {
                            g2.DrawString("signum out of range", f, blackbush, new Point(200, 230));
                        }
                        else
                        {
                            string signame = signals[signum];
                            g2.DrawString("give " + signame + " new handler", f, blackbush, new Point(200, 200));
                            Thread.Sleep(500);
                            name = sr.ReadLine();
                            if (name.Equals(""))
                            {
                                g2.DrawString("SIGKILL signal--no new handler", f, blackbush, new Point(200, 230));
                            }
                            Thread.Sleep(500);
                            g2.FillRectangle(whitebrush, 200, 200, 500, 70);
                        }
                    }
                }
                
                if (name.Contains("eax as return"))
                {
                    name = sr.ReadLine();
                    regi["eax"] = name;
                    registerShow("eax", name);
                    g3.FillRectangle(whitebrush, 0, 10, 100, 30);
                    g3.DrawString("return", mediumf, redbrush, new Point(0, 20));

                }
                if (name.Contains("fault address"))
                {
                    faultA = true;
                }
                if (name.Contains("error code"))
                {
                    ErrC = true;
                }
                if (range.Contains("schedule") && (!range.Contains("reschedule")))
                {
                    name = sr.ReadLine();
                    g2.DrawRectangle(p3, 200, 100, 140, 450);
                    g2.DrawString("Schedule", bigf, blackbush, new Point(280, 40));
                    g2.DrawString("task array", f, blackbush, new Point(170, 70));
                    g2.DrawString("last_task", mediumf, blackbush, new Point(345, 550));
                    g2.DrawString("first_task", mediumf, blackbush, new Point(345, 95));
                    drawArrow(g2, new Point(390, 535), new Point(350, 535));
                    g2.DrawString("pointer", mediumf, redbrush, new Point(393, 530));
                    g2.DrawLine(p3, new Point(200, 130), new Point(340, 130));
                    g2.DrawString("pid=0", mediumf, blackbush, new Point(205, 110));
                    g2.DrawLine(p3, new Point(200, 160), new Point(340, 160));
                    g2.DrawString("pid=1", mediumf, blackbush, new Point(205, 140));
                    g2.DrawLine(p3, new Point(200, 190), new Point(340, 190));
                    g2.DrawString("pid=" + pids[1], mediumf, blackbush, new Point(205, 170));
                    g2.DrawLine(p3, new Point(200, 220), new Point(340, 220));
                    g2.DrawString("pid=3", mediumf, blackbush, new Point(205, 200));
                    if (pids[2] != null)
                    {
                        g2.DrawLine(p3, new Point(200, 250), new Point(340, 250));
                        g2.DrawString("pid=" + pids[2], mediumf, blackbush, new Point(205, 230));
                    }
                    if (pids[3] != null)
                    {
                        g2.DrawLine(p3, new Point(200, 280), new Point(340, 280));
                        g2.DrawString("pid=" + pids[3], mediumf, blackbush, new Point(205, 260));
                    }

                    Thread.Sleep(500);

                    g2.DrawString(":check alarm", mediumf, redbrush, new Point(463, 530));

                    if (name.Equals("***in loop"))//进入了in loop
                    {
                        int fromposi; fromposi = 535;
                        while (true)
                        {
                            sr.ReadLine(); name = sr.ReadLine();
                            fromposi = checkTask("check alarm", fromposi, name);
                            g2.FillRectangle(whitebrush, 393, fromposi - 35, 300, 50);
                            g2.DrawString("SIGALRM", mediumf, redbrush, new Point(393, fromposi - 5));
                            Thread.Sleep(300);
                            g2.DrawString("alarm=0", mediumf, redbrush, new Point(393, fromposi + 15));
                            Thread.Sleep(300);
                            name = sr.ReadLine(); g2.FillRectangle(whitebrush, 393, fromposi - 35, 300, 100);
                            if (!name.Equals("***in loop"))
                            {
                                fromposi = checkTask("check alarm", fromposi, "0");
                                g2.FillRectangle(whitebrush, 393, fromposi - 35, 300, 50);
                                g2.DrawString("find end", mediumf, redbrush, new Point(393, fromposi - 5));
                                break;
                            }

                        }
                        g2.FillRectangle(whitebrush, 343, 70, 300, 500);
                    }
                    else
                    {
                        
                    
                        if (name.Contains("signal cause"))
                        {
                            int fromposi;fromposi = 535;
                            while (true)
                            {
                                name = sr.ReadLine();
                                int c = 0;
                                for (int i = 0; i < 4; i++)
                                {
                                    if (pids[i] != null)
                                    {
                                        if (pids[i].Equals(name))
                                        {
                                            states[i] = 0;
                                            c = i;
                                        }
                                    }
                                }
                                fromposi = checkTask("check alarm", fromposi, name);
                                g2.FillRectangle(whitebrush, 393, fromposi - 35, 300, 50);
                                g2.DrawString("signal cause change", mediumf, redbrush, new Point(393, fromposi - 5));
                                Thread.Sleep(500);
                                g2.FillRectangle(whitebrush, 393, fromposi - 5, 300, 20);
                                g2.DrawString("state=RUNNING", mediumf, redbrush, new Point(393, fromposi-5));
                                Thread.Sleep(300);
                                name = sr.ReadLine();
                                if(!name.Contains("signal cause"))
                                {
                                    fromposi = checkTask("check alarm", fromposi, "0");
                                    g2.FillRectangle(whitebrush, 393, fromposi - 35, 300, 50);
                                    g2.DrawString("find end", mediumf, redbrush, new Point(393, fromposi - 5));
                                    break;

                                }
                            }
                            g2.FillRectangle(whitebrush, 343, 70, 300, 500);

                        }
                        else
                        {
                            int fromposi = checkTask("check alarm", 535, "0");
                            g2.FillRectangle(whitebrush, 393, fromposi - 35, 300, 50);
                            g2.DrawString("find end", mediumf, redbrush, new Point(393, fromposi - 5));
                            Thread.Sleep(300);
                            g2.DrawString("no alarm", mediumf, redbrush, new Point(393, fromposi + 15));
                            Thread.Sleep(500);
                            g2.FillRectangle(whitebrush, 343, 70, 300, 500);
                        }


                    }
                    Label:
                    if (name.Equals("***in loop2"))
                    {
                        int fromposi = 535;
                        int first = 0;
                        while (true)
                        {
                            sr.ReadLine(); string state = sr.ReadLine(); sr.ReadLine(); string id = sr.ReadLine(); sr.ReadLine();
                            string counter = sr.ReadLine();
                            if (first == 0)
                            {
                                fromposi = checkTask("check counter", fromposi, id);//已经sleep过了
                                first = 1;
                            } else
                            {
                                fromposi = checkTask("check counter", fromposi, id, 1);
                            }

                            g2.FillRectangle(whitebrush, 393, fromposi - 35, 300, 50);
                            if (state.Equals("0"))
                            {
                                g2.FillRectangle(greenbush, 393, fromposi - 10, 5, 5);
                            } else
                            {
                                g2.FillRectangle(graybrush, 393, fromposi - 10, 5, 5);
                            }
                            g2.DrawString("counter " + counter, mediumf, redbrush, new Point(400, fromposi - 5));
                            Thread.Sleep(500);

                            g2.FillRectangle(whitebrush, 350, fromposi - 10, 40, 40);
                            g2.DrawLine(p, new Point(350, fromposi), new Point(390, fromposi));

                            name = sr.ReadLine();
                            if (!name.Equals("***in loop2"))
                            {
                                g2.FillRectangle(whitebrush, 393, fromposi - 15, 300, 30);
                                g2.DrawString("check end", mediumf, redbrush, new Point(393, fromposi - 5));
                                Thread.Sleep(500);
                                break;
                            }

                        }

                    }
                    if (name.Contains("***in loop3"))
                    {
                        int fromposi = 535;
                        int first = 0;
                        while (true)
                        {
                            
                            sr.ReadLine();
                            string loopPid = sr.ReadLine();
                            sr.ReadLine(); 
                            string loopCounter = sr.ReadLine(); sr.ReadLine();
                            int counter;int.TryParse(loopCounter, out counter);
                            string priority = sr.ReadLine();
                            int prior;int.TryParse(priority, out prior);
                            counter = (counter >> 1) + prior;//新的counter值

                            if(first==0)
                            {
                                fromposi = checkTask("update counter", fromposi, loopPid);
                                first = 1 ;
                            }else
                            {
                                fromposi = checkTask("update counter", fromposi, loopPid, 1);
                            }
                            g2.FillRectangle(whitebrush, 393, fromposi - 35, 300, 50);
                            g2.DrawString("counter " + counter, mediumf, redbrush, new Point(400, fromposi - 5));
                            Thread.Sleep(500);
                            g2.FillRectangle(whitebrush, 350, fromposi - 10, 40, 40);
                            g2.DrawLine(p, new Point(350, fromposi), new Point(390, fromposi));

                            sr.ReadLine();
                            name = sr.ReadLine();
                            if (name.Contains("***in loop3"))
                            {
                                continue;
                            }else
                            {
                                g2.FillRectangle(whitebrush, 393, fromposi - 15, 300, 30);
                                g2.DrawString("update end", mediumf, redbrush, new Point(393, fromposi - 5));
                                Thread.Sleep(500);
                                break;
                            }
                        }
                        goto Label;
                    }
                    if (name.Contains("task switch"))
                    {
                        name = sr.ReadLine();
                        int toPosi = computePosi(name);
                        g2.FillRectangle(whitebrush, 350, toPosi + 15, 350, 50);
                        drawArrow(g2, new Point(380, toPosi + 15), new Point(350, toPosi + 15));
                        g2.DrawString("task switch to", f, redbrush, new Point(380, toPosi));
                         int initconter = 0;/////why
                        string usdpid = curpid;
                        int stateRun = -1;curpid = name;//curpid是应当转到的
                        for (int i = 0; i < 4; i++)
                        {
                            if (pids[i] != null)
                            {
                                if (pids[i].Equals(curpid))
                                {
                                    states[i] = 4; initconter = i;
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            if (pids[i] != null)
                            {
                                if (pids[i].Equals(usdpid))
                                {
                                    stateRun = i;
                                    break;
                                }
                            }
                        }
                        if (stateRun != -1&&states[stateRun]!=3)
                        {
                            states[stateRun] = 0;
                        }
                        Thread.Sleep(500);
                        if (stateRun != initconter)
                        {
                            g1.FillEllipse(yellowbrush, new Rectangle(200, 10 + 70 * initconter, 50, 50));
                            g1.DrawString(pids[initconter], f, blackbush, new PointF(215, 25 + 70 * initconter));
                            if (states[stateRun] != 3)
                            {
                                g1.FillEllipse(greenbush, new Rectangle(200, 10 + 70 * stateRun, 50, 50));

                            }
                            g1.DrawString(pids[stateRun], f, blackbush, new PointF(215, 25 + 70 * stateRun));
                        }
                        needclear = true;
                    }
                }
                if (range.Contains("copy_process"))
                {
                    if (name.Contains("GDT") && name.Contains("tss"))
                    {
                        string cur = sr.ReadLine();
                        int cc; int.TryParse(cur, out cc);
                        cc = (cc + 1) * 8;
                        sr.ReadLine();
                        string tssA = sr.ReadLine();
                        sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                        string ldtA = sr.ReadLine();
                        gdtInF(cc.ToString(), tssA, ldtA);
                        Thread.Sleep(1000);
                        g4.FillRectangle(whitebrush, 10, 10, 1000, 800);
                        needToMainWindow = true;
                    }
                }
                if (range.Contains("copy") && range.Contains("page_table"))
                {
                    if (name.Contains("dir"))
                    {
                        splitContainer1.Visible = false;
                        pictureBox3.Visible = true;
                        contiCopyPage = true;
                        switchDraw = true;
                        break;

                    } else
                    {
                        if (contiCopyPage)
                        {
                            if (name.Contains("from_page_table"))
                            {
                                name = sr.ReadLine();//物理地址
                                drawArrow(g4, new Point(300, rowf + 10 * ff), new Point(500, rowf + 10 * ff));
                                g4.DrawRectangle(p3, new Rectangle(500, rowf + 10, 180, 180));
                                g4.DrawString("from page table", mediumf, redbrush, new Point(500, rowf - 35));
                                g4.DrawString("address " + name, mediumf, blackbush, new Point(500, rowf - 20));
                                get_free_page(g4, 800, 2);
                                Thread.Sleep(500);

                                //to dir指向to page table
                                g4.DrawLine(p, new Point(300, rowt + 10 * ff), new Point(350, rowt + 10 * ff));
                                g4.DrawLine(p, new Point(350, rowt + 10 * ff), new Point(350, 460));
                                drawArrow(g4, new Point(350, 460), new Point(800, 460));
                                g4.DrawString("to page table", mediumf, redbrush, new Point(800, 523));
                                Thread.Sleep(500);

                                //from page table --->to page table
                                g4.DrawLine(p, new Point(590, rowf + 190), new Point(590, 430));
                                drawArrow(g4, new Point(590, 430), new Point(800, 430));
                                g4.DrawString("Copy to new page table", mediumf, redbrush, new Point(550, 435));
                                g4.DrawString("pages read only", mediumf, redbrush, new Point(805, 420));
                                g4.DrawString("pages read only", mediumf, redbrush, new Point(505, rowf + 10 * ff + 80));
                                Thread.Sleep(1000);

                                g4.FillRectangle(whitebrush, new Rectangle(490, 100, 800, 800));
                                g4.FillRectangle(whitebrush, new Rectangle(303, rowf, 200, 40));
                                int from;
                                int.TryParse(old, out from); from = from / 1048576;
                                g4.DrawString(from.ToString(), mediumf, blackbush, new Point(303, rowf + 3));

                                g4.FillRectangle(whitebrush, new Rectangle(303, rowt - 3, 200, 400));
                                int to;
                                int.TryParse(newe, out to); to = to / 1048576;
                                g4.DrawString(to.ToString(), mediumf, blackbush, new Point(303, rowt + 3));
                                ff++;

                            }


                        }
                    }

                } else
                {
                    contiCopyPage = false;
                }
                //no word---whether should show
                if (LDTbuild)
                {
                    if (name.Contains("new_data_base"))
                    {
                        g2.DrawRectangle(p3, new Rectangle(200, 520, 400, 60));
                        newe = sr.ReadLine(); sr.ReadLine();
                        old = sr.ReadLine();
                        g2.DrawLine(p3, new Point(250, 520), new Point(250, 580));
                        g2.DrawString("old_data_base", mediumf, blackbush, new Point(140, 500));
                        Thread.Sleep(500);
                        g2.DrawString(old, mediumf, blackbush, new Point(170, 560));

                        Thread.Sleep(500);
                        g2.DrawLine(p3, new Point(300, 520), new Point(300, 580));
                        g2.DrawString("new_data_base=new_code_base", mediumf, redbrush, new Point(300,500));
                        Thread.Sleep(500);
                        g2.DrawString(newe, mediumf, redbrush, new Point(300, 560));
                        if (childpid.Equals(process1.id))
                        {
                            process1.data_base = newe;
                        } else
                        {
                            process2.data_base = newe;
                        }

                        Thread.Sleep(500);
                        drawArrow(g2, new Point(440, 520), new Point(440, 240));
                        g2.FillRectangle(whitebrush, new Rectangle(450, csrow + 3, 150, 15));
                        g2.DrawString("cs base " + newe, mediumf, redbrush, new Point(453, csrow ));
                        Thread.Sleep(500);
                        g2.FillRectangle(whitebrush, new Rectangle(450, dsrow + 3, 150, 15));
                        g2.DrawString("ds base " + newe, mediumf, redbrush, new Point(453, dsrow ));
                        Thread.Sleep(2000);

                    }
                }
                if (name.Contains("eax to nr"))
                {
                    name = regi["eax"];
                   
                    if (process1.id == null)
                    {
                        process1.nr = name;
                    } else
                    {
                        process2.nr =name;
                    }
                }
                if (name.Contains("new process tss"))
                {
                    DrawNewProcess();
                    needclear = true;
                    name = sr.ReadLine();
                    continue;
                }
                if (name.Contains("LDT index in GDT"))
                {
                    NewGDT();
                    LDTbuild = true;
                }
                if (name.Contains("push"))
                {
                    if (!stackInMap)
                    {
                        DrawStack();
                    }
                    string[] temp1 = name.Split(' ');
                    if (name.Contains("had"))
                    {
                        pushStack(temp1[temp1.Length - 1]);
                    } else
                    {
                        //此处要加入push的真正值
                        pushStack(temp1[temp1.Length - 1] + " " + regi[temp1[temp1.Length - 1]]);
                        stack.Push(regi[temp1[temp1.Length - 1]]);
                    }
                }
                if (name.Contains("pid"))
                {
                    if (range.Contains("do_exit"))
                    {
                        name = sr.ReadLine();
                        curpid = name;
                        sr.ReadLine();
                        father = sr.ReadLine();

                    }
                    else
                    {
                        if (name.Contains("new"))
                        {
                            name = sr.ReadLine();
                            childpid = name;
                            if (process1.id != null)
                            {
                                process2.id = name;

                            }
                            else
                            {
                                process1.id = name;
                            }
                            buildNewNode();
                        }
                        else
                        {
                            if (name.Contains("father"))
                            {
                                name = sr.ReadLine();
                                father = name;
                            }
                        }
                    }

                }
                /*if (name.Contains("pop"))
                {
                    string[] temp1 = name.Split(' ');
                    popStack();

                    if (!name.Contains("old"))
                    {
                        string outter = stack.Pop();

                        regi[temp1[1]] = outter;
                    }
                }*/
                if (name.Contains("state"))
                {
                    if (name.Contains("new"))
                    {
                        name = sr.ReadLine();
                        for (int i = 0; i < 4; i++)
                        {
                            if (pids[i] != null)
                            {
                                if (pids[i].Equals(childpid))
                                {
                                    int ss; int.TryParse(name, out ss);
                                    states[i] = ss;
                                }
                            }
                        }
                        if (name.Equals("2"))
                        {
                            g1.FillEllipse(bluebush, new Rectangle(200, 10 + 70 * (pidcounter - 1), 50, 50));
                            g1.DrawString(childpid, f, blackbush, new PointF(215, 25 + 70 * (pidcounter - 1)));
                            states[pidcounter - 1] = 2;
                        } else
                        {
                            if (name.Equals("0"))
                            {
                                g1.FillEllipse(greenbush, new Rectangle(200, 10 + 70 * (pidcounter - 1), 50, 50)); ;
                                g1.DrawString(childpid, f, blackbush, new PointF(215, 25 + 70 * (pidcounter - 1)));
                                states[pidcounter-1] = 0;
                            }
                        }

                    }
                    if (name.Contains("cur"))
                    {
                        g2.DrawString("after system call", f, blackbush, new Point(300, 100));
                        g2.FillEllipse(yellowbrush, new Rectangle(300, 200, 80, 80));
                        g2.DrawString(curpid, bigf, blackbush, new PointF(320, 220));
                        g2.DrawString("cur process", f, blackbush, new Point(300, 170));
                        name = sr.ReadLine();

                        Thread.Sleep(500);
                        int run = 1;
                        if (name.Equals("0"))
                        {
                            g2.DrawString("RUNNING state", f, blackbush, new Point(300, 280));
                        } else
                        {
                            g2.DrawString("NOT RUNNING", f, blackbush, new Point(300, 280));
                            run = 0;
                        }
                        Thread.Sleep(500);
                        sr.ReadLine(); name = sr.ReadLine();
                        g2.DrawString("time slice left " + name, f, blackbush, new Point(300, 310));
                        int coun = 1;
                        if (name.Equals("0"))
                        {
                            coun = 0;
                        }
                        Thread.Sleep(500);
                        drawArrow(g2, new Point(300, 310), new Point(300, 360));
                        if (coun == 1 && run == 1)
                        {
                            g2.DrawString("continue running", f, redbrush, new Point(300, 370));
                        } else
                        {
                            g2.DrawString("Resechdule", f, redbrush, new Point(300, 370));
                        }
                        Thread.Sleep(500);
                        g2.FillRectangle(whitebrush, 280, 160, 400, 600);
                        needclear = true;
                    }
                }
                if (name.Contains("register"))
                {
                    string[] temp1 = name.Split(' ');
                    string regis = temp1[temp1.Length - 1];
                    name = sr.ReadLine();
                    regi[regis] = name;
                    registerShow(regis, name);
                    if (regis.Equals("esp") && range.Contains("sys_fork"))
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            popStack();
                        }
                    }
                    if (regis.Equals("eax") && range.Contains("sys_execve"))
                    {
                        g3.FillRectangle(whitebrush, 0, 10, 100, 30);
                        g3.DrawString("&Eip(esp)", mediumf, redbrush, new Point(0, 20));
                        inSysExec = true;
                    }
                    if (faultA)/////////////////////////////////////
                    {
                        faultA = false;
                        g3.FillRectangle(whitebrush, 0, 115, 200, 30);
                        g3.DrawString("fault address", mediumf, redbrush, new Point(0, 125));
                        pagefaultAddress = name;
                    }
                    if (ErrC)
                    {
                        ErrC = false;
                        g3.FillRectangle(whitebrush, 0, 115, 200, 30);
                        g3.DrawString("error code", mediumf, redbrush, new Point(0, 20));
                        pictureBox3.Visible = true;
                        splitContainer1.Visible = false;
                        //转化到另一个绘图box
                    }

                }
                if (name.Contains("after free page"))//说明要展示内存分配动画
                {
                    if (!contiCopyPage)
                    {
                        get_free_page(g2);
                        getfreepage = true;
                    }
                }
                if (name.Contains("copy task"))
                {
                    if (getfreepage)
                    {

                        g2.DrawString("copy task struct to new page", mediumf, oranbush, new Point(210, 430));
                        getfreepage = false;
                        needclear = true;
                        Thread.Sleep(300);
                    }
                }
                name = sr.ReadLine();

            }
            flag = 0;
        }
        bool hd_out_aft=false;


        private void makeNewFrom()
        {
            int data1=0;int.TryParse(process1.data_base, out data1);
            int data2 = 0; int.TryParse(process2.data_base, out data2);
            Form2 sec = new Form2(calledPages,data1,pro6,pro7,data2);
            sec.ShowDialog();

        }

        //用int指示是否是bmap,,,,bmap=1,others=0
        private int do_hd_request(string name, Graphics g2,int row,int col)
        {

            if (name.Contains("call stack"))
            {
                string cur = sr.ReadLine();
                bool inode = false;
                bool bread = false;
                bool bread_page = false;
                bool file_write = false;
                bool bmap = false;
                while (!cur.Equals(""))
                {
                    cur = sr.ReadLine();
                    if (cur.Contains("inode.c:305"))
                    {
                        inode = true;
                    }
                    if (cur.Contains("buffer.c:278"))
                    {
                        bread = true;

                    }
                    if(cur.Contains("buffer.c:308"))
                    {
                        bread_page = true;
                    }
                    if(cur.Contains("file_dev"))
                    {
                        file_write = true;
                    }
                    if(cur.Contains("inode.c:142"))
                    {
                        bmap = true;
                    }
                    if (cur.Contains("***"))//说明call stack结束
                    {
                        sr.ReadLine();
                        break;
                    }
                    
                }
                if (inSysExec)
                {
                    if (inode)
                    {
                        g2.DrawString("from file name get inode", mediumf, blackbush, new Point(col, row));
                    }
                    else
                    {
                        if (bread)
                        {
                            g2.DrawString("bread file head", mediumf, blackbush, new Point(col, row));
                        }
                    }
                    Thread.Sleep(500);
                    drawArrow(g2, new Point(col+10, row+20), new Point(col+10,row+70));
                }
                if(bread_page)
                {
                    g2.DrawString("read block from file", mediumf, blackbush, new Point(col,row));//////////
                    Thread.Sleep(500);
                    drawArrow(g2, new Point(col + 10, row + 20), new Point(col + 10, row + 70));
                }
                if(file_write)
                {
                    g2.DrawString("file write", mediumf, blackbush, new Point(col, row));//////////
                    Thread.Sleep(500);
                    drawArrow(g2, new Point(col + 10, row + 20), new Point(col + 10, row + 70));
                    Thread.Sleep(500);
                }
                if(bmap)//此时必然也调用了bread
                {
                    g2.DrawString("bmap()", mediumf, blackbush, new Point(col, row));//////////
                    Thread.Sleep(500);
                    if(bread)
                    {
                        g2.DrawString("bread block", mediumf, blackbush, new Point(col, row+20));
                    }
                    drawArrow(g2, new Point(col + 10, row + 40), new Point(col + 10, row+70));
                }
                g2.DrawString("hard disk request", f, redbrush, new Point(col, row+70));
                Thread.Sleep(500);
                if (!cur.Equals(""))//hd out 有输出
                {
                    string[] temp1 = cur.Split(' ');
                    string op = temp1[temp1.Length - 1];
                    g2.DrawString("operation:" + op, mediumf, redbrush, new Point(col, row+100));
                    Thread.Sleep(500);
                    hd_out_aft = true;
                }
                else
                {
                    hd_out_aft = false;
                    needclear = true;
                    g2.FillRectangle(whitebrush, col,row, 500, 900);
                }
                if(bmap)
                {
                    return 1;
                }else
                {
                    return 0;
                }
                //即call stack为当前breakPoint的最后输出

            }
            return -1;
        }
        int[] Pposi = new int[50];//加设空间中不超过50个参数
        int PposiIndex = 0;
        private void create_tables()
        {
            string sp = sr.ReadLine();sr.ReadLine();
            string argc1 = sr.ReadLine();sr.ReadLine();
            string envc1 = sr.ReadLine();
            Graphics g4 = pictureBox3.CreateGraphics();
            g4.FillRectangle(whitebrush, colp, 410, 150, 87);
            drawArrow(g4, new Point(colp, 480), new Point(colp, 497));
            g4.DrawString("sp:" + sp, mediumf, redbrush, new Point(colp,460));
            g4.FillRectangle(whitebrush, 150, 0, 1500, 443);
            int envc;int.TryParse(envc1, out envc);
            int argc;int.TryParse(argc1, out argc);
            g4.DrawString("create_tables", bigf, blackbush, new Point(400, 30));
            g4.DrawLine(p, new Point(100, 500), new Point(100, 480));
            g4.DrawLine(p, new Point(100, 480), new Point(colp, 480));

            for(int i=0;i<PposiIndex;i++)
            {
                g4.DrawLine(p3, new Point(Pposi[i], 500), new Point(Pposi[i], 580));
            }

            drawArrow(g4, new Point(150, 475), new Point(150, 290));
            Thread.Sleep(500);
            g4.DrawRectangle(p3, 100, 200, 1000, 80);
            drawArrow(g4, new Point(1100, 180), new Point(1100, 197));
            g4.DrawString("sp:" + sp, mediumf, redbrush, new Point(1100, 160));
            g4.DrawString("space before sp", f, blackbush, new Point(100, 170));

            Thread.Sleep(500);
            colp = 1100;
            int colsp = colp - (envc + 1) * 20;
            for(int i=0;i<envc+2;i++)
            {
                g4.DrawLine(p3, new Point(colp-(i+1)*20, 200), new Point(colp-(i+1)*20, 280));
            }
            Thread.Sleep(500);
            g4.FillRectangle(whitebrush, colp, 160,35, 20);
            drawArrow(g4, new Point(colsp, 180), new Point(colsp, 197));
            int envpCol = colsp;
            g4.DrawString("envp", mediumf, redbrush, new Point(colsp, 160));//得到envp
            g4.DrawString("sp", mediumf, redbrush, new Point(colsp, 140));

            Thread.Sleep(500);
            colp = colsp;
            colsp = colp - (argc + 1)*20;
            for (int i = 0; i < argc + 2; i++)
            {
                g4.DrawLine(p3, new Point(colp - (i + 1) * 20, 200), new Point(colp - (i + 1) * 20, 280));
            }
            Thread.Sleep(500);
            g4.FillRectangle(whitebrush, colp, 140, 40, 20);
            drawArrow(g4, new Point(colsp, 180), new Point(colsp, 197));
            int argvCol = colsp;
            g4.DrawString("argv", mediumf, redbrush, new Point(colsp, 160));//得到envp
            g4.DrawString("sp", mediumf, redbrush, new Point(colsp, 140));

            Thread.Sleep(500);
            colsp = colsp - 20;

            drawArrow(g4, new Point(colsp, 180), new Point(colsp, 197));
            g4.FillRectangle(whitebrush, colsp + 20, 140, 40, 20);
            g4.DrawString("sp", mediumf, redbrush, new Point(colsp, 160));
            g4.DrawLine(p3, new Point(colsp, 200), new Point(colsp, 280));
            g4.DrawString("e", mediumf, blackbush, new Point(colsp + 3,203)); g4.DrawString("n", mediumf, blackbush, new Point(colsp + 3,223));
            g4.DrawString("v", mediumf, blackbush, new Point(colsp + 3,243)); g4.DrawString("p", mediumf, blackbush, new Point(colsp + 3,263));
            Thread.Sleep(500);
            
            colsp = colsp - 20;
            g4.FillRectangle(whitebrush, colsp + 22, 160, 20, 20);
            g4.DrawString("sp", mediumf, redbrush, new Point(colsp, 160));
            g4.DrawLine(p3, new Point(colsp, 200), new Point(colsp, 280));
            g4.DrawString("a", mediumf, blackbush, new Point(colsp + 3, 203)); g4.DrawString("r", mediumf, blackbush, new Point(colsp + 3, 223));
            g4.DrawString("g", mediumf, blackbush, new Point(colsp + 3, 243)); g4.DrawString("v", mediumf, blackbush, new Point(colsp + 3, 263));
            Thread.Sleep(500);

            colsp = colsp - 20;
            g4.FillRectangle(whitebrush, colsp + 20, 160, 22, 20);
            g4.DrawString("sp", mediumf, redbrush, new Point(colsp, 160));
            g4.DrawLine(p3, new Point(colsp, 200), new Point(colsp, 280));
            g4.DrawString("a", mediumf, blackbush, new Point(colsp + 3, 203)); g4.DrawString("r", mediumf, blackbush, new Point(colsp + 3, 223));
            g4.DrawString("g", mediumf, blackbush, new Point(colsp + 3, 243)); g4.DrawString("c", mediumf, blackbush, new Point(colsp + 3, 263));
            Thread.Sleep(500);

            int counter = PposiIndex - 1;
            for(int i=0;i<argc;i++)
            {
                //绘制箭头
                drawArrow(g4, new Point(argvCol, 180), new Point(argvCol, 197));
                drawArrow(g4, new Point(Pposi[counter], 480), new Point(Pposi[counter], 497));

                //绘制连线
                g4.DrawLine(p, new Point(Pposi[counter], 480), new Point(argvCol, 320));
                drawArrow(g4, new Point(argvCol, 320), new Point(argvCol, 283));
                Thread.Sleep(500);
               // g4.FillRectangle(whitebrush, argvCol - 10, 180, 20, 17);
              //  if(i!=0)
             //   {
            //        g4.FillRectangle(whitebrush, Pposi[counter] - 10, 480, 20, 17);
              //  }
                argvCol += 20;
                counter--;
            }
            drawArrow(g4, new Point(argvCol, 180), new Point(argvCol, 197));
            g4.DrawString("0", mediumf, blackbush, new Point(argvCol + 3, 203));

            for (int i = 0; i < envc; i++)
            {
                //绘制箭头
                drawArrow(g4, new Point(envpCol, 180), new Point(envpCol, 197));
                drawArrow(g4, new Point(Pposi[counter], 480), new Point(Pposi[counter], 497));

                //绘制连线
                g4.DrawLine(p, new Point(Pposi[counter], 480), new Point(envpCol, 320));
                drawArrow(g4, new Point(envpCol, 320), new Point(envpCol, 283));
                Thread.Sleep(500);
               // g4.FillRectangle(whitebrush, envpCol - 10, 180, 20, 17);
              //  if (i != 0)
              //  {
              //      g4.FillRectangle(whitebrush, Pposi[counter] - 10, 480, 20, 17);
              //  }
                envpCol += 20;
                counter--;
            }
            drawArrow(g4, new Point(envpCol, 180), new Point(envpCol, 197));
            g4.DrawString("0", mediumf, blackbush, new Point(envpCol + 3, 203));
            colp = colsp;
            Thread.Sleep(1000);
        }

        int do_no = 0;//计数0次
        ArrayList calledPages = new ArrayList();

        private void do_no_page()
        {
            //实则为g4
            Graphics g2 = pictureBox3.CreateGraphics();
            

            string address = sr.ReadLine();
            sr.ReadLine();sr.ReadLine();string cur=sr.ReadLine();
            if(cur.Contains("page_fault"))
            {//第一个do_no_page之后就是page fault,说明可能共享/超出数据区/出错
                    while(!cur.Equals(""))
                    {
                        cur=sr.ReadLine();
                    }
                    string line = sr.ReadLine();
                    cur = sr.ReadLine();
                    if(cur.Contains("system_call"))//do_no_page部分结束
                    {
                        store1 = line;
                        store2 = cur;
                    pictureBox3.Visible = false;
                    splitContainer1.Visible = true;
                    rebuildpic1 = true;
                    rebuildpic2 = true;
                        return;
                    }else
                    {
                        return;
                    }
            }else
            {
                 
                if(cur.Contains("do_no_page"))
                {
                    g2.FillRectangle(whitebrush, 420, 80, 1000, 1000);
                    g2.FillRectangle(whitebrush, 100, 70, 400, 1000);
                   // g2.FillRectangle(whitebrush, 310, 180, 200, 40);

                    sr.ReadLine();sr.ReadLine();sr.ReadLine();
                    string page = sr.ReadLine();sr.ReadLine();
                    string block = sr.ReadLine();
                    g2.DrawString("do_no_page",f,blackbush, new Point(500,10));
                    int addr; int.TryParse(address, out addr);
                    g2.DrawRectangle(p3, 200, 100, 80, 500);
                    int counter = 0;
                    calledPages.Sort();
                    int num = 0;//当当前地址小于之前地址时
                    int check = 1;//表示是否需要避让
                    foreach( var c in calledPages)
                    {
                        int row = counter * 40 + 100;
                        g2.DrawLine(p3, new Point(200, row+40), new Point(280, row+40));
                        string cc = c.ToString();
                        int ccc = 0;int.TryParse(cc, out ccc);
                        if (ccc > addr&&check==1)
                        {
                            num = counter;
                            counter++;
                            row = counter * 40 + 100;
                            g2.DrawLine(p3, new Point(200, row + 40), new Point(280, row + 40));
                            g2.DrawString(c.ToString(), mediumf, redbrush, new Point(283, row));
                            check = 0;
                          
                        }
                        g2.DrawString(c.ToString(), mediumf, redbrush, new Point(283, row));
                        counter++;
                    }
                    if (check == 0)
                    {
                        do_no = num;
                    }
                    else
                    {
                        do_no = counter;
                    }
                    g2.DrawString("linear address", f, blackbush, new Point(170, 70));
                    g2.DrawString("data base", mediumf, blackbush, new Point(100, 100));
                    if (addr % 67108864 == 0)
                    {//即为进程逻辑空间的开始部分

                        
                        calledPages.Add(address);//将此时缺页的地址加入数组
                        g2.DrawString(address, mediumf, redbrush, new Point(283, 100));
                        g2.DrawString("fault address", mediumf, redbrush, new Point(283, 115));
                        do_no++;
                    }
                    else
                    {
                        int row = do_no * 40 + 100;
                        g2.DrawLine(p3, new Point(200, row), new Point(280, row));
                        g2.DrawString(address, mediumf, redbrush, new Point(283, row));
                        calledPages.Add(address);
                        g2.DrawString("fault address", mediumf, redbrush, new Point(283, row + 15));
                        do_no++;
                    }
                    //上述完成address的显示
                    Thread.Sleep(500);
                    g2.DrawString("get free page", f, redbrush, new Point(500, 80));
                    Thread.Sleep(300);
                    drawArrow(g2, new Point(530, 100), new Point(530, 120));
                    g2.DrawString("phisical address:" + page, mediumf, blackbush, new Point(500, 120));
                    g2.DrawRectangle(p3, 500, 140, 100, 100);
                    Thread.Sleep(500);
                    g2.DrawString("Page initial block:" + block, mediumf, redbrush, new Point(480, 280));


                    //绘制block空间
                    Thread.Sleep(500);
                    g2.DrawString("Read Page from hardDisk", f, blackbush, new Point(900,100));
                    //此处应该画一个箭头
                    drawArrow(g2, new Point(480, 280), new Point(740, 280));

                    //硬盘逻辑空间
                    Thread.Sleep(500);
                    g2.DrawString("logical block", mediumf, redbrush, new Point(1000, 130));
                    g2.DrawRectangle(p3, 1000, 150, 100, 400);sr.ReadLine();sr.ReadLine();
                    string range=sr.ReadLine();
                    if (range.Contains("do_hd_request"))
                    {
                        sr.ReadLine(); sr.ReadLine(); string name = sr.ReadLine();
                        int Ifbmap = do_hd_request(name, g2, 300, 500);
                        if (Ifbmap == 1)//此时读了bmap
                        {
                            sr.ReadLine(); sr.ReadLine();
                            sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                            string drive = sr.ReadLine(); sr.ReadLine();
                            string head = sr.ReadLine();
                            drawArrow(g2, new Point(520, 440), new Point(520, 460));
                            g2.DrawString("drive:" + drive, mediumf, blackbush, new Point(500, 460));
                            g2.DrawString("head:" + head, mediumf, blackbush, new Point(580, 460));
                            Thread.Sleep(500);
                            g2.FillRectangle(whitebrush, 500, 300, 300, 1000);
                            Thread.Sleep(500);
                            sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                        }
                    }

                        sr.ReadLine(); sr.ReadLine(); sr.ReadLine();

                    for (int j = 0; j < 4; j++)
                    {
                        string blockn = sr.ReadLine();
                        g2.DrawString(blockn, mediumf, redbrush, new Point(1103,155 + j * 50));
                        g2.DrawLine(p3, new Point(1000, 200 + j * 50), new Point(1100, 200 + j * 50));
                    }

                    for (int j = 0; j < 4; j++)//j表示要读取四块
                    {//要读一个空格
                        sr.ReadLine(); sr.ReadLine();
                        string curren = sr.ReadLine();
                        if (curren.Contains("do_hd_request"))
                        {
                            sr.ReadLine(); sr.ReadLine(); string name = sr.ReadLine();
                            do_hd_request(name, g2, 300, 500);

                        }
                        sr.ReadLine(); sr.ReadLine();
                        sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                        string drive = sr.ReadLine(); sr.ReadLine();
                        string head = sr.ReadLine();
                        
                            g2.DrawString("drive:" + drive, mediumf, redbrush, new Point(700, 140 + 20 * j));
                            g2.DrawString("head:" + head, mediumf, redbrush, new Point(780, 140 + 20 * j));
                            Thread.Sleep(500);
                            drawArrow(g2, new Point(870, 140 + 20 * j), new Point(610, 140 + 20 * j));

                            g2.DrawLine(p, new Point(1000, 175 + j * 50), new Point(870, 175 + j * 50));
                            g2.DrawLine(p, new Point(870, 175 + j * 50), new Point(870, 140 + 20 * j));
                        
                        g2.FillRectangle(whitebrush, 500, 300, 300, 1000);
                        Thread.Sleep(500);
                        //sr.ReadLine(); //多了一个空格
                    }
                    sr.ReadLine();
                    cur=sr.ReadLine();

                    int pageTable = ((addr >> 20) & 0xffc);//在页目录中的索引

                    int rows = do_no * 40 + 100;
                    g2.DrawLine(p, new Point(430, rows), new Point(430, 350));
                    drawArrow(g2, new Point(430, 350), new Point(500, 350));

                    Thread.Sleep(500);
                    g2.DrawString("Dictionary", f, blackbush, new Point(500, 320));
                    g2.DrawRectangle(p3, 500, 350, 100, 200);
                    Thread.Sleep(500);

                    int Dicrow = pageTable * 200 / 4096 + 350;

                    g2.DrawLine(p3, new Point(500, Dicrow), new Point(600, Dicrow));
                    g2.DrawLine(p3, new Point(500, Dicrow + 20), new Point(600, Dicrow + 20));
                    g2.DrawString(pageTable.ToString(), mediumf, redbrush, new Point(600, Dicrow));

                    Thread.Sleep(500);

                    if (!cur.Contains("40"))//有page fault时候
                    {
                        while (!cur.Equals(""))//消除掉put page
                        {
                            cur = sr.ReadLine();
                        }
                        cur = sr.ReadLine();
                        g2.DrawString("Page Table not in memory", mediumf, blackbush, new Point(640, Dicrow));
                        Thread.Sleep(1000);
                        g2.FillRectangle(whitebrush, 640, Dicrow, 270, 30);
                        g2.DrawString("Get a new page", mediumf, blackbush, new Point(670, 350));
                        Thread.Sleep(500);
                        
                       
                    }else
                    {
                        g2.DrawString("Page Table has in memory", mediumf, blackbush, new Point(640,Dicrow));
                        Thread.Sleep(500);
                        g2.FillRectangle(whitebrush, 640, Dicrow, 270, 30);
                        Thread.Sleep(500);
                    }

                        g2.DrawRectangle(p3, 700, Dicrow+20, 80, 80);
                        Thread.Sleep(500);

                        g2.DrawLine(p, new Point(600,Dicrow), new Point(640,Dicrow));
                        g2.DrawLine(p, new Point(640, Dicrow), new Point(640, Dicrow + 20));
                        drawArrow(g2, new Point(640, Dicrow+20), new Point(695, Dicrow+20));
                        Thread.Sleep(500);

                        //page table指向physical page
                        int tableDir = (addr >> 12) & 0x3ff;
                        int tableRow = tableDir * 80 / 4096+Dicrow+20;
                        g2.DrawLine(p3, new Point(700, tableRow), new Point(780, tableRow));
                        g2.DrawLine(p3, new Point(700, tableRow + 20), new Point(780,tableRow + 20));
                        g2.DrawString(tableDir.ToString(), mediumf, redbrush, new Point(783, tableRow+10));
                        Thread.Sleep(500);

                        g2.DrawLine(p, new Point(820, tableRow), new Point(840, tableRow));
                        g2.DrawLine(p, new Point(840, tableRow), new Point(840, 240));
                        drawArrow(g2, new Point(840, 240), new Point(603, 240));
                        Thread.Sleep(500);
                    while (!cur.Equals(""))
                    {
                        cur = sr.ReadLine();
                        if (cur == null)
                        {
                            return;
                        }
                    }//读完page fault结束
                    g2.DrawString("Page fault end", f, blackbush, new Point(500, 300));
                    Thread.Sleep(500);

                    string line=sr.ReadLine();
                    cur = sr.ReadLine();
                    if(!cur.Contains("page_fault"))
                    {
                        store1 = line;
                        store2 = cur;
                        pictureBox3.Visible = false;
                        splitContainer1.Visible = true;
                        rebuildpic1 = true;
                        rebuildpic2 = true;
                    }else
                    {
                        store1 = line;
                        store2 = cur;
                    }
                    
                }
            }
            
        }
        private void ret_from_sys_call()
        {
            Graphics g2 = pictureBox2.CreateGraphics();
            string cur = sr.ReadLine();//task的值
            g2.DrawString("ret_from_sys_call", f, blackbush, new Point(200, 100));
            g2.DrawString("task array:" + cur, mediumf, redbrush, new Point(200, 130));
            sr.ReadLine(); sr.ReadLine();
            cur = sr.ReadLine();
            g2.DrawString("cur task:" + cur, mediumf, redbrush, new Point(200, 150));
            Thread.Sleep(500);
            drawArrow(g2, new Point(210, 170), new Point(210, 200));
            cur = sr.ReadLine();
            if (!cur.Equals(""))
            {
                g2.DrawString("not task0", mediumf, bluebush, new Point(200, 200));
                cur = sr.ReadLine();
                g2.DrawString("check continue", mediumf, blackbush, new Point(200, 220));
                cur = sr.ReadLine(); Thread.Sleep(500);
                if (!cur.Equals(""))
                {
                    g2.DrawString("cur task is user", mediumf, bluebush, new PointF(200, 240));
                    Thread.Sleep(300);
                    drawArrow(g2, new Point(210, 260), new Point(210, 290));
                    g2.DrawString("signal processing", f, blackbush, new Point(200, 290));

                    Thread.Sleep(500);
                    sr.ReadLine();//signal map
                    sr.ReadLine();
                    string ebbx = sr.ReadLine();
                    regi["ebx"] = ebbx;
                    registerShow("ebx", ebbx);
                    Graphics g3 = pictureBox4.CreateGraphics();
                    g3.FillRectangle(whitebrush, 0, 50, 100, 30);
                    g3.DrawString("signal map", mediumf, redbrush, new Point(0, 50));

                    sr.ReadLine(); sr.ReadLine();
                    string eccx = sr.ReadLine();
                    regi["ecx"] = eccx;
                    registerShow("ecx", eccx);
                    g3.FillRectangle(whitebrush, 0, 80, 100, 30);
                    g3.DrawString("block map", mediumf, redbrush, new Point(0, 80));

                    g2.DrawString("signal map:" + ebbx, mediumf, redbrush, new Point(200, 320));
                    g2.DrawString("block map:" + eccx, mediumf, redbrush, new Point(200, 340));
                    sr.ReadLine(); sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                    Thread.Sleep(500);
                    drawArrow(g2, new Point(210, 360), new Point(210, 380));

                    eccx = sr.ReadLine();
                    regi["ecx"] = eccx;
                    registerShow("ecx", eccx);
                    g3.FillRectangle(whitebrush, 0, 80, 100, 30);
                    g3.DrawString("signum-1", mediumf, redbrush, new Point(0, 80));
                    cur = sr.ReadLine();
                    if (cur!=null&&!cur.Equals(""))
                    {
                        sr.ReadLine(); sr.ReadLine(); sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                        g2.DrawString("reset signal in map", mediumf, bluebush, new Point(200, 380));
                        eccx = sr.ReadLine();
                        regi["ecx"] = eccx;
                        registerShow("ecx", eccx);
                        g3.FillRectangle(whitebrush, 0, 80, 100, 30);
                        g3.DrawString("signum", mediumf, redbrush, new Point(0, 80));
                        Thread.Sleep(500);
                        int num; int.TryParse(eccx, out num);
                        string signame = signals[num];
                        g2.DrawString("signal name:" + signame, mediumf, redbrush, new Point(200, 400));

                        sr.ReadLine();
                        string contain = "ecx " + eccx;
                        pushStack(contain);

                    }
                    else
                    {
                        g2.DrawString("no signal need to process", mediumf, redbrush, new Point(200, 380));
                    }

                }
                else
                {
                    g2.DrawString("not user task--->end", mediumf, bluebush, new Point(200, 240));
                }
            } else
            {
                g2.DrawString("task0--->end", mediumf, bluebush, new Point(200, 200));
            }

            Thread.Sleep(500);
            clearStack();
            g2.DrawString("System call end", f, blackbush, new Point(200, 420));


        }
        private void clearAll()
        {
            clearStack();
            stack.Clear();
            drawStack.Clear();
            eaxBox.Clear();
            ebxBox.Clear();
            ecxBox.Clear();
            edxBox.Clear();
            csBox.Clear();
            dsBox.Clear();
            esBox.Clear();
            fsBox.Clear();
            gsBox.Clear();
            esiBox.Clear(); ediBox.Clear();
            ssBox.Clear();
            espBox.Clear();
            ebpBox.Clear();
            Graphics g2 = pictureBox2.CreateGraphics();
            g2.FillRectangle(whitebrush, 190, 40, 500, 800);
            Graphics g3 = pictureBox4.CreateGraphics();
            g3.FillRectangle(whitebrush, 0, 0, 200, 900);
        }

        string[] pages=new string[34];//128kb连续空间最多占用34个页面
        int pagesIndex = 0;//说明下一个物理page应当填入的位置
        private void change_ldt()
        {
            Node processMes;
            if (curpid.Equals(process1.id))
            {
                processMes = process1;
            }
            else
            {
                processMes = process2;
            }
            Graphics g4 = pictureBox3.CreateGraphics();
            g4.DrawString("change_ldt", bigf, blackbush, new Point(500, 10));
            //g4.DrawString("Copy String", f, blackbush, new Point(500, 10));
            g4.DrawRectangle(p3, new Rectangle(100, 80, 800, 80));
            g4.DrawString("process logical memory", f, blackbush, new Point(100, 50));
            g4.DrawString(processMes.nr + "*64M", mediumf, blackbush, new Point(70, 163));
            g4.DrawString("(" + processMes.nr + "+1)*64M", mediumf, blackbush, new Point(900, 163));
            g4.DrawString(processMes.data_base, mediumf, redbrush, new Point(80, 183));
            int datacol = 100 + 800 / 2;

            int colpara = 740;
            g4.DrawLine(p3, new Point(colpara, 80), new Point(colpara, 160));
            g4.DrawString("parameter", f, greenbush, new Point(750, 100));
            g4.DrawLine(p, new Point(900, 160), new Point(900, 180));
            g4.DrawLine(p, new Point(800, 160), new Point(800, 180));
            g4.DrawLine(p, new Point(800, 180), new Point(900, 180));
            g4.DrawLine(p, new Point(150, 180), new Point(800, 180));
            drawArrow(g4, new Point(150, 180), new Point(150, 440));
            g4.DrawRectangle(p3, new Rectangle(100, 500, 1000, 80));
            g4.DrawString("parameter space", f, blackbush, new Point(100, 440));
            drawArrow(g4, new Point(1100, 470), new Point(1100, 495));

            g4.DrawString("pointer", mediumf, redbrush, new Point(colp, 450));
            g4.DrawString(paraPointer, mediumf, blackbush, new Point(colp, 430));
            drawArrow(g4, new Point(colp, 470), new Point(colp, 500));
            //架构画好了
            //增加一个phisical address长方形
            Thread.Sleep(500);
            int num = pagesIndex;//涉及物理页面的数量
            int rows = num * 40;
            g4.DrawRectangle(p3,1100, 100, 80, rows);
            g4.DrawString("physical pages", f, blackbush, new Point(1070, 70));
            for(int i1=0;i1<num;i1++)
            {
                g4.DrawString(pages[i1], mediumf, redbrush, new Point(1183, 100 + 40 * i1));
            }

            Thread.Sleep(500);
            sr.ReadLine();sr.ReadLine();
            string code_limit = sr.ReadLine();
            int codeL;int.TryParse(code_limit, out codeL);
            int colCode = codeL * 800 / 67108864;
            if (colCode <= 0)
            {
                colCode = 100 + 20;
            }
            else
            {
                colCode = 100 + colCode;
            }
            g4.DrawLine(p3, new Point(colCode, 80), new Point(colCode, 160));
            g4.DrawString("set code_limit", f,blackbush, new Point(colCode, 103));
            g4.DrawString(code_limit, f, redbrush, new Point(colCode, 130));
            sr.ReadLine();sr.ReadLine();
            string curren = sr.ReadLine();
            int i = 0;
            Thread.Sleep(1500);
            g4.FillRectangle(whitebrush, 60, 50, 1000, 150);//删除第一个长方形
                g4.FillRectangle(whitebrush, 140, 175, 20, 270);
            while(curren.Contains("change_ldt"))
            {
                sr.ReadLine();
                sr.ReadLine();sr.ReadLine();
                curren = sr.ReadLine();//put page的线性地址
                int addr;int.TryParse(curren, out addr);
                sr.ReadLine(); sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                sr.ReadLine(); sr.ReadLine(); sr.ReadLine(); sr.ReadLine();
                string page = sr.ReadLine();
                int page10;int.TryParse(page, out page10);
                string page16=Convert.ToString(page10, 16);
                page16 = "0x" + page16; 
                for(int j=0;j<num;j++)
                {
                    if (pages[j].Equals(page16))
                    {
                        i = j;
                        break;
                    }
                }//说明是第几项
                int dirIndex = (addr >> 20) & (0xffc);
                int tableIndex = (addr >> 12) & (0x3ff);
                Thread.Sleep(500);

                //draw dictionary
                g4.DrawRectangle(p3,200,80,180,350);
                g4.DrawString("Dictionary", f, blackbush, new Point(200, 40));
                int dirRow = 80+dirIndex * 350 / 1024;
                g4.DrawLine(p3, new Point(200, dirRow), new Point(380, dirRow));//绘制对应位置的线
                if (dirRow + 20 > 430)
                {
                    g4.DrawLine(p3, new Point(200, 410), new Point(380, 410));
                    dirRow -= 20;
                }
                else
                {
                    g4.DrawLine(p3, new Point(200, dirRow + 20), new Point(380, dirRow + 20));
                }
                g4.DrawLine(p3, new Point(200, dirRow + 20), new Point(380, dirRow + 20));

                g4.DrawLine(p, new Point(1100, 440), new Point(1100, 500));
                g4.DrawLine(p, new Point(1100, 440), new Point(170, 440));
                g4.DrawLine(p, new Point(170, 440), new Point(170, dirRow));
                drawArrow(g4, new Point(170, dirRow), new Point(197, dirRow));//绘制完成指向dir箭头
                g4.DrawString(dirIndex.ToString(), mediumf, redbrush, new Point(383, dirRow - 10));//输出index

                //根据线性地址找到页表项
                Thread.Sleep(500);
                int tableRow = 80 + tableIndex * 300 / 1024;
                g4.DrawLine(p, new Point(470, 440), new Point(470, tableRow));
                drawArrow(g4, new Point(470, tableRow), new Point(497, tableRow));
                
                //绘制页表
                Thread.Sleep(500);
                g4.DrawRectangle(p3,500, 80, 180, 300);
                g4.DrawString("page table", mediumf,redbrush, new Point(500, 60));
                g4.DrawLine(p3, new Point(500, tableRow), new Point(680, tableRow));
                //g4.DrawLine(p3, new Point(500, tableRow + 20), new Point(680, tableRow+20));
                if(tableRow+20>380)
                {
                    g4.DrawLine(p3, new Point(500, 360), new Point(680, 360));
                    tableRow -= 20;
                }else
                {
                    g4.DrawLine(p3, new Point(500, tableRow + 20), new Point(680, tableRow + 20));
                }
                g4.DrawString(tableIndex.ToString(), mediumf, redbrush, new Point(683, tableRow - 10));

                //页目录指向页表
                Thread.Sleep(500);
                g4.DrawLine(p, new Point(383, dirRow + 10), new Point(400, tableRow));
                drawArrow(g4, new Point(400, tableRow), new Point(497, tableRow));
                Thread.Sleep(300);

                g4.DrawLine(p, new Point(683, tableRow + 10), new Point(700, tableRow + 10));
                g4.DrawLine(p, new Point(700, tableRow + 10), new Point(700, 100 + 40 * i));
                drawArrow(g4, new Point(700, 100 + 40 * i), new Point(1097, 100 + 40 * i));
                g4.DrawString("change page table", f, redbrush, new Point(730, 110 + 40 * i));

                Thread.Sleep(1000);
                sr.ReadLine();sr.ReadLine();
                sr.ReadLine();sr.ReadLine();sr.ReadLine();
                curren = sr.ReadLine();
                i++;
                
            }
            curren = sr.ReadLine();
         
            g4.FillRectangle(whitebrush, colp, 430, 100, 20);
            g4.DrawString(curren, mediumf, redbrush, new Point(colp, 430));
            g4.DrawString("logical address", mediumf, blackbush, new Point(colp, 410));

        }
        int colp = 1100;
        int coltmp = 970;
        private void Copy_String()
        {
            PposiIndex = 0;
            Node processMes;
            if(curpid.Equals(process1.id))
            {
                processMes = process1;
            }else
            {
                processMes = process2;
            }

            string tmp;string len;string pointer;
            string minT;string maxT;
            Graphics g4 = pictureBox3.CreateGraphics();
            g4.DrawRectangle(p3, new Rectangle(100,80,800,80));
            g4.DrawString("Copy String", f, blackbush, new Point(500, 10));
            g4.DrawString("process logical memory", f, blackbush, new Point(100,50));
            g4.DrawString(processMes.nr + "*64M", mediumf,blackbush,new Point(70,163));
            g4.DrawString("("+processMes.nr + "+1)*64M", mediumf, blackbush, new Point(900, 163));
            g4.DrawString(processMes.data_base, mediumf, redbrush, new Point(80, 183));

            int datacol = 100+ 800 / 2;
            g4.DrawLine(p3, new Point(datacol, 80), new Point(datacol, 160));
            g4.DrawString("code seg", f, blackbush, new Point(110, 100));//说明data段的位置
            int colpara = 740;
            g4.DrawLine(p3, new Point(colpara, 80), new Point(colpara, 160));
            g4.DrawString("parameter", f, greenbush, new Point(745, 100));

            Thread.Sleep(500);
            g4.DrawLine(p, new Point(900, 160), new Point(900, 180));
            g4.DrawLine(p, new Point(800, 160), new Point(800, 180));
            g4.DrawLine(p, new Point(800, 180), new Point(900, 180));
            g4.DrawLine(p, new Point(150, 180), new Point(800, 180));
            drawArrow(g4, new Point(150, 180), new Point(150, 440));

            g4.DrawRectangle(p3, new Rectangle(100, 500, 1000, 80));
            g4.DrawString("parameter space", f, blackbush, new Point(100, 440));
            drawArrow(g4, new Point(1100, 470), new Point(1100, 495));
           // g4.DrawString("parameter pointer", mediumf, redbrush, new Point(1100, 450));
            int length = 128 * 1024;
            //初始化指针的像素位置
            int scale = 3;
            int offset;
            string pPage="";
            colp = 1100;
            //tmp表示：
            g4.DrawRectangle(p3,new Rectangle(170,280,800,80));
            int fir = 0;

            while(true)
            {
                Pposi[PposiIndex] = colp;
                PposiIndex++; 
                tmp = sr.ReadLine();sr.ReadLine();
                len = sr.ReadLine();sr.ReadLine();
                pointer = sr.ReadLine();
                int p1;int.TryParse(pointer, out p1);
                int len1;int.TryParse(len, out len1);

                g4.DrawLine(p3, new Point(colp, 500), new Point(colp, 580));
                g4.DrawString("parameter pointer", f, redbrush, new Point(colp, 430));
                drawArrow(g4, new Point(colp, 470), new Point(colp, 495));
                Point rightP = new Point(colp, 500);
                int curcol = colp - len1 * scale;

                g4.DrawLine(p3, new Point(curcol, 500), new Point(curcol, 580));
                g4.DrawString(len, mediumf, redbrush, new Point(curcol, 582));
                if (fir == 0)
                {
                    g4.DrawString(pointer, mediumf, redbrush, new Point(colp + 3, 480));
                   
                }
                offset = p1 % 4096;//当前页面的偏移量

                Thread.Sleep(500);
                //绘制tmp所在的空间
                drawArrow(g4, new Point(coltmp, 260), new Point(coltmp, 276));
                g4.DrawString("tmp:"+tmp, mediumf, redbrush, new Point(coltmp,240));
                g4.DrawLine(p3, new Point(coltmp, 280), new Point(coltmp, 360));
                Point rightTmp = new Point(coltmp, 360);
                int lesstmp = coltmp - len1 * scale;
                g4.DrawLine(p3, new Point(lesstmp, 280), new Point(lesstmp, 360));
                g4.DrawString(len, mediumf, blackbush, new Point(lesstmp, 260));

                Thread.Sleep(500);
                //绘制指向箭头：
                g4.DrawLine(p, rightTmp, new Point(rightTmp.X, rightTmp.Y + 20));
                g4.DrawLine(p, new Point(rightTmp.X, rightTmp.Y + 20), new Point(rightP.X, rightTmp.Y + 20));
                drawArrow(g4, new Point(rightP.X, rightTmp.Y + 20), rightP);
                Thread.Sleep(500);

                g4.FillRectangle(whitebrush, rightTmp.X-3, rightTmp.Y-3, 10, 23);
                g4.FillRectangle(whitebrush, rightP.X - 10, rightTmp.Y+15, 20, rightP.Y- rightTmp.Y - 15);
               g4.DrawLine(p3,new Point(170,360),new Point(970,360));
                
                if(rightTmp.X<rightP.X)
                {
                    g4.FillRectangle(whitebrush, rightTmp.X - 10, rightTmp.Y +15, rightP.X-rightTmp.X+10, 14);
                }else
                {
                    g4.FillRectangle(whitebrush, rightP.X - 10, rightTmp.Y + 15, rightTmp.X-rightP.X + 10, 14);
                }
              
                g4.FillRectangle(whitebrush, coltmp, 240, 150, 17);//tmp删除
                g4.FillRectangle(whitebrush, colp, 430, 290, 30);
                g4.DrawLine(p3, new Point(100, 500), new Point(1100, 500));

                string cur = sr.ReadLine();
                if(cur.Contains("physical pointer"))
                {
                    cur = sr.ReadLine();
                    if (!cur.Equals(pPage))
                    {
                        pages[pagesIndex] = cur;
                        pagesIndex++;
                         g4.DrawRectangle(p3, 1120, 350, 80, 80);
                        g4.DrawString("physical page", f, blackbush, new Point(1100, 320));
                        g4.DrawLine(p, new Point(1190, 430), new Point(1190, 540));
                        drawArrow(g4, new Point(1190, 540), new Point(1100, 540));
                        g4.FillRectangle(whitebrush, 1233, 345, 200, 30);
                        g4.DrawString(cur, mediumf, redbrush, new Point(1203, 350));
                       
                    }
                    pPage = cur;
                    cur = sr.ReadLine();
                    
                }
                if(cur.Equals(""))
                {

                    colp = curcol;
                    Pposi[PposiIndex] = colp;
                    g4.DrawString("pointer", mediumf, redbrush, new Point(colp, 430));
                    drawArrow(g4, new Point(colp, 470), new Point(colp, 495));
                    PposiIndex++;
                    break;
                }
                g4.DrawLine(p, new Point(1190, 430), new Point(1190, 540));
                fir = 1;
                colp = curcol;
                coltmp = lesstmp-20;//coltmp如何设置？
            }
        }
        private void un_wp_page()
        {
            bool prePhysi = false;
            Graphics g4 = pictureBox3.CreateGraphics();
            int rowTable=0;
            while (true)
            {
                string Nouse = sr.ReadLine();//当前项目的第一行
                string store = sr.ReadLine();
                if(!store.Contains("un_wp_page")&&prePhysi)
                {
                    g4.FillRectangle(whitebrush, 563, rowTable - 7, 160, 30);
                    g4.DrawLine(p3, new Point(560, rowTable + 10), new Point(740, rowTable + 10));
                    g4.DrawString("read and write", mediumf, redbrush, new Point(563, rowTable - 7));
                    Thread.Sleep(500);
                    while(true)
                    {
                        string curr = sr.ReadLine();
                        if(curr.Equals(""))
                        {
                            break;
                        }
                    }
                    break;
                }
                string[] temp = store.Split(' ');
                textBox1.Clear(); textBox1.AppendText(temp[temp.Length - 1]);
                richTextBox1.AppendText(Nouse.ToString() + "\n");
                sr.ReadLine(); sr.ReadLine();//跳过pid相关部分
                string cur = sr.ReadLine();
               
                if (cur.Contains("table entry"))
                {
                    string address = pagefaultAddress;
                    int adr = Convert.ToInt32(address, 16);
                    int table_index = (adr >> 10) & 0xffc;
                    int log_index = (adr >> 20) & 0xffc;
                    string Ptable_index = sr.ReadLine();
                    int Ptable = Convert.ToInt32(Ptable_index, 16);
                    int tableAddress = Ptable - table_index;
                    string tableAA = Convert.ToString(tableAddress, 16);
                    drawArrow(g4, new Point(200, 60), new Point(200, 120));
                    g4.DrawRectangle(p3, 180, 150, 180, 400);
                    g4.DrawString("Directory", f, blackbush, new Point(180, 120));//页目录的名字？
                    int rowLog = log_index * 400 / 4096 + 150;
                    if (rowLog + 20 >= 550)
                    {
                        rowLog = 540;
                    }
                    g4.DrawLine(p3, new Point(180, rowLog - 10), new Point(360, rowLog - 10));
                    g4.DrawString(log_index.ToString(), mediumf, redbrush, new Point(363, rowLog - 20));
                    g4.DrawLine(p3, new Point(180, rowLog + 10), new Point(360, rowLog + 10));
                    g4.DrawString("cur entry", mediumf, redbrush, new Point(185, rowLog - 7));
                    Thread.Sleep(500);

                    g4.DrawLine(p, new Point(363, rowLog), new Point(500, rowLog));
                    g4.DrawLine(p, new Point(500, rowLog), new Point(500, 180));
                    drawArrow(g4, new Point(500, 180), new Point(557, 180));
                    Thread.Sleep(500);

                    g4.DrawRectangle(p3, 560, 180, 180, 400);
                    g4.DrawString("Page Table", f, blackbush, new Point(560, 150));
                    g4.DrawString("0x" + tableAA, mediumf, redbrush, new Point(743, 180));
                    Thread.Sleep(500);

                    rowTable = table_index * 400 / 4096 + 180;
                    if(rowTable-20<180)
                    {
                        rowTable = 200;
                    }
                    if (rowTable + 20 > 580)
                    {
                        rowTable = 570;
                    }
                    g4.DrawLine(p3, new Point(560, rowTable - 10), new Point(740, rowTable - 10));
                    g4.DrawString(table_index.ToString(), mediumf, redbrush, new Point(510, rowTable - 20));
                    g4.DrawLine(p3, new Point(560, rowTable + 10), new Point(740, rowTable + 10));
                    g4.DrawString("cur entry", mediumf, redbrush, new Point(565, rowTable - 7));
                    Thread.Sleep(500);
                    //此时未画页面本身
                    g4.DrawLine(p, new Point(743, rowTable), new Point(850, rowTable));
                    g4.DrawLine(p, new Point(850, rowTable), new Point(850, 180));
                    drawArrow(g4, new Point(850, 180), new Point(940, 180));
                    Thread.Sleep(500);

                }
                if(cur.Contains("physical page"))
                {
                    cur = sr.ReadLine();//物理地址
                    int phy;int.TryParse(cur, out phy);
                    string physical = Convert.ToString(phy, 16);//输出的物理地址
                    
                    g4.DrawRectangle(p3, 940, 180, 180, 180);
                    g4.DrawString("Old Page", f, blackbush, new Point(940, 150));
                    g4.DrawString("0x" + physical, mediumf, redbrush, new Point(1120, 180));
                    prePhysi = true;//说明前一个是phisical page断点
                    Thread.Sleep(500);

                }
                if(cur.Contains("only change"))
                {
                    g4.FillRectangle(whitebrush, 563, rowTable - 7, 160, 30);
                    g4.DrawLine(p3, new Point(560, rowTable + 10), new Point(740, rowTable + 10));
                    g4.DrawString("read and write", mediumf, redbrush, new Point(563, rowTable - 7));
                    Thread.Sleep(500);
                    break;
                }
                if(cur.Contains("new page"))
                {
                    cur = sr.ReadLine();
                    g4.DrawString("get a new page", f, redbrush, new Point(940, 400));
                    Thread.Sleep(500);
                    int phy; int.TryParse(cur, out phy);
                    string physical = Convert.ToString(phy, 16);//输出的物理地址

                    drawArrow(g4, new Point(950, 430), new Point(950, 470));
                    Thread.Sleep(500);

                    g4.DrawRectangle(p3, 940,500, 180, 180);
                    g4.DrawString("New Page", f, blackbush, new Point(940, 470));
                    g4.DrawString("0x" + physical, mediumf, redbrush, new Point(1120, 500));
                    Thread.Sleep(500);

                    //消除旧的箭头
                    g4.FillRectangle(whitebrush, 743, rowTable-10, 110, 20);
                    g4.FillRectangle(whitebrush, 840, 180, 20, rowTable - 170);
                    g4.FillRectangle(whitebrush, 850, 160, 90, 40);
                    Thread.Sleep(500);

                    //画新箭头
                    g4.DrawLine(p, new Point(743, rowTable), new Point(850, rowTable));
                    g4.DrawLine(p, new Point(850, rowTable), new Point(850, 500));
                    drawArrow(g4, new Point(850, 500), new Point(940, 500));
                    g4.FillRectangle(whitebrush, 563, rowTable - 7, 160, 30);
                    g4.DrawLine(p3, new Point(560, rowTable + 10), new Point(740, rowTable + 10));
                    g4.DrawString("read and write", mediumf, redbrush, new Point(563, rowTable - 7));
                    Thread.Sleep(500);


                    //画copy箭头
                    drawArrow(g4, new Point(1020, 370), new Point(1020, 470));
                    g4.DrawString("Copy Page", f, redbrush, new Point(1025, 430));
                    Thread.Sleep(500);
                    break;
                }
                cur = sr.ReadLine();
                if(cur.Equals(""))
                {
                    continue;
                }
            }

           

        }
        private int computePosi(string name)
        {
            int id; int.TryParse(name, out id);
            int toPosi;
            if (id <= 3)
            {
                toPosi = 100 + 30 * id;//上部线的位置
            }
            else
            {
                if (name.Equals(pids[1]))
                {
                    toPosi = 160;
                }
                else
                {
                    if (name.Equals(pids[2]))
                    {
                        toPosi = 220;
                    }
                    else
                    {
                        toPosi = 250;
                    }
                }
            }
            return toPosi;
        }
        private int checkTask(string word,int fromposi,string name,int check=0)
        {
            Graphics g2 = pictureBox2.CreateGraphics();
            int id; int.TryParse(name, out id);
            int toPosi;
            toPosi=computePosi(name);
            //得到to与from
            if (fromposi > 280)
            {
                int i = 0;
                while (i < 14)
                {
                    if (fromposi - 30 < toPosi||fromposi-30<100)
                    {
                        break;
                    }
                    if ((i != 0) || (check != 1))
                    {
                        g2.FillRectangle(whitebrush, 350, fromposi - 30, 300, 50);
                    }
                    drawArrow(g2, new Point(390, fromposi - 30), new Point(350, fromposi - 30));
                    g2.DrawString("pointer:"+word, mediumf, redbrush, new Point(393, fromposi - 35));
                    fromposi -= 30;
                    i++;
                    Thread.Sleep(300);
                }
                //出来时候已经sleep了很久--fromposi为当前指针位置
            }
            else
            {
                int i = 0;
                while (true)
                {
                    if (fromposi - 30 < toPosi||fromposi-30<100)
                    {
                        break;
                    }
                    if ((i != 0) || (check != 1))
                    {
                        g2.FillRectangle(whitebrush, 350, fromposi - 30, 300, 50);
                    }
                    drawArrow(g2, new Point(390, fromposi - 30), new Point(350, fromposi - 30));
                    g2.DrawString("pointer:"+word, mediumf, redbrush, new Point(393, fromposi - 35));
                    fromposi -= 30;
                    i++;
                    Thread.Sleep(300);
                }
            }

            return fromposi;
        }
        private void gdtInF(string posi,string tssA,string ldtA)
        {
            Graphics g4 = pictureBox3.CreateGraphics();
            g4.DrawString("Change GDT Table", f, blackbush, new Point(500, 10));
            int begin = baseDrawGDT(200, g4,100,posi);
            Thread.Sleep(1000);

            g4.DrawRectangle(p3,600, 200, 180, 400);
            string pageA="";
            for(int i=0;i<4;i++)
            {
                if (pids[i] != null)
                {
                    if (pids[i].Equals(childpid))
                    {
                        pageA = stuctPosi[i];
                    }
                }
            }
            g4.DrawString("physical address: "+pageA, mediumf, redbrush, new Point(600, 180));
            g4.DrawString("task_struct", f, blackbush, new Point(600, 150));
            int pageAI=Convert.ToInt32(pageA, 16);
            int tssAI = Convert.ToInt32(tssA, 16);
            int ldtAI = Convert.ToInt32(ldtA, 16);
            int interv = ldtAI - pageAI;
            int ldtr = interv * 400 / 4096 + 200;
            int ldtn = (tssAI - ldtAI) / 8;
            int tssr = ldtr + ldtn * 20;
            g4.DrawLine(p3, new Point(600, ldtr), new Point(780, ldtr));
            drawArrow(g4, new Point(830, ldtr + 10), new Point(790, ldtr + 10));
            g4.DrawString("LDT table", mediumf, redbrush, new Point(833, ldtr + 5));
            g4.DrawString("address " + ldtA, mediumf, redbrush, new Point(833, ldtr + 25));
            g4.DrawLine(p3, new Point(600, ldtr + ldtn * 20), new Point(780, ldtr + ldtn * 20));

            drawArrow(g4, new Point(830, tssr + 10), new Point(790, tssr + 10));
            g4.DrawString("TSS Section", mediumf, redbrush, new Point(833, tssr + 5));
            g4.DrawString("address " + tssA, mediumf, redbrush, new Point(833, tssr + 25));
            g4.DrawString(ldtn.ToString() + " entry", f, blackbush, new Point(650, ldtr + 20));

            Thread.Sleep(1000);
            //又一次对GDT操作
            //80+20*begin---ldt位置
            g4.FillRectangle(whitebrush, new Rectangle(388, 50 + 20 * begin, 80, 80));
            int pp;int.TryParse(posi, out pp);
            int tssGN = pp - 8;
            g4.DrawString("cur tss", mediumf, redbrush, new Point(210, 63 + 20 * begin));
            g4.DrawString(tssGN.ToString(), mediumf, blackbush, new Point(170, 63 + 20 * begin));

            Thread.Sleep(500);
            g4.DrawLine(p, new Point(390, 70 + 20 * begin), new Point(480, 70 + 20 * begin));
            g4.DrawLine(p, new Point(480, 70 + 20 * begin), new Point(480, tssr));
            drawArrow(g4, new Point(480, tssr), new Point(590, tssr));

            Thread.Sleep(500);
            g4.DrawString("cur ldt", mediumf, redbrush, new Point(210, 83 + 20 * begin));
            g4.DrawString(pp.ToString(), mediumf, blackbush, new Point(170, 83 + 20 * begin));

            Thread.Sleep(500);
            g4.DrawLine(p, new Point(390, 90 + 20 * begin), new Point(480, 90 + 20 * begin));
            g4.DrawLine(p, new Point(480, 90 + 20 * begin), new Point(480, ldtr));
            drawArrow(g4, new Point(480, ldtr), new Point(590, ldtr));

            


        }
        private void Rebuild()
        {
            Graphics g1 = pictureBox1.CreateGraphics();
            int initconter = 1;
            for(initconter=1;initconter<4;initconter++)
            {
                if (pids[initconter] != null)
                {
                    if (states[initconter] == 0)
                    {
                        g1.FillEllipse(greenbush, new Rectangle(200, 10 + 70 * initconter, 50, 50));
                    }
                    else
                    {
                        if (states[initconter] == 1)
                        {
                            g1.FillEllipse(graybrush, new Rectangle(200, 10 + 70 * initconter, 50, 50));

                        }
                        else
                        {
                            if (states[initconter] == 2)
                            {
                                g1.FillEllipse(bluebush, new Rectangle(200, 10 + 70 * initconter, 50, 50));

                            }
                            else
                            {
                                if (states[initconter] == 3)
                                {
                                    g1.FillEllipse(oranbush, new Rectangle(200, 10 + 70 * initconter, 50, 50));

                                }
                                else
                                {
                                    if (states[initconter] == 4)
                                    {
                                        g1.FillEllipse(yellowbrush, new Rectangle(200, 10 + 70 * initconter, 50, 50));

                                    }
                                }
                            }
                        }
                    }

                    g1.DrawString(pids[initconter], f, blackbush, new PointF(215, 25 + 70 * initconter));
                    if (initconter == 3 && process2.isFather1)
                    {
                        g1.DrawLine(p, new Point(200, 35), new Point(180, 35));
                        g1.DrawLine(p, new Point(180, 35), new Point(180, 10+70*initconter + 25));
                        drawArrow(g1, new Point(180, 10+70*initconter + 25), new Point(200, 10+70*initconter+ 25));
                    }
                    else
                    {
                        drawArrow(g1, new Point(225, 60 + 70 * (initconter - 1)), new Point(225, 10 + 70 * initconter));
                    }
                        //initconter++;
                    if (initconter == pidcounter)
                    {
                        break;
                    }
                }
            }
        }
        int rowf;
        int rowt;
        private void CopyPage(string cur)
        {
            
            Graphics p4 = pictureBox3.CreateGraphics();
            
            p4.DrawRectangle(p3, new Rectangle(100, 100, 200, 500));
            p4.DrawString("Directory",f,blackbush,new Point(100,70));
            p4.DrawString("Copy page table", f, blackbush, new Point(500, 10));
            int from;
            int.TryParse(old, out from);
            from = from / 1048576;
            int to;int.TryParse(newe, out to);
            to = to / 1048576;
             rowf= from* 500 / 1024 +100;
             rowt = to* 500 / 1024 +100;
            sr.ReadLine();
            string size = sr.ReadLine();
            
            //页目录项中的位置
            Thread.Sleep(500);
            p4.DrawLine(p3, new Point(100, rowf), new Point(300, rowf));
            p4.DrawString(from.ToString(), mediumf, blackbush, new Point(303, rowf + 3));
            p4.DrawLine(p3, new Point(100, rowf + 20), new Point(300, rowf + 20));
            p4.DrawString("from entry", mediumf, redbrush, new Point(303, rowf - 20));
            drawArrow(p4, new Point(303,rowf-10), new Point(303,rowf+3));


            p4.DrawLine(p3, new Point(100, rowt), new Point(300, rowt));
            p4.DrawString(to.ToString(), mediumf, blackbush, new Point(303, rowt + 3));
            p4.DrawLine(p3, new Point(100, rowt + 20), new Point(300, rowt + 20));
            p4.DrawString("to entry", mediumf, redbrush, new Point(303, rowt - 20));
            drawArrow(p4, new Point(303, rowt - 10), new Point(303, rowt + 3));

            p4.DrawString("number of tables=" + size, mediumf, redbrush, new Point(280, 100));

        }
        int csrow;
        int dsrow;
        private int baseDrawGDT(int col,Graphics g2,int row,string input=null)
        {
            string posi = "";
            if (input == null)
            {
                posi = sr.ReadLine();
            }
            else
            {
                posi = input;
            }

            int pp; int.TryParse(posi, out pp);
            int index = pp / 8;
            g2.DrawRectangle(p3, new Rectangle(col, row, 180, 400));
            g2.DrawString("GDT table", f, blackbush, new Point(col, row-30));
            int begin = 6;

            g2.DrawLine(p3, new Point(col, row+60), new Point(180+col, row+60));
            g2.DrawString("FIRST TSS ENTRY", Smallf, blackbush, new Point(col+10, row+63));
            g2.DrawLine(p3, new Point(col, row+80), new Point(col+180, row+80));
            drawArrow(g2, new Point(col+220, row+70), new Point(col+190, row+70));
            g2.DrawString("32", mediumf, blackbush, new Point(col+220, row+65));

            g2.DrawString("FIRST LDT ENTRY", Smallf, blackbush, new Point(col+10, row+83));
            g2.DrawLine(p3, new Point(col, row+100), new Point(col+180, row+100));
            drawArrow(g2, new Point(col+220, row+90), new Point(col+190, row+90));
            g2.DrawString("40", mediumf, blackbush, new Point(col+220, row+85));
            int task = 1;
            while (begin < index - 1)
            {
                g2.DrawString("tss" + task, Smallf, blackbush, new Point(col+10, row+103 + 20 * (begin - 6)));
                g2.DrawLine(p3, new Point(col, row+100 + 20 * (begin - 5)), new Point(col+180, row+100 + 20 * (begin - 5)));
                begin++;
                g2.DrawString("ldt" + task, Smallf, blackbush, new Point(col+10, row+103 + 20 * (begin - 6)));
                g2.DrawLine(p3, new Point(col, row+100 + 20 * (begin - 5)), new Point(col+180, row+100 + 20 * (begin - 5)));
                begin++; task++;
            }
            g2.DrawLine(p3, new Point(col, row+100+ 20 * (begin - 5)), new Point(col+180, row+100 + 20 * (begin - 5)));
            begin++; 
            g2.DrawLine(p3, new Point(col, row+100 + 20 * (begin - 5)), new Point(col+180, row+100 + 20 * (begin - 5)));
            g2.DrawString(posi, mediumf, blackbush, new Point(col+190, row+75 +  20* (begin - 5)));
            drawArrow(g2, new Point(col + 220, row+95 + 20 * (begin - 5)), new Point(col + 190, row+95 + 20 * (begin - 5)));
            return begin;

        }
        private void NewGDT()//要求输入新插入的ldt的地址（字节为单位）
        {
            Graphics g2 = pictureBox2.CreateGraphics();
            int begin=baseDrawGDT(200,g2,40);
            Thread.Sleep(1000);

            //绘制tss.ldt指向的改变
            //g2.DrawString("ldt" + task, Smallf, blackbush, new Point(210, 143 + 20 * (begin - 6)));
            string structAddress = "";
            for(int i=0;i<4;i++)
            {
                if(pids[i].Equals(childpid))
                {
                    structAddress = stuctPosi[i];
                    break;
                }
            }
            g2.DrawRectangle(p3, new Rectangle(450, 20 * (begin - 5), 160, 300));
            g2.DrawString("p->tss.ldt", mediumf, redbrush, new Point(453, 20 * (begin - 5) + 200));//
            Thread.Sleep(500);

            drawArrow(g2, new Point(430, 135 + 20 * (begin - 5)), new Point(390, 135 + 20 * (begin - 5)));
            g2.DrawLine(p, new Point(430, 135 + 20 * (begin - 5)), new Point(430, 20 * (begin - 5)+200));
            g2.DrawLine(p, new Point(430, 20 * (begin - 5) + 200), new Point(450, 20 * (begin - 5) + 200));
            g2.DrawString("task_struct", f, blackbush, new Point(450, 20 * (begin - 5) - 45));
            g2.DrawString(structAddress, mediumf, blackbush, new Point(450,  20 * (begin - 5) - 20));

            //draw LDT
            Thread.Sleep(500);
            g2.DrawLine(p3, new Point(450, 20 * (begin - 5) + 50), new Point(610, 20 * (begin - 5) + 50));
            g2.DrawLine(p3, new Point(450, 70 + 20 * (begin - 5)), new Point(610, 70 + 20 * (begin - 5)));
            g2.DrawString("0", mediumf, blackbush, new Point(460, 53 + 20 * (begin - 5)));
            g2.DrawString("p->ldt", mediumf, redbrush, new Point(390, 53 + 20 * (begin - 5) - 6));
            Thread.Sleep(500);

            csrow = 70 + 20 * (begin - 5);
            g2.DrawLine(p3, new Point(450, 90 + 20 * (begin - 5)), new Point(610, 90 + 20 * (begin - 5)));
            string cur = sr.ReadLine();
            cur = sr.ReadLine();
            g2.DrawString("cs "+cur, mediumf, blackbush, new Point(460, 73 + 20 * (begin - 5)));
            Thread.Sleep(500);

            dsrow = 90 + 20 * (begin - 5);
            g2.DrawLine(p3, new Point(450, 110 + 20 * (begin - 5)), new Point(610, 110 + 20 * (begin - 5)));
            cur = sr.ReadLine();
            cur = sr.ReadLine();
            g2.DrawString("ds " + cur, mediumf, blackbush, new Point(460, 93 + 20 * (begin - 5)));
            Thread.Sleep(500);
        }
        private void DrawNewProcess()
        {
            Graphics g2 = pictureBox2.CreateGraphics();
            g2.DrawRectangle(p3, new Rectangle(200, 130, 180, 180));
            g2.DrawString("task_struct", f, blackbush, new Point(200, 100));
            drawArrow(g2, new Point(420, 130), new Point(390, 130));
            g2.DrawString("physical address", mediumf, blackbush, new Point(420, 130));
            g2.DrawString(eaxBox.Text, mediumf, blackbush, new Point(420, 150));
            Thread.Sleep(1000);
            drawArrow(g2, new Point(420, 310), new Point(390, 310));
            g2.DrawString("tss.esp0", mediumf, redbrush, new Point(420, 310));
            string adr = sr.ReadLine();
            g2.DrawString(adr, mediumf, blackbush, new Point(420, 330));
            Thread.Sleep(500);
        }
        //设置data for timer2
        Point curPointer;//pointer指向的位置
        int PointerLength;
        Point finalPointer;
        private void get_free_page(Graphics g2,int startCol=200,int type=1)
        {
            g2.DrawRectangle(p3, new Rectangle(startCol, 130,180, 180));
            g2.DrawString("mem_map", f, blackbush, new Point(startCol, 100));
            drawArrow(g2,new Point(startCol+220, 310), new Point(startCol+190, 310));
            g2.DrawString("edi", mediumf, blackbush, new Point(startCol+220, 310));
            curPointer = new Point(startCol+190, 310);
            PointerLength = 70;
            finalPointer = new Point(startCol+190, 250);
            int interv = curPointer.Y - finalPointer.Y;
            int oneT = interv / 9;
            int counter = 0;
            Thread.Sleep(300);
            while(counter<9)
            {
                g2.FillRectangle(whitebrush,new Rectangle(curPointer.X,curPointer.Y-20,500,50));
                drawArrow(g2, new Point(startCol+220, curPointer.Y-oneT), new Point(startCol+190, curPointer.Y-oneT));
                g2.DrawString("edi", mediumf, blackbush, new Point(startCol+220, curPointer.Y-oneT));
                Thread.Sleep(100);
                curPointer = new Point(curPointer.X, curPointer.Y -oneT);
                counter++;
            }
            g2.DrawLine(p3, new Point(startCol, curPointer.Y), new Point(startCol+180, curPointer.Y));
            g2.DrawLine(p3, new Point(startCol, curPointer.Y + 2*oneT), new Point(startCol+180, curPointer.Y + 2*oneT));
            g2.DrawString("0", Smallf, blackbush, new Point(startCol+80, curPointer.Y));
            Thread.Sleep(1000);
            g2.FillRectangle(whitebrush, new Rectangle(startCol+220, curPointer.Y, 40, 20));
            g2.DrawString("find a blank page", mediumf, blackbush, new Point(startCol+220, curPointer.Y));
            g2.FillRectangle(whitebrush, new Rectangle(startCol+80, curPointer.Y, 10, 10));
            g2.DrawString("1", Smallf, blackbush, new Point(startCol+80, curPointer.Y));
            string ecx="";
            string edi="";
            if (type == 1)
            {
                string cur = sr.ReadLine();  ecx = sr.ReadLine();
                sr.ReadLine();edi = sr.ReadLine();
            }
            sr.ReadLine(); string eax = sr.ReadLine();//eax 中是to page table
           
         
            g2.DrawRectangle(p3, new Rectangle(startCol, 340,180,180));
            g2.DrawLine(p, new Point(startCol+240, curPointer.Y+20), new Point(startCol+240, 340));
            g2.DrawLine(p, new Point(startCol+240, 340), new Point(startCol+250, 340));

            g2.DrawString("edx", mediumf, blackbush, new Point(startCol+250, 320));//edx指向
            edxBox.Clear();edxBox.AppendText(eax);
            drawArrow(g2, new Point(startCol+240, 340), new Point(startCol+190, 340));

            g2.DrawString("physical address:" + eax, mediumf, bluebush, new Point(startCol+100, 340));
            
            
            Graphics g3 = pictureBox4.CreateGraphics();
            if (type == 1)
            {
                //加入task struct的地址
                for(int i=0;i<4;i++)
                {
                    if(pids[i].Equals(childpid))
                    {
                        stuctPosi[i] = eax;
                        break;
                    }
                }
                g3.DrawString("address", mediumf, oranbush, new Point(0, 125));
                Thread.Sleep(300);
                ecxBox.Clear(); ecxBox.AppendText("0x400");//ecx修改为1024
                g3.DrawString("counter", mediumf, oranbush, new Point(0, 85));
            }

            curPointer = new Point(startCol+190, 520);
            interv = 180;
            oneT = 180 / 20;
            drawArrow(g2, new Point(startCol+220, 520), new Point(startCol+190, 520));
            g2.DrawString("edi", mediumf, blackbush, new Point(startCol+220, 520));counter = 0;
            while (counter < 20)
            {
                g2.FillRectangle(whitebrush, new Rectangle(curPointer.X, curPointer.Y - 30, 500, 50));
                drawArrow(g2, new Point(startCol+220, curPointer.Y - oneT), new Point(startCol+190, curPointer.Y - oneT));
                g2.DrawString("edi:clear the page", mediumf, blackbush, new Point(startCol+220, curPointer.Y - oneT));
                Thread.Sleep(50);
                curPointer = new Point(curPointer.X, curPointer.Y - oneT);
                counter++;
            }
            eaxBox.Clear();eaxBox.AppendText(eax);
            to_page_table = eax;
            if (type == 1)
            {
                g3.DrawString("address", mediumf, oranbush, new Point(0, 20));
                g3.FillRectangle(whitebrush, new Rectangle(0, 75, 100, 40));

                ediBox.Clear(); ediBox.AppendText(edi);
                ecxBox.Clear(); ecxBox.AppendText(ecx);
            }
            
        }
        private void registerShow(string regis,string inner)
        {
            if(regis.Contains("eax"))
            {
                eaxBox.Clear();
                eaxBox.AppendText(inner);
            }
            else
            {
                if (regis.Contains("ebx"))
                {
                    ebxBox.Clear();
                    ebxBox.AppendText(inner);
                }else
                {
                    if (regis.Contains("ecx"))
                    {
                        ecxBox.Clear();
                        ecxBox.AppendText(inner);
                    }else
                    {
                        if (regis.Contains("edx"))
                        {
                            edxBox.Clear();
                            edxBox.AppendText(inner);
                        }else
                        {
                            if (regis.Contains("ds"))
                            {
                                dsBox.Clear();
                                dsBox.AppendText(inner);
                            }else
                            {
                                if (regis.Contains("esi"))
                                {
                                    esiBox.Clear();
                                    esiBox.AppendText(inner);
                                }else
                                {
                                    if (regis.Contains("fs"))
                                    {
                                        fsBox.Clear();
                                        fsBox.AppendText(inner);
                                    }
                                    else
                                    {
                                        if (regis.Contains("gs"))
                                        {
                                            gsBox.Clear();
                                            gsBox.AppendText(inner);
                                        }else
                                        {
                                            if (regis.Contains("ss"))
                                            {
                                                ssBox.Clear();
                                                ssBox.AppendText(inner);
                                            }else
                                            {
                                                if (regis.Contains("cs"))
                                                {
                                                    csBox.Clear();
                                                    csBox.AppendText(inner);
                                                }else
                                                {
                                                    if (regis.Contains("esp"))
                                                    {
                                                        espBox.Clear();
                                                        espBox.AppendText(inner);
                                                    }
                                                    else
                                                    {
                                                        if (regis.Contains("edi"))
                                                        {
                                                            ediBox.Clear();
                                                            ediBox.AppendText(inner);
                                                        }else
                                                        {
                                                            if (regis.Contains("es"))
                                                            {
                                                                esBox.Clear();
                                                                esBox.AppendText(inner);
                                                            }
                                                            else
                                                            {
                                                                if (regis.Contains("ebp"))
                                                                {
                                                                    ebpBox.Clear();
                                                                    ebpBox.AppendText(inner);
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        //pciture box1部分
        private void buildNewNode()
        {
            Graphics g1 = pictureBox1.CreateGraphics();
           /* g1.FillEllipse(greenbush, new Rectangle(200, 10 + 70 * (pidcounter-1), 50, 50));
            for (int i = 0; i < 4; i++)
            {
                if (pids[i] != null)
                {
                    if (pids[i].Equals(father))
                    {
                        states[i] = 0;
                    }
                }
            }
            g1.DrawString(father, f, blackbush, new PointF(215, 25 + 70 * (pidcounter-1)));*/
            g1.FillEllipse(graybrush, new Rectangle(200, 10 + 70 * pidcounter, 50, 50));
            pids[pidcounter] = childpid;states[pidcounter] = 1;

            g1.DrawString(childpid, f, blackbush, new PointF(215, 25 + 70 * pidcounter));
            drawArrow(g1,new Point(225, 60 + 70 * (pidcounter - 1)), new Point(225, 10 + 70 * pidcounter));
            pidcounter++;
        }
        //stack左上角(20,40)
        int stackroof = 0;//从0记录绘制栈中的内容长度
        int stackcap = 240;


        //picture box2部分
        private void popStack()
        {
            Graphics g2 = pictureBox2.CreateGraphics();
            g2.FillRectangle(whitebrush, new Rectangle(23,50+30*(stackroof-1), 115,30));
            if (stack.Count != 0)
            {
                string value = stack.Peek();
                string draw = drawStack.Peek();
                string[] temp = draw.Split(' ');
                regi[temp[1]] = value;
                registerShow(temp[1], value);
                stack.Pop();
            }
            drawStack.Pop();
            if(50 + 30 * (stackroof - 1)<stackcap-240)
            {
                stacksmaller();
            }
            stackroof--;
            
        }
        private void pushStack(string container)
        {
            Graphics g2 = pictureBox2.CreateGraphics();
            g2.DrawString(container, mediumf, blackbush, new Point(20,50+stackroof*30));
            drawStack.Push(container);
            g2.DrawLine(p3, new Point(20, 73 + stackroof * 30), new Point(140, 73 + stackroof * 30));
            stackroof++;
            if(73 + stackroof * 30>=240)
            {
                stackLonger();
            }
        }
        private void stacksmaller()
        {
            int oldcap = stackcap;
            stackcap -= 240;
            Graphics g2 = pictureBox2.CreateGraphics();
            g2.DrawLine(p3w, new Point(20, oldcap), new Point(20, stackcap));
            g2.DrawLine(p3w, new Point(140, oldcap), new Point(140, stackcap));
        }
        private void stackLonger()
        {
            int oldcap = stackcap;
            stackcap += 240;
            Graphics g2 = pictureBox2.CreateGraphics();
            g2.DrawLine(p3, new Point(20, oldcap), new Point(20, stackcap));
            g2.DrawLine(p3, new Point(140, oldcap), new Point(140, stackcap));
        }
        private void clearStack()
        {
            Graphics g2 = pictureBox2.CreateGraphics();
            g2.Clear(Color.White);
            //恢复第二个绘制页面的内容
            DrawStack();
        }
        private void DrawStack(int length=240)
        {
            stackroof = 0;
            stackcap = 240;
            Graphics g2 = pictureBox2.CreateGraphics();
            Point left = new Point(20, 40);
            Point right = new Point(140, 40);
            g2.DrawLine(p3, left, right);
            Point leftD = new Point(20, length);
            Point rightD = new Point(140, length);
            g2.DrawLine(p3, left, leftD);
            g2.DrawLine(p3, right, rightD);
            stackInMap = true;
        }
        //默认只画平行或竖直的箭头
        private void drawArrow(Graphics g1,Point fromP,Point toP)
        {
           // Graphics g1 = pictureBox1.CreateGraphics();
            g1.DrawLine(p, fromP,toP); 
            Point left;
            Point right; 
            if (fromP.X == toP.X)
            {
                
                if (fromP.Y < toP.Y)
                {
                    left= new Point(toP.X - 10, toP.Y - 10);
                    right= new Point(toP.X + 10, toP.Y - 10);
                }else
                {
                    left = new Point(toP.X - 10, toP.Y +10);
                    right = new Point(toP.X + 10, toP.Y + 10);
                }
                g1.DrawLine(p, left, toP);
                g1.DrawLine(p, right, toP);
            }else
            {
                if(fromP.Y==toP.Y)
                {
                    if(fromP.X<toP.X)
                    {
                        left = new Point(toP.X - 10, toP.Y - 10);
                        right = new Point(toP.X - 10, toP.Y + 10);
                    }else
                    {
                        left = new Point(toP.X + 10, toP.Y - 10);
                        right = new Point(toP.X + 10, toP.Y + 10);
                    }
                    g1.DrawLine(p, left, toP);
                    g1.DrawLine(p, right, toP);
                }
                else
                {
                    Console.WriteLine("Error:the Arrow isn't valid"); 
                }
            }
           
        }

        //



        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
