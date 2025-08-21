using Components;
using Components.Scene;
using Services;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LifetimeScopes
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private SceneEnvironment _sceneEnvironment;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private ServicesTable _servicesTable;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_sceneEnvironment)
                .As<ISceneEnvironment>();
            builder.RegisterInstance(_cameraController)
                .As<ICameraController>();
            _inputHandler.Register(builder);
            _servicesTable.RegisterServices(builder);
        }
    }
}