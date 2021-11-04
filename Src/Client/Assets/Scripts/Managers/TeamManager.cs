using Models;
using SkillBridge.Message;

namespace Managers
{
    class TeamManager : Singleton<TeamManager>
    {
        public void UpdateTeamInfo(NTeamInfo team)
        {
            User.Instance.TeamInfo = team;
            ShowTeamUI(team != null);
        }

        public void ShowTeamUI(bool show)
        {
            UIMain.Instance.ShowTeamUI(show);
        }
    }
}
