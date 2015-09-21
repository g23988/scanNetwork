using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SnmpSharpNet;
using System.Text.RegularExpressions;

namespace scanNetwork
{
    class snmp
    {
        //snmp識別身分
        private string _community = System.Configuration.ConfigurationManager.AppSettings["communityName"];
        //snmp版本
        private string _version = "1";
        //snmp目標
        private string _ip = "";

        public string community {
            get { return _community; }
            set { _community = value; }
        }
        public string version {
            get { return _version; }
            set { _version = value; }
        }
        public string ip {
            get { return _ip; }
            set { _ip = value; }
        }

        public string getHostname(string ip) {
            string hostname = null;
            try
            {
                OctetString comm = new OctetString(_community);
                AgentParameters param = new AgentParameters(comm);
                param.Version = SnmpVersion.Ver1;
                IpAddress agent = new IpAddress(ip);
                UdpTarget target = new UdpTarget((IPAddress)agent,161,2000,1);
                Pdu pdu = new Pdu(PduType.Get);
                pdu.VbList.Add("1.3.6.1.2.1.1.5.0");
                SnmpV1Packet result = (SnmpV1Packet)target.Request(pdu, param);

                if (result!=null)
                {
                    if (result.Pdu.ErrorStatus!= 0)
                    {
                        hostname = null;
                    }
                    else
                    {
                        string resultPdu = pdu.VbList[0].Value.ToString();
                        string[] cutPdu = Regex.Split(resultPdu, @"\.");
                        hostname = cutPdu[0];
                    }
                }
                else
                {
                    hostname = null;
                }
            }
            catch (Exception)
            {
                hostname = null;
            }

            return hostname;
        }

    }
}
