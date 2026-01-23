namespace Identity.DTO.Accounts
{
    public sealed record ConfirmEmailDto(Guid userId, string code);

}

