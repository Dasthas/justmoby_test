using System;

namespace Services.Characteristics.Settings
{
    [Serializable]
    public struct CharacteristicValue
    {
        public CharacteristicValueType UpgradeValueType;
        public float Value;
        public uint MaxLevel;
    }
}