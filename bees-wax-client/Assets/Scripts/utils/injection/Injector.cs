using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

//contains functionality classically implemented in a context.
//combined in attempt to simplify

namespace utils.injection
{
    public class Injector
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.DeclaredOnly |
                                                  System.Reflection.BindingFlags.NonPublic |
                                                  System.Reflection.BindingFlags.Instance;

        public static Injector Instance;
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        private readonly Dictionary<Type, List<FieldInfo>> _injectionPoints = new Dictionary<Type, List<FieldInfo>>();

        public static void Setup(Assembly assembly)
        {
            if (Instance != null)
                return;
            
            Instance = new Injector();

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a != assembly)
                    continue;
                
                a.GetTypes().ToList().ForEach(Instance.ProcessType);
            }
        }

        private void ProcessType(Type type)
        {
            if (type.IsAbstract)
                return;

            ProcessInjectionPoints(type, type);
        }

        private void ProcessInjectionPoints(Type owner, Type type)
        {
            foreach (var member in type.GetFields(BindingFlags))
            {
                if (member.GetCustomAttributes(typeof(Inject), true).Length == 0)
                    continue;

                if (_injectionPoints.ContainsKey(owner))
                    _injectionPoints[owner].Add(member);
                else
                    _injectionPoints.Add(owner, new List<FieldInfo> {member});
            }

            if (type.BaseType != null)
                ProcessInjectionPoints(owner, type.BaseType);
        }

        public void Resolve(object obj, int ownerId = 0)
        {
            if (!_injectionPoints.ContainsKey(obj.GetType()))
                return;

            foreach (var member in _injectionPoints[obj.GetType()])
            {
                var val = ResolveValue(member, obj, ownerId);
                member.SetValue(obj, val);
            }
        }

        //TODO decorate with resolvers
        private object ResolveValue(FieldInfo member, object obj, int ownerId)
        {
            if (member.FieldType.GetCustomAttributes(typeof(Singleton), false).Length > 0)
                return GetValue(member.FieldType);

            return GetValue(member.FieldType, ownerId);
        }

        private object GetValue(Type type, int id = 0)
        {
            var key = id != 0 ? $"{type}:{id}" : type.ToString();

            if (!_cache.ContainsKey(key))
                _cache.Add(key, Activator.CreateInstance(type));

            return _cache[key];
        }
    }
}