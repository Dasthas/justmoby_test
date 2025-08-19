using System;
using VContainer;

namespace Services
{
    public interface IService
    {
        public void RegisterSelf(IContainerBuilder builder);
    }
}