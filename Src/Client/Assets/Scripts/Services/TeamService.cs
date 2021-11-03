using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    class TeamService:Singleton<TeamService>
    {
        public void Init()
        {

        }

        internal void SendTeamInviteRequest(int id, string name)
        {
            throw new NotImplementedException();
        }

        internal void SendTeamLeaveRequest(int id)
        {
            throw new NotImplementedException();
        }
    }
}
