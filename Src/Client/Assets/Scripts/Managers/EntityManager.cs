using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    interface IEntityNotify 
    {
        void OnEntityRemoved();
    }
    
    class EntityManager : Singleton<EntityManager>
    {
        Dictionary<int, Entity> entites = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();
        
        public void RegisterEntityChangeNotify(int entityId,IEntityNotify notify)
        {
            this.notifiers[entityId] = notify;
        }
        public void AddEntity(Entity entity)
        {
            entites[entity.entityId] = entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            this.entites.Remove(entity.Id);
            if (notifiers.ContainsKey(entity.Id))
            {
                notifiers[entity.Id].OnEntityRemoved();
                notifiers.Remove(entity.Id);
            }
        }
       
    }
}
