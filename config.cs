using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace scanNetwork
{
    class config
    {
        //起始計算的ip
        private string _ip = "127.0.0.1";
        //起始計算的mask
        private string _range = "D";
        //保存要檢查的ip列表
        private List<string> _iplist = new List<string>();
        //多執行續的數量
        private int _maxThread = 10;
        

        /// <summary>
        /// 起始點的ip,預設 127.0.0.1
        /// </summary>
        public string ip {
            get { return _ip; }
            set { _ip = value; }
        }
        /// <summary>
        /// 設定範圍 C or D,預設 D
        /// </summary>
        public string range {
            get { return _range; }
            set { _range = value; }
        }
        /// <summary>
        /// 執行續的最大數量,預設 10
        /// </summary>
        public int maxThread {
            get { return _maxThread; }
            set { _maxThread = value;}
        }



        /// <summary>
        /// 取得待檢查的ip列表
        /// </summary>
        public Boolean createList() {
            try
            {
                bool result = false;
                string range = this._range;
                switch (range.ToLower())
                {
                    case "c":
                        for (int i = 1; i < 254; i++)
                        {
                            for (int j = 1; j < 254; j++)
                            {
string[] cut = Regex.Split(_ip,@"\.");
                                string thisIP = cut[0] + "." + cut[1] + "." + i + "." + j;
                                _iplist.Add(thisIP);
                            }
                        }
                        result = true;
                        break;

                    case "d":
                        for (int i = 1; i < 254; i++)
                        {
                                string[] cut = Regex.Split(_ip,@"\.");
                                string thisIP = cut[0] + "." + cut[1] + "." + cut[2] + "." + i;
                                _iplist.Add(thisIP);
                        }
                        result = true;
                        break;
                    default:
                        result = false;
                        break;
                }
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
        
    }
}
