using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.IO;


namespace scanNetwork
{
    class Program
    {
        private static config config = new config();
        //回收的多執行續數量
        private static int backcount = 0;
        //重試過的次數
        private static int retrytimes = 0;
        //確定回應的數量
        private static int existcount = 0;

        static void Main(string[] args)
        {
            //耗時計算
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Reset();
            sw.Start();
            Console.WriteLine("紀錄位置："+config.logpath);
            //讀取輸入的資料
            Readinput();
            //多執行續啟動 設定執行緒續數量
            ThreadPool.SetMinThreads(config.maxThread, config.maxThread);


            foreach (string item in config.getList())
            {
                //Console.WriteLine(item);
                ThreadPool.QueueUserWorkItem(new WaitCallback(checkPing), item);
            }

            while (true)
            {
                if (config.listlen() == backcount)
                {
                    Console.WriteLine("done!");
                    Console.WriteLine("執行了{0}次", backcount.ToString());
                    Console.WriteLine("重試了{0}次", retrytimes.ToString());
                    Console.WriteLine("回應數量:{0}", existcount.ToString());
                    break;
                }
                Thread.Sleep(300);
            }
            sw.Stop();
            Console.WriteLine("耗時：{0} ms",((int)sw.Elapsed.TotalMilliseconds).ToString()); 
            Console.WriteLine("按下任意鍵結束...");
            Console.ReadKey();
        }

        //多執行續處理的部分
        private static void checkPing(object obj) {
            
            try
            {
                string ip = (string)obj;
                
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                int timeout = config.pingTimeout;
                PingReply reply = pingSender.Send(ip,timeout);
                if (reply.Status == IPStatus.Success)
                {
                    File.AppendAllText(config.logpath, obj.ToString()+"\n");
                    Console.Write(obj.ToString() + " => ");
                    Console.WriteLine("Pong ! {0},time:{1}", reply.Address.ToString(), reply.RoundtripTime);
                    existcount++;
                }
                else
                {
                    Console.WriteLine(obj.ToString() + " => ");
                }
                backcount++;
            }
            catch (Exception e)
            {
                Console.WriteLine("異常回應，進行重試 ! {0}",(string)obj);
                retrytimes++;
                ThreadPool.QueueUserWorkItem(new WaitCallback(checkPing), (string)obj);
            }
            
        }
        







        private static void Readinput(){
            Console.WriteLine("輸入起點ipv4,預設是"+config.ip);
            string inputip = Console.ReadLine();
            if (inputip != "")
            {
                config.ip = inputip;
            }
            Console.WriteLine("輸入終點ipv4,預設是" + config.endip);
            string inputendip = Console.ReadLine();
            if (inputendip != "")
            {
                config.endip = inputendip;
            }
            Console.WriteLine("輸入允許執行thread的最大數量,預設是"+config.maxThread.ToString());
            string inputmaxThread = Console.ReadLine();
            if (inputmaxThread!="")
            {
                config.maxThread = int.Parse(Regex.Replace(inputmaxThread,"[^0-9]",""));
            }
            //創造清單
            if (config.createList()!=true) Console.WriteLine("fail...");
            
            //Console.ReadLine();

        }
    }
}
