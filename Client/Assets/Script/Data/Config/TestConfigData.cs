using System;
public static class TestConfigData
{
    private static IceCreamData[] m_IceCreamDatas;
    private static MaterialData[] m_MaterialDatas;
    private static PropData[] m_PropDatas;

    #region public methods

    /// <summary>
    /// 获取个冰淇淋配置数据
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
                string asset = "";
                switch (type)
                {
                    case IceCreamType.Red:
                        name = "红色冰淇淋";
                        asset = "RedIceCream";
                        break;
                    case IceCreamType.Blue:
                        name = "蓝色冰淇淋";
                        asset = "BlueIceCream";
                        break;
                }
                m_IceCreamDatas[i] = new IceCreamData() { Name= name, Asset= asset, Type= type};
            }
        }
        return m_IceCreamDatas;
    }

    /// <summary>
    /// 获取个物品配置数据
    /// </summary>
    /// <returns></returns>
    public static MaterialData[] GetMaterialDatas()
    {
        if (m_MaterialDatas == null)
        {
            Array array = Enum.GetValues(typeof(MarterialType));
            m_MaterialDatas = new MaterialData[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                MarterialType type = (MarterialType)array.GetValue(i);
                string name = "";
                string asset = "";
                switch (type)
                {
                    case MarterialType.Chocolate:
                        name = "巧克力";
                        asset = "Other";
                        break;
                    case MarterialType.Corn:
                        name = "玉米";
                        asset = "Other";
                        break;
                    case MarterialType.Ormosia:
                        name = "红豆";
                        asset = "Other";
                        break;
                    case MarterialType.MungBean:
                        name = "绿豆";
                        asset = "Other";
                        break;
                    case MarterialType.Peanut:
                        name = "花生";
                        asset = "Other";
                        break;
                    case MarterialType.Egusi:
                        name = "瓜子";
                        asset = "Other";
                        break;
                    case MarterialType.Filbert:
                        name = "榛子";
                        asset = "Other";
                        break;
                    case MarterialType.Raisin:
                        name = "葡萄干";
                        asset = "Other";
                        break;
                    case MarterialType.Freezer:
                        name = "冰箱";
                        asset = "Other";
                        break;
                }
                m_MaterialDatas[i] = new MaterialData() { Name = name, Asset = asset, Type = type };
            }
        }
        return m_MaterialDatas;
    }

    /// <summary>
    /// 获取个道具配置数据
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
                string asset = "";
                switch (type)
                {
                    case PropType.Converter:
                        name = "转换器";
                        asset = "Other";
                        break;
                    case PropType.OnOf:
                        name = "重力开关";
                        asset = "Other";
                        break;
                    case PropType.Door:
                        name = "门";
                        asset = "Other";
                        break;
                    case PropType.CoolAir:
                        name = "冷气";
                        asset = "Other";
                        break;
                    case PropType.Stove:
                        name = "火炉";
                        asset = "Other";
                        break;
                    case PropType.StayWarm:
                        name = "保温罩";
                        asset = "Other";
                        break;
                    case PropType.Quickness:
                        name = "急速";
                        asset = "Other";
                        break;
                }

                m_PropDatas[i] = new PropData() { Name = name, Asset = asset, Type = type };
            }
        }
        return m_PropDatas;
    }

    public static RoleDataBase[] GetRoleDataByType(RoleType roleType)
    {
        switch (roleType)
        {
            case RoleType.IceCream:
                return GetIceCreamDatas();
            case RoleType.Prop:
                return GetPropDatas();
            case RoleType.Marterial:
                return GetMaterialDatas();
        }
        return null;
    }
    #endregion
}
