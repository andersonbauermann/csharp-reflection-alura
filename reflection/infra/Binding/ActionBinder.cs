using System.Reflection;

namespace reflection.infra.Binding
{
    public class ActionBinder
    {
        public ActionBindingInfo GetActionBindInfo(object? controller, string path)
        {
            var idxInterrogation = path.IndexOf('?');
            var queryStringExists = idxInterrogation >= 0;

            if (!queryStringExists)
            {
                var nameAction = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var methodInfo = controller.GetType().GetMethod(nameAction);

                return new ActionBindingInfo(methodInfo, Enumerable.Empty<NameArgumentValue>());
            }
            else
            {
                var controllerNameAction = path[..idxInterrogation];
                var nameAction = controllerNameAction.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var queryString = path.Substring(idxInterrogation + 1);
                var tuplasNameValue = GetArgumentNameValue(queryString);
                var argumentsName = tuplasNameValue.Select(tupla => tupla.Name).ToArray();

                var methodInfo = GetMethodInfoByNameAndArguments(nameAction, argumentsName, controller);
                return new ActionBindingInfo(methodInfo, tuplasNameValue);
            }
        }

        private IEnumerable<NameArgumentValue> GetArgumentNameValue(string queryString) 
        {
            var tuplasNameValue = queryString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var tupla in tuplasNameValue)
            {
                var partsTupla = tupla.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                yield return new NameArgumentValue(partsTupla[0], partsTupla[1]);
            }
        }

        private MethodInfo GetMethodInfoByNameAndArguments(string nameAction, string[] arguments, object controller)
        {
            var argumentsCount = arguments.Length;

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

            var methods = controller.GetType().GetMethods(bindingFlags);
            var overloads = methods.Where(m => m.Name == nameAction);

            foreach (var overload in overloads)
            {
                var parameters = overload.GetParameters();

                if (arguments.Length != parameters.Length)
                    continue;

                var match = parameters.All(param => arguments.Contains(param.Name));
                if (match)
                {
                    return overload;
                }
            }

            throw new ArgumentException($"Overload of method {nameAction} not found!");
        }
    }
}
