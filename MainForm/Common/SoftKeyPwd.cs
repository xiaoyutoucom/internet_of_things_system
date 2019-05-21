/*********************************************
* 命名空间:SmartKylinApp.Common
* 功 能： 加密狗帮助类
* 类 名： SoftKeyPwd
* 作 者:  东腾
* 时 间： 2018-08-08 22:12:18 
**********************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartKylinApp.Common
{
    public struct GUID
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Data1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Data2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Data3;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data4;
    }

    public struct SP_INTERFACE_DEVICE_DATA
    {
        public int cbSize;
        public GUID InterfaceClassGuid;
        public int Flags;
        public int Reserved;
    }

    public struct SP_INTERFACE_DEVICE_DATA_64
    {
        public long cbSize;
        public GUID InterfaceClassGuid;
        public int Flags;
        public int Reserved;
    }

    public struct SP_DEVINFO_DATA
    {
        public int cbSize;
        public GUID ClassGuid;
        public int DevInst;
        public int Reserved;
    }


    public struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public int cbSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        public byte[] DevicePath;
    }


    public struct HIDD_ATTRIBUTES
    {
        public int Size;
        public ushort VendorID;
        public ushort ProductID;
        public ushort VersionNumber;
    }


    public struct HIDP_CAPS
    {
        public short Usage;
        public short UsagePage;
        public short InputReportByteLength;
        public short OutputReportByteLength;
        public short FeatureReportByteLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public short[] Reserved;

        public short NumberLinkCollectionNodes;
        public short NumberInputButtonCaps;
        public short NumberInputValueCaps;
        public short NumberInputDataIndices;
        public short NumberOutputButtonCaps;
        public short NumberOutputValueCaps;
        public short NumberOutputDataIndices;
        public short NumberFeatureButtonCaps;
        public short NumberFeatureValueCaps;
        public short NumberFeatureDataIndices;
    }

    internal class SoftKeyPWD
    {
        private const ushort VID = 0x3689;
        private const ushort PID = 0x8762;
        private const ushort PID_NEW = 0X2020;
        private const ushort VID_NEW = 0X3689;
        private const ushort PID_NEW_2 = 0X2020;
        private const ushort VID_NEW_2 = 0X2020;
        private const short DIGCF_PRESENT = 0x2;
        private const short DIGCF_DEVICEINTERFACE = 0x10;
        private const short INVALID_HANDLE_VALUE = -1;
        private const short ERROR_NO_MORE_ITEMS = 259;

        private const uint GENERIC_READ = 0x80000000;
        private const int GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x1;
        private const uint FILE_SHARE_WRITE = 0x2;
        private const uint OPEN_EXISTING = 3;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        private const uint INFINITE = 0xFFFF;

        private const short MAX_LEN = 495;

        public const int FAILEDGENKEYPAIR = -21;
        public const int FAILENC = -22;
        public const int FAILDEC = -23;
        public const int FAILPINPWD = -24;
        public const int USBStatusFail = -50; //USB操作失败，可能是没有找到相关的指令

        public const int SM2_ADDBYTE = 97; //加密后的数据会增加的长度
        public const int MAX_ENCLEN = 128; //最大的加密长度分组
        public const int MAX_DECLEN = MAX_ENCLEN + SM2_ADDBYTE; //最大的解密长度分组
        public const int SM2_USENAME_LEN = 80; // '最大的用户名长度


        public const int ECC_MAXLEN = 32;
        public const int PIN_LEN = 16;

        private const byte GETVERSION = 0x01;
        private const byte GETID = 0x02;
        private const byte GETVEREX = 0x05;
        private const byte CAL_TEA = 0x08;
        private const byte SET_TEAKEY = 0x09;
        private const byte READBYTE = 0x10;
        private const byte WRITEBYTE = 0x11;
        private const byte YTREADBUF = 0x12;
        private const byte YTWRITEBUF = 0x13;
        private const byte MYRESET = 0x20;
        private const byte YTREBOOT = 0x24;
        private const byte SET_ECC_PARA = 0x30;
        private const byte GET_ECC_PARA = 0x31;
        private const byte SET_ECC_KEY = 0x32;
        private const byte GET_ECC_KEY = 0x33;
        private const byte MYENC = 0x34;
        private const byte MYDEC = 0X35;
        private const byte SET_PIN = 0X36;
        private const byte GEN_KEYPAIR = 0x37;
        private const byte YTSIGN = 0x51;
        private const byte YTVERIFY = 0x52;
        private const byte GET_CHIPID = 0x53;
        private const byte YTSIGN_2 = 0x53;

        [DllImport("kernel32.dll")]
        public static extern int lstrlenA(string InString);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyStringToByte(byte[] pDest, string pSourceg, int ByteLenr);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyByteToString(StringBuilder pDest, byte[] pSource, int ByteLenr);

        [DllImport("HID.dll")]
        private static extern bool HidD_GetAttributes(int HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

        [DllImport("HID.dll")]
        private static extern int HidD_GetHidGuid(ref GUID HidGuid);

        [DllImport("HID.dll")]
        private static extern bool HidD_GetPreparsedData(int HidDeviceObject, ref IntPtr PreparsedData);

        [DllImport("HID.dll")]
        private static extern int HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);

        [DllImport("HID.dll")]
        private static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);

        [DllImport("HID.dll")]
        private static extern bool HidD_SetFeature(int HidDeviceObject, byte[] ReportBuffer, int ReportBufferLength);

        [DllImport("HID.dll")]
        private static extern bool HidD_GetFeature(int HidDeviceObject, byte[] ReportBuffer, int ReportBufferLength);

        [DllImport("SetupApi.dll")]
        private static extern IntPtr
            SetupDiGetClassDevsA(ref GUID ClassGuid, int Enumerator, int hwndParent, int Flags);

        [DllImport("SetupApi.dll")]
        private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("SetupApi.dll")]
        private static extern bool SetupDiGetDeviceInterfaceDetailA(IntPtr DeviceInfoSet,
            ref SP_INTERFACE_DEVICE_DATA DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize,
            ref int RequiredSize, int DeviceInfoData);

        [DllImport("SetupApi.dll", EntryPoint = "SetupDiGetDeviceInterfaceDetailA")]
        private static extern bool SetupDiGetDeviceInterfaceDetailA_64(IntPtr DeviceInfoSet,
            ref SP_INTERFACE_DEVICE_DATA_64 DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize,
            ref int RequiredSize, int DeviceInfoData);

        [DllImport("SetupApi.dll")]
        private static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, int DeviceInfoData,
            ref GUID InterfaceClassGuid, int MemberIndex, ref SP_INTERFACE_DEVICE_DATA DeviceInterfaceData);

        [DllImport("SetupApi.dll", EntryPoint = "SetupDiEnumDeviceInterfaces")]
        private static extern bool SetupDiEnumDeviceInterfaces_64(IntPtr DeviceInfoSet, ulong DeviceInfoData,
            ref GUID InterfaceClassGuid, int MemberIndex, ref SP_INTERFACE_DEVICE_DATA_64 DeviceInterfaceData);

        [DllImport("kernel32.dll", EntryPoint = "CreateFileA")]
        private static extern int CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode,
            uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, uint hTemplateFile);

        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(int hObject);

        [DllImport("kernel32.dll")]
        private static extern int GetLastError();


        [DllImport("kernel32.dll", EntryPoint = "CreateSemaphoreA")]
        private static extern int CreateSemaphore(int lpSemaphoreAttributes, int lInitialCount, int lMaximumCount,
            string lpName);

        [DllImport("kernel32.dll")]
        private static extern int WaitForSingleObject(int hHandle, uint dwMilliseconds);

        [DllImport("kernel32.dll")]
        private static extern int ReleaseSemaphore(int hSemaphore, int lReleaseCount, int lpPreviousCount);

        private readonly bool Is32;

        public SoftKeyPWD()
        {
            Is32 = IntPtr.Size == 4;
        }

        //以下函数用于将字节数组转化为宽字符串
        private static string ByteConvertString(byte[] buffer)
        {
            char[] null_string = { '\0', '\0' };
            var encoding = Encoding.Default;
            return encoding.GetString(buffer).TrimEnd(null_string);
        }

        //以下用于将16进制字符串转化为无符号长整型
        private uint HexToInt(string s)
        {
            string[] hexch =
            {
                "0", "1", "2", "3", "4", "5", "6", "7",
                "8", "9", "A", "B", "C", "D", "E", "F"
            };
            s = s.ToUpper();
            int i, j;
            int r, n, k;
            string ch;

            k = 1;
            r = 0;
            for (i = s.Length; i > 0; i--)
            {
                ch = s.Substring(i - 1, 1);
                n = 0;
                for (j = 0; j < 16; j++)
                    if (ch == hexch[j])
                        n = j;
                r += n * k;
                k *= 16;
            }

            return unchecked((uint)r);
        }

        private int HexStringToByteArray(string InString, ref byte[] b)
        {
            int nlen;
            int retutn_len;
            int n, i;
            string temp;
            nlen = InString.Length;
            if (nlen < 16) retutn_len = 16;
            retutn_len = nlen / 2;
            b = new byte[retutn_len];
            i = 0;
            for (n = 0; n < nlen; n = n + 2)
            {
                temp = InString.Substring(n, 2);
                b[i] = (byte)HexToInt(temp);
                i = i + 1;
            }

            return retutn_len;
        }


        public void EnCode(byte[] inb, byte[] outb, string Key)
        {
            uint cnDelta, y, z, a, b, c, d, temp_2;
            var buf = new uint[16];
            int n, i, nlen;
            uint sum;
            //UInt32 temp, temp_1;
            string temp_string;


            cnDelta = 2654435769;
            sum = 0;

            nlen = Key.Length;
            i = 0;
            for (n = 1; n <= nlen; n = n + 2)
            {
                temp_string = Key.Substring(n - 1, 2);
                buf[i] = HexToInt(temp_string);
                i = i + 1;
            }

            a = 0;
            b = 0;
            c = 0;
            d = 0;
            for (n = 0; n <= 3; n++)
            {
                a = (buf[n] << (n * 8)) | a;
                b = (buf[n + 4] << (n * 8)) | b;
                c = (buf[n + 4 + 4] << (n * 8)) | c;
                d = (buf[n + 4 + 4 + 4] << (n * 8)) | d;
            }


            y = 0;
            z = 0;
            for (n = 0; n <= 3; n++)
            {
                temp_2 = inb[n];
                y = (temp_2 << (n * 8)) | y;
                temp_2 = inb[n + 4];
                z = (temp_2 << (n * 8)) | z;
            }


            n = 32;

            while (n > 0)
            {
                sum = cnDelta + sum;

                /*temp = (z << 4) & 0xFFFFFFFF;
                temp = (temp + a) & 0xFFFFFFFF;
                temp_1 = (z + sum) & 0xFFFFFFFF;
                temp = (temp ^ temp_1) & 0xFFFFFFFF;
                temp_1 = (z >> 5) & 0xFFFFFFFF;
                temp_1 = (temp_1 + b) & 0xFFFFFFFF;
                temp = (temp ^ temp_1) & 0xFFFFFFFF;
                temp = (temp + y) & 0xFFFFFFFF;
                y = temp & 0xFFFFFFFF;*/
                y += ((z << 4) + a) ^ (z + sum) ^ ((z >> 5) + b);

                /*temp = (y << 4) & 0xFFFFFFFF;
                temp = (temp + c) & 0xFFFFFFFF;
                temp_1 = (y + sum) & 0xFFFFFFFF;
                temp = (temp ^ temp_1) & 0xFFFFFFFF;
                temp_1 = (y >> 5) & 0xFFFFFFFF;
                temp_1 = (temp_1 + d) & 0xFFFFFFFF;
                temp = (temp ^ temp_1) & 0xFFFFFFFF;
                temp = (z + temp) & 0xFFFFFFFF;
                z = temp & 0xFFFFFFFF;*/
                z += ((y << 4) + c) ^ (y + sum) ^ ((y >> 5) + d);
                n = n - 1;
            }

            for (n = 0; n <= 3; n++)
            {
                outb[n] = Convert.ToByte((y >> (n * 8)) & 255);
                outb[n + 4] = Convert.ToByte((z >> (n * 8)) & 255);
            }
        }

        public void DeCode(byte[] inb, byte[] outb, string Key)
        {
            uint cnDelta, y, z, a, b, c, d, temp_2;
            var buf = new uint[16];
            int n, i, nlen;
            uint sum;
            //UInt32 temp, temp_1;
            string temp_string;


            cnDelta = 2654435769;
            sum = 0xC6EF3720;

            nlen = Key.Length;
            i = 0;
            for (n = 1; n <= nlen; n = n + 2)
            {
                temp_string = Key.Substring(n - 1, 2);
                buf[i] = HexToInt(temp_string);
                i = i + 1;
            }

            a = 0;
            b = 0;
            c = 0;
            d = 0;
            for (n = 0; n <= 3; n++)
            {
                a = (buf[n] << (n * 8)) | a;
                b = (buf[n + 4] << (n * 8)) | b;
                c = (buf[n + 4 + 4] << (n * 8)) | c;
                d = (buf[n + 4 + 4 + 4] << (n * 8)) | d;
            }


            y = 0;
            z = 0;
            for (n = 0; n <= 3; n++)
            {
                temp_2 = inb[n];
                y = (temp_2 << (n * 8)) | y;
                temp_2 = inb[n + 4];
                z = (temp_2 << (n * 8)) | z;
            }


            n = 32;

            while (n-- > 0)
            {
                z -= ((y << 4) + c) ^ (y + sum) ^ ((y >> 5) + d);
                y -= ((z << 4) + a) ^ (z + sum) ^ ((z >> 5) + b);
                sum -= cnDelta;
            }

            for (n = 0; n <= 3; n++)
            {
                outb[n] = Convert.ToByte((y >> (n * 8)) & 255);
                outb[n + 4] = Convert.ToByte((z >> (n * 8)) & 255);
            }
        }


        public string StrEnc(string InString, string Key) //使用增强算法，加密字符串
        {
            byte[] b, outb;
            byte[] temp = new byte[8], outtemp = new byte[8];
            int n, i, nlen, outlen;
            string outstring;


            nlen = lstrlenA(InString) + 1;
            if (nlen < 8)
                outlen = 8;
            else
                outlen = nlen;
            b = new byte[outlen];
            outb = new byte[outlen];

            CopyStringToByte(b, InString, nlen);

            b.CopyTo(outb, 0);

            for (n = 0; n <= outlen - 8; n = n + 8)
            {
                for (i = 0; i < 8; i++) temp[i] = b[i + n];
                EnCode(temp, outtemp, Key);
                for (i = 0; i < 8; i++) outb[i + n] = outtemp[i];
            }

            outstring = "";
            for (n = 0; n <= outlen - 1; n++) outstring = outstring + outb[n].ToString("X2");
            return outstring;
        }

        public string StrDec(string InString, string Key) //使用增强算法，加密字符串
        {
            byte[] b, outb;
            byte[] temp = new byte[8], outtemp = new byte[8];
            int n, i, nlen, outlen;
            string temp_string;
            StringBuilder c_str;


            nlen = InString.Length;
            if (nlen < 16) outlen = 16;
            outlen = nlen / 2;
            b = new byte[outlen];
            outb = new byte[outlen];

            i = 0;
            for (n = 1; n <= nlen; n = n + 2)
            {
                temp_string = InString.Substring(n - 1, 2);
                b[i] = Convert.ToByte(HexToInt(temp_string));
                i = i + 1;
            }

            b.CopyTo(outb, 0);

            for (n = 0; n <= outlen - 8; n = n + 8)
            {
                for (i = 0; i < 8; i++) temp[i] = b[i + n];
                DeCode(temp, outtemp, Key);
                for (i = 0; i < 8; i++) outb[i + n] = outtemp[i];
            }

            c_str = new StringBuilder("", outlen);
            CopyByteToString(c_str, outb, outlen);
            return c_str.ToString();
        }

        private bool isfindmydevice(int pos, ref int count, ref string OutPath)
        {
            if (Is32)
                return isfindmydevice_32(pos, ref count, ref OutPath);
            return isfindmydevice_64(pos, ref count, ref OutPath);
        }

        private bool isfindmydevice_64(int pos, ref int count, ref string OutPath)
        {
            IntPtr hardwareDeviceInfo;
            var DeviceInfoData = new SP_INTERFACE_DEVICE_DATA_64();
            int i;
            var HidGuid = new GUID();
            var functionClassDeviceData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
            int requiredLength;
            int d_handle;
            var Attributes = new HIDD_ATTRIBUTES();

            i = 0;
            count = 0;
            HidD_GetHidGuid(ref HidGuid);

            hardwareDeviceInfo = SetupDiGetClassDevsA(ref HidGuid, 0, 0, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);

            if (hardwareDeviceInfo == (IntPtr)INVALID_HANDLE_VALUE) return false;

            DeviceInfoData.cbSize = Marshal.SizeOf(DeviceInfoData);

            while (SetupDiEnumDeviceInterfaces_64(hardwareDeviceInfo, 0, ref HidGuid, i, ref DeviceInfoData))
            {
                if (GetLastError() == ERROR_NO_MORE_ITEMS) break;
                functionClassDeviceData.cbSize = 8;
                requiredLength = 0;
                if (!SetupDiGetDeviceInterfaceDetailA_64(hardwareDeviceInfo, ref DeviceInfoData,
                    ref functionClassDeviceData, 300, ref requiredLength, 0))
                {
                    SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
                    return false;
                }

                OutPath = ByteConvertString(functionClassDeviceData.DevicePath);
                d_handle = CreateFile(OutPath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, 0,
                    OPEN_EXISTING, 0, 0);
                if (INVALID_HANDLE_VALUE != d_handle)
                {
                    if (HidD_GetAttributes(d_handle, ref Attributes))
                        if (Attributes.ProductID == PID && Attributes.VendorID == VID ||
                            Attributes.ProductID == PID_NEW && Attributes.VendorID == VID_NEW ||
                            Attributes.ProductID == PID_NEW_2 && Attributes.VendorID == VID_NEW_2)
                        {
                            if (pos == count)
                            {
                                SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
                                CloseHandle(d_handle);
                                return true;
                            }

                            count = count + 1;
                        }

                    CloseHandle(d_handle);
                }

                i = i + 1;
            }

            SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
            return false;
        }

        private bool isfindmydevice_32(int pos, ref int count, ref string OutPath)
        {
            IntPtr hardwareDeviceInfo;
            var DeviceInfoData = new SP_INTERFACE_DEVICE_DATA();
            int i;
            var HidGuid = new GUID();
            var functionClassDeviceData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
            int requiredLength;
            int d_handle;
            var Attributes = new HIDD_ATTRIBUTES();

            i = 0;
            count = 0;
            HidD_GetHidGuid(ref HidGuid);

            hardwareDeviceInfo = SetupDiGetClassDevsA(ref HidGuid, 0, 0, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);

            if (hardwareDeviceInfo == (IntPtr)INVALID_HANDLE_VALUE) return false;

            DeviceInfoData.cbSize = Marshal.SizeOf(DeviceInfoData);

            while (SetupDiEnumDeviceInterfaces(hardwareDeviceInfo, 0, ref HidGuid, i, ref DeviceInfoData))
            {
                if (GetLastError() == ERROR_NO_MORE_ITEMS) break;
                functionClassDeviceData.cbSize = Marshal.SizeOf(functionClassDeviceData) - 255; // 5;
                requiredLength = 0;
                if (!SetupDiGetDeviceInterfaceDetailA(hardwareDeviceInfo, ref DeviceInfoData,
                    ref functionClassDeviceData, 300, ref requiredLength, 0))
                {
                    SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
                    return false;
                }

                OutPath = ByteConvertString(functionClassDeviceData.DevicePath);
                d_handle = CreateFile(OutPath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, 0,
                    OPEN_EXISTING, 0, 0);
                if (INVALID_HANDLE_VALUE != d_handle)
                {
                    if (HidD_GetAttributes(d_handle, ref Attributes))
                        if (Attributes.ProductID == PID && Attributes.VendorID == VID ||
                            Attributes.ProductID == PID_NEW && Attributes.VendorID == VID_NEW ||
                            Attributes.ProductID == PID_NEW_2 && Attributes.VendorID == VID_NEW_2)
                        {
                            if (pos == count)
                            {
                                SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
                                CloseHandle(d_handle);
                                return true;
                            }

                            count = count + 1;
                        }

                    CloseHandle(d_handle);
                }

                i = i + 1;
            }

            SetupDiDestroyDeviceInfoList(hardwareDeviceInfo);
            return false;
        }

        private bool GetFeature(int hDevice, byte[] array_out, int out_len)
        {
            bool FeatureStatus;
            bool Status;
            int i;
            var FeatureReportBuffer = new byte[512];
            var Ppd = IntPtr.Zero;
            var Caps = new HIDP_CAPS();

            if (!HidD_GetPreparsedData(hDevice, ref Ppd)) return false;

            if (HidP_GetCaps(Ppd, ref Caps) <= 0)
            {
                HidD_FreePreparsedData(Ppd);
                return false;
            }

            Status = true;

            FeatureReportBuffer[0] = 1;

            FeatureStatus = HidD_GetFeature(hDevice, FeatureReportBuffer, Caps.FeatureReportByteLength);
            if (FeatureStatus)
                for (i = 0; i < out_len; i++)
                    array_out[i] = FeatureReportBuffer[i];


            Status = Status && FeatureStatus;
            HidD_FreePreparsedData(Ppd);

            return Status;
        }

        private bool SetFeature(int hDevice, byte[] array_in, int in_len)
        {
            bool FeatureStatus;
            bool Status;
            int i;
            var FeatureReportBuffer = new byte[512];
            var Ppd = IntPtr.Zero;
            var Caps = new HIDP_CAPS();

            if (!HidD_GetPreparsedData(hDevice, ref Ppd)) return false;

            if (HidP_GetCaps(Ppd, ref Caps) <= 0)
            {
                HidD_FreePreparsedData(Ppd);
                return false;
            }

            Status = true;

            FeatureReportBuffer[0] = 2;

            for (i = 0; i < in_len; i++) FeatureReportBuffer[i + 1] = array_in[i + 1];
            FeatureStatus = HidD_SetFeature(hDevice, FeatureReportBuffer, Caps.FeatureReportByteLength);


            Status = Status && FeatureStatus;
            HidD_FreePreparsedData(Ppd);

            return Status;
        }

        private int NT_FindPort(int start, ref string OutPath)
        {
            var count = 0;
            if (!isfindmydevice(start, ref count, ref OutPath)) return -92;
            return 0;
        }

        private int OpenMydivece(ref int hUsbDevice, string Path)
        {
            string OutPath;
            bool biao;
            var count = 0;
            if (Path.Length < 1)
            {
                OutPath = "";
                biao = isfindmydevice(0, ref count, ref OutPath);
                if (!biao) return -92;
                hUsbDevice = CreateFile(OutPath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, 0,
                    OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
                if (hUsbDevice == INVALID_HANDLE_VALUE) return -92;
            }
            else
            {
                hUsbDevice = CreateFile(Path, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, 0,
                    OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
                if (hUsbDevice == INVALID_HANDLE_VALUE) return -92;
            }

            return 0;
        }


        private int GetIDVersion(ref short Version, string Path)
        {
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 1;
            if (!SetFeature(hUsbDevice, array_in, 1))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 1))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            CloseHandle(hUsbDevice);
            Version = array_out[0];
            return 0;
        }

        private int NT_GetID(ref int ID_1, ref int ID_2, string Path)
        {
            var t = new int[8];
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 2;
            if (!SetFeature(hUsbDevice, array_in, 1))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 8))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            CloseHandle(hUsbDevice);
            t[0] = array_out[0];
            t[1] = array_out[1];
            t[2] = array_out[2];
            t[3] = array_out[3];
            t[4] = array_out[4];
            t[5] = array_out[5];
            t[6] = array_out[6];
            t[7] = array_out[7];
            ID_1 = t[3] | (t[2] << 8) | (t[1] << 16) | (t[0] << 24);
            ID_2 = t[7] | (t[6] << 8) | (t[5] << 16) | (t[4] << 24);
            return 0;
        }


        private int Y_Read(byte[] OutData, int address, int nlen, byte[] password, string Path, int pos)
        {
            int addr_l;
            int addr_h;
            int n;
            var array_in = new byte[512];
            var array_out = new byte[512];
            if (address > MAX_LEN || address < 0) return -81;
            if (nlen > 255) return -87;
            if (nlen + address > MAX_LEN) return -88;
            addr_h = (address >> 8) * 2;
            addr_l = address & 255;
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;

            array_in[1] = 0x12;
            array_in[2] = (byte)addr_h;
            array_in[3] = (byte)addr_l;
            array_in[4] = (byte)nlen;
            for (n = 0; n <= 7; n++) array_in[5 + n] = password[n];
            if (!SetFeature(hUsbDevice, array_in, 13))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, nlen + 1))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0) return -83;
            for (n = 0; n < nlen; n++) OutData[n + pos] = array_out[n + 1];
            return 0;
        }

        private int Y_Write(byte[] indata, int address, int nlen, byte[] password, string Path, int pos)
        {
            int addr_l;
            int addr_h;
            int n;
            var array_in = new byte[512];
            var array_out = new byte[512];
            if (nlen > 255) return -87;
            if (address + nlen - 1 > MAX_LEN + 17 || address < 0) return -81;
            addr_h = (address >> 8) * 2;
            addr_l = address & 255;
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 0x13;
            array_in[2] = (byte)addr_h;
            array_in[3] = (byte)addr_l;
            array_in[4] = (byte)nlen;
            for (n = 0; n <= 7; n++) array_in[5 + n] = password[n];
            for (n = 0; n < nlen; n++) array_in[13 + n] = indata[n + pos];
            if (!SetFeature(hUsbDevice, array_in, 13 + nlen))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 2))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0) return -82;
            return 0;
        }

        private int NT_Cal(byte[] InBuf, byte[] outbuf, string Path, int pos)
        {
            int n;
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 8;
            for (n = 2; n <= 9; n++) array_in[n] = InBuf[n - 2 + pos];
            if (!SetFeature(hUsbDevice, array_in, 9))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 9))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            CloseHandle(hUsbDevice);
            for (n = 0; n < 8; n++) outbuf[n + pos] = array_out[n];
            if (array_out[8] != 0x55) return -20;
            return 0;
        }

        private int NT_SetCal_2(byte[] indata, byte IsHi, string Path, short pos)
        {
            int n;
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 9;
            array_in[2] = IsHi;
            for (n = 0; n < 8; n++) array_in[3 + n] = indata[n + pos];
            if (!SetFeature(hUsbDevice, array_in, 11))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 2))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0) return -82;

            return 0;
        }

        public int NT_GetIDVersion(ref short Version, string Path)
        {
            int ret;
            int hsignal;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = GetIDVersion(ref Version, Path);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int GetID(ref int ID_1, ref int ID_2, string Path)
        {
            int ret;
            int hsignal;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_GetID(ref ID_1, ref ID_2, Path);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }


        public int YWriteEx(byte[] indata, int address, int nlen, string HKey, string LKey, string Path)
        {
            var ret = 0;
            int hsignal;
            var password = new byte[8];
            int n, trashLen = 0;
            int leave;
            int temp_leave;
            if (address + nlen - 1 > MAX_LEN || address < 0) return -81;

            ret = GetTrashBufLen(Path, ref trashLen);
            if (trashLen < 100) trashLen = 16;
            trashLen = trashLen - 8;
            if (ret != 0) return ret;

            myconvert(HKey, LKey, password);
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            temp_leave = address % trashLen;
            leave = trashLen - temp_leave;
            if (leave > nlen) leave = nlen;
            if (leave > 0)
            {
                for (n = 0; n < leave / trashLen; n++)
                {
                    ret = Y_Write(indata, address + n * trashLen, trashLen, password, Path, trashLen * n);
                    if (ret != 0)
                    {
                        ReleaseSemaphore(hsignal, 1, 0);
                        CloseHandle(hsignal);
                        return sub_YWriteEx(indata, address, nlen, HKey, LKey, Path);
                    }
                }

                if (leave - trashLen * n > 0)
                {
                    ret = Y_Write(indata, address + n * trashLen, leave - n * trashLen, password, Path, trashLen * n);
                    if (ret != 0)
                    {
                        ReleaseSemaphore(hsignal, 1, 0);
                        CloseHandle(hsignal);
                        return sub_YWriteEx(indata, address, nlen, HKey, LKey, Path);
                    }
                }
            }

            nlen = nlen - leave;
            address = address + leave;
            if (nlen > 0)
            {
                for (n = 0; n < nlen / trashLen; n++)
                {
                    ret = Y_Write(indata, address + n * trashLen, trashLen, password, Path, leave + trashLen * n);
                    if (ret != 0)
                    {
                        ReleaseSemaphore(hsignal, 1, 0);
                        CloseHandle(hsignal);
                        return sub_YWriteEx(indata, address, nlen, HKey, LKey, Path);
                    }
                }

                if (nlen - trashLen * n > 0)
                {
                    ret = Y_Write(indata, address + n * trashLen, nlen - n * trashLen, password, Path,
                        leave + trashLen * n);
                    if (ret != 0)
                    {
                        ReleaseSemaphore(hsignal, 1, 0);
                        CloseHandle(hsignal);
                        return sub_YWriteEx(indata, address, nlen, HKey, LKey, Path);
                    }
                }
            }

            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int YReadEx(byte[] OutData, short address, short nlen, string HKey, string LKey, string Path)
        {
            var ret = 0;
            int hsignal;
            var password = new byte[8];
            int n, trashLen = 0;

            if (address + nlen - 1 > MAX_LEN || address < 0) return -81;

            ret = GetTrashBufLen(Path, ref trashLen);
            if (trashLen < 100) trashLen = 16;
            if (ret != 0) return ret;


            myconvert(HKey, LKey, password);
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            for (n = 0; n < nlen / trashLen; n++)
            {
                ret = Y_Read(OutData, address + n * trashLen, trashLen, password, Path, n * trashLen);
                if (ret != 0)
                {
                    ReleaseSemaphore(hsignal, 1, 0);
                    CloseHandle(hsignal);
                    return sub_YReadEx(OutData, address, nlen, HKey, LKey, Path);
                }
            }

            if (nlen - trashLen * n > 0)
            {
                ret = Y_Read(OutData, address + n * trashLen, nlen - trashLen * n, password, Path, trashLen * n);
                if (ret != 0)
                {
                    ReleaseSemaphore(hsignal, 1, 0);
                    CloseHandle(hsignal);
                    return sub_YReadEx(OutData, address, nlen, HKey, LKey, Path);
                }
            }

            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int FindPort(int start, ref string OutPath)
        {
            int ret;
            int hsignal;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_FindPort(start, ref OutPath);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }


        private string AddZero(string InKey)
        {
            int nlen;
            int n;
            nlen = InKey.Length;
            for (n = nlen; n <= 7; n++) InKey = "0" + InKey;
            return InKey;
        }

        private void myconvert(string HKey, string LKey, byte[] out_data)
        {
            HKey = AddZero(HKey);
            LKey = AddZero(LKey);
            int n;
            for (n = 0; n <= 3; n++) out_data[n] = (byte)HexToInt(HKey.Substring(n * 2, 2));
            for (n = 0; n <= 3; n++) out_data[n + 4] = (byte)HexToInt(LKey.Substring(n * 2, 2));
        }

        public int YRead(ref byte indata, int address, string HKey, string LKey, string Path)
        {
            int ret;
            int hsignal;
            var ary1 = new byte[8];

            if (address > 495 || address < 0) return -81;
            myconvert(HKey, LKey, ary1);
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = sub_YRead(ref indata, address, ary1, Path);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        private int sub_YRead(ref byte OutData, int address, byte[] password, string Path)
        {
            int n;
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            byte opcode;
            if (address > 495 || address < 0) return -81;
            opcode = 128;
            if (address > 255)
            {
                opcode = 160;
                address = address - 256;
            }

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 16;
            array_in[2] = opcode;
            array_in[3] = (byte)address;
            array_in[4] = (byte)address;
            for (n = 0; n < 8; n++) array_in[5 + n] = password[n];
            if (!SetFeature(hUsbDevice, array_in, 13))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 2))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 83) return -83;
            OutData = array_out[1];
            return 0;
        }

        public int YWrite(byte indata, int address, string HKey, string LKey, string Path)
        {
            int ret;
            int hsignal;
            var ary1 = new byte[8];

            if (address > 495 || address < 0) return -81;
            myconvert(HKey, LKey, ary1);
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = sub_YWrite(indata, address, ary1, Path);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        private int sub_YWrite(byte indata, int address, byte[] password, string Path)
        {
            int n;
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            byte opcode;
            if (address > 511 || address < 0) return -81;
            opcode = 64;
            if (address > 255)
            {
                opcode = 96;
                address = address - 256;
            }

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 17;
            array_in[2] = opcode;
            array_in[3] = (byte)address;
            array_in[4] = indata;
            for (n = 0; n < 8; n++) array_in[5 + n] = password[n];
            if (!SetFeature(hUsbDevice, array_in, 13))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 2))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[1] != 1) return -82;
            return 0;
        }

        public int SetReadPassword(string W_HKey, string W_LKey, string new_HKey, string new_LKey, string Path)
        {
            int ret;
            int hsignal;
            var ary1 = new byte[8];
            var ary2 = new byte[8];
            short address;
            myconvert(W_HKey, W_LKey, ary1);
            myconvert(new_HKey, new_LKey, ary2);
            address = 496;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = Y_Write(ary2, address, 8, ary1, Path, 0);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }


        public int SetWritePassword(string W_HKey, string W_LKey, string new_HKey, string new_LKey, string Path)
        {
            int ret;
            int hsignal;
            var ary1 = new byte[8];
            var ary2 = new byte[8];
            short address;
            myconvert(W_HKey, W_LKey, ary1);
            myconvert(new_HKey, new_LKey, ary2);
            address = 504;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = Y_Write(ary2, address, 8, ary1, Path, 0);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int YWriteString(string InString, int address, string HKey, string LKey, string Path)
        {
            var ret = 0;
            var ary1 = new byte[8];
            int hsignal;
            int n, trashLen = 0;
            int outlen;
            int total_len;
            int temp_leave;
            int leave;
            byte[] b;
            if (address < 0) return -81;

            ret = GetTrashBufLen(Path, ref trashLen);
            if (trashLen < 100) trashLen = 16;
            trashLen = trashLen - 8;
            if (ret != 0) return ret;

            myconvert(HKey, LKey, ary1);

            outlen = lstrlenA(InString); //注意，这里不写入结束字符串，与原来的兼容，也可以写入结束字符串，与原来的不兼容，写入长度会增加1
            b = new byte[outlen];
            CopyStringToByte(b, InString, outlen);

            total_len = address + outlen;
            if (total_len > MAX_LEN) return -47;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            temp_leave = address % trashLen;
            leave = trashLen - temp_leave;
            if (leave > outlen) leave = outlen;

            if (leave > 0)
            {
                for (n = 0; n < leave / trashLen; n++)
                {
                    ret = Y_Write(b, address + n * trashLen, trashLen, ary1, Path, n * trashLen);
                    if (ret != 0)
                    {
                        ReleaseSemaphore(hsignal, 1, 0);
                        CloseHandle(hsignal);
                        return sub_YWrite_new(InString, address, HKey, LKey, Path);
                    }
                }

                if (leave - trashLen * n > 0)
                {
                    ret = Y_Write(b, address + n * trashLen, leave - n * trashLen, ary1, Path, trashLen * n);
                    if (ret != 0)
                    {
                        ReleaseSemaphore(hsignal, 1, 0);
                        CloseHandle(hsignal);
                        return sub_YWrite_new(InString, address, HKey, LKey, Path);
                    }
                }
            }

            outlen = outlen - leave;
            address = address + leave;
            if (outlen > 0)
            {
                for (n = 0; n < outlen / trashLen; n++)
                {
                    ret = Y_Write(b, address + n * trashLen, trashLen, ary1, Path, leave + n * trashLen);
                    if (ret != 0)
                    {
                        ReleaseSemaphore(hsignal, 1, 0);
                        CloseHandle(hsignal);
                        return ret;
                    }
                }

                if (outlen - trashLen * n > 0)
                {
                    ret = Y_Write(b, address + n * trashLen, outlen - n * trashLen, ary1, Path, leave + trashLen * n);
                    if (ret != 0)
                    {
                        ReleaseSemaphore(hsignal, 1, 0);
                        CloseHandle(hsignal);
                        return ret;
                    }
                }
            }

            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int YReadString(ref string OutString, int address, int nlen, string HKey, string LKey, string Path)
        {
            var ret = 0;
            var ary1 = new byte[8];
            int hsignal;
            int n, trashLen = 0;
            int total_len;
            byte[] outb;
            StringBuilder temp_OutString;
            outb = new byte[nlen];
            myconvert(HKey, LKey, ary1);
            if (address < 0) return -81;

            ret = GetTrashBufLen(Path, ref trashLen);
            if (trashLen < 100) trashLen = 16;
            if (ret != 0) return ret;

            total_len = address + nlen;
            if (total_len > MAX_LEN) return -47;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            for (n = 0; n < nlen / trashLen; n++)
            {
                ret = Y_Read(outb, address + n * trashLen, trashLen, ary1, Path, n * trashLen);
                if (ret != 0)
                {
                    ReleaseSemaphore(hsignal, 1, 0);
                    CloseHandle(hsignal);
                    return sub_YRead_new(ref OutString, address, nlen, HKey, LKey, Path);
                }
            }

            if (nlen - trashLen * n > 0)
            {
                ret = Y_Read(outb, address + n * trashLen, nlen - trashLen * n, ary1, Path, trashLen * n);
                if (ret != 0)
                {
                    ReleaseSemaphore(hsignal, 1, 0);
                    CloseHandle(hsignal);
                    return sub_YRead_new(ref OutString, address, nlen, HKey, LKey, Path);
                }
            }

            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            temp_OutString = new StringBuilder("", nlen);
            //初始化数据为0，注意，这步一定是需要的
            for (n = 0; n < nlen; n++) temp_OutString.Append(0);
            CopyByteToString(temp_OutString, outb, nlen);
            OutString = temp_OutString.ToString();
            return ret;
        }

        public int SetCal_2(string Key, string Path)
        {
            int ret;
            int hsignal;
            var KeyBuf = new byte[16];
            var inb = new byte[8];
            HexStringToByteArray(Key, ref KeyBuf);
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_SetCal_2(KeyBuf, 0, Path, 8);
            if (ret != 0) goto error1;
            ret = NT_SetCal_2(KeyBuf, 1, Path, 0);
            error1:
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int Cal(byte[] InBuf, byte[] outbuf, string Path)
        {
            int ret;
            int hsignal;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_Cal(InBuf, outbuf, Path, 0);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int EncString(string InString, ref string OutString, string Path)
        {
            int hsignal;
            byte[] b;
            byte[] outb;
            int n;
            int nlen, temp_len;
            var ret = 0;

            nlen = lstrlenA(InString) + 1;
            temp_len = nlen;
            if (nlen < 8) nlen = 8;


            b = new byte[nlen];
            outb = new byte[nlen];

            CopyStringToByte(b, InString, temp_len);

            b.CopyTo(outb, 0);

            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            for (n = 0; n <= nlen - 8; n = n + 8)
            {
                ret = NT_Cal(b, outb, Path, n);
                if (ret != 0) break;
            }

            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            OutString = "";
            for (n = 0; n < nlen; n++) OutString = OutString + outb[n].ToString("X2");
            return ret;
        }

        public int ReSet(string Path)
        {
            int ret;
            int hsignal;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_ReSet(Path);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        private int NT_ReSet(string Path)
        {
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 32;
            if (!SetFeature(hUsbDevice, array_in, 2))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 2))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0) return -82;
            return 0;
        }


        public int NT_GetVersionEx(ref short Version, string Path)
        {
            int ret;
            int hsignal;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = F_GetVersionEx(ref Version, Path);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        private int F_GetVersionEx(ref short Version, string Path)
        {
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 5;
            if (!SetFeature(hUsbDevice, array_in, 1))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 1))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            CloseHandle(hUsbDevice);
            Version = array_out[0];
            return 0;
        }

        private int sub_YRead_new(ref string OutString, int Address, int nlen, string HKey, string LKey, string Path)
        {
            int n, ret = 0;
            byte[] outb;
            StringBuilder temp_OutString;
            outb = new byte[nlen];
            for (n = 0; n < nlen; n++)
            {
                ret = YRead(ref outb[n], Address + n, HKey, LKey, Path);
                if (ret != 0) return ret;
            }

            temp_OutString = new StringBuilder("", nlen);
            //初始化数据为0，注意，这步一定是需要的
            for (n = 0; n < nlen; n++) temp_OutString.Append(0);
            CopyByteToString(temp_OutString, outb, nlen);
            OutString = temp_OutString.ToString();
            return ret;
        }

        private int sub_YWrite_new(string InString, int Address, string HKey, string LKey, string Path)
        {
            int n, ret = 0;
            byte[] b;
            var outlen = lstrlenA(InString); //注意，这里不写入结束字符串，与原来的兼容，也可以写入结束字符串，与原来的不兼容，写入长度会增加1
            b = new byte[outlen];
            CopyStringToByte(b, InString, outlen);
            for (n = 0; n < outlen; n++)
            {
                ret = YWrite(b[n], Address + n, HKey, LKey, Path);
                if (ret != 0) return ret;
            }

            return ret;
        }

        private int sub_YReadEx(byte[] OutData, int Address, int nlen, string HKey, string LKey, string Path)
        {
            int n, ret = 0;
            for (n = 0; n < nlen; n++)
            {
                ret = YRead(ref OutData[n], Address + n, HKey, LKey, Path);
                if (ret != 0) return ret;
            }

            return ret;
        }

        private int sub_YWriteEx(byte[] indata, int Address, int len, string HKey, string LKey, string Path)
        {
            int n, ret = 0;
            for (n = 0; n < len; n++)
            {
                ret = YWrite(indata[n], Address + n, HKey, LKey, Path);
                if (ret != 0) return ret;
            }

            return ret;
        }

        public int SetHidOnly(bool IsHidOnly, string Path)
        {
            int ret;
            int hsignal;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_SetHidOnly(IsHidOnly, Path);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        private int NT_SetHidOnly(bool IsHidOnly, string Path)
        {
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 0x55;
            if (IsHidOnly) array_in[2] = 0;
            else array_in[2] = 0xff;
            if (!SetFeature(hUsbDevice, array_in, 3))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 1))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0) return -82;
            return 0;
        }

        public int SetUReadOnly(string Path)
        {
            int ret;
            int hsignal;
            hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_SetUReadOnly(Path);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        private int NT_SetUReadOnly(string Path)
        {
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 0x56;
            if (!SetFeature(hUsbDevice, array_in, 3))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 1))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0) return -82;
            return 0;
        }

        private int sub_GetTrashBufLen(int hDevice, ref int out_len)
        {
            var Ppd = IntPtr.Zero;
            var Caps = new HIDP_CAPS();

            if (!HidD_GetPreparsedData(hDevice, ref Ppd)) return -93;

            if (HidP_GetCaps(Ppd, ref Caps) <= 0)
            {
                HidD_FreePreparsedData(Ppd);
                return -93;
            }

            HidD_FreePreparsedData(Ppd);
            out_len = Caps.FeatureReportByteLength - 5;
            return 0;
        }

        private int GetTrashBufLen(string Path, ref int out_len)
        {
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            var ret = sub_GetTrashBufLen(hUsbDevice, ref out_len);
            CloseHandle(hUsbDevice);
            return ret;
        }

        private int NT_Set_SM2_KeyPair(byte[] PriKey, byte[] PubKeyX, byte[] PubKeyY, byte[] sm2_UerName, string Path)
        {
            var array_in = new byte[256];
            var array_out = new byte[25];
            var n = 0;
            var hUsbDevice = 0;

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 0x32;
            for (n = 0; n < ECC_MAXLEN; n++)
            {
                array_in[2 + n + ECC_MAXLEN * 0] = PriKey[n];
                array_in[2 + n + ECC_MAXLEN * 1] = PubKeyX[n];
                array_in[2 + n + ECC_MAXLEN * 2] = PubKeyY[n];
            }

            for (n = 0; n < SM2_USENAME_LEN; n++) array_in[2 + n + ECC_MAXLEN * 3] = sm2_UerName[n];

            if (!SetFeature(hUsbDevice, array_in, ECC_MAXLEN * 3 + SM2_USENAME_LEN + 2))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 2))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0x20) return USBStatusFail;

            return 0;
        }

        private int NT_GenKeyPair(byte[] PriKey, byte[] PubKey, string Path)
        {
            var array_in = new byte[256];
            var array_out = new byte[256];
            var n = 0;
            var hUsbDevice = 0;
            array_out[0] = 0xfb;

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = GEN_KEYPAIR;
            if (!SetFeature(hUsbDevice, array_in, 2))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, ECC_MAXLEN * 3 + 2))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0x20) return FAILEDGENKEYPAIR; //表示读取失败；
            for (n = 0; n < ECC_MAXLEN; n++) PriKey[n] = array_out[1 + ECC_MAXLEN * 0 + n];
            for (n = 0; n < ECC_MAXLEN * 2 + 1; n++) PubKey[n] = array_out[1 + ECC_MAXLEN * 1 + n];
            return 0;
        }

        private int NT_GetChipID(byte[] OutChipID, string Path)
        {
            var t = new int[8];
            int n;
            var array_in = new byte[25];
            var array_out = new byte[25];
            var hUsbDevice = 0;
            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = GET_CHIPID;
            if (!SetFeature(hUsbDevice, array_in, 1))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 17))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0x20) return USBStatusFail;
            for (n = 0; n < 16; n++) OutChipID[n] = array_out[1 + n];

            return 0;
        }


        private int NT_Get_SM2_PubKey(byte[] KGx, byte[] KGy, byte[] sm2_UerName, string Path)
        {
            var array_in = new byte[256];
            var array_out = new byte[256];
            var n = 0;
            var hUsbDevice = 0;

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = 0x33;
            if (!SetFeature(hUsbDevice, array_in, 2))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, ECC_MAXLEN * 2 + SM2_USENAME_LEN + 2))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0x20) return USBStatusFail;

            for (n = 0; n < ECC_MAXLEN; n++)
            {
                KGx[n] = array_out[1 + ECC_MAXLEN * 0 + n];
                KGy[n] = array_out[1 + ECC_MAXLEN * 1 + n];
            }

            for (n = 0; n < SM2_USENAME_LEN; n++) sm2_UerName[n] = array_out[1 + ECC_MAXLEN * 2 + n];

            return 0;
        }

        private int NT_Set_Pin(string old_pin, string new_pin, string Path)
        {
            var array_in = new byte[256];
            var array_out = new byte[256];
            var n = 0;
            var hUsbDevice = 0;

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = SET_PIN;
            var b_oldpin = new byte[PIN_LEN];
            CopyStringToByte(b_oldpin, old_pin, PIN_LEN);
            var b_newpin = new byte[PIN_LEN];
            CopyStringToByte(b_newpin, new_pin, PIN_LEN);
            for (n = 0; n < PIN_LEN; n++)
            {
                array_in[2 + PIN_LEN * 0 + n] = b_oldpin[n];
                array_in[2 + PIN_LEN * 1 + n] = b_newpin[n];
            }

            if (!SetFeature(hUsbDevice, array_in, PIN_LEN * 2 + 2))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 2))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0x20) return USBStatusFail;
            if (array_out[1] != 0x20) return FAILPINPWD;
            return 0;
        }


        private int NT_SM2_Enc(byte[] inbuf, byte[] outbuf, byte inlen, string Path)
        {
            var array_in = new byte[256];
            var array_out = new byte[256];
            var n = 0;
            var hUsbDevice = 0;

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = MYENC;
            array_in[2] = inlen;
            for (n = 0; n < inlen; n++) array_in[3 + n] = inbuf[n];
            if (!SetFeature(hUsbDevice, array_in, inlen + 1 + 2))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, inlen + SM2_ADDBYTE + 3))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[0] != 0x20) return USBStatusFail;
            if (array_out[1] == 0) return FAILENC;

            for (n = 0; n < inlen + SM2_ADDBYTE; n++) outbuf[n] = array_out[2 + n];

            return 0;
        }

        private int NT_SM2_Dec(byte[] inbuf, byte[] outbuf, byte inlen, string pin, string Path)
        {
            var array_in = new byte[256];
            var array_out = new byte[256];
            var n = 0;
            var hUsbDevice = 0;

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = MYDEC;
            var b_pin = new byte[PIN_LEN];
            CopyStringToByte(b_pin, pin, PIN_LEN);
            for (n = 0; n < PIN_LEN; n++) array_in[2 + PIN_LEN * 0 + n] = b_pin[n];
            array_in[2 + PIN_LEN] = inlen;
            for (n = 0; n < inlen; n++) array_in[2 + PIN_LEN + 1 + n] = inbuf[n];
            if (!SetFeature(hUsbDevice, array_in, inlen + 1 + 2 + PIN_LEN))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, inlen - SM2_ADDBYTE + 4))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[2] != 0x20) return FAILPINPWD;
            if (array_out[1] == 0) return FAILENC;
            if (array_out[0] != 0x20) return USBStatusFail;
            for (n = 0; n < inlen - SM2_ADDBYTE; n++) outbuf[n] = array_out[3 + n];

            return 0;
        }

        private int NT_Sign(byte[] inbuf, byte[] outbuf, string pin, string Path)
        {
            var array_in = new byte[256];
            var array_out = new byte[256];
            var n = 0;
            var hUsbDevice = 0;

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = YTSIGN;
            var b_pin = new byte[PIN_LEN];
            CopyStringToByte(b_pin, pin, PIN_LEN);
            for (n = 0; n < PIN_LEN; n++) array_in[2 + PIN_LEN * 0 + n] = b_pin[n];
            for (n = 0; n < 32; n++) array_in[2 + PIN_LEN + n] = inbuf[n];
            if (!SetFeature(hUsbDevice, array_in, 32 + 2 + PIN_LEN))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 64 + 3))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[1] != 0x20) return FAILPINPWD;
            if (array_out[0] != 0x20) return USBStatusFail;
            for (n = 0; n < 64; n++) outbuf[n] = array_out[2 + n];

            return 0;
        }

        private int NT_Sign_2(byte[] inbuf, byte[] outbuf, string pin, string Path)
        {
            var array_in = new byte[256];
            var array_out = new byte[256];
            var n = 0;
            var hUsbDevice = 0;

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = YTSIGN_2;
            var b_pin = new byte[PIN_LEN];
            CopyStringToByte(b_pin, pin, PIN_LEN);
            for (n = 0; n < PIN_LEN; n++) array_in[2 + PIN_LEN * 0 + n] = b_pin[n];
            for (n = 0; n < 32; n++) array_in[2 + PIN_LEN + n] = inbuf[n];
            if (!SetFeature(hUsbDevice, array_in, 32 + 2 + PIN_LEN))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 64 + 3))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            if (array_out[1] != 0x20) return FAILPINPWD;
            if (array_out[0] != 0x20) return USBStatusFail;
            for (n = 0; n < 64; n++) outbuf[n] = array_out[2 + n];

            return 0;
        }


        private int NT_Verfiy(byte[] inbuf, byte[] InSignBuf, ref bool outbiao, string Path)
        {
            var array_in = new byte[256];
            var array_out = new byte[256];
            var n = 0;
            var hUsbDevice = 0;

            if (OpenMydivece(ref hUsbDevice, Path) != 0) return -92;
            array_in[1] = YTVERIFY;
            for (n = 0; n < 32; n++) array_in[2 + n] = inbuf[n];
            for (n = 0; n < 64; n++) array_in[2 + 32 + n] = InSignBuf[n];
            if (!SetFeature(hUsbDevice, array_in, 32 + 2 + 64))
            {
                CloseHandle(hUsbDevice);
                return -93;
            }

            if (!GetFeature(hUsbDevice, array_out, 3))
            {
                CloseHandle(hUsbDevice);
                return -94;
            }

            CloseHandle(hUsbDevice);
            outbiao = array_out[1] != 0;
            if (array_out[0] != 0x20) return USBStatusFail;

            return 0;
        }

        private string ByteArrayToHexString(byte[] in_data, int nlen)
        {
            var OutString = "";
            int n;
            for (n = 0; n < nlen; n++) OutString = OutString + in_data[n].ToString("X2");
            return OutString;
        }

        public int YT_GenKeyPair(ref string PriKey, ref string PubKeyX, ref string PubKeyY, string InPath)
        {
            int ret, n;
            byte[] b_PriKey = new byte[ECC_MAXLEN], b_PubKey = new byte[ECC_MAXLEN * 2 + 1]; //其中第一个字节是标志位，忽略
            var hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_GenKeyPair(b_PriKey, b_PubKey, InPath);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            PriKey = ByteArrayToHexString(b_PriKey, ECC_MAXLEN);
            PubKeyX = "";
            PubKeyY = "";
            for (n = 0; n < ECC_MAXLEN; n++)
            {
                PubKeyX = PubKeyX + b_PubKey[n + 1].ToString("X2");
                PubKeyY = PubKeyY + b_PubKey[n + 1 + ECC_MAXLEN].ToString("X2");
            }

            return ret;
        }

        public int Set_SM2_KeyPair(string PriKey, string PubKeyX, string PubKeyY, string SM2_UserName, string InPath)
        {
            int ret;
            byte[] b_PriKey = new byte[ECC_MAXLEN],
                b_PubKeyX = new byte[ECC_MAXLEN],
                b_PubKeyY = new byte[ECC_MAXLEN],
                b_SM2UserName = new byte[SM2_USENAME_LEN];
            HexStringToByteArray(PriKey, ref b_PriKey);
            HexStringToByteArray(PubKeyX, ref b_PubKeyX);
            HexStringToByteArray(PubKeyY, ref b_PubKeyY);
            CopyStringToByte(b_SM2UserName, SM2_UserName, SM2_USENAME_LEN);
            var hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_Set_SM2_KeyPair(b_PriKey, b_PubKeyX, b_PubKeyY, b_SM2UserName, InPath);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);

            return ret;
        }

        public int Get_SM2_PubKey(ref string PubKeyX, ref string PubKeyY, ref string sm2UserName, string InPath)
        {
            int ret;
            byte[] b_PubKeyX = new byte[ECC_MAXLEN],
                b_PubKeyY = new byte[ECC_MAXLEN],
                b_sm2UserName = new byte[SM2_USENAME_LEN];
            var hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_Get_SM2_PubKey(b_PubKeyX, b_PubKeyY, b_sm2UserName, InPath);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            PubKeyX = ByteArrayToHexString(b_PubKeyX, ECC_MAXLEN);
            PubKeyY = ByteArrayToHexString(b_PubKeyY, ECC_MAXLEN);
            var c_str = new StringBuilder("", SM2_USENAME_LEN);
            CopyByteToString(c_str, b_sm2UserName, SM2_USENAME_LEN);
            sm2UserName = c_str.ToString();
            return ret;
        }

        public int GetChipID(ref string OutChipID, string InPath)
        {
            int ret;
            var b_OutChipID = new byte[16];
            var hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_GetChipID(b_OutChipID, InPath);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            OutChipID = ByteArrayToHexString(b_OutChipID, 16);
            return ret;
        }

        public int SM2_EncBuf(byte[] InBuf, byte[] OutBuf, int inlen, string InPath)
        {
            int ret = 0, n, temp_inlen, incount = 0, outcount = 0;
            byte[] temp_InBuf = new byte[MAX_ENCLEN + SM2_ADDBYTE], temp_OutBuf = new byte[MAX_ENCLEN + SM2_ADDBYTE];
            var hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            while (inlen > 0)
            {
                if (inlen > MAX_ENCLEN)
                    temp_inlen = MAX_ENCLEN;
                else
                    temp_inlen = inlen;
                for (n = 0; n < temp_inlen; n++) temp_InBuf[n] = InBuf[incount + n];
                ret = NT_SM2_Enc(temp_InBuf, temp_OutBuf, (byte)temp_inlen, InPath);
                for (n = 0; n < temp_inlen + SM2_ADDBYTE; n++) OutBuf[outcount + n] = temp_OutBuf[n];
                if (ret != 0) goto err;
                inlen = inlen - MAX_ENCLEN;
                incount = incount + MAX_ENCLEN;
                outcount = outcount + MAX_DECLEN;
            }

            err:
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int SM2_DecBuf(byte[] InBuf, byte[] OutBuf, int inlen, string pin, string InPath)
        {
            int ret = 0, temp_inlen, n, incount = 0, outcount = 0;
            byte[] temp_InBuf = new byte[MAX_ENCLEN + SM2_ADDBYTE], temp_OutBuf = new byte[MAX_ENCLEN + SM2_ADDBYTE];
            var hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            while (inlen > 0)
            {
                if (inlen > MAX_DECLEN)
                    temp_inlen = MAX_DECLEN;
                else
                    temp_inlen = inlen;
                for (n = 0; n < temp_inlen; n++) temp_InBuf[n] = InBuf[incount + n];
                ret = NT_SM2_Dec(InBuf, OutBuf, (byte)temp_inlen, pin, InPath);
                for (n = 0; n < temp_inlen - SM2_ADDBYTE; n++) OutBuf[outcount + n] = temp_OutBuf[n];
                if (ret != 0) goto err;
                inlen = inlen - MAX_DECLEN;
                incount = incount + MAX_DECLEN;
                outcount = outcount + MAX_ENCLEN;
            }

            err:
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int SM2_EncString(string InString, ref string OutString, string InPath)
        {
            int n, incount = 0, outcount = 0;
            byte[] temp_InBuf = new byte[MAX_ENCLEN + SM2_ADDBYTE], temp_OutBuf = new byte[MAX_ENCLEN + SM2_ADDBYTE];
            var inlen = lstrlenA(InString) + 1;
            var outlen = (inlen / MAX_ENCLEN + 1) * SM2_ADDBYTE + inlen;
            var OutBuf = new byte[outlen];
            var InBuf = new byte[inlen];
            CopyStringToByte(InBuf, InString, inlen);
            int ret = 0, temp_inlen;
            var hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            while (inlen > 0)
            {
                if (inlen > MAX_ENCLEN)
                    temp_inlen = MAX_ENCLEN;
                else
                    temp_inlen = inlen;
                for (n = 0; n < temp_inlen; n++) temp_InBuf[n] = InBuf[incount + n];
                ret = NT_SM2_Enc(temp_InBuf, temp_OutBuf, (byte)temp_inlen, InPath);
                for (n = 0; n < temp_inlen + SM2_ADDBYTE; n++) OutBuf[outcount + n] = temp_OutBuf[n];
                if (ret != 0) goto err;
                inlen = inlen - MAX_ENCLEN;
                incount = incount + MAX_ENCLEN;
                outcount = outcount + MAX_DECLEN;
            }

            err:
            OutString = ByteArrayToHexString(OutBuf, outlen);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }

        public int SM2_DecString(string InString, ref string OutString, string pin, string InPath)
        {
            int n, incount = 0, outcount = 0;
            byte[] temp_InBuf = new byte[MAX_ENCLEN + SM2_ADDBYTE], temp_OutBuf = new byte[MAX_ENCLEN + SM2_ADDBYTE];
            var inlen = lstrlenA(InString) / 2;
            var outlen = inlen - (inlen / MAX_DECLEN + 1) * SM2_ADDBYTE;
            var InBuf = new byte[inlen];
            var OutBuf = new byte[outlen];
            int ret = 0, temp_inlen;
            HexStringToByteArray(InString, ref InBuf);
            var hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            while (inlen > 0)
            {
                if (inlen > MAX_DECLEN)
                    temp_inlen = MAX_DECLEN;
                else
                    temp_inlen = inlen;
                for (n = 0; n < temp_inlen; n++) temp_InBuf[n] = InBuf[incount + n];
                ret = NT_SM2_Dec(temp_InBuf, temp_OutBuf, (byte)temp_inlen, pin, InPath);
                for (n = 0; n < temp_inlen - SM2_ADDBYTE; n++) OutBuf[outcount + n] = temp_OutBuf[n];
                if (ret != 0) goto err;
                inlen = inlen - MAX_DECLEN;
                incount = incount + MAX_DECLEN;
                outcount = outcount + MAX_ENCLEN;
            }

            err:
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            var c_str = new StringBuilder("", outlen);
            CopyByteToString(c_str, OutBuf, outlen);
            OutString = c_str.ToString();
            return ret;
        }


        public int YtSetPin(string old_pin, string new_pin, string InPath)
        {
            int ret;
            var hsignal = CreateSemaphore(0, 1, 1, "ex_sim");
            WaitForSingleObject(hsignal, INFINITE);
            ret = NT_Set_Pin(old_pin, new_pin, InPath);
            ReleaseSemaphore(hsignal, 1, 0);
            CloseHandle(hsignal);
            return ret;
        }
    }
}
