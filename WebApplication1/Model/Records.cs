namespace WebApplication1.Model
{


    sealed record CreateAlertRequest(
        string Name,
        List<string> Keywords,
        List<string> Categories,
        List<string> Channels);

    sealed record UpdateAlertRequest(
        string Name,
        List<string> Keywords,
        List<string> Categories,
        List<string> Channels,
        bool Enabled);

    sealed record WorldEvent(
        string Title,
        string Description,
        string Category);


}
