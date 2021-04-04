#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
int main()
{
    FILE *fp=fopen("test","a+");
    int pid;
    if(!(pid=fork()))
    {
    int i=0;
    while(1)
    {
       printf("input B\n");
       fputs("B",fp);
       i++;
       if(i>10)
       {
           break;
       }
    }
    }
    if(pid>0)
    {
        int i=0;
    while(1)
    {
       printf("input A\n");
       fputs("A",fp);
       i++;
       if(i>10)
       {
           break;
       }
       }
    }
    fclose(fp);
    return 0;
}
