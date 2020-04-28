namespace WordGrid.Api.Client.Interface
{
    public class Dice
    {
        public int Position { get; set; }
        public string Value { get; set; }
        public int Rotation { get; set; }
        public bool ShouldUnderline { get; set; }
    }
}