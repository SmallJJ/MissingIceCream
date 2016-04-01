using System;
using System.Linq;

/// <summary>
/// 这个类只是测试用，到后边会配静态表,以数据集方式操作
/// </summary>
public static class TestConfigData
{
    private static IceCreamData[] m_IceCreamDatas;
    private static MaterialData[] m_MaterialDatas;
    private static PropData[] m_PropDatas;
    private static RoadData[] m_RoadDatas;

    #region public methods

    /// <summary>
    /// 获取冰淇淋配置数据
    /// </summary>
    /// <returns></returns>
    public static IceCreamData[] GetIceCreamDatas()
    {
        if (m_IceCreamDatas == null)
        {
            Array array = Enum.GetValues(typeof(IceCreamType));
            m_IceCreamDatas = new IceCreamData[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                IceCreamType type = (IceCreamType)array.GetValue(i);
                string name = "";
                string asset = type.ToString();
                string description = "";
                switch (type)
                {
                    case IceCreamType.Red:
                        name = "红色冰淇淋";
                        description = "根据时间的流逝会慢慢融化";
                        break;
                    case IceCreamType.Blue:
                        name = "蓝色冰淇淋";
                        description = "根据行走的路程会慢慢融化";
                        break;
                }
                m_IceCreamDatas[i] = new IceCreamData() { Name= name, Asset= asset, Type= type, Description= description};
            }
        }
        return m_IceCreamDatas;
    }

    /// <summary>
    /// 通过类型获取一个冰淇淋
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IceCreamData GetIceCreamDataByType(IceCreamType type)
    {
        return GetIceCreamDatas().FirstOrDefault(a => a.Type == type);
    }

    /// <summary>
    /// 获取物品配置数据
    /// </summary>
    /// <returns></returns>
    public static MaterialData[] GetMaterialDatas()
    {
        if (m_MaterialDatas == null)
        {
            Array array = Enum.GetValues(typeof(MaterialType));
            m_MaterialDatas = new MaterialData[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                MaterialType type = (MaterialType)array.GetValue(i);
                string name = "";
                string asset = type.ToString();
                string description = "";
                switch (type)
                {
                    case MaterialType.Chocolate:
                        name = "巧克力";
                        description = "巧克力，苦咖啡，加分50";
                        break;
                    case MaterialType.Corn:
                        name = "玉米";
                        description = "小时候的怀念，加分30";
                        break;
                    case MaterialType.Ormosia:
                        name = "红豆";
                        description = "红豆生南国，加分10";
                        break;
                    case MaterialType.MungBean:
                        name = "绿豆";
                        description = "清凉一夏，加分15";
                        break;
                    case MaterialType.Peanut:
                        name = "花生";
                        description = "你脑子进花生啦，加分20";
                        break;
                    case MaterialType.Egusi:
                        name = "瓜子";
                        description = "今年最流行焦糖味的瓜子啦，加分25";
                        break;
                    case MaterialType.Filbert:
                        name = "榛子";
                        description = "松鼠的最爱，加分50";
                        break;
                    case MaterialType.Raisin:
                        name = "葡萄干";
                        description = "葡萄美人，千万不要爱上我，加分40";
                        break;
                    case MaterialType.Freezer:
                        name = "冰箱";
                        description = "Came on，冰淇淋快到我的怀里来";
                        break;
                }
                m_MaterialDatas[i] = new MaterialData() { Name = name, Asset = asset, Type = type, Description= description };
            }
        }
        return m_MaterialDatas;
    }

    /// <summary>
    /// 通过类型获取一个物品
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static MaterialData GetMaterialDataByType(MaterialType type)
    {
        return GetMaterialDatas().FirstOrDefault(a => a.Type == type);
    }

    /// <summary>
    /// 获取道具配置数据
    /// </summary>
    /// <returns></returns>
    public static PropData[] GetPropDatas()
    {
        if (m_PropDatas == null)
        {
            Array array = Enum.GetValues(typeof(PropType));
            m_PropDatas = new PropData[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                PropType type = (PropType)array.GetValue(i);
                string name = "";
                string asset = type.ToString();
                string description = "";
                switch (type)
                {
                    case PropType.Converter:
                        name = "转换器";
                        description = "红色冰淇淋和蓝色冰淇淋互相转换";
                        break;
                    case PropType.OnOf:
                        name = "重力开关";
                        description = "踩到这个开关会消除障碍门";
                        break;
                    case PropType.Door:
                        name = "门";
                        description = "障碍门禁止通行，需要用重力开关消除";
                        break;
                    case PropType.CoolAir:
                        name = "冷气";
                        description = "恢复冰淇淋少部分融化";
                        break;
                    case PropType.Stove:
                        name = "火炉";
                        description = "加速冰淇淋融化速度一定的时间";
                        break;
                    case PropType.StayWarm:
                        name = "保温罩";
                        description = "保护冰淇淋不融化一定的时间";
                        break;
                    case PropType.Quickness:
                        name = "急速";
                        description = "加速冰淇淋移动速度一定的时间";
                        break;
                }

                m_PropDatas[i] = new PropData() { Name = name, Asset = asset, Type = type, Description= description };
            }
        }
        return m_PropDatas;
    }

    /// <summary>
    /// 通过类型获取一个道具
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static PropData GetPropDataByType(PropType type)
    {
        return GetPropDatas().FirstOrDefault(a => a.Type == type);
    }

    public static RoadData[] GetRoadDatas()
    {
        if (m_RoadDatas == null)
        {
            Array array = Enum.GetValues(typeof(RoadType));
            m_RoadDatas = new RoadData[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                RoadType type = (RoadType)array.GetValue(i);
                string name = "";
                string asset = type.ToString();
                string descrition = "";
                bool isBarrier=false;
                switch (type)
                {
                    case RoadType.Wall:
                        name = "墙壁";
                        descrition = "冰淇淋不可以在上面行走";
                        isBarrier = true;
                        break;
                    case RoadType.Road:
                        name = "道路";
                        descrition = "冰淇淋可以再上面行走";
                        isBarrier = false;
                        break;
                }
                m_RoadDatas[i] = new RoadData() { Name = name, Asset = asset,  Description= descrition,  IsBarrier= isBarrier, Type=type};
            }
        }
        return m_RoadDatas;
    }

    public static RoadData GetRoadDataByType(RoadType type)
    {
        return GetRoadDatas().FirstOrDefault(a => a.Type==type);
    }

    public static RoleDataBase[] GetRoleDataByType(UIRoleType roleType)
    {
        switch (roleType)
        {
            case UIRoleType.IceCream:
                return GetIceCreamDatas();
            case UIRoleType.Prop:
                return GetPropDatas();
            case UIRoleType.Marterial:
                return GetMaterialDatas();
            case UIRoleType.Ground:
                return GetRoadDatas();
        }
        return null;
    }
    #endregion
}
