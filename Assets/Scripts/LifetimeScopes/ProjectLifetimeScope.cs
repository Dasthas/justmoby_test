using Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LifetimeScopes
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        [SerializeField] private ServicesTable _servicesTable;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("Project LifetimeScope");
            _servicesTable.Configure(builder);
        }
    }
}
