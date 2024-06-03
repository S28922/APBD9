namespace ENtityFramework.DTO;

public record ClientTripDto
{
    public int IdClient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}