using System.Collections;
using UnityEngine;
using CrashKonijn.Goap;
namespace Assets.Scripts.GOAP
{
    public interface IInjectableObj
    {
        public void Inject(DependencyInjector injector);
    }
}