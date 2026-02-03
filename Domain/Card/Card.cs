
public class Card{
    private int Rank { get; set; }
    private int Value { get; set; }
    private string Suit { get; set; }


    public Card(int rank, int value, string suit){
        Rank = rank;
        Value = value;
        Suit = suit;
    }
}