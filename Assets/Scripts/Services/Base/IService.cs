using System;
using VContainer;

namespace Services.Base
{
    public interface IService
    {
        public Service RegisterAndGetInstance(IContainerBuilder builder);
        void Dispose();
    }
}