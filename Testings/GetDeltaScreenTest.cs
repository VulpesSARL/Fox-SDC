using FoxSDC_Agent;
using FoxSDC_Agent.Redirs;
using FoxSDC_Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Testings
{
    [TestClass]
    public class GetDeltaScreenTest
    {
        [TestMethod]
        public void TestGetDeltaScreen()
        {
#if DEBUG
            if (Directory.Exists(PipeScreenData.Framebufferpath) == false)
            {
                Assert.Inconclusive("Cannot find " + PipeScreenData.Framebufferpath);
                return;
            }

            List<string> FileList = Directory.EnumerateFiles(PipeScreenData.Framebufferpath, "*.png", SearchOption.TopDirectoryOnly).ToList();
            if (FileList == null || FileList.Count == 0)
            {
                Assert.Inconclusive("No PNG files in " + PipeScreenData.Framebufferpath);
                return;
            }

            PipeScreenData psd = new PipeScreenData();
            decimal sz = 0;
            decimal tz = 0;

            for (int i = 0; i < FileList.Count; i++)
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                PushScreenData s = psd.GetDeltaScreenDebug();
                byte[] d = MainScreenSystem.ProcessPushScreenData(s);
                st.Stop();
                tz += st.ElapsedMilliseconds;
                sz += d.Length;
            }

            sz /= (decimal)FileList.Count;
            tz /= (decimal)FileList.Count;

            Console.WriteLine("Pictures: " + FileList.Count + " - AVG SZ=" + sz.ToString("0.00") + " bytes - AVG TZ= " + tz.ToString("0.0000") + " ms");
            Assert.AreEqual(0, 0);
#else
            Assert.Inconclusive("Unavalible in Release");
#endif
        }

        [TestMethod]
        public void TestGetDeltaScreen_via_WCF()
        {
#if DEBUG
            if (Directory.Exists(PipeScreenData.Framebufferpath) == false)
            {
                Assert.Inconclusive("Cannot find " + PipeScreenData.Framebufferpath);
                return;
            }

            List<string> FileList = Directory.EnumerateFiles(PipeScreenData.Framebufferpath, "*.png", SearchOption.TopDirectoryOnly).ToList();
            if (FileList == null || FileList.Count == 0)
            {
                Assert.Inconclusive("No PNG files in " + PipeScreenData.Framebufferpath);
                return;
            }

            ProgramAgent.Init();
            if (ProgramAgent.LoadDLLForced(SystemInfos.CPUType.EM64T) == false)
                Assert.Inconclusive("Cannot load Agent supplemental DLL");

            decimal sz = 0;
            decimal tz = 0;

            MainScreenSystem.Ping();

            for (int i = 0; i < FileList.Count; i++)
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                byte[] d = MainScreenSystem.GetDeltaScreen2Debug();
                st.Stop();
                tz += st.ElapsedMilliseconds;
                sz += d.Length;
            }

            sz /= (decimal)FileList.Count;
            tz /= (decimal)FileList.Count;

            Console.WriteLine("Pictures: " + FileList.Count + " - AVG SZ=" + sz.ToString("0.00") + " bytes - AVG TZ= " + tz.ToString("0.0000") + " ms");
            Assert.AreEqual(0, 0);
#else
            Assert.Inconclusive("Unavalible in Release");
#endif
        }

        [TestMethod]
        public void TestGetDeltaStream_WebsocketWrite()
        {
#if DEBUG
            if (Directory.Exists(PipeScreenData.Framebufferpath) == false)
            {
                Assert.Inconclusive("Cannot find " + PipeScreenData.Framebufferpath);
                return;
            }

            List<string> FileList = Directory.EnumerateFiles(PipeScreenData.Framebufferpath, "*.png", SearchOption.TopDirectoryOnly).ToList();
            if (FileList == null || FileList.Count == 0)
            {
                Assert.Inconclusive("No PNG files in " + PipeScreenData.Framebufferpath);
                return;
            }

            ProgramAgent.Init();
            if (ProgramAgent.LoadDLLForced(SystemInfos.CPUType.EM64T) == false)
                Assert.Inconclusive("Cannot load Agent supplemental DLL");
            ScreenDataWS ws = new ScreenDataWS(null, "TEST");

            decimal sz = 0;
            decimal tz = 0;

            ws.Ws_OnMessage(new byte[] { 0x46, 0x52, 0x53, 0x1, 0x4, 0x0, 0x0, 0x0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            for (int i = 0; i < FileList.Count; i++)
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                ws.Ws_OnMessage(new byte[] { 0x46, 0x52, 0x53, 0x1, 0x4, 0x0, 0x0, 0x0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                st.Stop();
                tz += st.ElapsedMilliseconds;
                sz += 0;
            }

            sz /= (decimal)FileList.Count - 1;
            tz /= (decimal)FileList.Count - 1;

            Console.WriteLine("Pictures: " + (FileList.Count - 1) + " - AVG SZ=" + sz.ToString("0.00") + " bytes - AVG TZ= " + tz.ToString("0.0000") + " ms");
            Assert.AreEqual(0, 0);

#else
            Assert.Inconclusive("Unavalible in Release");
#endif
        }
    }
}
