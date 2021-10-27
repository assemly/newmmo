using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class HelloWorldService:Singleton<HelloWorldService>
    {
        public void Init()
        {

        }
        //public void Start()
        //{
        //    MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);
        //}

        //private void OnFirstTestRequest(NetConnection<NetSession> sender, FirstTestRequest message)
        //{
        //    Log.InfoFormat("UserLoginRequest: HelloWorld:{0}", message.Helloworld);
        //}

        public void Stop()
        {

        }
    }
}
