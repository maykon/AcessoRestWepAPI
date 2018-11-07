using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;

namespace RegrasNegocio
{
  public static class DiContainer
  {
    private static IWindsorContainer _container = new WindsorContainer();

    public static void Reset()
    {
      _container?.Dispose();
      _container = new WindsorContainer();
    }

    public static void Register<IT,CT>() where IT : class where CT : class
    {
      _container.Register(Component.For<IT>().ImplementedBy(typeof(CT)).LifestyleTransient());
    }


    public static void Register<T>(T item) where T : class
    {
      _container.Register(Component.For<T>().Instance(item)); 
    }

    public static void Register<IT, CT>(DepType dependency) where IT : class where CT : class
    {
      _container.Register(Component.For<IT>().ImplementedBy(typeof(CT))
        .DependsOn(Dependency.OnComponent(dependency.Key, dependency.Value))
        .LifestyleTransient());
    }

    public static void Register<IT, CT>(DepValue dependency) where IT : class where CT : class
    {
      _container.Register(Component.For<IT>().ImplementedBy(typeof(CT))
        .DependsOn(Dependency.OnValue(dependency.Key, dependency.Value))
        .LifestyleTransient());
    }


    public static void Register<IT, CT>(DepType dependency01, DepType dependency02) where IT : class where CT : class
    {
      _container.Register(Component.For<IT>().ImplementedBy(typeof(CT))
        .DependsOn(Dependency.OnComponent(dependency01.Key, dependency01.Value))
        .DependsOn(Dependency.OnComponent(dependency02.Key, dependency02.Value))
        .LifestyleTransient());
    }

    public static T Resolve<T>() where T : class
    {
      return _container.Resolve<T>();
    }
  }


  public class Depend<T1,T2>
  {
    public Depend(T1 key, T2 value)
    {
      Key = key;
      Value = value;
    }
    public T1 Key { get; set; }
    public T2 Value { get; set; }
  }

  public class DepName : Depend<string, string>
  {
    public DepName(string key, string value) : base(key, value)
    {
    }
  }

  public class DepType : Depend<Type, Type>
  {
    public DepType(Type key, Type value) : base(key, value)
    {
    }
  }

  public class DepValue : Depend<string, object>
  {
    public DepValue(string key, object value) : base(key, value)
    {
    }
  }


}
