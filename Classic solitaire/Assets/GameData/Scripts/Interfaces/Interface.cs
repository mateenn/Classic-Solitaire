namespace TheSyedMateen.ClassicSolitaire
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}
