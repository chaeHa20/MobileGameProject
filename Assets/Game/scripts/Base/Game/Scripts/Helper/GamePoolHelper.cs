using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper;
using System;

public class GamePoolHelper : PoolHelper
{
    public static GamePoolHelper getInstance()
    {
        return getInstance<GamePoolHelper>();
    }

    public override void initialize()
    {
        base.initialize();

        initPreload();
    }

    private void initPreload()
    {
        var resourceTable = GameTableHelper.instance.getTable<ResourceTable>((int)eTable.Resource);
        resourceTable.forEach<ResourceRow>((row) =>
        {
            if (0 < row.poolId)
            {
                var poolRow = GameTableHelper.instance.getRow<PoolRow>((int)eTable.Pool, row.poolId);
                if (poolRow.isPreload)
                {
                    if (!existPool(row.filename))
                    {
                        createPool(row, null);
                    }
                }
            }
        });
    }

    public void popEntityModel<T>(eResource resourceId, int modelId, Action<T> callback) where T : PoolObject
    {
        var resourceRow = GameTableHelper.instance.getRow<ResourceRow>((int)eTable.Resource, (int)resourceId);
        var filename = string.Format(resourceRow.filename, modelId);
        if (existPool(filename))
        {
            pop<T>(filename, callback);
        }
        else
        {
            var resPath = GameTableHelper.instance.getEntityModelPath(resourceId, modelId);
            createPool(resPath, resourceRow.poolId, () =>
            {
                pop<T>(filename, callback);
            });
        }
    }

    public void pop<T>(eResource resourceId, Action<T> callback) where T : PoolObject
    {
        pop<T>((int)resourceId, callback);
    }

    public void pop<T>(int resourceId, Action<T> callback) where T : PoolObject
    {
        if (Logx.isActive)
            Logx.assert(0 < resourceId, "Invalid resource id {0}", resourceId);

        var resourceRow = GameTableHelper.instance.getRow<ResourceRow>((int)eTable.Resource, resourceId);
        if (existPool(resourceRow.filename))
        {
            pop<T>(resourceRow.filename, callback);
        }
        else
        {
            createPool(resourceRow, () =>
            {
                pop<T>(resourceRow.filename, callback);
            });
        }
    }

    private void createPool(ResourceRow resourceRow, Action callback)
    {
        var resPath = GameTableHelper.instance.getResourcePath(resourceRow.id);
        createPool(resPath, resourceRow.poolId, callback);
    }

    private void createPool(string resPath, int poolId, Action callback)
    {
        if (Logx.isActive)
            Logx.assert(0 < poolId, "Invalid pool id {0}", poolId);

        var poolRow = GameTableHelper.instance.getRow<PoolRow>((int)eTable.Pool, poolId);
        var createPoolData = new CreatePoolData
        {
            resPath = resPath,
            initCount = poolRow.initCount,
            maxCount = poolRow.maxCount,
            isUi = poolRow.isUI,
            isAsync = poolRow.isAync,
            reallocateCount = poolRow.reallocateCount
        };

        createPool(createPoolData, callback);
    }

    // 창이 닫혀도 pool이 파괴되지 않도록
    private void setParentToLastSafeArea(Transform particle, GameObject positionObject)
    {
        var safeArea = GameUIHelper.getInstance().canvasGroup.getLastSafeArea();
        UIHelper.instance.setParent(safeArea, particle.gameObject, SetParentOption.notFullAndReset());
        particle.position = positionObject.transform.position;
    }
}
