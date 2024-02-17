using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: DisableRuntimeMarshalling]

partial class Program
{
    public static void Main()
    {
        // to force version.dll to be linked against
        _ = System.Diagnostics.FileVersionInfo.GetVersionInfo(Environment.ProcessPath!);

        IntPtr hModule = GetModuleHandleW("version");
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

        Console.WriteLine("version.dll loaded from (if it's not in c:\\Windows somewhere, DLL injection happened):");
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