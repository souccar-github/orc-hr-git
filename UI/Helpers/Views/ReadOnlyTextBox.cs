namespace UI.Helpers.Views
{
    public class ReadOnlyTextBox
    {
        public ReadOnlyTextBox(bool @readonly, string @class)
        {
            Readonly = @readonly;
            Class = @class;
        }

        public bool Readonly { get; private set; }

        public string Class { get; private set; }
    }
}