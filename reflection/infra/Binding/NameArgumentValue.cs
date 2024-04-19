namespace reflection.infra.Binding
{
    public class NameArgumentValue
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public NameArgumentValue(string name, string value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }   
    }
}
