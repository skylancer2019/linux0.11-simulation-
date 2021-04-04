import java.io.*;
public class transferOS {

int state=0;
static FileReader fr;
static BufferedReader br;
static FileWriter fw;
static int num=0;
static int scheNUm=0;
static boolean flag=false;
static String ScheStr="";
static boolean isgetInto=false;
static boolean register_flag=false;
	public static void main(String[] args) throws IOException {
		// TODO Auto-generated method stub
		fr=new FileReader("D://gdb_output16.txt");
		br=new BufferedReader(fr);
		fw=new FileWriter("D://new_datan.txt");	
			while(true)
			{
				String cur=br.readLine();
				if(cur==null)
				{
					break;
				}
				/*人工得到具体需要再分析的区域*/
				if(cur.contains("-------------"))
				{
					if(isgetInto)
					{
						isgetInto=false;
						break;
					}else
					{
						isgetInto=true;
						cur=br.readLine();
					}
				}
				if(!isgetInto)
				{
					continue;
				}
				if(cur.contains("Breakpoint")||cur.contains("at"))
				{
					if(cur.contains("Breakpoint 69"))
					{
						System.out.println("rr");
					}
					solveOnePoint();
					System.out.println("FINISH ONE:"+num);
					num++;
				}else
				{
					if(cur.equals(""))
					{
						//fw.write(cur+"\n");
						continue;
					}else
					{
						System.out.println("Error:partition Fault");
					}
				}
			}
			fr.close();
			br.close();
			fw.close();
			
			fr=new FileReader("D://new_datan.txt");
			br=new BufferedReader(fr);
			fw=new FileWriter("D://real_datan.txt");
			int i=0;
			while(true)
			{
				
				String cur=br.readLine();
				
				if(cur==null)
				{
					break;
				}
			    if(cur.equals(""))
				{
					continue;
				}else
				{
					if(i==949)
					{
						System.out.println("check");
					}
					DDDschedule(cur);System.out.println("Second process"+i);i++;
				}
				
			}
			fr.close();
			br.close();
			fw.close();
			fr=new FileReader("D://real_datan.txt");
			br=new BufferedReader(fr);
			fw=new FileWriter("D://input_datan.txt");
			i=0;String cur="";
			while(true)
			{
				if(cur==null)
				{
					break;
				}
				cur=br.readLine();
				
				if(cur==null)
				{
					break;
				}
			    if(cur.equals(""))
				{
					continue;
				}else
				{
					if(i==622)
					{
						System.out.println("check");
					}
					int a=DDLschedule(cur);System.out.println("Third process"+i);i++;
					if(a==1)
					{
						break;
					}
					fw.write("\n");
				}
				
			}
			fr.close();
			br.close();
			fw.close();
			
	}
	static int retFrom=0;
	static boolean eaxre=false;
	static int signal=0;
	static int copy_page=0;//指示当前页面前面没有copy_page_tables
	static int schedule=0;
	static int jumpchange=0;
	static int Dohd=0;
	static int copyString=0;
	static int hd_out_before=0;
	static int sys_ioctl=0;
	static String pre="";
	public static int DDLschedule(String in)throws IOException
	{
		String cur=in;//跳过无用数据
		String Nouse=cur;
		String store="";
		store=br.readLine();
		if(store==null)
		{
			System.out.println("Store is null error");
			return 1;
		}
		cur=br.readLine();
		if(pre.equals(""))
		{
			pre=Nouse;
		}
		if(store.contains("ret_from_sys"))
		{
			if(!pre.contains("97"))
			{
				while(!cur.equals(""))
				{
					cur=br.readLine();
				}
				return 2;
			}
		}
		fw.write(Nouse+"\n");
		fw.write(store+"\n");
		
		while(!cur.equals(""))
		{
			fw.write(cur+"\n");
			cur=br.readLine();
			if(cur==null)
			{
				
				return 1;
			}
		}
		pre=Nouse;
		return 0;
	}
	public static void DDDschedule(String in)throws IOException
	{
		if(copy_page==0&&schedule==0&&signal==0&&retFrom==0&&Dohd==0&&copyString==0&&sys_ioctl==0)
		{
			fw.write("\n");
		}
		String cur=in;//跳过无用数据
		String Nouse=cur;
		String store="";
		store=br.readLine();
		if(store==null)
		{
			System.out.println("Store is null error");
			return;
		}
		cur=br.readLine();
		if(store.contains("hd_out"))
		{
			hd_out_before=1;
		}
		
		if(store.contains("copy_page_tables"))//进入新的syscall
		{	
			
			if(schedule==1)
			{
				fw.write("\n");
				schedule=0;
			}
			if(signal==1)
			{
				fw.write("\n");
				signal=0;
			}
			if(retFrom==1)
			{
				fw.write("\n");
				retFrom=0;
			}
			if(Dohd==1)
			{
				fw.write("\n");
				Dohd=0;
			}
			if(copyString==1)
			{
				fw.write("\n");
				copyString=0;
				
			}
			if(sys_ioctl==1)
			{
				fw.write("\n");
				sys_ioctl=0;
			}
			if(copy_page==0)
			{
				fw.write(Nouse+"\n");
				fw.write(store+"\n");
				fw.write(cur+"\n");
				cur=br.readLine();
				fw.write(cur+"\n");
				cur=br.readLine();
			}else
			{
				cur=br.readLine();
				cur=br.readLine();				
			}//____pid项目之后的值			
			while(!cur.equals(""))
			{
				fw.write(cur+"\n");
				cur=br.readLine();
			}
			copy_page=1;
		}else
		{//非考虑情况需要正常输出
			if(store.contains("in schedule"))
			{
				if(copy_page==1)
				{
					fw.write("\n");
					copy_page=0;
				}
				if(signal==1)
				{
					fw.write("\n");
					signal=0;
				}
				if(retFrom==1)
				{
					fw.write("\n");
					retFrom=0;
				}
				if(Dohd==1)
				{
					fw.write("\n");
					Dohd=0;
				}
				if(copyString==1)
				{
					fw.write("\n");
					copyString=0;
					
				}
				if(sys_ioctl==1)
				{
					fw.write("\n");
					sys_ioctl=0;
				}
				if(schedule==0)
				{
					fw.write(Nouse+"\n");
					fw.write(store+"\n");
					fw.write(cur+"\n");
				}
				if(cur.contains("process loop for alarm"))
				{
					while(!cur.equals(""))
					{
						cur=br.readLine();
					}
				}
				while(!cur.equals(""))
				{
					fw.write(cur+"\n");
					cur=br.readLine();
				}
				schedule=1;
			}else
			{
				if(store.contains("sys_signal"))
				{
					if(copy_page==1)
					{
						fw.write("\n");
						copy_page=0;
					}
					if(schedule==1)
					{
						fw.write("\n");
						schedule=0;
					}
					if(retFrom==1)
					{
						fw.write("\n");
						retFrom=0;
					}
					if(Dohd==1)
					{
						fw.write("\n");
						Dohd=0;
					}
					if(copyString==1)
					{
						fw.write("\n");
						copyString=0;
						
					}
					if(sys_ioctl==1)
					{
						fw.write("\n");
						sys_ioctl=0;
					}
					if(signal==0)
					{
						fw.write(Nouse+"\n");
						fw.write(store+"\n");
						fw.write(cur+"\n");
						cur=br.readLine();
						fw.write(cur+"\n");
						cur=br.readLine();
					}else
					{
						cur=br.readLine();
						cur=br.readLine();				
					}//____pid项目之后的值			
					while(!cur.equals(""))
					{
						fw.write(cur+"\n");
						cur=br.readLine();
					}
					signal=1;
				}else
				{
					if(store.contains("ret_from_sys"))
					{
						if(copy_page==1)
						{
							fw.write("\n");
							copy_page=0;
						}
						if(schedule==1)
						{
							fw.write("\n");
							schedule=0;
						}
						if(signal==1)
						{
							fw.write("\n");
							signal=0;
						}
						if(Dohd==1)
						{
							fw.write("\n");
							Dohd=0;
						}
						if(copyString==1)
						{
							fw.write("\n");
							copyString=0;
							
						}
						if(sys_ioctl==1)
						{
							fw.write("\n");
							sys_ioctl=0;
						}
						if(retFrom==0)
						{
							fw.write(Nouse+"\n");
							fw.write(store+"\n");
							fw.write(cur+"\n");
							cur=br.readLine();
							fw.write(cur+"\n");
							cur=br.readLine();
						}else
						{
							cur=br.readLine();
							cur=br.readLine();				
						}//____pid项目之后的值			
						while(!cur.equals(""))
						{
							fw.write(cur+"\n");
							cur=br.readLine();
						}
						retFrom=1;
					}else
					{	
						if(store.contains("do_hd_request"))
						{
							if(hd_out_before==1)
							{
								while(!cur.equals(""))
								{
									cur=br.readLine();
								}
								br.readLine();
								hd_out_before=0;
								return;
							}
							if(copy_page==1)
							{
								fw.write("\n");
								copy_page=0;
							}
							if(schedule==1)
							{
								fw.write("\n");
								schedule=0;
							}
							if(signal==1)
							{
								fw.write("\n");
								signal=0;
							}
							if(retFrom==1)
							{
								fw.write("\n");
								retFrom=0;
							}
							if(copyString==1)
							{
								fw.write("\n");
								copyString=0;
								
							}
							if(sys_ioctl==1)
							{
								fw.write("\n");
								sys_ioctl=0;
							}
							
							if(Dohd==0)
							{
								fw.write(Nouse+"\n");
								fw.write(store+"\n");
								fw.write(cur+"\n");
								cur=br.readLine();
								fw.write(cur+"\n");
								cur=br.readLine();
							}else
							{
								cur=br.readLine();
								cur=br.readLine();				
							}//____pid项目之后的值			
							while(!cur.equals(""))
							{
								fw.write(cur+"\n");
								cur=br.readLine();
							}
							Dohd=1;
						}else
						{
							if(store.contains("copy_string"))
							{
								if(copy_page==1)
								{
									fw.write("\n");
									copy_page=0;
								}
								if(schedule==1)
								{
									fw.write("\n");
									schedule=0;
								}
								if(signal==1)
								{
									fw.write("\n");
									signal=0;
								}
								if(retFrom==1)
								{
									fw.write("\n");
									retFrom=0;
								}
								if(Dohd==1)
								{
									fw.write("\n");
									Dohd=0;
									
								}
								if(sys_ioctl==1)
								{
									fw.write("\n");
									sys_ioctl=0;
								}
								if(copyString==0)
								{
									fw.write(Nouse+"\n");
									fw.write(store+"\n");
									fw.write(cur+"\n");
									cur=br.readLine();
									fw.write(cur+"\n");
									cur=br.readLine();
								}else
								{
									cur=br.readLine();
									cur=br.readLine();				
								}//____pid项目之后的值			
								while(!cur.equals(""))
								{
									fw.write(cur+"\n");
									cur=br.readLine();
								}
								copyString=1;
							}else
							{
								if(store.contains("sys_ioctl"))
								{
									if(schedule==1)
									{
										fw.write("\n");
										schedule=0;
									}
									if(signal==1)
									{
										fw.write("\n");
										signal=0;
									}
									if(retFrom==1)
									{
										fw.write("\n");
										retFrom=0;
									}
									if(Dohd==1)
									{
										fw.write("\n");
										Dohd=0;
									}
									if(copy_page==1)
									{
										fw.write("\n");
										copy_page=0;
									}
									if(copyString==1)
									{
										fw.write("\n");
										copyString=0;
										
									}
									if(sys_ioctl==0)
									{
										fw.write(Nouse+"\n");
										fw.write(store+"\n");
										fw.write(cur+"\n");
										cur=br.readLine();
										fw.write(cur+"\n");
										cur=br.readLine();
									}else
									{
										cur=br.readLine();
										cur=br.readLine();				
									}//____pid项目之后的值			
									while(!cur.equals(""))
									{
										fw.write(cur+"\n");
										cur=br.readLine();
									}
									sys_ioctl=1;
									
								}else
								{
									if(copy_page==1)
									{
										copy_page=0;
										
										signal=0;
										fw.write("\n");
									}
									if(schedule==1)
									{
										schedule=0;
										fw.write("\n");
									}
									if(signal==1)
									{
										signal=0;
										fw.write("\n");
									}
									if(retFrom==1)
									{
										retFrom=0;
										fw.write("\n");
									}
									if(Dohd==1)
									{
										Dohd=0;
										fw.write("\n");
									}
									if(sys_ioctl==1)
									{
										sys_ioctl=0;
										fw.write("\n");
									}
									if(copyString==1)
									{
										copyString=0;
										fw.write("\n");
									}
									
									fw.write(Nouse+"\n");
									fw.write(store+"\n");
									while(!cur.equals(""))
									{
										fw.write(cur+"\n");
									
										cur=br.readLine();
									}
								}

							
							}
							
						}
					}
				}
			}
			
		}
		if(!store.contains("hd_out"))
		{
			hd_out_before=0;
		}
	}
	public static void DOSchedule(String in) throws IOException
	{
		String cur=in;//跳过无用数据
		String Nouse=cur;
		String store="";
		store=br.readLine();
		if(store==null)
		{
			System.out.println("Store is null error");
			return;
		}
		cur=br.readLine();
		if(store.contains("in schedule"))//进程调度的task switch to 0的一堆切换删除掉
		{
			if(Nouse.contains("111"))
			{	
				ScheStr=Nouse+"\n"+store+"\n"+cur+"\n";
				flag=false;
			}else
			{
				if(Nouse.contains("141"))
				{
					String curr=br.readLine();
					
					if(flag==true)//有中间节点//默认当前不是pause
					{
						fw.write(Nouse+"\n");
						fw.write(store+"\n");
						fw.write(cur+"\n");
						fw.write(curr+"\n");
						flag=false;
						fw.write("\n");
					}else//没有中间节点
					{
						if(curr.contains("= 0"))
						{//当前为pause
							if(scheNUm==0)//前面没有pause
							{
								fw.write(ScheStr+"\n");
								ScheStr="";
								fw.write(Nouse+"\n");
								fw.write(store+"\n");
								fw.write(cur+"\n");
								fw.write(curr+"\n");
								scheNUm++;
								fw.write("\n");
							}else//前面有pause----第一个pause输出ScheStr内容
							{
								scheNUm++;
							}
						}else//当前不是pause
						{
							//默认有中间节点
							System.out.println("Error: Schedule Sequence Error");
						}
					}
					
					
				}else//有中间节点时候
				{
					flag=true;
					if(scheNUm!=0)
					{
						fw.write("pause accur NUM= "+scheNUm+"\n\n");
						scheNUm=0;
					}
					fw.write(ScheStr+"\n");
					fw.write(Nouse+"\n");
					fw.write(store+"\n");
					fw.write(cur+"\n");
					while(!cur.equals(""))
					{
						cur=br.readLine();
						if(!cur.equals(""))
						{
							fw.write(cur+"\n");
						}
					}
					fw.write("\n");
					
				}
			}
		}else
		{//其他字段时
			if(scheNUm!=0)
			{
				fw.write("pause accur NUM= "+scheNUm+"\n\n");
				scheNUm=0;
			}
			flag=false;
			scheNUm=0;
			fw.write(Nouse+"\n");
			fw.write(store+"\n");
			fw.write(cur+"\n");
			while(!cur.equals(""))
			{
				cur=br.readLine();
				if(!cur.equals(""))
				{
					fw.write(cur+"\n");
				}
			}
			fw.write("\n");
		}
	}
	
