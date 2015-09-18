using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;


namespace scanNetwork
{
    class Program
    {
        private static config config = new config();
        //回收的多執行續數量
        private static int backcount = 0;

        static void Main(string[] args)
        {
            //讀取輸入的資料
            Readinput();
            //多執行續啟動
            ThreadPool.SetMaxThreads(config.maxThread,config.maxThread);

            foreach (string item in config.getList())
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(checkPing), item);
            }

            while (true)
            {
                if (config.listlen() == backcount)
                {
                    Console.WriteLine("done!");
                    Console.WriteLine(backcount.ToString());
                    Console.ReadLine();
                    break;
                }
            }
        }

        //多執行續處理的部分
        private static void checkPing(object obj) {
            string ip = (string)obj;
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            
            //Console.WriteLine((string)obj);
            backcount++;
        }
        







        private static void Readinput(){
            Console.WriteLine("輸入本地ipv4,預設是"+config.ip);
            string inputip = Console.ReadLine();
            if (inputip != "")
            {
                config.ip = inputip;
            }
            Console.WriteLine("輸入需要的範圍,class C or D,預設是"+config.range);
            string inputrange = Console.ReadLine();
            if (inputrange!="")
            {
                config.range = inputrange;    
            }
            Console.WriteLine("輸入允許執行thread的最大數量,預設是"+config.maxThread.ToString());
            string inputmaxThread = Console.ReadLine();
            if (inputmaxThread!="")
            {
                config.maxThread = int.Parse(inputmaxThread);
            }
            //創造清單
            if (config.createList())
            {
                foreach (string item in config.getList())
                {
                    //Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("fail...");
            }
            

            //Console.ReadLine();

        }
    }
}
