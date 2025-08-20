using System;
using Services.Characteristics.Settings.Data.Value;
using UnityEngine;

namespace Services.Characteristics.Settings.Data
{
    [Serializable]
    public struct CharacteristicData
    {
        public string NameLocalizationKey;
        public Sprite Icon;
        public CharacteristicType Type;
        public CharacteristicValue Value;
    }
}