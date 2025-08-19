using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Services
{
    [CreateAssetMenu(menuName = "Settings/ServicesTable", fileName = "ServicesTable")]
    public class ServicesTable : ScriptableObject
    {
        [SerializeReference]
        [ValidateInput(nameof(ValidateServices))]
        private IService[] _services = new IService[0];

        public void Configure(IContainerBuilder builder)
        {
            foreach (var service in _services)
            {
                service.RegisterSelf(builder);
            }
        }

        private bool ValidateServices(IService[] services, out string message)
        {
            foreach (var service in services)
            {
                var sum = services.Sum(item => item.GetType() == service.GetType() ? 1 : 0);
                if (sum > 1)
                {
                    message = "List have duplicate service " + service.GetType().Name;
                    return false;
                }
            }
            message = string.Empty;
            return true;
        }
    }
}