using System.Threading.Tasks;

namespace HidLibrary
{
    public class HidFastReadDevice : HidDevice
    {
        internal HidFastReadDevice(string devicePath, string description = null)
            : base(devicePath, description) { }

        // FastRead assumes that the device is connected,
        // which could cause stability issues if hardware is
        // disconnected during a read
        public HidDeviceData FastRead()
        {
            return FastRead(0);
        }

        public HidDeviceData FastRead(int timeout)
        {
            try
            {
                return ReadData(timeout);
            }
            catch
            {
                return new HidDeviceData(HidDeviceData.ReadStatus.ReadError);
            }
        }

        public void FastRead(ReadCallback callback)
        {
            FastRead(callback, 0);
        }

        public void FastRead(ReadCallback callback, int timeout)
        {
#if NETCOREAPP1_0_OR_GREATER
            // start new task with FastRead() and call callback afterwards
            Task<HidDeviceData> readTask = Task.Run(() => FastRead(timeout));
            readTask.ContinueWith(task => callback.Invoke(task.Result));
#else
            var readDelegate = new ReadDelegate(FastRead);
            var asyncState = new HidAsyncState(readDelegate, callback);
            readDelegate.BeginInvoke(timeout, EndRead, asyncState);
#endif
        }

        public async Task<HidDeviceData> FastReadAsync(int timeout = 0)
        {
#if NET20 || NET35 || NETCOREAPP1_0_OR_GREATER
            return await Task<HidDeviceData>.Factory.StartNew(() => FastRead(timeout));
#else
            var readDelegate = new ReadDelegate(FastRead);
            return await Task<HidDeviceData>.Factory.FromAsync(readDelegate.BeginInvoke, readDelegate.EndInvoke, timeout, null);
#endif
        }

        public HidReport FastReadReport()
        {
            return FastReadReport(0);
        }

        public HidReport FastReadReport(int timeout)
        {
            return new HidReport(Capabilities.InputReportByteLength, FastRead(timeout));
        }

        public void FastReadReport(ReadReportCallback callback)
        {
            FastReadReport(callback, 0);
        }

        public void FastReadReport(ReadReportCallback callback, int timeout)
        {
#if NETCOREAPP1_0_OR_GREATER
            // start new task with FastReadReport() and call callback afterwards
            Task<HidReport> readReportTask = Task.Run(() => FastReadReport(timeout));
            readReportTask.ContinueWith(task => callback.Invoke(task.Result));
#else
            var readReportDelegate = new ReadReportDelegate(FastReadReport);
            var asyncState = new HidAsyncState(readReportDelegate, callback);
            readReportDelegate.BeginInvoke(timeout, EndReadReport, asyncState);
#endif
        }

        public async Task<HidReport> FastReadReportAsync(int timeout = 0)
        {
#if NET20 || NET35 || NETCOREAPP1_0_OR_GREATER
            return await Task<HidReport>.Factory.StartNew(() => FastReadReport(timeout));
#else
            var readReportDelegate = new ReadReportDelegate(FastReadReport);
            return await Task<HidReport>.Factory.FromAsync(readReportDelegate.BeginInvoke, readReportDelegate.EndInvoke, timeout, null);
#endif
        }
    }
}
