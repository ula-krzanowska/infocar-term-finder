namespace DrivingLicenceTermsFinder
{
    public class Word
    {
        public string Id { get; }
        public  string Name { get; }

        public Word(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}