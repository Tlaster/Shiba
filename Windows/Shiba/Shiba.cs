using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Shiba.Controls;

namespace Shiba
{
    public abstract class AbstractShiba
    {
        protected AbstractShiba(Action<ShibaConfiguration> action)
        {
            action?.Invoke(Configuration);
            ViewMapping.Init();
        }

        public ViewMapping ViewMapping { get; } = new ViewMapping();

        public ShibaConfiguration Configuration { get; } = new ShibaConfiguration();

        public static AbstractShiba Instance { get; protected set; }

        public void AddConverter(string converter)
        {
            if (Configuration?.ConverterExecutor is DefaultConverterExecutor executor) executor.Register(converter);
        }
    }

    public class ShibaConfiguration
    {
        public IConverterExecutor ConverterExecutor { get; set; } = new DefaultConverterExecutor();
        public string PlatformType { get; set; } = "Windows";

        public List<IExtensionExecutor> ExtensionExecutors { get; } =
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(it => !it.IsDynamic)
                .SelectMany(it => it.ExportedTypes)
                .Where(it => it.IsClass && !it.IsAbstract && typeof(IExtensionExecutor).IsAssignableFrom(it))
                .Select(it => Activator.CreateInstance(it) as IExtensionExecutor).ToList();

        public List<ICommonProperty> CommonProperties { get; } =
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(it => !it.IsDynamic)
                .SelectMany(it => it.ExportedTypes)
                .Where(it => it.IsClass && !it.IsAbstract && typeof(ICommonProperty).IsAssignableFrom(it))
                .Select(it => Activator.CreateInstance(it) as ICommonProperty).ToList();
    }

    public interface IConverterExecutor
    {
        object Execute(string functionName, params object[] parameters);
    }

    public class DefaultConverterExecutor : IConverterExecutor
    {
        private readonly Engine _engine;

        public DefaultConverterExecutor(Engine engine = null)
        {
            _engine = engine ?? new Engine();
        }

        public object Execute(string functionName, params object[] parameters)
        {
            var jsConverter = _engine.GetValue(functionName);
            if (jsConverter != null && jsConverter.Is<FunctionInstance>())
                return jsConverter.Invoke(parameters.Select(it => JsValue.FromObject(_engine, it)).ToArray())
                    .ToObject();


            throw new EntryPointNotFoundException();
        }

        public void Register(string converter)
        {
            _engine.Execute(converter);
        }

        public void Register(string name, Delegate @delegate)
        {
        }
    }
}