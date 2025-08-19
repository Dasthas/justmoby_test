using Components;
using Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LifetimeScopes
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        [SerializeField] private SceneEnvironment _sceneEnvironment;
        [SerializeField] private ServicesTable _servicesTable;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_sceneEnvironment)
                .As<ISceneEnvironment>();
            Debug.Log("Project LifetimeScope");
            _servicesTable.Configure(builder);
        }
    }
}
