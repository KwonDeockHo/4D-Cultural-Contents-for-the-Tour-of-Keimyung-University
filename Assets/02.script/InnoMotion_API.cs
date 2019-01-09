
using System.Runtime.InteropServices;     // DLL support
namespace Inno_MotionController
{
    class CInnoMotion_API
    {
        public const int OFF = 0;
        public const int ON = 1;

        public const int AXIS_NUM = 3;
           
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCREADMOTIONINFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public int[] bBusy;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public int[] bHome;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public int[] bAlarm;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public int[] bInpos;

            public int bEmer;
            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public double[] dCmd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public double[] dEnc;
        };

        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NMCREADMOTIONOUT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public int[] bCurrentOn;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public int[] bServoOn;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public int[] bDCCOn;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = AXIS_NUM)]
            public int[] bAlarmResetOn;
        };
        //------------------------------------------------------------------------------

        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetEquipNumber(int nEquipNo);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int OpenDevice();
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseDevice();
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetServoOnOff(int nAxisNo, int bOnOff);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetAlarmOnOff(int nAxisNo, int bOnOff);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadAllData(out NMCREADMOTIONINFO ReadMotionInfoData, out NMCREADMOTIONOUT ReadMotionOutData);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadMotionInfo(out NMCREADMOTIONINFO ReadMotionInfoData);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadMotionOut(out NMCREADMOTIONOUT ReadMotionOutData);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadBusyData(int nAxisNo);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadInpoData(int nAxisNo);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadAlarmData(int nAxisNo);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ReadCmdData(int nAxisNo);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ReadEncData(int nAxisNo);
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadEmerData();
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ResetEncoder();
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSettle();
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetNeutral();
        [DllImport("InnoMotionModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetOperation(
            double dAmpHeave, double dAmpRoll, double dAmpPitch, 
            double dFreHeave, double dFreRoll, double dFrePitch
            );
    };
};
//------------------------------------------------------------------------------



