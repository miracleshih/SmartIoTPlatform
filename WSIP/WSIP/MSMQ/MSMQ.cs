using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WSIP
{
    public partial class MSMQ : IConnect
    {
        List<_receiveNotify> NotifyList = new List<_receiveNotify>();
        //public MSMQReceive msmqReceive = new MSMQReceive();
        //public MSMQSend msmqSend = new MSMQSend();

        List<MessageQueue> msMQSend = new List<MessageQueue>();
        List<MessageQueue> msMQRecive = new List<MessageQueue>();

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

        /// <summary>
        /// Initial MSMQ service.
        /// </summary>
        /// <param name="sendQueue"></param>
        /// <param name="receiveQueue"></param>
        public MSMQ(string sendQueue, string receiveQueue)
        {
            if(!(sendQueue is null || sendQueue.Length <= 0))
            {
                msMQSend.Add(new MessageQueue(sendQueue));
            }

            if (!(receiveQueue is null || receiveQueue.Length <= 0))
            {
                MessageQueue mq = new MessageQueue(receiveQueue);
                mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                mq.ReceiveCompleted += Mq_ReceiveCompleted;
                // mq.BeginReceive();
                msMQRecive.Add(mq);

                //Task tk = new Task(TaskReceive);
                // tk.Start();
            }
        }

        public MSMQ(List<string> listsendQueue, List<string> listreceiveQueue)
        {
            LastError = "";

            if (!(listsendQueue is null || listsendQueue.Count <= 0))
            {
                for(int i=0; i<listsendQueue.Count; i++)
                {
                    try
                    {
                        msMQSend.Add(new MessageQueue(listsendQueue[i]));
                    }
                    catch(Exception ex)
                    {
                        LastError += $"ERROR: Send queue error: {listreceiveQueue[i]}\n";
                    }
                }
            }

            if (!(listreceiveQueue is null || listreceiveQueue.Count <= 0))
            {
                for(int i=0; i< listreceiveQueue.Count; i++)
                {
                    try
                    {
                        MessageQueue mq = new MessageQueue(listreceiveQueue[i]);
                        mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                        mq.ReceiveCompleted += Mq_ReceiveCompleted;
                        // mq.BeginReceive();
                        msMQRecive.Add(mq);
                    }
                    catch(Exception ex)
                    {
                        LastError += $"ERROR: Receive queue error: {listreceiveQueue[i]}\n";
                    }
                }
                //Task tk = new Task(TaskReceive);
                // tk.Start();
            }
        }

        public bool StartReceive()
        {
            LastError = "";
            for (int i = 0; i < msMQRecive.Count; i++)
            {
                try
                {
                    msMQRecive[i].BeginReceive();
                }
                catch (Exception ex)
                {
                    LastError += $"ERROR: ReceiveQueue enable receive has problem: {msMQRecive[i].QueueName}";
                }
            }
            if (LastError == "")
                return true;
            else
                return false;

        }



        private void Mq_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue myQueue = (MessageQueue)sender;
                // End the asynchronous receive operation.
                Message message = myQueue.EndReceive(e.AsyncResult);

                for (int i = 0; i < NotifyList.Count; i++)
                {
                    NotifyList[i](message);
                }


                myQueue.BeginReceive();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.ToString()}");
            }

            return;
        }

        public bool ClearInQueue()
        {
            for(int i=0; i<msMQRecive.Count; i++)
            {
                msMQRecive[i].Purge();
            }
            return true;    // msmqReceive.ClearQueue();
        }

        public bool ClearOutQueue()
        {
            for (int i = 0; i < msMQRecive.Count; i++)
            {
                msMQSend[i].Purge();
            }
            return true;    // msmqSend.ClearQueue();
        }

        //public void TaskReceive()
        //{
        //    string queueName = "";
        //    int idx = 0;
        //    for(; ; idx++)
        //    {
        //        try
        //        {
        //            idx %= msMQRecive.Count;
        //            queueName = msMQRecive[idx].QueueName;

        //            if (!msMQRecive[idx].CanRead)
        //            {
        //                System.Threading.Thread.Sleep(100);
        //                continue;
        //            }

        //            Message obj = ((MessageQueue)msMQRecive[idx]).Receive();

        //            // Message notify
        //            for (int i = 0; i < NotifyList.Count; i++)
        //            {
        //                NotifyList[i](obj);
        //            }
        //        }
        //        catch(Exception ex)
        //        {
        //            Console.WriteLine($"ERROR: Queue {queueName} exception.\n    {ex.ToString()}");
        //        }
        //    }
        //}

        public bool Send<T>(T sendobj)
        {
            //return msmqSend.SendMessage(sendobj);
            string queueName = "";
            int cntError = 0;

            Message MyMessage = new Message();
            MyMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });
            MyMessage.Label = typeof(T).ToString();
            MyMessage.Priority = MessagePriority.High;
            MyMessage.Body = sendobj;


            for (int i=0; i<msMQSend.Count; i++)
            {
                try
                {
                    queueName = msMQSend[i].QueueName;
                    
                    if (!msMQSend[i].CanWrite)
                        continue;

                    msMQSend[i].Send(MyMessage);
                }
                catch (Exception ex)
                {
                    LastError = $"ERROR: Queue {queueName} exception.\n    {ex.ToString()}";
                    cntError++;
                }
            }

            if(cntError > 0)
            {
                LastError = $"Total send queue error: {cntError}";
                return false;
            }
            return true;
        }

        public bool NotifyRegister(_receiveNotify func)
        {
            NotifyList.Add((_receiveNotify)func);
            return true;
        }
    }
}
