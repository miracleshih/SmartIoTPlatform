using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WSIP.RS232;

namespace WSIP
{
    public class WRS232 : IConnect
    {

        List<_receiveNotify> NotifyList = new List<_receiveNotify>();

        
        /// <summary>
        /// Error logs in functions.
        /// </summary>
        private string _LastError = "";
        public string LastError
        {
            get { return _LastError; }
            set
            {
                _LastError = value;
                Console.WriteLine($"{_LastError}");
            }
        }

        SerialPort serial = new SerialPort();


        public WRS232(string comport, int baudrate, _receiveNotify func)
        {
            NotifyList.Add(func);

            try
            {
                serial = new SerialPort(comport, baudrate);
                serial.DataReceived += Serial_DataReceived;
                serial.Open();
            }
            catch (Exception ex)
            {
                serial = null;
            }
        }

        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int bytes = sp.BytesToRead;
            byte[] buffer = new byte[bytes];
            sp.Read(buffer, 0, bytes);

            for(int i=0; i< NotifyList.Count; i++)
            {
                NotifyList[i](buffer);
            }

            return;
        }

        public bool Send<T>(T sendobj)
        {
            if (!serial.IsOpen)
                return false;

            try
            {
                if (typeof(T) == typeof(string))
                {   // Send string via RS232.
                    string str = (string)sendobj.ToString();
                    serial.Write(str);
                }
                else if (typeof(T) == typeof(byte[]))
                {   // Send binary data via RS232.
                    byte[] data = (byte[])((object)sendobj);
                    serial.Write(data, 0, data.Length);
                }
                else
                {   // Send other data via RS232. Convert to XML(string)
                    // XmlSerializer ser = new XmlSerializer(typeof(T));
                    RS232Entity data = new RS232Entity()
                    {
                        Data = (object)sendobj,
                    };
                    string str = data.SerializeObject();
                    serial.Write(str);
                }
            }
            catch(Exception ex)
            {
                LastError = $"ERROR: Send data error: {ex.ToString()}";
                return false;
            }

            return true;
        }

        public bool NotifyRegister(_receiveNotify func)
        {
            NotifyList.Add(func);
            return true;
        }

        public bool ClearInQueue()
        {
            return true;
        }

        public bool ClearOutQueue()
        {
            return true;
        }



    }
}
