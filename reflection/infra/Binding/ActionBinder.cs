
namespace reflection.infra.Binding
{
    public class ActionBinder
    {
        public object GetMethodInfo(object? controller, string path)
        {
            var idxInterrogation = path.IndexOf('?');
            var queryStringExists = idxInterrogation >= 0;

            if (!queryStringExists)
            {
                var nameAction = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var methodInfo = controller.GetType().GetMethod(nameAction);

                return methodInfo;
            }
            else
            {
                var controllerNameAction = path[..idxInterrogation];
                var nameAction = controllerNameAction.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var queryString = path.Substring(idxInterrogation + 1);
                var tuplasNameValue = GetArgumentNameValue(queryString);
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
    }
}
