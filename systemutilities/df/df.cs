using System;
using JFG.SysUtils;
using JFG.SysUtils.OSutils;

namespace df
{
    internal class df
    {
        private static void Main(string[] args)
        {
            diskInformation di;
            string drv;
            di_struct dis;

            for (char c = 'a'; c <= 'z'; c++)
            {
                dis.driveName = dis.driveType = dis.volumeName = "";
                dis.usedSpace = dis.totalSpace = dis.freeSpace = 0;
                bool res;
                drv = new string(c, 1);
                di = new diskInformation(drv);

                res = di.getInfo(drv, ref dis);

                if (res == false)
                    continue;

                Console.WriteLine("{0} : {1}\t({2})\t{3} MB libres\t{4} MB total\t({5,3:F}% libre).", dis.driveName.ToUpper(), dis.volumeName, dis.driveType,
                    sysutils.SI(dis.freeSpace / 1024 / 1024), sysutils.SI(dis.totalSpace / 1024 / 1024), (float)(dis.freeSpace / (float)dis.totalSpace) * 100);
            }
        }
    }
}