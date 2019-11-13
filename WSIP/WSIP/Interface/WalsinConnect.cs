using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WSIP
{
    public delegate bool _receiveNotify(object message);
    // public delegate bool _NotifyRS232(byte[] message);


    public interface IConnect
    {
        bool Send<T>(T message);

        //bool ReceiveData(out string message);
        bool NotifyRegister(_receiveNotify func);

        bool ClearInQueue();

        bool ClearOutQueue();
    }
}
