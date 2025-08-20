using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Services.Characteristics.Settings
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