namespace QueryHandler
{
    public class PersonCommand : ICommand<int>
    {
        public Person Person { get; private set; }

        public PersonCommand(Person person)
        {
            this.Person = person;
        }
    }
}
