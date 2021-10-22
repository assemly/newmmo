using Common;
using GameServer.Managers;

namespace GameServer.Services
{
    class MapService:Singleton<MapService>
    {
        public MapService()
        {

        }
        public void Init()
        {
            MapManager.Instance.Init();
        }
    }
}
