using System.ServiceModel;


namespace wcf_chat
{
    
    [ServiceContract(CallbackContract = typeof(IServerChatCallBack))]
    public interface IServiceChat
    {
        [OperationContract]
        int Connect(string name);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract(IsOneWay = true)]
        void SendMSG(string msg, int id);// передает сам меседж, а айди нужен чтобы понять от кого
    }

    public interface IServerChatCallBack
    {

        [OperationContract (IsOneWay = true)]
        void MsgCallback(string msg);

    }
}
