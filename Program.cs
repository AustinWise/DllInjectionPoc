using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Net.Quic;
using System.Net;
using System.Net.Security;

[assembly: DisableRuntimeMarshalling]

partial class Program
{
    public static void Main()
    {
        // load the msquic.dll
        var options = new QuicListenerOptions()
        {
            ListenEndPoint = new IPEndPoint(IPAddress.Loopback, 8888),
            ApplicationProtocols = new List<SslApplicationProtocol>()
            {
                SslApplicationProtocol.Http3,
            },
            ConnectionOptionsCallback = (_, _, _) => default,
        };
        _ = QuicListener.ListenAsync(options);

        // The rest of the code just prints where WINMM.dll has been loaded from.
        
        IntPtr hModule = GetModuleHandleW("WINMM");
        if (hModule == IntPtr.Zero)
        {
            Console.WriteLine($"GetModuleHandleW failed: {Marshal.GetLastPInvokeErrorMessage()}");
            return;
        }

        Span<char> chars = stackalloc char[1024];
        int len;
        if (0 == (len = (int)GetModuleFileNameW(hModule, chars, (uint)chars.Length)))
        {
            Console.WriteLine($"GetModuleFileNameW failed: {Marshal.GetLastPInvokeErrorMessage()}");
            return;
        }

        Console.WriteLine(chars.Slice(0, len).ToString());
    }

    [LibraryImport(
     "Kernel32",
     EntryPoint = "GetModuleHandleW",
     SetLastError = true,
     StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr GetModuleHandleW(string lpModuleName);

    [LibraryImport(
    "Kernel32",
    EntryPoint = "GetModuleFileNameW",
    SetLastError = true,
    StringMarshalling = StringMarshalling.Utf16)]
    private static partial uint GetModuleFileNameW(IntPtr hModule, Span<char> lpFilename, uint nSize);

}