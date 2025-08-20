using System.Linq;
using Services.Base;
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

        public void RegisterServices(IContainerBuilder builder)
        {
            var runtimeServices = new IService[_services.Length];
            for (var i = 0; i < _services.Length; i++)
            {
                var service = _services[i];
                runtimeServices[i] = service.RegisterAndGetInstance(builder);
            }

            builder.RegisterBuildCallback(container =>
            {
                foreach (var service in runtimeServices)
                {
                    container.Inject(service);
                }
            });
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