using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace wcf_chat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]//все клиенты которые будут подк к хосту(нашему сервису), будут работать именно с нашим сервисом
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;
       

        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;
            SendMSG(" <=== " + user.Name + " ===> "+" connected to the chat!", 0);
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            //находим юзера и ремуваем
            var user = users.FirstOrDefault(i => i.ID == id);
            if(user!=null)
            {
                users.Remove(user);
                SendMSG(" <=== " + user.Name + " ===> " + " disconnect from chat!", 0);
            }
        }

        public void SendMSG(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();//дата сообщ серва текущ время

                var user = users.FirstOrDefault(i => i.ID == id);//находим юзера по йади
                if (user !=null)

                {
                    var Na ="<=== " + user.Name + " ===>";
                    answer += " " + Na + "\n ";
                }
                answer += msg;
                item.operationContext.GetCallbackChannel<IServerChatCallBack>().MsgCallback(answer);
            }
        }
    }
}
