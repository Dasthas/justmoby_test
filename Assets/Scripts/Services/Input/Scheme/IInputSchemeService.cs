using System;

namespace Services.Input.Scheme
{
    public interface IInputSchemeService
    {
        void ChangeInputScheme(InputSchemeType newInputScheme);
        IObservable<InputSchemeType> OnInputSchemeChanged { get; }
        InputSchemeType CurrentScheme { get; }
    }
}