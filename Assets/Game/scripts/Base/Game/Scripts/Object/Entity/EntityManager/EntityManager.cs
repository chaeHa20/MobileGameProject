using System;
using System.Collections.Generic;
using UnityHelper;

public class EntityManager : NonMonoSingleton<EntityManager>, IDisposable
{
    private UUID m_uuid = new UUID();
    private Dictionary<long, Entity> m_entities = new Dictionary<long, Entity>();
    private Dictionary<eTeam, HashSet<long>> m_entityUuidsByTeamType = new Dictionary<eTeam, HashSet<long>>();
    private Dictionary<eEntity, HashSet<long>> m_entityUuidsByEntityType = new Dictionary<eEntity, HashSet<long>>();
    private List<Entity> m_additionalEntities = new List<Entity>();

    private List<long> m_removeEntities = new List<long>();

    public UUID uuid => m_uuid;
    public Dictionary<long, Entity> entities => m_entities;
    public Dictionary<eTeam, HashSet<long>> entityUuidsByTeamType => m_entityUuidsByTeamType;
    public Dictionary<eEntity, HashSet<long>> entityUuidsByEntityType => m_entityUuidsByEntityType;
    public List<Entity> additionalEntities => m_additionalEntities;

    public virtual void initialize()
    {
        m_uuid.initialize();
    }

    public void createEntity<T>(EntityData entityData, Action<T> callback) where T : Entity
    {
        GamePoolHelper.getInstance().pop<T>(entityData.resourceId, (t) =>
        {
            entityData.loadModelCallback = () =>
            {
                m_additionalEntities.Add(t);
                addEntityUuidByTeamType(entityData.team, entityData.uuid);
                addEntityUuidByEntityType(entityData.type, entityData.uuid);

                callback?.Invoke(t);
            };

            entityData.uuid = m_uuid.make();
            t.initialize(entityData);
        });
    }

    private void addEntityUuidByTeamType(eTeam team, long entityUuid)
    {
        if (!m_entityUuidsByTeamType.TryGetValue(team, out HashSet<long> entityUuids))
        {
            entityUuids = new HashSet<long>();
            m_entityUuidsByTeamType.Add(team, entityUuids);
        }

        entityUuids.Add(entityUuid);
    }

    private void addEntityUuidByEntityType(eEntity entityType, long entityUuid)
    {
        if (!m_entityUuidsByEntityType.TryGetValue(entityType, out HashSet<long> entityUuids))
        {
            entityUuids = new HashSet<long>();
            m_entityUuidsByEntityType.Add(entityType, entityUuids);
        }

        entityUuids.Add(entityUuid);
    }

    private void removeEntityUuidByTeamType(eTeam team, long entityUuid)
    {
        if (m_entityUuidsByTeamType.TryGetValue(team, out HashSet<long> entityUuids))
        {
            entityUuids.Remove(entityUuid);
        }
    }

    private void removeEntityUuidByEntityType(eEntity entityType, long entityUuid)
    {
        if (m_entityUuidsByEntityType.TryGetValue(entityType, out HashSet<long> entityUuids))
        {
            entityUuids.Remove(entityUuid);
        }
    }

    public void addRemoveEntity(long uuid)
    {
        if (Logx.isActive)
            Logx.assert(0 < uuid, "uuid {0}", uuid);

        m_removeEntities.Add(uuid);
    }

    public T getEntity<T>(long uuid) where T : Entity
    {
        Entity entity;
        if (!m_entities.TryGetValue(uuid, out entity))
        {
            return null;
        }

        return entity as T;
    }

    public void updateEntityAbility<T>(eEntity type, LocalAbilities abilities) where T : AbilityEntity
    {
        if (m_entityUuidsByEntityType.TryGetValue(type, out HashSet<long> entityUuids))
        {
            foreach (var uuid in entityUuids)
            {
                var target = getEntity<T>(uuid);
                if (null != target)
                    target.setAbilities(abilities);
            }
        }
    }

    public List<T> findEntities<T>(eEntity entityType) where T : Entity
    {
        var res = new List<T>();
        if (m_entityUuidsByEntityType.TryGetValue(entityType, out HashSet<long> entityUuids))
        {
            foreach (var uuid in entityUuids)
            {
                var entity = getEntity<T>(uuid);
                res.Add(entity);
            }
        }

        return res;
    }

    public T findEntityByLocalUuid<T>(long localUuid) where T : Entity
    {
        foreach (var pair in m_entities)
        {
            if (pair.Value.localUuid == localUuid)
                return pair.Value as T;
        }

        return null;
    }

    public bool isExist(long uuid)
    {
        return m_entities.ContainsKey(uuid);
    }

    public T findEntity<T>(eEntity entityType, long entityUuid) where T : Entity
    {
        if (m_entityUuidsByEntityType.TryGetValue(entityType, out HashSet<long> entityUuids))
        {
            foreach (var uuid in entityUuids)
            {
                var entity = getEntity<T>(uuid);
                if (entity.uuid == entityUuid)
                    return entity;
            }
        }

        return null;
    }

    public void update(float dt)
    {
        foreach (var pair in m_entities)
        {
            pair.Value.update(dt);
        }

        updateEntities();
        removeUpdate();
    }

    private void updateEntities()
    {
        if (0 < m_additionalEntities.Count)
        {
            for (int index = 0; index < m_additionalEntities.Count; index++)
            {
                var entity = m_additionalEntities[index];
                if (!m_entities.ContainsKey(entity.uuid))
                    m_entities.Add(entity.uuid, entity);
                else
                    m_entities[entity.uuid] = entity;
            }
            m_additionalEntities.Clear();
        }
    }

    private void removeUpdate()
    {
        if (0 == m_removeEntities.Count)
            return;

        var removedEntityTypes = new HashSet<eEntity>();

        foreach (int uuid in m_removeEntities)
        {
            Entity entity = getEntity<Entity>(uuid);
            if (null == entity)
                return;

            if (!removedEntityTypes.Contains(entity.type))
                removedEntityTypes.Add(entity.type);

            removeEntityUuidByTeamType(entity.team, uuid);
            removeEntityUuidByEntityType(entity.type, uuid);

            if (Logx.isActive)
                Logx.trace("Remove character {0}, {1}", entity.type, uuid);

            entity.Dispose();
            m_entities.Remove(uuid);
        }

        m_removeEntities.Clear();
        removedEntityTypes.Clear();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var entity in m_entities)
            {
                entity.Value.Dispose();
            }

            m_entities.Clear();
            m_entityUuidsByTeamType.Clear();
            m_entityUuidsByEntityType.Clear();
            m_additionalEntities.Clear();
            m_removeEntities.Clear();
        }
    }
}

