namespace BusinessLogic.SequenceGenerator
{
    public sealed class SequenceGenerator : ISequenceGenerator
    {
        private int _current;

        public SequenceGenerator()
        {
            var random = new Random();
            var start = random.Next(0, 99999999); //Just for this project, I did not want to create a new table where the sequence can be stored as this is a simple project without multi-users/pods
            _current = start;
        }

        public int GetNext()
        {
            int value = _current;
            _current++;
            return value;
        }
    }
}