	public static void solveOnePoint() throws IOException
	{
		register_flag=false;
		String cur=br.readLine();//跳过无用数据
		String Nouse=cur;
		String store="";
		store=br.readLine();
		if(store==null)
		{
			System.out.println("Store is null error");
			return;
		}
		cur=br.readLine();
					
			if(!cur.equals("___pid"))//特判几个没有输出pid与进程切换相关的断点
			{
			
				fw.write(Nouse+"\n");
				fw.write(store+"\n");
				fw.write(cur+"\n");
				boolean firstBeBlank=true;
				while(!cur.equals(""))
				{
					cur=br.readLine();
					if(!cur.equals(""))
					{
						firstBeBlank=false;
						if(cur.contains("="))
						{
							String temp[]=cur.split("=");
							cur=temp[temp.length-1];
							if(cur.contains("{"))
							{
								temp=cur.split("{");
								cur=temp[1];
								temp=cur.split(",");
								cur=temp[0];
							}else
							{
								temp=cur.split(" ");
								if(store.contains("copy_string")||Nouse.contains("e_uid = "))
								{
									cur=temp[1];
								}else
								{
									cur=temp[temp.length-1];
								}
							}
		
							
						}
						if(cur.contains("register")||cur.contains("eax:physical")||cur.contains("eax as"))
						{
							register_flag=true;
						}else
						{
							if(register_flag)
							{
								String temp[]=cur.split("0x");
								char str[]=temp[1].toCharArray();
								int i=0;
								String real="";
								while(true)
								{
									char cc=str[i];
									if(cc!=' '&&cc!='\t')
									{
										real=real+String.valueOf(cc);
									}else
									{
										break;
									}
									i++;
									if(i>=7)
									{
										break;
									}
									
								}
								cur="0x"+real;
								register_flag=false;
							}
						}
						fw.write(cur+"\n");
					}else
					{
						if(firstBeBlank)
						{
						    return;
						}
					}
				}
				fw.write("\n");
				return;
			}
			//=号行读取
			cur=br.readLine();
			String[] temp=cur.split("=");
			if(temp[1].equals(" 6")||temp[1].equals(" 7")||temp[1].equals(" 8")||(temp[1].equals(" 4"))||(temp[1].equals(" 1")))
			{
				fw.write(Nouse+"\n");
				fw.write(store+"\n");
				fw.write("___pid\n");
				fw.write(cur+"\n");
				if(cur.contains("19555"))
				{
					System.out.println("check");
				}
				if(store.contains("copy_string"))
					{
						System.out.println("check");
					}
				while(!cur.equals(""))
				{
					
					cur=br.readLine();
					if(!cur.equals(""))
					{ 
						if(cur.contains("="))
						{
							String[] temp1=cur.split("=");
							cur=temp1[temp1.length-1];
							if(cur.contains("{"))
							{
								temp=cur.split(",");
								cur=temp[0];
								cur=cur.substring(2,cur.length());
							}else
							{
								temp=cur.split(" ");
								if(store.contains("copy_string")||Nouse.contains("e_uid = "))
								{
									cur=temp[1];
								}else
								{
									cur=temp[temp.length-1];
								}
							}
						
						}
							if(cur.contains("register")||cur.contains("eax:physical")||cur.contains("eax as"))
							{
								register_flag=true;
							}else
							{
								if(register_flag)
								{
									String temp2[]=cur.split("0x");
									char str[]=temp2[temp2.length-1].toCharArray();
									
									int i=0;
									String real="";
									while(true)
									{
										char cc=str[i];
										if(cc!=' '&&cc!='\t')
										{
											real=real+String.valueOf(cc);
										}else
										{
											break;
										}
										i++;
										if(i>=9||i>=temp2[temp2.length-1].length())
										{
											if(i>=9)
											{
												System.out.println("real esixt----------------------------");
											}
											break;
										}
						
										
									}
									cur="0x"+real;
									register_flag=false;
								}
							}
						}
						fw.write(cur+"\n");
					
				}
				fw.write("\n");
				
			}else
			{
				while(!cur.equals(""))
				{
					cur=br.readLine();
				}
			}
			
		
	}

}
