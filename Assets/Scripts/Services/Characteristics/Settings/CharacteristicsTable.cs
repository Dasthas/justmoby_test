using System;
using System.Collections.Generic;
using System.Linq;
using Services.Characteristics.Settings.Data;
using Services.Characteristics.Settings.Data.Value;
using UnityEngine;

namespace Services.Characteristics.Settings
{
    [CreateAssetMenu(menuName = "Settings/CharacteristicsTable", fileName = "CharacteristicsTable")]
    public class CharacteristicsTable : ScriptableObject
    {
        [SerializeField]
        private List<CharacteristicData> _characteristics = new();

        public IReadOnlyList<CharacteristicData> Characteristics => _characteristics;

        public CharacteristicValue GetDataForType(CharacteristicType upgradeType)
        {
            if (_characteristics.Any((item) => item.Type == upgradeType))
            {
                return _characteristics.FindLast((item) => item.Type == upgradeType).Value;
            }
            else
            {
                throw new Exception("UpgradesData not found for type " + upgradeType.ToString());
            }
        }
    }
}