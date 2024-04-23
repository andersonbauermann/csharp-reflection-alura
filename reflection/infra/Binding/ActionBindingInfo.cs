using System.Collections.ObjectModel;
using System.Reflection;

namespace reflection.infra.Binding
{
    public class ActionBindingInfo
    {
        public MethodInfo MethodInfo { get; private set; }
        public IReadOnlyCollection<NameArgumentValue> TuplasArgumentsNameValue { get; private set; }

        public ActionBindingInfo(MethodInfo methodInfo, IReadOnlyCollection<NameArgumentValue> tuplasArgumentsNameValue)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

            if (tuplasArgumentsNameValue is null)
            {
                throw new ArgumentNullException(nameof(tuplasArgumentsNameValue));
            }

            TuplasArgumentsNameValue = new ReadOnlyCollection<NameArgumentValue>(tuplasArgumentsNameValue.ToList());
        }

        public object Invoke(object controller)
        {
            var countParams = TuplasArgumentsNameValue.Count;
            var haveArgs = countParams > 0;

            if (!haveArgs)
            {
                return MethodInfo.Invoke(controller, Array.Empty<object>());
            }

            var paramsMethodIndo = MethodInfo.GetParameters();
            var paramsInvoke = new object[countParams];
            
            for (int i = 0; i < countParams; i++)
            {
                var param = paramsMethodIndo[i];
                var nameParam = param.Name;

                var argument = TuplasArgumentsNameValue.Single(tupla => tupla.Name == nameParam);
                paramsInvoke[i] = Convert.ChangeType(argument.Value, param.ParameterType);
            }

            return MethodInfo.Invoke(controller, paramsInvoke);
        }
    }
}
