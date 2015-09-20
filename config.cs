using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

namespace scanNetwork
{
    class config
    {

        //起始計算的ip
        private string _ip = System.Configuration.ConfigurationManager.AppSettings["default_ip"];
        
        //終點計算的ip
        private string _endip = System.Configuration.ConfigurationManager.AppSettings["default_endip"];

        //保存要檢查的ip列表
        private List<string> _iplist = new List<string>();
        //多執行續的數量
        private int _maxThread = int.Parse(System.Configuration.ConfigurationManager.AppSettings["minThread"]);
        //寫log的檔名
        private string _logpath = DateTime.Now.ToString("ddMMyyyy-hhmm")+ ".log";

        //ping的timeout時間
        private int _pingTimeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["pingTimeout"]);

        

        /// <summary>
        /// 起始點的ip,預設 192.168.1.1
        /// </summary>
        public string ip {
            get { return _ip; }
            set { _ip = value; }
        }
        /// <summary>
        /// 結束點的ip 預設 192.168.1.254
        /// </summary>
        public string endip {
            get { return _endip; }
            set { _endip = value; }
        }

        /// <summary>
        /// 執行續的最大數量,預設 10
        /// </summary>
        public int maxThread {
            get { return _maxThread; }
            set { _maxThread = value;}
        }
        /// <summary>
        /// log檔名
        /// </summary>
        public string logpath {
            get { return _logpath; }
            set { _logpath = value; }
        }

        /// <summary>
        /// ping 的 timeout時間
        /// </summary>
        public int pingTimeout {
            get { return _pingTimeout; }
            set { _pingTimeout = value; }
        }


        /// <summary>
        /// 取得待檢查的ip列表
        /// </summary>
        public Boolean createList() {
            try
            {
                bool result = false;
                genIPList(this.ip,this.endip);
                result = true;
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 取得所有ip列表
        /// </summary>
        /// <returns></returns>
        public List<string> getList() {
            return _iplist;
        }
        /// <summary>
        /// 取得ip列表的數量
        /// </summary>
        /// <returns></returns>
        public int listlen() {
            return _iplist.Count();
        }

        /// <summary>
        /// 產生範圍內的ip清冊
        /// </summary>
        /// <param name="startIP">起始ip</param>
        /// <param name="endIP">終點ip</param>
        /// <returns></returns>
        private Boolean genIPList(string startIP,string endIP) {
            bool result = false;
            try
            {
                string[] cutStart = Regex.Split(startIP, @"\.");
                string[] cutEnd = Regex.Split(endIP, @"\.");
                //組出bclass 例如 192.168
                string bclass = cutStart[0] + "." + cutStart[1]; 
                //創造清單
                if (Convert.ToInt32(cutEnd[2]) >= Convert.ToInt32(cutStart[2]))
                {
                    //class d起點
                    int classd = Convert.ToInt32(cutStart[3]);
                    for (int i = Convert.ToInt32(cutStart[2]); i <= Convert.ToInt32(cutEnd[2]); i++)
                    {
                        for (int j = classd; j <= ((i == Convert.ToInt32(cutEnd[2])) ? Convert.ToInt32(cutEnd[3]) : 254); j++)
                            {
                                string thisIP = bclass + "." + i + "." + j;
                                _iplist.Add(thisIP);
                            }
                        classd = 1;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        
    }
}
