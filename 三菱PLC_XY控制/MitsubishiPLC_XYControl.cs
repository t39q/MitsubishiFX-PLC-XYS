using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 三菱PLC_XY控制
{
    class MitsubishiPLC_XYControl
    {
        public static byte[] OpenY(string Y)
        {
            //bool Flag = false;
            byte[] OpenByte = new byte[9];
            byte[] YAddressByte = GetYAddress(Y);
            OpenByte[0] = 0x02;
            OpenByte[1] = 0x37;
            OpenByte[2] = YAddressByte[0];
            OpenByte[3] = YAddressByte[1]; 
            OpenByte[4] = YAddressByte[2]; 
            OpenByte[5] = YAddressByte[3];
            OpenByte[6] = 0x03;
            byte[] CheckSumByte = new byte[6];
            for (int i = 1; i < CheckSumByte.Length+1; i++)
            {
                CheckSumByte[i-1] = OpenByte[i];
            }
            byte[] Check2Byte = new byte[2];
            Check2Byte = CheckSum(CheckSumByte);
            OpenByte[7] = Check2Byte[0];
            OpenByte[8] = Check2Byte[1];
            return OpenByte;
        }
        public static byte[] OpenX(string X)
        {
            //bool Flag = false;
            byte[] OpenByte = new byte[9];
            byte[] XAddressByte = GetXAddress(X);
            OpenByte[0] = 0x02;
            OpenByte[1] = 0x37;
            OpenByte[2] = XAddressByte[0];
            OpenByte[3] = XAddressByte[1];
            OpenByte[4] = XAddressByte[2];
            OpenByte[5] = XAddressByte[3];
            OpenByte[6] = 0x03;
            byte[] CheckSumByte = new byte[6];
            for (int i = 1; i < CheckSumByte.Length + 1; i++)
            {
                CheckSumByte[i - 1] = OpenByte[i];
            }
            byte[] Check2Byte = new byte[2];
            Check2Byte = CheckSum(CheckSumByte);
            OpenByte[7] = Check2Byte[0];
            OpenByte[8] = Check2Byte[1];
            return OpenByte;
        }
        public static byte[] OpenS(string S)
        {
            //bool Flag = false;
            byte[] OpenByte = new byte[9];
            byte[] SAddressByte = GetSAddress(S);
            OpenByte[0] = 0x02;
            OpenByte[1] = 0x37;
            OpenByte[2] = SAddressByte[0];
            OpenByte[3] = SAddressByte[1];
            OpenByte[4] = SAddressByte[2];
            OpenByte[5] = SAddressByte[3];
            OpenByte[6] = 0x03;
            byte[] CheckSumByte = new byte[6];
            for (int i = 1; i < CheckSumByte.Length + 1; i++)
            {
                CheckSumByte[i - 1] = OpenByte[i];
            }
            byte[] Check2Byte = new byte[2];
            Check2Byte = CheckSum(CheckSumByte);
            OpenByte[7] = Check2Byte[0];
            OpenByte[8] = Check2Byte[1];
            return OpenByte;
        }
        public static bool SendControlCmd(SerialPort serialPort,byte[] bytes)
        {
            bool Flag = false;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    serialPort.Write(bytes, 0, bytes.Length);
                    Thread.Sleep(50);
                    byte[] RcvBytes = new byte[1];
                    serialPort.Read(RcvBytes, 0, 1);
                    if (RcvBytes[0] != 6)
                    {
                        continue;
                    }
                    else
                    {
                        Flag = true;
                        break;
                    }
                }
                catch (Exception)
                {
                    continue;
                    //throw;
                }
                
            }
            return Flag;
        }
        public static string ReadXStatus(SerialPort serialPort, string X)
        {
            string OutResult = "";
            string Result = "";
            if (MitsubishiPLC_XYControl.SendReadCmd(serialPort, MitsubishiPLC_XYControl.ReadX(X),
                X, out OutResult))
            {
                Result = OutResult;
            }
            else
            {
                Result = "-1";
            }
            return Result;
        }
        public static string ReadSStatus(SerialPort serialPort, string S)
        {
            string OutResult = "";
            string Result = "";
            if (MitsubishiPLC_XYControl.SendReadCmdS(serialPort, MitsubishiPLC_XYControl.ReadS(S),
                S, out OutResult))
            {
                Result = OutResult;
            }
            else
            {
                Result = "-1";
            }
            return Result;
        }
        public static bool SendReadCmd(SerialPort serialPort, byte[] bytes,string X,out string Result)
        {
            string BinResult="";
            Result = "";
            bool Flag = false;
            for (int j = 0; j < 3; j++)
            {
                try
                {
                    serialPort.Write(bytes, 0, bytes.Length);
                    Thread.Sleep(50);
                    byte[] RcvBytes = new byte[8];
                    serialPort.Read(RcvBytes, 0, RcvBytes.Length);
                    byte[] CheckSumByte = new byte[5];
                    for (int i = 1; i < CheckSumByte.Length + 1; i++)
                    {
                        CheckSumByte[i - 1] = RcvBytes[i];
                    }
                    byte[] Check2Byte = new byte[2];
                    Check2Byte = CheckSum(CheckSumByte);
                    if (RcvBytes[6] == Check2Byte[0] &&
                    RcvBytes[7] == Check2Byte[1])
                    {
                        //02 30 41 38 30 03 44 43   
                        // 从第二个字节算起，顺序为1032，38 30 30 41
                        //当进行按字节转换为ASCII时，高位在右边
                        byte[] ResultByte = new byte[4];
                        ResultByte[0] = RcvBytes[3]; //3
                        ResultByte[1] = RcvBytes[4]; //2
                        ResultByte[2] = RcvBytes[1]; //1
                        ResultByte[3] = RcvBytes[2]; //0
                        string HexResult = Encoding.ASCII.GetString(ResultByte);
                        BinResult = Convert.ToString(Convert.ToInt32(HexResult, 16), 2).PadLeft(16, '0');
                        BinResult = BinResult.Substring(BinResult.Length - 8, 8);
                        Result = JudgingByBit(BinResult, X);
                        Flag = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    continue;
                    //throw;
                }
                
            }
            return Flag;
        }
        public static bool SendReadCmdS(SerialPort serialPort, byte[] bytes, string S, out string Result)
        {
            string BinResult = "";
            Result = "";
            bool Flag = false;
            for (int j = 0; j < 3; j++)
            {
                try
                {
                    serialPort.Write(bytes, 0, bytes.Length);
                    Thread.Sleep(50);
                    byte[] RcvBytes = new byte[8];
                    serialPort.Read(RcvBytes, 0, RcvBytes.Length);
                    byte[] CheckSumByte = new byte[5];
                    for (int i = 1; i < CheckSumByte.Length + 1; i++)
                    {
                        CheckSumByte[i - 1] = RcvBytes[i];
                    }
                    byte[] Check2Byte = new byte[2];
                    Check2Byte = CheckSum(CheckSumByte);
                    if (RcvBytes[6] == Check2Byte[0] &&
                    RcvBytes[7] == Check2Byte[1])
                    {
                        byte[] ResultByte = new byte[4];
                        //02 30 41 38 30 03 44 43   
                        // 从第二个字节算起，顺序为1032，38 30 30 41
                        //当进行按字节转换为ASCII时，高位在右边
                        ResultByte[0] = RcvBytes[3]; //3
                        ResultByte[1] = RcvBytes[4]; //2
                        ResultByte[2] = RcvBytes[1]; //1
                        ResultByte[3] = RcvBytes[2]; //0
                        string HexResult = Encoding.ASCII.GetString(ResultByte);
                        BinResult= Convert.ToString(Convert.ToInt32(HexResult, 16), 2).PadLeft(16, '0');
                        BinResult = BinResult.Substring(BinResult.Length - 8, 8);
                        Result = JudgingByBitS(BinResult, S);
                        Flag = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    continue;
                    //throw;
                }

            }
            return Flag;
        }
        public static string JudgingByBit(string BinResult,string X)
        {
            char[] BinResultArray = BinResult.ToCharArray();
            string Result = "";
            switch (X)
            {
                case "X0":
                    Result = BinResultArray[7].ToString();
                    break;
                case "X1":
                    Result = BinResultArray[6].ToString();
                    break;
                case "X2":
                    Result = BinResultArray[5].ToString();
                    break;
                case "X3":
                    Result = BinResultArray[4].ToString();
                    break;
                case "X4":
                    Result = BinResultArray[3].ToString();
                    break;
                case "X5":
                    Result = BinResultArray[2].ToString();
                    break;
                case "X6":
                    Result = BinResultArray[1].ToString();
                    break;
                case "X7":
                    Result = BinResultArray[0].ToString();
                    break;
            }
            return Result;
        }
        public static string JudgingByBitS(string BinResult, string S)
        {
            char[] BinResultArray = BinResult.ToCharArray();
            string Result = "";
            switch (S)
            {
                case "S0":
                    Result = BinResultArray[7].ToString();
                    break;
                case "S1":
                    Result = BinResultArray[6].ToString();
                    break;
                case "S2":
                    Result = BinResultArray[5].ToString();
                    break;
                case "S3":
                    Result = BinResultArray[4].ToString();
                    break;
                case "S4":
                    Result = BinResultArray[3].ToString();
                    break;
                case "S5":
                    Result = BinResultArray[2].ToString();
                    break;
                case "S6":
                    Result = BinResultArray[1].ToString();
                    break;
                case "S7":
                    Result = BinResultArray[0].ToString();
                    break;
                case "S8":
                    Result = BinResultArray[7].ToString();
                    break;
                case "S9":
                    Result = BinResultArray[6].ToString();
                    break;
                case "S10":
                    Result = BinResultArray[5].ToString();
                    break;
                case "S11":
                    Result = BinResultArray[4].ToString();
                    break;
                case "S12":
                    Result = BinResultArray[3].ToString();
                    break;
                case "S13":
                    Result = BinResultArray[2].ToString();
                    break;
                case "S14":
                    Result = BinResultArray[1].ToString();
                    break;
                case "S15":
                    Result = BinResultArray[0].ToString();
                    break;
            }
            return Result;
        }
        public static byte[] CloseY(string Y)
        {
            //bool Flag = false;
            byte[] OpenByte = new byte[9];
            byte[] YAddressByte = GetYAddress(Y);
            OpenByte[0] = 0x02;
            OpenByte[1] = 0x38;
            OpenByte[2] = YAddressByte[0];
            OpenByte[3] = YAddressByte[1];
            OpenByte[4] = YAddressByte[2];
            OpenByte[5] = YAddressByte[3];
            OpenByte[6] = 0x03;
            byte[] CheckSumByte = new byte[6];
            for (int i = 1; i < CheckSumByte.Length + 1; i++)
            {
                CheckSumByte[i - 1] = OpenByte[i];
            }
            byte[] Check2Byte = new byte[2];
            Check2Byte = CheckSum(CheckSumByte);
            OpenByte[7] = Check2Byte[0];
            OpenByte[8] = Check2Byte[1];
            return OpenByte;
        }
        public static byte[] CloseS(string S)
        {
            //bool Flag = false;
            byte[] OpenByte = new byte[9];
            byte[] SAddressByte = GetSAddress(S);
            OpenByte[0] = 0x02;
            OpenByte[1] = 0x38;
            OpenByte[2] = SAddressByte[0];
            OpenByte[3] = SAddressByte[1];
            OpenByte[4] = SAddressByte[2];
            OpenByte[5] = SAddressByte[3];
            OpenByte[6] = 0x03;
            byte[] CheckSumByte = new byte[6];
            for (int i = 1; i < CheckSumByte.Length + 1; i++)
            {
                CheckSumByte[i - 1] = OpenByte[i];
            }
            byte[] Check2Byte = new byte[2];
            Check2Byte = CheckSum(CheckSumByte);
            OpenByte[7] = Check2Byte[0];
            OpenByte[8] = Check2Byte[1];
            return OpenByte;
        }
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr +=bytes[i].ToString("X2")+" ";
                }
            }
            return returnStr;
        }
        public static byte[] CheckSum(byte[] Byte)
        {
            //做加法后转为16进制，取右边两位的ASCII
            int Sum = 0;
            for (int i = 0; i < Byte.Length; i++)
            {
                Sum = Sum + Byte[i];
            }
            string CheckSumString = Sum.ToString("X8");//大写X代表大写字母，小写代表小写，此处不同，需要注意。
            string Right2String = CheckSumString.Substring(CheckSumString.Length-2,2);
            byte[] CheckSumByte = new byte[2];
            ASCIIEncoding charToASCII = new ASCIIEncoding();
            CheckSumByte = charToASCII.GetBytes(Right2String.ToArray());
            return CheckSumByte;
        }
        public static byte[] ReadX(string X)
        {
            //bool Flag = false;
            byte[] ReadByte = new byte[11];
            byte[] XAddressByte = GetX_ReadAddress(X);
            ReadByte[0] = 0x02;
            ReadByte[1] = 0x30;
            ReadByte[2] = XAddressByte[0];
            ReadByte[3] = XAddressByte[1];
            ReadByte[4] = XAddressByte[2];
            ReadByte[5] = XAddressByte[3];
            ReadByte[6] = 0x30;
            ReadByte[7] = 0x32;
            ReadByte[8] = 0x03;
            byte[] CheckSumByte = new byte[9];
            for (int i = 1; i < CheckSumByte.Length + 1; i++)
            {
                CheckSumByte[i - 1] = ReadByte[i];
            }
            byte[] Check2Byte = new byte[2];
            Check2Byte = CheckSum(CheckSumByte);
            ReadByte[9] = Check2Byte[0];
            ReadByte[10] = Check2Byte[1];
            return ReadByte;
        }
        public static byte[] ReadS(string S)
        {
            //bool Flag = false;
            byte[] ReadByte = new byte[11];
            byte[] SAddressByte = GetS_ReadAddress(S);
            ReadByte[0] = 0x02;
            ReadByte[1] = 0x30;
            ReadByte[2] = SAddressByte[0];
            ReadByte[3] = SAddressByte[1];
            ReadByte[4] = SAddressByte[2];
            ReadByte[5] = SAddressByte[3];
            ReadByte[6] = 0x30;
            ReadByte[7] = 0x32;
            ReadByte[8] = 0x03;
            byte[] CheckSumByte = new byte[9];
            for (int i = 1; i < CheckSumByte.Length + 1; i++)
            {
                CheckSumByte[i - 1] = ReadByte[i];
            }
            byte[] Check2Byte = new byte[2];
            Check2Byte = CheckSum(CheckSumByte);
            ReadByte[9] = Check2Byte[0];
            ReadByte[10] = Check2Byte[1];
            return ReadByte;
        }
        public static void OpenMitsubishiPLC(SerialPort serialPort,string COM)
        {
            try
            {
                serialPort.PortName = COM;
                serialPort.BaudRate = Convert.ToInt32(9600);
                serialPort.Parity = Parity.Even;
                serialPort.DataBits = Convert.ToInt32(7);
                serialPort.StopBits = StopBits.One;
                serialPort.Handshake = Handshake.None;
                serialPort.WriteTimeout = 500;
                serialPort.ReadTimeout = 500;
                if (serialPort.IsOpen == false)
                {
                    serialPort.Open();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static byte[] GetXAddress(string X)
        {

            string XAddress = "";
            switch (X)
            {
                case "X0":
                    XAddress = "0004";
                    break;
                case "X1":
                    XAddress = "0104";
                    break;
                case "X2":
                    XAddress = "0204";
                    break;
                case "X3":
                    XAddress = "0304";
                    break;
                case "X4":
                    XAddress = "0404";
                    break;
                case "X5":
                    XAddress = "0504";
                    break;
                case "X6":
                    XAddress = "0604";
                    break;
                case "X7":
                    XAddress = "0704";
                    break;
            }
            byte[] YAddressByte = new byte[4];
            ASCIIEncoding charToASCII = new ASCIIEncoding();
            YAddressByte = charToASCII.GetBytes(XAddress.ToArray());
            return YAddressByte;
        }
        public static byte[] GetYAddress(string Y)
        {

            string YAddress="";
            switch (Y)
            {
                case "Y0":
                    //05 00
                    YAddress = "0005";
                    break;
                case "Y1":
                    //05 01
                    YAddress = "0105";
                    break;
                case "Y2":
                    //05 02
                    YAddress = "0205";
                    break;
                case "Y3":
                    //05 03
                    YAddress = "0305";
                    break;
                case "Y4":
                    //05 04
                    YAddress = "0405";
                    break;
                case "Y5":
                    //05 05
                    YAddress = "0505";
                    break;
                case "Y6":
                    //05 06
                    YAddress = "0605";
                    break;
                case "Y7":
                    //05 07
                    YAddress = "0705";
                    break;
                case "Y10":
                    //05 00
                    YAddress = "0805";
                    break;
                case "Y11":
                    //05 01
                    YAddress = "0905";
                    break;
                case "Y12":
                    //05 02
                    YAddress = "0A05";
                    break;
                case "Y13":
                    //05 03
                    YAddress = "0B05";
                    break;
                case "Y14":
                    //05 04
                    YAddress = "0C05";
                    break;
                case "Y15":
                    //05 05
                    YAddress = "0D05";
                    break;
                case "Y16":
                    //05 06
                    YAddress = "0E05";
                    break;
                case "Y17":
                    //05 07
                    YAddress = "0F05";
                    break;
            }
            byte[] YAddressByte = new byte[4]; 
            ASCIIEncoding charToASCII = new ASCIIEncoding();
            YAddressByte = charToASCII.GetBytes(YAddress.ToArray());
            return YAddressByte;
        }
        public static byte[] GetSAddress(string S)
        {

            string SAddress = "";
            switch (S)
            {
                case "S0":
                    SAddress = "0000";
                    break;
                case "S1":
                    SAddress = "0100";
                    break;
                case "S2":
                    SAddress = "0200";
                    break;
                case "S3":
                    SAddress = "0300";
                    break;
                case "S4":
                    SAddress = "0400";
                    break;
                case "S5":
                    SAddress = "0500";
                    break;
                case "S6":
                    SAddress = "0600";
                    break;
                case "S7":
                    SAddress = "0700";
                    break;
                case "S8":
                    SAddress = "0800";
                    break;
                case "S9":
                    SAddress = "0900";
                    break;
                case "S10":
                    SAddress = "0A00";
                    break;
                case "S11":
                    SAddress = "0B00";
                    break;
                case "S12":
                    SAddress = "0C00";
                    break;
                case "S13":
                    SAddress = "0D00";
                    break;
                case "S14":
                    SAddress = "0E00";
                    break;
                case "S15":
                    SAddress = "0F00";
                    break;
            }
            byte[] YAddressByte = new byte[4];
            ASCIIEncoding charToASCII = new ASCIIEncoding();
            YAddressByte = charToASCII.GetBytes(SAddress.ToArray());
            return YAddressByte;
        }
        public static byte[] GetX_ReadAddress(string X)//For Read
        {
            // X0-X7读取的地址相同，将得到的数字转为2进制判断通断。
            string XAddress = "";
            switch (X)
            {
                case "X0":
                    XAddress = "0080";
                    break;
                case "X1":
                    XAddress = "0080";
                    break;
                case "X2":
                    XAddress = "0080";
                    break;
                case "X3":
                    XAddress = "0080";
                    break;
                case "X4":
                    XAddress = "0080";
                    break;
                case "X5":
                    XAddress = "0080";
                    break;
                case "X6":
                    XAddress = "0080";
                    break;
                case "X7":
                    XAddress = "0080";
                    break;
            }
            byte[] YAddressByte = new byte[4];
            ASCIIEncoding charToASCII = new ASCIIEncoding();
            YAddressByte = charToASCII.GetBytes(XAddress.ToArray());
            return YAddressByte;
        }
        public static byte[] GetS_ReadAddress(string S)//For Read
        {
            // X0-X7读取的地址相同，将得到的数字转为2进制判断通断。
            string SAddress = "";
            switch (S)
            {
                case "S0":
                    SAddress = "0000";
                    break;
                case "S1":
                    SAddress = "0000";
                    break;
                case "S2":
                    SAddress = "0000";
                    break;
                case "S3":
                    SAddress = "0000";
                    break;
                case "S4":
                    SAddress = "0000";
                    break;
                case "S5":
                    SAddress = "0000";
                    break;
                case "S6":
                    SAddress = "0000";
                    break;
                case "S7":
                    SAddress = "0000";
                    break;
                case "S8":
                    SAddress = "0001";
                    break;
                case "S9":
                    SAddress = "0001";
                    break;
                case "S10":
                    SAddress = "0001";
                    break;
                case "S11":
                    SAddress = "0001";
                    break;
                case "S12":
                    SAddress = "0001";
                    break;
                case "S13":
                    SAddress = "0001";
                    break;
                case "S14":
                    SAddress = "0001";
                    break;
                case "S15":
                    SAddress = "0001";
                    break;
            }
            byte[] YAddressByte = new byte[4];
            ASCIIEncoding charToASCII = new ASCIIEncoding();
            YAddressByte = charToASCII.GetBytes(SAddress.ToArray());
            return YAddressByte;
        }
    }
}
