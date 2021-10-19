using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FileTransfer
{
    class IpUtil
    {
        /// <summary>
        /// 获取本地ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

        /// <summary>
        /// 产生随机端口
        /// </summary>
        /// <returns></returns>
        public static int GetRandomPort()
        {


            return new Random().Next(1000) + 5000;
        }
    }
}
