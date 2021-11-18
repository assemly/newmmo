using GameServer.Core;
using GameServer.Entities;
using SkillBridge.Message;

namespace GameServer.Battle
{
    class BattleContext
    {
        public Battle Battle;
        public Creature Caster;
        public Creature Target;

        public NSkillCastInfo CastSkill;
        public NDamageInfo Damage;

        public SkillResult Result;
        public Vector3Int Position;
        public BattleContext(Battle battle)
        {
            this.Battle = battle;
        }
    }
}
